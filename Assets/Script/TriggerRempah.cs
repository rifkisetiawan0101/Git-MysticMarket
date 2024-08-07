using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRempah : MonoBehaviour {
    float penampungKoinRempah = 0;
    public GameObject collectCanvas; // Tambahkan ini
    public GameObject collectButton; // Tambahkan ini

    private void Start() {
        collectCanvas.SetActive(false); // Pastikan CollectCanvas tidak aktif di awal
        collectButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnCollectButtonClick); // Tambahkan event listener
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Kunchan")) {
            TambahKoin(250);
        }
    }

    private void TambahKoin(float jumlah) {
        penampungKoinRempah += jumlah;
        Debug.Log("Penampung Koin Rempah = " + penampungKoinRempah);

        if (penampungKoinRempah >= 750) {
            ActiveCollectButton();
        }
    }

    private void ActiveCollectButton() {
        collectCanvas.SetActive(true); // Aktifkan CollectCanvas
        Debug.Log("CollectCanvas diaktifkan!");
    }

    private void OnCollectButtonClick() {
        Koin.koin.updateKoin(penampungKoinRempah);
        Debug.Log("Koin di Setor!");
        penampungKoinRempah = 0;
        Debug.Log("Penampung Koin Rempah Saat Ini = " + penampungKoinRempah);
        collectCanvas.SetActive(false); // Nonaktifkan CollectCanvas
    }
}
