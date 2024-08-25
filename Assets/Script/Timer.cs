using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour {
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] InvoiceUI invoiceUI;
    [SerializeField] NotifUI notifUI;

    public static float elapsedTime = 0;
    private float totalInGameMinutes = 12 * 60; // Total waktu 12 jam in-game
    private float realLifeDuration = 3f * 60; // Durasi menit di real life dalam detik
    
    public int hours;
    public int minutes;
    
    private void Update() {
        elapsedTime += Time.deltaTime;

        // Menghitung waktu in-game berdasarkan rasio real life
        float inGameMinutes = (elapsedTime / realLifeDuration) * totalInGameMinutes;

        // Menghitung jam dan menit berdasarkan waktu in-game
        hours = 18 + Mathf.FloorToInt(inGameMinutes / 60);
        minutes = Mathf.FloorToInt(inGameMinutes % 60);

        // Reset jam menjadi 0 saat melewati 24:00
        if (hours >= 24) {
            hours -= 24;
        }

        timer.text = string.Format("{0:00}:{1:00}", hours, minutes);

        if (hours == 20 && minutes == 52) {
            StartCoroutine(notifUI.PlayNotifUto());
        }

        if (hours == 0 && minutes == 12) {
            StartCoroutine(notifUI.PlayNotifUto());
        }

        if (hours == 3 && minutes == 32) {
            StartCoroutine(notifUI.PlayNotifUto());
        }

        if (hours == 5 && minutes == 32) {
            StartCoroutine(notifUI.PlayNotifMalam());
        }

        if (hours == 6 && minutes == 0) {
            if (PersistentManager.Instance.isInvoiceShown == false) {
                invoiceUI.ShowInvoice();
                PersistentManager.Instance.isInvoiceShown = true;
            }
        }
    }
}
