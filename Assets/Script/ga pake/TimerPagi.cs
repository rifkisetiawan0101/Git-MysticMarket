using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerPagi : MonoBehaviour {
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] BukaPasarUI bukaPasarUI;

    private float elapsedTime = 0f; // Waktu yang berlalu dalam real time
    private float totalInGameMinutes = 12 * 60; // Total waktu 12 jam in-game
    private float realLifeDuration = 0.1f * 60; // Durasi 2 menit di real life dalam detik
    public bool bukaPasarShown = false;

    private void Update() {
        // PersistentManager.isInvoiceShown = false;
        elapsedTime += Time.deltaTime;

        // Menghitung waktu in-game berdasarkan rasio real life
        float inGameMinutes = (elapsedTime / realLifeDuration) * totalInGameMinutes;

        // Menghitung jam dan menit berdasarkan waktu in-game
        int hours = 6 + Mathf.FloorToInt(inGameMinutes / 60);
        int minutes = Mathf.FloorToInt(inGameMinutes % 60);

        // Reset jam menjadi 0 saat melewati 24:00
        if (hours >= 24) {
            hours -= 24;
        }

        timer.text = string.Format("{0:00}:{1:00}", hours, minutes);

        // Jika waktu in-game mencapai 18:00, hentikan timer dan tampilkan BukaPasarUI
        // if (hours == 18 && minutes == 0) {
        //     if (PersistentManager.isBukaPasarShown == false) {
        //         bukaPasarUI.ShowBukaPasarUI();
        //         PersistentManager.isBukaPasarShown = true;
        //     }
        // }
    }
}
