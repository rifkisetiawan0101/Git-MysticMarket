using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MerchantSelectUI : MonoBehaviour {
    [SerializeField] private List<MerchantTypeSO> merchantTypeSOList;
    [SerializeField] private MerchantManager merchantManager;
    
    private List<Transform> merchantButtonList;
    private RectTransform rectTransform;
    public GameObject cursorInstance; // Instance dari prefab kursor

    private void Awake() {
        Transform merchantBtnTemplate = transform.Find("MerchantBtnTemplate");
        merchantBtnTemplate.gameObject.SetActive(false);
        merchantButtonList = new List<Transform>();

        int index = 0;

        foreach (MerchantTypeSO merchantTypeSO in merchantTypeSOList) {
            Transform merchantBtnTransform = Instantiate(merchantBtnTemplate, transform);
            merchantBtnTransform.gameObject.SetActive(true);

            merchantBtnTransform.GetComponent<RectTransform>().anchoredPosition += new Vector2(index * 115, 0);
            merchantBtnTransform.Find("Image").GetComponent<Image>().sprite = merchantTypeSO.merchantButton;

            merchantBtnTransform.GetComponent<Button>().onClick.AddListener(() => {   
                if (merchantManager.GetActiveMerchantType() == merchantTypeSO) {
                    merchantManager.SetActiveMerchantType(null);
                    DestroyCursorMerchant(); // Hapus kursor jika merchantTypeSO diaktifkan/dinonaktifkan
                    StartCoroutine (merchantManager.ActivateIsMerchantPlaced(0.5f));
                    merchantManager.DestroyPlacementInstance();
                    
                    UIManager.Instance.ActivateUI();
                } else {
                    merchantManager.SetActiveMerchantType(merchantTypeSO);
                    SetCursor(merchantTypeSO.merchantCursor); // Atur kursor saat merchant dipilih
                    StartCoroutine (merchantManager.DeactivateIsMerchantPlaced(0.5f));
                    merchantManager.DestroyPlacementInstance();

                    UIManager.Instance.DeactivateUI();
                }
                UpdateSelectedVisual();
            });
            merchantButtonList.Add(merchantBtnTransform);

            index++;
        }

        rectTransform = GetComponent<RectTransform>();
    }

    private void Start() {
        merchantManager.OnMerchantPlaced += HandleMerchantPlaced;

        Button buttonFurnitur = GameObject.Find("ButtonFurnitur").GetComponent<Button>(); 
        buttonFurnitur.onClick.AddListener(() => {
            merchantManager.SetActiveMerchantType(null); // Set activeMerchantType ke null
            DestroyCursorMerchant(); // Hapus kursor
            UpdateSelectedVisual(); // Update visual untuk memastikan tidak ada yang selected
        });

        Button buttonSpesial = GameObject.Find("ButtonSpesial").GetComponent<Button>(); 
        buttonSpesial.onClick.AddListener(() => {
            merchantManager.SetActiveMerchantType(null); // Set activeMerchantType ke null
            DestroyCursorMerchant(); // Hapus kursor
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
        UpdateSelectedVisual();
    }

    private void HandleMerchantPlaced() {
        UpdateSelectedVisual(); // Panggil update visual setelah merchant ditempatkan
    }

    private void UpdateSelectedVisual() {
        MerchantTypeSO activeMerchantType = merchantManager.GetActiveMerchantType();

        foreach (Transform merchantBtnTransform in merchantButtonList) {
            Image image = merchantBtnTransform.Find("Image").GetComponent<Image>();
            GameObject selected = merchantBtnTransform.Find("Selected").gameObject;
            GameObject merchantWindow = merchantBtnTransform.Find("MerchantWindow").gameObject;

            if (activeMerchantType != null && image.sprite == activeMerchantType.merchantButton) {
                image.gameObject.SetActive(false);
                selected.SetActive(true);
                selected.GetComponent<Image>().sprite = activeMerchantType.selectedMerchantButton;
                merchantWindow.SetActive(true);
                merchantWindow.GetComponent<Image>().sprite = activeMerchantType.merchantWindow;
            } else {
                image.gameObject.SetActive(true);
                selected.SetActive(false);
                merchantWindow.SetActive(false);
            }
        }
    }

    private void SetCursor(GameObject merchantCursor) {
        if (cursorInstance != null) {
            Destroy(cursorInstance);
        }

        if (merchantCursor != null) {
            cursorInstance = Instantiate(merchantCursor);
        }
    }

    public void DestroyCursorMerchant() {
        if (cursorInstance != null) {
            Destroy(cursorInstance);
        }
    }
}
