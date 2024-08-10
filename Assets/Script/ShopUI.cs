using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour {
    [SerializeField] private Transform player;
    [SerializeField] private Vector2 targetPosition;
    [SerializeField] private float smoothingSpeed;

    private Vector2 initialPosition;
    private bool isMoving = false;
    private RectTransform rectTransform;
    
    [SerializeField] private GameObject merchantSelectUIObject;
    [SerializeField] private GameObject hiasanSelectUIObject;

    private MerchantSelectUI merchantSelectUI;
    private HiasanSelectUI hiasanSelectUI;

    [SerializeField] private Button buttonMerchant;
    [SerializeField] private Button buttonHiasan;

    [SerializeField] private Sprite merchantSelectedSprite;
    [SerializeField] private Sprite merchantNormalSprite;
    [SerializeField] private Sprite hiasanSelectedSprite;
    [SerializeField] private Sprite hiasanNormalSprite;

    private void Start() {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null) {
            initialPosition = rectTransform.anchoredPosition; 
        }

        // Ambil komponen MerchantSelectUI dan HiasanSelectUI dari GameObject terkait
        merchantSelectUI = merchantSelectUIObject.GetComponent<MerchantSelectUI>();
        hiasanSelectUI = hiasanSelectUIObject.GetComponent<HiasanSelectUI>();

        ShowMerchantUI();
        
        buttonMerchant.onClick.AddListener(ShowMerchantUI);
        buttonHiasan.onClick.AddListener(ShowHiasanUI);
    }

    private void Update() {
        if (player != null && (player.GetComponent<Rigidbody2D>().velocity.magnitude > 0)) {
            isMoving = true;
        }
        else {
            isMoving = false;
        }

        if (isMoving) {
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPosition, smoothingSpeed * Time.deltaTime);
        }
        else {
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, initialPosition, smoothingSpeed * Time.deltaTime);
        }
    }

    private void ShowMerchantUI() {
        merchantSelectUIObject.SetActive(true);
        hiasanSelectUIObject.SetActive(false);
        
        buttonMerchant.image.sprite = merchantSelectedSprite;
        buttonHiasan.image.sprite = hiasanNormalSprite;

        hiasanSelectUI.DestroyCursorHiasan(); // Panggil method melalui komponen HiasanSelectUI
    }

    private void ShowHiasanUI() {
        merchantSelectUIObject.SetActive(false);
        hiasanSelectUIObject.SetActive(true);
        
        buttonMerchant.image.sprite = merchantNormalSprite;
        buttonHiasan.image.sprite = hiasanSelectedSprite;

        merchantSelectUI.DestroyCursorMerchant(); // Panggil method melalui komponen MerchantSelectUI
    }
}
