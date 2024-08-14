using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour {
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] InvoiceUI invoiceUI; // Referensi ke script InvoiceUI melalui Inspector

    float elapsedTime;
    float totalInGameMinutes = 12 * 60; // Total waktu in-game dari 18:00 sampai 06:00 dalam menit
    float realLifeDuration = 5f * 60; // Durasi 2 menit di real life dalam detik
    int dayCounter = 1; // Hari ke berapa saat game dimainkan

    private void Start() {
        UpdateDayText(); // Menampilkan hari pertama di InvoiceUI
    }

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
            invoiceUI.ShowInvoice(Koin.koin.koins, dayCounter); // Menampilkan invoice
            Time.timeScale = 0; // Hentikan timer (pause game)
        }
    }

    public void NextDay() {
        dayCounter++;
        UpdateDayText();
    }

    private void UpdateDayText() {
        if (invoiceUI != null) {
            invoiceUI.UpdateDayText(dayCounter); // Update Day Text di InvoiceUI hanya dengan angka
        }
    }
}
