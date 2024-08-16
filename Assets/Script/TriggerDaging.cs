using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerDaging : MonoBehaviour {
    float penampungKoinDaging = 0;
    float hargaDaging = 500;
    public GameObject collectButton;

    private void Start() {
        collectButton.SetActive(false);
        collectButton.GetComponent<Button>().onClick.AddListener(OnCollectButtonClick);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("NPC")) {
            penampungKoinDaging += hargaDaging;
            Debug.Log("Penampung Koin Daging bertambah " + penampungKoinDaging);
            if (penampungKoinDaging >= 1500) {
                collectButton.SetActive(true);
            }
        }
    }

    private void OnCollectButtonClick() {
        PersistentManager.Instance.UpdateKoin(penampungKoinDaging);
        Debug.Log("Koin di Setor!");
        penampungKoinDaging = 0;
        Debug.Log("Penampung Koin Daging Saat Ini = " + penampungKoinDaging);
        collectButton.SetActive(false);
    }
}
