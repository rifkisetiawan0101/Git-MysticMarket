using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSayur : MonoBehaviour {
    float penampungKoinSayur = 0;
    public GameObject collectCanvas; // Tambahkan ini
    public GameObject collectButton; // Tambahkan ini

    private void Start() {
        collectCanvas.SetActive(false); // Pastikan CollectCanvas tidak aktif di awal
        collectButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnCollectButtonClick); // Tambahkan event listener
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Pocin")) {
            TambahKoin(100);
        }
    }

    private void TambahKoin(float jumlah) {
        penampungKoinSayur += jumlah;
        Debug.Log("Penampung Koin Rempah = " + penampungKoinSayur);

        if (penampungKoinSayur >= 300) {
            ActiveCollectButton();
        }
    }

    private void ActiveCollectButton() {
        collectCanvas.SetActive(true); // Aktifkan CollectCanvas
        Debug.Log("CollectCanvas diaktifkan!");
    }

    private void OnCollectButtonClick() {
        Koin.koin.updateKoin(penampungKoinSayur);
        Debug.Log("Koin di Setor!");
        penampungKoinSayur = 0;
        Debug.Log("Penampung Koin Rempah Saat Ini = " + penampungKoinSayur);
        collectCanvas.SetActive(false); // Nonaktifkan CollectCanvas
    }
}
