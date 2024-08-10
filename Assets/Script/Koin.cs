using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Koin : MonoBehaviour {
    public static Koin koin; 

    public float koins = 100000;
    public Text koinUI;

    private void Awake() {
        koin = this;
    }

    public void updateKoin(float amount) {
        koins += amount;
        koinUI.text = koins.ToString() + "K";
    }
}