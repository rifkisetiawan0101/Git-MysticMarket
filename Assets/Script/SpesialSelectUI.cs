using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpesialSelectUI : MonoBehaviour {
    [SerializeField] private List<SpesialTypeSO> spesialTypeSOList;
    [SerializeField] private SpesialManager spesialManager;
    
    private List<Transform> spesialButtonList;
    private RectTransform rectTransform;
    private GameObject cursorInstance; // Instance dari prefab kursor

    private void Awake() {
        Transform spesialBtnTemplate = transform.Find("SpesialBtnTemplate");
        spesialBtnTemplate.gameObject.SetActive(false);
        spesialButtonList = new List<Transform>();

        int index = 0;

        foreach (SpesialTypeSO spesialTypeSO in spesialTypeSOList) {
            Transform spesialBtnTransform = Instantiate(spesialBtnTemplate, transform);
            spesialBtnTransform.gameObject.SetActive(true);

            spesialBtnTransform.GetComponent<RectTransform>().anchoredPosition += new Vector2(index * 115, 0);
            spesialBtnTransform.Find("Image").GetComponent<Image>().sprite = spesialTypeSO.spesialButton;

            spesialBtnTransform.GetComponent<Button>().onClick.AddListener(() => {
                if (spesialManager.GetActiveSpesialType() == spesialTypeSO) {
                    spesialManager.SetActiveSpesialType(null);
                    DestroyCursorSpesial(); // Hapus kursor jika spesialTypeSO diaktifkan/dinonaktifkan
                    StartCoroutine (spesialManager.ActivateIsSpesialPlaced(0.5f));
                    spesialManager.DestroyPlacementInstance();
                } else {
                    spesialManager.SetActiveSpesialType(spesialTypeSO);
                    SetCursor(spesialTypeSO.spesialCursor); // Atur kursor saat spesial dipilih
                    StartCoroutine (spesialManager.DeactivateIsSpesialPlaced(0.5f));
                    spesialManager.DestroyPlacementInstance();
                }
                UpdateSelectedVisual();
            });
            spesialButtonList.Add(spesialBtnTransform);

            index++;
        }

        rectTransform = GetComponent<RectTransform>();
    }

    private void Start() {
        spesialManager.OnSpesialPlaced += HandleSpesialPlaced;

        Button buttonMerchant = GameObject.Find("ButtonMerchant").GetComponent<Button>(); 
        buttonMerchant.onClick.AddListener(() => {
            spesialManager.SetActiveSpesialType(null); // Set activeSpesialType ke null
            DestroyCursorSpesial(); // Hapus kursor
            UpdateSelectedVisual(); // Update visual untuk memastikan tidak ada yang selected
        });

        Button buttonFurnitur = GameObject.Find("ButtonFurnitur").GetComponent<Button>(); 
        buttonFurnitur.onClick.AddListener(() => {
            spesialManager.SetActiveSpesialType(null); // Set activeSpesialType ke null
            DestroyCursorSpesial(); // Hapus kursor
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

    private void HandleSpesialPlaced() {
        UpdateSelectedVisual(); // Panggil update visual setelah spesial ditempatkan
    }

    private void OnDestroy() {
        spesialManager.OnSpesialPlaced -= HandleSpesialPlaced; // Unregister event listener
    }

    private void UpdateSelectedVisual() {
        SpesialTypeSO activeSpesialType = spesialManager.GetActiveSpesialType();

        foreach (Transform spesialBtnTransform in spesialButtonList) {
            Image image = spesialBtnTransform.Find("Image").GetComponent<Image>();
            GameObject selected = spesialBtnTransform.Find("Selected").gameObject;
            GameObject spesialWindow = spesialBtnTransform.Find("SpesialWindow").gameObject;

            if (activeSpesialType != null && image.sprite == activeSpesialType.spesialButton) {
                image.gameObject.SetActive(false);
                selected.SetActive(true);
                selected.GetComponent<Image>().sprite = activeSpesialType.selectedSpesialButton;
                spesialWindow.SetActive(true);
                spesialWindow.GetComponent<Image>().sprite = activeSpesialType.spesialWindow;
            } else {
                image.gameObject.SetActive(true);
                selected.SetActive(false);
                spesialWindow.SetActive(false);
            }
        }
    }
    
    private void SetCursor(GameObject spesialCursor) {
        if (cursorInstance != null) {
            Destroy(cursorInstance);
        }

        if (spesialCursor != null) {
            cursorInstance = Instantiate(spesialCursor);
        }
    }

    public void DestroyCursorSpesial() {
        if (cursorInstance != null) {
            Destroy(cursorInstance);
        }
    }
}
