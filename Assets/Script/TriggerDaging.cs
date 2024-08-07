using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDaging : MonoBehaviour {
    float penampungKoinDaging = 0;
    public GameObject collectCanvas; // Tambahkan ini
    public GameObject collectButton; // Tambahkan ini

    private void Start() {
        collectCanvas.SetActive(false); // Pastikan CollectCanvas tidak aktif di awal
        collectButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnCollectButtonClick); // Tambahkan event listener
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Ayang")) {
            TambahKoin(500);
        }
    }

    private void TambahKoin(float jumlah) {
        penampungKoinDaging += jumlah;
        Debug.Log("Penampung Koin Rempah = " + penampungKoinDaging);

        if (penampungKoinDaging >= 1500) {
            ActiveCollectButton();
        }
    }

    private void ActiveCollectButton() {
        collectCanvas.SetActive(true); // Aktifkan CollectCanvas
        Debug.Log("CollectCanvas diaktifkan!");
    }

    private void OnCollectButtonClick() {
        Koin.koin.updateKoin(penampungKoinDaging);
        Debug.Log("Koin di Setor!");
        penampungKoinDaging = 0;
        Debug.Log("Penampung Koin Rempah Saat Ini = " + penampungKoinDaging);
        collectCanvas.SetActive(false); // Nonaktifkan CollectCanvas
    }
}
