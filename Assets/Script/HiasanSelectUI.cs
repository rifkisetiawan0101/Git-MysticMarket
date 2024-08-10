using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiasanSelectUI : MonoBehaviour {
    [SerializeField] private List<HiasanTypeSO> hiasanTypeSOList;
    [SerializeField] private HiasanManager hiasanManager;
    
    private List<Transform> hiasanButtonList;
    private RectTransform rectTransform;
    private GameObject cursorInstance; // Instance dari prefab kursor

    private void Awake() {
        Transform hiasanBtnTemplate = transform.Find("HiasanBtnTemplate");
        hiasanBtnTemplate.gameObject.SetActive(false);
        hiasanButtonList = new List<Transform>();

        int index = 0;

        foreach (HiasanTypeSO hiasanTypeSO in hiasanTypeSOList) {
            Transform hiasanBtnTransform = Instantiate(hiasanBtnTemplate, transform);
            hiasanBtnTransform.gameObject.SetActive(true);

            hiasanBtnTransform.GetComponent<RectTransform>().anchoredPosition += new Vector2(index * 130, 0);
            hiasanBtnTransform.Find("Image").GetComponent<Image>().sprite = hiasanTypeSO.hiasanButton;

            hiasanBtnTransform.GetComponent<Button>().onClick.AddListener(() => {
                if (hiasanManager.GetActiveHiasanType() == hiasanTypeSO) {
                    hiasanManager.SetActiveHiasanType(null);
                    DestroyCursorHiasan(); // Hapus kursor jika hiasanTypeSO diaktifkan/dinonaktifkan
                } else {
                    hiasanManager.SetActiveHiasanType(hiasanTypeSO);
                    SetCursor(hiasanTypeSO.hiasanCursor); // Atur kursor saat hiasan dipilih
                }
                UpdateSelectedVisual();
            });
            hiasanButtonList.Add(hiasanBtnTransform);

            index++;
        }

        rectTransform = GetComponent<RectTransform>();
    }

    private void Start() {
        hiasanManager.OnHiasanPlaced += HandleHiasanPlaced;

        Button buttonMerchant = GameObject.Find("ButtonMerchant").GetComponent<Button>(); 
        buttonMerchant.onClick.AddListener(() => {
            hiasanManager.SetActiveHiasanType(null); // Set activeHiasanType ke null
            DestroyCursorHiasan(); // Hapus kursor
            UpdateSelectedVisual(); // Update visual untuk memastikan tidak ada yang selected
        });
        
        UpdateSelectedVisual();
    }

    private void Update() {
        if (cursorInstance != null) {
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorPosition.z = 0;
            cursorInstance.transform.position = cursorPosition;
        }
    }

    private void HandleHiasanPlaced() {
        UpdateSelectedVisual(); // Panggil update visual setelah hiasan ditempatkan
    }

    private void OnDestroy() {
        hiasanManager.OnHiasanPlaced -= HandleHiasanPlaced; // Unregister event listener
    }

    private void UpdateSelectedVisual() {
        HiasanTypeSO activeHiasanType = hiasanManager.GetActiveHiasanType();

        foreach (Transform hiasanBtnTransform in hiasanButtonList) {
            Image image = hiasanBtnTransform.Find("Image").GetComponent<Image>();
            GameObject selected = hiasanBtnTransform.Find("Selected").gameObject;
            GameObject hiasanWindow = hiasanBtnTransform.Find("HiasanWindow").gameObject;

            if (activeHiasanType != null && image.sprite == activeHiasanType.hiasanButton) {
                image.gameObject.SetActive(false);
                selected.SetActive(true);
                selected.GetComponent<Image>().sprite = activeHiasanType.selectedHiasanButton;
                hiasanWindow.SetActive(true);
                hiasanWindow.GetComponent<Image>().sprite = activeHiasanType.hiasanWindow;
            } else {
                image.gameObject.SetActive(true);
                selected.SetActive(false);
                hiasanWindow.SetActive(false);
            }
        }
    }

    private void SetCursor(GameObject hiasanCursor) {
        if (cursorInstance != null) {
            Destroy(cursorInstance);
        }

        if (hiasanCursor != null) {
            cursorInstance = Instantiate(hiasanCursor);
        }
    }

    public void DestroyCursorHiasan() {
        if (cursorInstance != null) {
            Destroy(cursorInstance);
        }
    }
}
