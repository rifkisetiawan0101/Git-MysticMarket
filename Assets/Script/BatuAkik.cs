using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BatuAkik : MonoBehaviour
{
    public static BatuAkik batuAkik;

    public float jmlBatuAkik = 1000;
    public TextMeshProUGUI batuAkikUI;

    private void Awake()
    {
        batuAkik = this;
    }

    public static event Action OnTotalBatuAkikChanged;
    public void updateBatuAkik(float amount)
    {
        jmlBatuAkik += amount;
        batuAkikUI.text = jmlBatuAkik.ToString("N0");
        Debug.Log("Batu akik saat ini " + jmlBatuAkik);
        OnTotalBatuAkikChanged?.Invoke();
    }
}