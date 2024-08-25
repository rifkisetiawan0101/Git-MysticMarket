using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class InvoiceUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI totalKeuanganText;
    [SerializeField] private TextMeshProUGUI jumlahPedagangText;
    [SerializeField] private TextMeshProUGUI setoranPedagangText;
    [SerializeField] private Button buttonMenujuPagi; // Referensi ke button Tutup Pasar

    private void Start() {
        buttonMenujuPagi.onClick.AddListener(OnMenujuPagiClicked);
    }

    public void OnMenujuPagiClicked() {
        Time.timeScale = 1;
        SceneManager.LoadScene("InGamePagi");
        PersistentManager.Instance.UpdateDayCounter(1);
        PersistentManager.Instance.isNowMalam = false;
        gameObject.SetActive(false);
    }

    public void ShowInvoice() {
        PersistentManager.Instance.isUIOpen = true;

        Time.timeScale = 0;

        gameObject.SetActive(true); // Menampilkan InvoiceUI

        float nilaiSetoran = PersistentManager.Instance.dataTotalMerchant * 100;
        setoranPedagangText.text = nilaiSetoran.ToString("N0") + "K";

        dayText.text = PersistentManager.Instance.nightCounter.ToString();
        totalKeuanganText.text = (PersistentManager.Instance.dataKoin + nilaiSetoran).ToString("N0") + "K"; 
        jumlahPedagangText.text = PersistentManager.Instance.dataTotalMerchant.ToString();

        if (PersistentManager.Instance.isInvoiceShown == false) {
            PersistentManager.Instance.UpdateKoin(nilaiSetoran);
        }
    }
    
    [SerializeField] private Image buttonMenujuPagiImage;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite highlightedSprite;

    public void OnHighlightButton() {
        buttonMenujuPagiImage.sprite = highlightedSprite;
    }

    public void OnUnhighlightButton() {
        buttonMenujuPagiImage.sprite = normalSprite;
    }
}

