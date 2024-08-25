using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class BukaPasarUI : MonoBehaviour {
    [SerializeField] private GameObject bukaPasarWindow;
    [SerializeField] private Button buttonOpenWindow;
    [SerializeField] private Button buttonMenujuMalam;
    [SerializeField] private GameObject overlay;

    private void Start() {
        overlay.SetActive(false);
        bukaPasarWindow.SetActive(false);

        buttonOpenWindow.onClick.AddListener(() => {
            bukaPasarWindow.SetActive(true);
            overlay.SetActive(true);
        });

        buttonMenujuMalam.onClick.AddListener(() => {
            overlay.SetActive(false);
            SceneManager.LoadScene("InGame");
            PersistentManager.Instance.UpdateNightCounter(1);
            PersistentManager.Instance.isNowMalam = true;
            PersistentManager.Instance.dataTotalSpawnNpc = 0;
        });
    }

    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite highlightedSprite;

    public void OnHighlightButton() {
        buttonOpenWindow.image.sprite = highlightedSprite;
    }

    public void OnUnhighlightButton() {
        buttonOpenWindow.image.sprite = normalSprite;
    }
}