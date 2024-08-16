using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour {
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] InvoiceUI invoiceUI; // Referensi ke script InvoiceUI melalui Inspector

    private float elapsedTime;
    private float totalInGameMinutes = 12 * 60; // Total waktu in-game dari 18:00 sampai 06:00 dalam menit
    private float realLifeDuration = 1.5f * 60; // Durasi 2 menit di real life dalam detik
    public static int dayCounter = 1; // Hari ke berapa saat game dimainkan
    public bool invoiceShown = false;

    private void Update() {
        elapsedTime += Time.deltaTime;

        // Menghitung waktu in-game berdasarkan rasio real life
        float inGameMinutes = (elapsedTime / realLifeDuration) * totalInGameMinutes;

        // Menghitung jam dan menit berdasarkan waktu in-game
        int hours = 18 + Mathf.FloorToInt(inGameMinutes / 60);
        int minutes = Mathf.FloorToInt(inGameMinutes % 60);

        // Reset jam menjadi 0 saat melewati 24:00
        if (hours >= 24) {
            hours -= 24;
        }

        timer.text = string.Format("{0:00}:{1:00}", hours, minutes);

        // Jika waktu in-game mencapai 06:00 pagi, hentikan timer dan tampilkan InvoiceUI
        if (hours == 6 && minutes == 0) {
            if (!invoiceShown) {
                invoiceUI.ShowInvoice();
                Time.timeScale = 0;
            }
            invoiceShown = true;
        }
    }

    public void NextDay() {
        dayCounter++;
        PlayerPrefs.SetInt("DayCounter", dayCounter); // Simpan nilai ke PlayerPrefs
        invoiceUI.UpdateDayText();
    }
}
