using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class InfoDaging : MonoBehaviour {
    [Header("---Window---")]
    [SerializeField] private GameObject infoDagingCanvas;
    [SerializeField] private TextMeshProUGUI teksLevelDaging;
    [SerializeField] private TextMeshProUGUI teksHargaDaging;
    [SerializeField] private TextMeshProUGUI teksStokDagangan; // dengan max stok
    [SerializeField] private TextMeshProUGUI teksUpLevel;
    [SerializeField] private TextMeshProUGUI teksHargaUpgradeDaging;
    [SerializeField] private TextMeshProUGUI teksDagingBatu;
    [SerializeField] private TextMeshProUGUI teksDagingKayu;
    [SerializeField] private TextMeshProUGUI teksDagingTanahLiat;
    [SerializeField] private TextMeshProUGUI teksBatuAkik;
    [SerializeField] private Button closeInfoButton;
    [SerializeField] private Button restokDaganganButton;

    [Header("---Panel---")]
    [SerializeField] private Button tingkatkanButton;
    [SerializeField] private GameObject konfirmasiPanel;
    [SerializeField] private Button konfirmasiButton;
    [SerializeField] private TextMeshProUGUI teksKonfirm;
    [SerializeField] private Button batalButton;

    [Header("---Info Luar---")]
    [SerializeField] private TextMeshProUGUI teksPenghasilanDaging;
    [SerializeField] private TextMeshProUGUI teksStokLuar;

    private int merchantIndex;

    private void Start() {
        konfirmasiPanel.SetActive(false);
        infoDagingCanvas.SetActive(false);

        closeInfoButton.GetComponent<Button>().onClick.AddListener(() => {
            infoDagingCanvas.SetActive(false);
            PersistentManager.Instance.isUIOpen = false;
        });

        restokDaganganButton.GetComponent<Button>().onClick.AddListener(RestokDaging);

        tingkatkanButton.GetComponent<Button>().onClick.AddListener(UpgradeKondisi);

        konfirmasiButton.GetComponent<Button>().onClick.AddListener(() => {
            UpgradeDaging();
            infoDagingCanvas.SetActive(false);
            PersistentManager.Instance.isUIOpen = false;
        });

        batalButton.GetComponent<Button>().onClick.AddListener(() => {
            konfirmasiPanel.SetActive(false);
        });

        // Dapatkan indeks merchant dari MerchantManager
        merchantIndex = MerchantManager.Instance.GetCurrentMerchantIndex();
        Debug.Log ("Merchant index ke: " + merchantIndex);
        var merchantData = PersistentManager.Instance.dataMerchantList[merchantIndex];
        merchantData.levelMerchant = 1;
        merchantData.hargaDagangan = 500f;
        merchantData.stokDagangan = 10f;
        merchantData.maxStokDagangan = 10f;
        merchantData.costUpBatu = 10;
        merchantData.costUpKayu = 10;
        merchantData.costUpTanahLiat = 10;
        merchantData.costUpBatuAkik = 1;
        merchantData.hargaUpgrade = 1250;
    }

    private void Update() {
        UpdateInfo();
        UpdateStokGFX();
        UpdateLevelGFX();
    }

    private void UpdateInfo() {
        var merchantData = PersistentManager.Instance.dataMerchantList[merchantIndex];

        teksLevelDaging.text = "Pedagang Daging Level " + merchantData.levelMerchant.ToString();
        teksHargaDaging.text = merchantData.hargaDagangan.ToString("N0") + "K / Pengunjung";
        teksStokDagangan.text = Mathf.Round(merchantData.stokDagangan).ToString() + "/" + merchantData.maxStokDagangan;
        teksUpLevel.text = "Up Level " + (merchantData.levelMerchant + 1).ToString();
        teksHargaUpgradeDaging.text = merchantData.hargaUpgrade.ToString("N0") + "K";
        teksDagingBatu.text = (PersistentManager.Instance.dataBatu + "/" + merchantData.costUpBatu).ToString();
        teksDagingKayu.text = (PersistentManager.Instance.dataKayu + "/" + merchantData.costUpKayu).ToString();
        teksDagingTanahLiat.text = (PersistentManager.Instance.dataTanahLiat + "/" + merchantData.costUpTanahLiat).ToString();
        teksBatuAkik.text = (PersistentManager.Instance.dataBatuAkik + "/" + merchantData.costUpBatuAkik).ToString();
        teksKonfirm.text = merchantData.hargaUpgrade.ToString("N0") + "K";

        teksPenghasilanDaging.text = merchantData.penghasilanMerchant.ToString("N0");
        teksStokLuar.text = Mathf.Round(merchantData.stokDagangan).ToString() + "/" + merchantData.maxStokDagangan;
    }

    private void UpgradeDaging() {
        var merchantData = PersistentManager.Instance.dataMerchantList[merchantIndex];

        if (PersistentManager.Instance.dataKoin >= merchantData.hargaUpgrade && PersistentManager.Instance.dataBatu >= merchantData.costUpBatu && PersistentManager.Instance.dataKayu >= merchantData.costUpKayu && PersistentManager.Instance.dataTanahLiat >= merchantData.costUpTanahLiat && merchantData.levelMerchant < 3) {
            PersistentManager.Instance.UpdateKoin(-merchantData.hargaUpgrade);

            merchantData.levelMerchant++;
            
            merchantData.hargaDagangan = merchantData.hargaDagangan * merchantData.levelMerchant;
            merchantData.maxStokDagangan = merchantData.maxStokDagangan * merchantData.levelMerchant;
            merchantData.costUpBatu = merchantData.costUpBatu * merchantData.levelMerchant;
            merchantData.costUpKayu = merchantData.costUpKayu * merchantData.levelMerchant;
            merchantData.costUpTanahLiat = merchantData.costUpTanahLiat * merchantData.levelMerchant;
            merchantData.costUpBatuAkik = merchantData.costUpBatuAkik++;
            merchantData.hargaUpgrade = merchantData.hargaUpgrade * merchantData.levelMerchant;

            konfirmasiPanel.SetActive(false);
        }
    }

    [SerializeField] private GameObject LessKoinNotif;
    [SerializeField] private GameObject LessBahanNotif;

    private void UpgradeKondisi() {
        var merchantData = PersistentManager.Instance.dataMerchantList[merchantIndex];
        
        if ((PersistentManager.Instance.dataBatu < merchantData.costUpBatu || PersistentManager.Instance.dataKayu < merchantData.costUpKayu || PersistentManager.Instance.dataTanahLiat < merchantData.costUpTanahLiat) && PersistentManager.Instance.dataKoin < merchantData.hargaUpgrade) {
            StartCoroutine(PlayLessBahan());
            StartCoroutine(PlayLessKoin());
        }
        else if (PersistentManager.Instance.dataBatu < merchantData.costUpBatu || PersistentManager.Instance.dataKayu < merchantData.costUpKayu || PersistentManager.Instance.dataTanahLiat < merchantData.costUpTanahLiat) {
            StartCoroutine(PlayLessBahan());
        } 
        else if (PersistentManager.Instance.dataKoin < merchantData.hargaUpgrade) {
            StartCoroutine(PlayLessKoin());
        }

        if (PersistentManager.Instance.dataKoin >= merchantData.hargaUpgrade && PersistentManager.Instance.dataBatu >= merchantData.costUpBatu && PersistentManager.Instance.dataKayu >= merchantData.costUpKayu && PersistentManager.Instance.dataTanahLiat >= merchantData.costUpTanahLiat && merchantData.levelMerchant < 3) {
            konfirmasiPanel.SetActive(true);
        }
    }

    private IEnumerator PlayLessKoin() {
        konfirmasiPanel.SetActive(false);
        LessKoinNotif.SetActive(true);
        yield return new WaitForSeconds(1.277f);
        LessKoinNotif.SetActive(false);
    }

    private IEnumerator PlayLessBahan() {
        konfirmasiPanel.SetActive(false);
        LessBahanNotif.SetActive(true);
        yield return new WaitForSeconds(1.277f);
        LessBahanNotif.SetActive(false);
    }

    private void RestokDaging() {
        var merchantData = PersistentManager.Instance.dataMerchantList[merchantIndex];

        if (PersistentManager.Instance.dataStokDaging > 0) {
            merchantData.stokDagangan++;
            PersistentManager.Instance.dataStokDaging--;
        }
    }

    [Header("--- GFX Update ---")]
    [SerializeField] private GameObject merchantGFX;
    [SerializeField] private Sprite GFXLevel_1;
    [SerializeField] private Sprite GFXLevel_2;
    [SerializeField] private Sprite GFXLevel_3;

    [SerializeField] private Sprite GFXMatiLevel_1;
    [SerializeField] private Sprite GFXMatiLevel_2;
    [SerializeField] private Sprite GFXMatiLevel_3;

    private void UpdateLevelGFX() {
        var merchantData = PersistentManager.Instance.dataMerchantList[merchantIndex];

        switch (merchantData.levelMerchant) {
            case 1:
                merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXLevel_1;
                break;
            case 2:
                merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXLevel_2;
                break;
            case 3:
                merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXLevel_3;
                break;
            default:
                Debug.LogWarning("Level Merchant tidak valid!");
                break;
        }
    }

    private void UpdateStokGFX() {
        var merchantData = PersistentManager.Instance.dataMerchantList[merchantIndex];

        if (merchantData.stokDagangan == 0) {
            switch (merchantData.levelMerchant) {
                case 1:
                    merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXMatiLevel_1;
                break;
            case 2:
                merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXMatiLevel_2;
                break;
            case 3:
                merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXMatiLevel_3;
                break;
            default:
                Debug.LogWarning("Level Merchant tidak valid!");
                break;
            }
        } else {
            switch (merchantData.levelMerchant) {
                case 1:
                    merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXLevel_1;
                    break;
                case 2:
                    merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXLevel_2;
                    break;
                case 3:
                    merchantGFX.GetComponent<SpriteRenderer>().sprite = GFXLevel_3;
                    break;
                default:
                    Debug.LogWarning("Level Merchant tidak valid!");
                    break;
            }
        }
    }

    [Header("--- Button State ---")]

    [SerializeField] private Sprite normalKonfirm;
    [SerializeField] private Sprite highlightedKonfirm;
    [SerializeField] private Color normalColorKonfirm = Color.black;
    [SerializeField] private Color highlightedColorKonfirm = Color.white;

    public void OnHighlightKonfirm() {
        konfirmasiButton.image.sprite = highlightedKonfirm;
        teksKonfirm.color = highlightedColorKonfirm;
    }

    public void OnUnhighlightKonfirm() {
        konfirmasiButton.image.sprite = normalKonfirm;
        teksKonfirm.color = normalColorKonfirm;
    }

    [SerializeField] private Sprite normalBatal;
    [SerializeField] private Sprite highlightedBatal;

    public void OnHighlightBatal() {
        batalButton.image.sprite = highlightedBatal;
    }

    public void OnUnhighlightBatal() {
        batalButton.image.sprite = normalBatal;
    }
}
