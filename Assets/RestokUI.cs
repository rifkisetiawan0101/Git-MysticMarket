using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestokUI : MonoBehaviour {
    [SerializeField] private Button buttonBeliSayur;
    [SerializeField] private Button buttonBeliRempah;
    [SerializeField] private Button buttonBeliDaging;
    [SerializeField] private Button buttonCloseRestok;
    [SerializeField] private GameObject windowRestok;
    [SerializeField] private GameObject overlay;

    private void Start() {
        overlay.SetActive(false);
        buttonBeliSayur.GetComponent<Button>().onClick.AddListener(() => {
            if (PersistentManager.Instance.dataKoin > 10) {
                PersistentManager.Instance.dataKoin -= 10;
                PersistentManager.Instance.dataStokSayur++;
            }
        });

        buttonBeliRempah.GetComponent<Button>().onClick.AddListener(() => {
            if (PersistentManager.Instance.dataKoin > 25) {
                PersistentManager.Instance.dataKoin -= 25;
                PersistentManager.Instance.dataStokRempah++;
            }
        });

        buttonBeliDaging.GetComponent<Button>().onClick.AddListener(() => {
            if (PersistentManager.Instance.dataKoin > 50) {
                PersistentManager.Instance.dataKoin -= 50;
                PersistentManager.Instance.dataStokDaging++;
            }
        });

        buttonCloseRestok.GetComponent<Button>().onClick.AddListener(() => {
            windowRestok.SetActive(false);
            overlay.SetActive(false);
        });
    }

    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite highlightedSprite;

    public void OnHighlightSayur() {
        buttonBeliSayur.image.sprite = highlightedSprite;
    }

    public void OnUnHighlightSayur() {
        buttonBeliSayur.image.sprite = normalSprite;
    }

    public void OnHighlightRempah() {
        buttonBeliRempah.image.sprite = highlightedSprite;
    }

    public void OnUnHighlightRempah() {
        buttonBeliRempah.image.sprite = normalSprite;
    }

    public void OnHighlightDaging() {
        buttonBeliDaging.image.sprite = highlightedSprite;
    }

    public void OnUnHighlightDaging() {
        buttonBeliDaging.image.sprite = normalSprite;
    }
}
