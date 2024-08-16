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
    [SerializeField] private Timer timer;

    private void Start() {
        buttonMenujuPagi.onClick.AddListener(() => {
            Time.timeScale = 1;
            SceneManager.LoadScene("InGamePagi");
            timer.NextDay();
            gameObject.SetActive(false);
        });
    }

    public void ShowInvoice() {
        dayText.text = Timer.dayCounter.ToString();
        totalKeuanganText.text = PersistentManager.Instance.Koins.ToString("N0") + "K"; 
        jumlahPedagangText.text = MerchantManager.totalMerchant.ToString();

        int nilaiSetoran = MerchantManager.totalMerchant * 100;
        setoranPedagangText.text = nilaiSetoran.ToString("N0") + "K";

        if (!timer.invoiceShown) {
            PersistentManager.Instance.UpdateKoin(nilaiSetoran);
        }

        gameObject.SetActive(true); // Menampilkan InvoiceUI
    }
    
    public void UpdateDayText() {
        dayText.text = Timer.dayCounter.ToString(); // Update Day Text dengan angka hari
    }
}

