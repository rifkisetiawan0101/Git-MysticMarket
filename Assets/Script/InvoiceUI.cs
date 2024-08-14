using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InvoiceUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI totalKeuanganText;
    [SerializeField] private Button buttonTutupPasar; // Referensi ke button Tutup Pasar

    private void Start() {
        // Menambahkan listener ke buttonTutupPasar untuk berpindah ke scene "InGamePagi"
        buttonTutupPasar.onClick.AddListener(() => {
            Time.timeScale = 1; // Melanjutkan waktu (jika diperlukan)
            SceneManager.LoadScene("InGamePagi");
        });
    }

    public void ShowInvoice(float totalKeuangan, int day) {
        totalKeuanganText.text = totalKeuangan.ToString("N0") + "K"; // Menampilkan total keuangan
        dayText.text = day.ToString(); // Menampilkan hanya angka hari
        gameObject.SetActive(true); // Menampilkan InvoiceUI
    }

    public void UpdateDayText(int day) {
        dayText.text = day.ToString(); // Update Day Text dengan angka hari
    }
}
