using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerRempah : MonoBehaviour {
    float penampungKoinRempah = 0;
    float hargaRempah = 200;
    public GameObject collectButton;

    private void Start() {
        collectButton.SetActive(false);
        collectButton.GetComponent<Button>().onClick.AddListener(OnCollectButtonClick);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("NPC")) {
            penampungKoinRempah += hargaRempah;
            Debug.Log("Penampung Koin Rempah bertambah " + penampungKoinRempah);
            if (penampungKoinRempah >= 750) {
                collectButton.SetActive(true);
            }
        }
    }

    private void OnCollectButtonClick() {
        PersistentManager.Instance.UpdateKoin(penampungKoinRempah);
        Debug.Log("Koin di Setor!");
        penampungKoinRempah = 0;
        Debug.Log("Penampung Koin Rempah Saat Ini = " + penampungKoinRempah);
        collectButton.SetActive(false);
    }
}
