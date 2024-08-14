using System;
using UnityEngine;
using UnityEngine.UI;

public class PenghargaanUI : MonoBehaviour {
    [SerializeField] private GameObject penghargaanWindow;
    [SerializeField] private Button buttonPenghargaan;
    [SerializeField] private Button buttonClose;
    [SerializeField] private ShopUI shopUI;

    // Sprites untuk kondisi normal, selected, dan highlighted
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Sprite highlightedSprite;

    private bool isWindowOpen = false;

    private void Start() {
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
            buttonPenghargaan.image.sprite = selectedSprite;
            penghargaanWindow.SetActive(true);
            shopUI.CloseShopUI();
        } else {
            buttonPenghargaan.image.sprite = normalSprite;
            penghargaanWindow.SetActive(false);
            shopUI.OpenShopUI();
        }
    }

    private void ClosePenghargaanWindow() {
        isWindowOpen = false;
        penghargaanWindow.SetActive(false);

        buttonPenghargaan.image.sprite = normalSprite;
        shopUI.CloseShopUI();
    }

    public void OnHighlightButton() {
        if (!isWindowOpen) {
            buttonPenghargaan.image.sprite = highlightedSprite;
        }
    }

    public void OnUnhighlightButton() {
        if (!isWindowOpen) {
            buttonPenghargaan.image.sprite = normalSprite;
        }
    }
}