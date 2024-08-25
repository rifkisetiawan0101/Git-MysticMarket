using System;
using UnityEngine;
using UnityEngine.UI;

public class PenghargaanUI : MonoBehaviour {
    [SerializeField] private GameObject penghargaanWindow;
    [SerializeField] private Button buttonPenghargaan;
    [SerializeField] private Button buttonClose;
    [SerializeField] private ShopUI shopUI;

    [SerializeField] private GameObject overlay;

    // Sprites untuk kondisi normal, selected, dan highlighted
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Sprite highlightedSprite;

    private bool isWindowOpen = false;

    private void Start() {
        overlay.SetActive(false);
        shopUI = FindObjectOfType<ShopUI>();

        buttonPenghargaan.onClick.AddListener(TogglePenghargaanWindow);
        buttonClose.onClick.AddListener(ClosePenghargaanWindow);

        // Set sprite awal ke normal
        buttonPenghargaan.image.sprite = normalSprite;
    }

    private void TogglePenghargaanWindow() {
        isWindowOpen = !isWindowOpen;

        // Mengatur sprite berdasarkan kondisi
        if (isWindowOpen) {
            PersistentManager.Instance.isUIOpen = true;
            buttonPenghargaan.image.sprite = selectedSprite;
            penghargaanWindow.SetActive(true);
            
            overlay.SetActive(true);
            shopUI.CloseShopUI();

            FindObjectOfType<PlayerMovementNew>().StopPlayer();
        } else {
            PersistentManager.Instance.isUIOpen = false;
            buttonPenghargaan.image.sprite = normalSprite;
            penghargaanWindow.SetActive(false);

            overlay.SetActive(false);
            shopUI.OpenShopUI();
        }
    }

    private void ClosePenghargaanWindow() {
        PersistentManager.Instance.isUIOpen = false;
        isWindowOpen = false;
        penghargaanWindow.SetActive(false);

        buttonPenghargaan.image.sprite = normalSprite;
        overlay.SetActive(false);
        shopUI.CloseShopUI();
    }

    public void OnHighlightButton() {
        buttonPenghargaan.image.sprite = highlightedSprite;
    }

    public void OnUnhighlightButton() {
        buttonPenghargaan.image.sprite = normalSprite;
    }
}