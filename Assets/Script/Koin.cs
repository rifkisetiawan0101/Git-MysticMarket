using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Koin : MonoBehaviour {
    public static Koin koin; 

    public float koins = 100000;
    public TextMeshProUGUI koinUI;

    private void Awake() {
        koin = this;
    }

    public static event Action OnTotalKoinChanged;
    public void updateKoin(float amount) {
        koins += amount;
        koinUI.text = koins.ToString("N0") + "K";
        Debug.Log ("Koin saat ini " + koins);
        OnTotalKoinChanged?.Invoke();
    }
}