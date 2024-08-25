using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerRempah : MonoBehaviour {
    [SerializeField] private GameObject collectButton;
    [SerializeField] private Image collectButtonImage;
    [SerializeField] private Sprite[] gifFrames;
    private float frameDelay = 0.083f;
    private int currentFrame;

    [SerializeField] private GameObject openInfo;
    [SerializeField] private GameObject openInfoButton;
    [SerializeField] private Image openInfoButtonImage;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite highlightedSprite;
    [SerializeField] private GameObject infoRempahCanvas;

    [SerializeField] private GameObject collectFeedback;
    [SerializeField] private Image collectFeedbackImage;
    [SerializeField] private Sprite[] collectFrames;
    private float collectDelay = 0.055f;
    private int merchantIndex;

    private void Start() {
        merchantIndex = MerchantManager.Instance.GetCurrentMerchantIndex();

        collectButton.SetActive(false);
        collectButton.GetComponent<Button>().onClick.AddListener(OnCollectButtonClick);

        openInfo.SetActive(false);
        infoRempahCanvas.SetActive(false);

        openInfoButton.GetComponent<Button>().onClick.AddListener(() => {
            infoRempahCanvas.SetActive(true);
            PersistentManager.Instance.isUIOpen = true;

            FindObjectOfType<PlayerMovementNew>().StopPlayer();
        });

        StartCoroutine(PlayGIF());

        collectFeedback.SetActive(false);
    }

    private Dictionary<Collider2D, float> npcTimers = new Dictionary<Collider2D, float>();
    public float requiredTime = 2f;
    public float requiredUtoTime = 3f;

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("NPC")) {
            if (!npcTimers.ContainsKey(collision)) {
                npcTimers[collision] = 0f;  // Mulai timer untuk NPC baru yang masuk
            }

            npcTimers[collision] += Time.deltaTime;  // Tambah waktu untuk NPC ini

            if (npcTimers[collision] >= requiredTime) {
                TambahPenghasilanMerchant(collision);
                npcTimers.Remove(collision);  // Hapus NPC dari daftar setelah logika dijalankan
            }
        }

        if (collision.CompareTag("Uto")) {
            if (!npcTimers.ContainsKey(collision)) {
                npcTimers[collision] = 0f;  // Mulai timer untuk NPC baru yang masuk
            }

            npcTimers[collision] += Time.deltaTime;  // Tambah waktu untuk NPC ini

            if (npcTimers[collision] >= requiredUtoTime) {
                ResetPenghasilanMerchant(collision);
                npcTimers.Remove(collision);  // Hapus NPC dari daftar setelah logika dijalankan
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("NPC")) {
            npcTimers.Remove(collision);  // Hapus timer jika NPC keluar sebelum 3 detik
        }

        if (collision.CompareTag("Uto")) {
            npcTimers.Remove(collision);  // Hapus timer jika NPC keluar sebelum 3 detik
        }
    }

    private void TambahPenghasilanMerchant(Collider2D collision) {
        var merchantData = PersistentManager.Instance.dataMerchantList[merchantIndex];

        if (merchantData.stokDagangan > 0) {
            merchantData.penghasilanMerchant += merchantData.hargaDagangan;
            merchantData.stokDagangan -= 1f;
            Debug.Log("Penghasilan Rempah bertambah " + merchantData.penghasilanMerchant);
        }
        
        if (merchantData.penghasilanMerchant >= 750) {
            collectButton.SetActive(true);
        }
    }

    private void ResetPenghasilanMerchant(Collider2D collision) {
        var merchantData = PersistentManager.Instance.dataMerchantList[merchantIndex];

        merchantData.penghasilanMerchant = 0;
        Debug.Log("Penghasilan Daging di-reset oleh Uto!");
        collectButton.SetActive(false);
    }

    private IEnumerator PlayGIF() {
        while (true) {
            collectButtonImage.sprite = gifFrames[currentFrame];
            currentFrame = (currentFrame + 1) % gifFrames.Length;
            yield return new WaitForSeconds(frameDelay);
        }
    }

    private void OnCollectButtonClick() {
        var merchantData = PersistentManager.Instance.dataMerchantList[merchantIndex];

        PersistentManager.Instance.UpdateKoin(merchantData.penghasilanMerchant);
        Debug.Log("Koin di Setor!");
        merchantData.penghasilanMerchant = 0;
        Debug.Log("Penghasilan Rempah Saat Ini = " + merchantData.penghasilanMerchant);
        collectButton.SetActive(false);
        StartCoroutine(PlayCollectKoin());
    }

    private IEnumerator PlayCollectKoin() {
        collectFeedback.SetActive(true);
        for (int i = 0; i < collectFrames.Length; i++) {
            collectFeedbackImage.sprite = collectFrames[i];
            yield return new WaitForSeconds(collectDelay);
        }
        collectFeedback.SetActive(false);
    }

    public void OnHighlightButton() {
        openInfoButtonImage.sprite = highlightedSprite;
    }

    public void OnUnhighlightButton() {
        openInfoButtonImage.sprite = normalSprite;
    }
}
