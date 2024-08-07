using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MerchantSelectUI : MonoBehaviour {
    public List<MerchantTypeSO> merchantTypeSOList;
    public MerchantTypeSO GetThirdMerchant() {
        return merchantTypeSOList[2];
    }

    [SerializeField] private List<MerchantTerkunciSO> merchantTerkunciSOList;
    [SerializeField] private MerchantManager merchantManager;
    [SerializeField] private Transform player; // Referensi ke player object
    [SerializeField] private Vector2 targetPosition; // Posisi target saat player bergerak
    [SerializeField] private float smoothingSpeed; // Kecepatan smoothing

    private List<Transform> merchantButtonList;
    private List<Transform> merchantBtnTerkunciList;
    private Vector2 initialPosition; // Posisi awal UI element
    private bool isMoving = false; // Status player bergerak
    private RectTransform rectTransform;
    private GameObject cursorInstance; // Instance dari prefab kursor

    private void Awake() {
        Transform merchantBtnTemplate = transform.Find("MerchantBtnTemplate");
        merchantBtnTemplate.gameObject.SetActive(false);
        merchantButtonList = new List<Transform>();

        Transform merchantBtnTerkunci = transform.Find("MerchantBtnTerkunci");
        merchantBtnTerkunci.gameObject.SetActive(false);
        merchantBtnTerkunciList = new List<Transform>();

        int index = 0;
        int indexTerkunci = 3; 

        foreach (MerchantTypeSO merchantTypeSO in merchantTypeSOList) {
            Transform merchantBtnTransform = Instantiate(merchantBtnTemplate, transform);
            merchantBtnTransform.gameObject.SetActive(true);

            merchantBtnTransform.GetComponent<RectTransform>().anchoredPosition += new Vector2(index * 130, 0);
            merchantBtnTransform.Find("Image").GetComponent<Image>().sprite = merchantTypeSO.spriteButton;

            merchantBtnTransform.GetComponent<Button>().onClick.AddListener(() => {
                if (merchantManager.GetActiveMerchantType() == merchantTypeSO) {
                    merchantManager.SetActiveMerchantType(null);
                    DestroyCursorInstance(); // Hapus kursor jika merchantTypeSO diaktifkan/dinonaktifkan
                } else {
                    merchantManager.SetActiveMerchantType(merchantTypeSO);
                    SetCursor(merchantTypeSO.cursorPrefab); // Atur kursor saat merchant dipilih
                }
                UpdateSelectedVisual();
            });
            merchantButtonList.Add(merchantBtnTransform);

            index++;
        }

        foreach (MerchantTerkunciSO merchantTerkunciSO in merchantTerkunciSOList) {
            Transform merchantBtnTransformTerkunci = Instantiate(merchantBtnTerkunci, transform);
            merchantBtnTransformTerkunci.gameObject.SetActive(true);

            merchantBtnTransformTerkunci.GetComponent<RectTransform>().anchoredPosition += new Vector2(indexTerkunci * 130, 0);
            merchantBtnTransformTerkunci.Find("Image").GetComponent<Image>().sprite = merchantTerkunciSO.spriteButton;
            merchantBtnTerkunciList.Add(merchantBtnTransformTerkunci);

            indexTerkunci++;
        }

        rectTransform = GetComponent<RectTransform>();
    }

    private void Start() {
        UpdateSelectedVisual();
        if (rectTransform != null) {
            initialPosition = rectTransform.anchoredPosition; // Simpan posisi awal UI element
        }
    }

    private void Update() {
        // Deteksi pergerakan player (sesuaikan sesuai dengan cara kamu mendeteksi pergerakan player)
        if (player != null && (player.GetComponent<Rigidbody2D>().velocity.magnitude > 0)) {
            isMoving = true;
        }
        else {
            isMoving = false;
        }
        // Pindahkan UI element dengan smoothing berdasarkan status pergerakan player
        if (isMoving) {
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPosition, smoothingSpeed * Time.deltaTime);
        }
        else {
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, initialPosition, smoothingSpeed * Time.deltaTime);
        }

        if (cursorInstance != null) {
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorPosition.z = 0;
            cursorInstance.transform.position = cursorPosition;
        }

        merchantManager.OnMerchantPlaced += HandleMerchantPlaced;
    }

    private void HandleMerchantPlaced() {
        UpdateSelectedVisual(); // Panggil update visual setelah merchant ditempatkan
    }

    private void OnDestroy() {
        merchantManager.OnMerchantPlaced -= HandleMerchantPlaced; // Unregister event listener
    }

    private void UpdateSelectedVisual() {
        MerchantTypeSO activeMerchantType = merchantManager.GetActiveMerchantType();

        foreach (Transform merchantBtnTransform in merchantButtonList) {
            Image image = merchantBtnTransform.Find("Image").GetComponent<Image>();
            GameObject selected = merchantBtnTransform.Find("Selected").gameObject;
            GameObject merchantWindow = merchantBtnTransform.Find("MerchantWindow").gameObject;

            if (activeMerchantType != null && image.sprite == activeMerchantType.spriteButton) {
                image.gameObject.SetActive(false);
                selected.SetActive(true);
                selected.GetComponent<Image>().sprite = activeMerchantType.selectedSpriteButton;
                merchantWindow.SetActive(true);
                merchantWindow.GetComponent<Image>().sprite = activeMerchantType.merchantWindow;
            } else {
                image.gameObject.SetActive(true);
                selected.SetActive(false);
                merchantWindow.SetActive(false);
            }
        }
    }

    private void SetCursor(GameObject cursorPrefab) {
        if (cursorInstance != null) {
            Destroy(cursorInstance);
        }

        if (cursorPrefab != null) {
            cursorInstance = Instantiate(cursorPrefab);
        }
    }

    public void DestroyCursorInstance() {
        if (cursorInstance != null) {
            Destroy(cursorInstance);
        }
    }
}
