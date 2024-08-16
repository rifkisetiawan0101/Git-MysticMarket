using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerSayur : MonoBehaviour {
    float penampungKoinSayur = 0;
    float hargaSayur = 100;
    public GameObject collectButton;

    private void Start() {
        collectButton.SetActive(false);
        collectButton.GetComponent<Button>().onClick.AddListener(OnCollectButtonClick);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("NPC")) {
            penampungKoinSayur += hargaSayur;
            Debug.Log("Penampung Koin Sayur bertambah " + penampungKoinSayur);
            if (penampungKoinSayur >= 300) {
                collectButton.SetActive(true);
            }
        }
    }

    private void OnCollectButtonClick() {
        PersistentManager.Instance.UpdateKoin(penampungKoinSayur);
        Debug.Log("Koin di Setor!");
        penampungKoinSayur = 0;
        Debug.Log("Penampung Koin Sayur Saat Ini = " + penampungKoinSayur);
        collectButton.SetActive(false);
    }
}
