using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerPagi : MonoBehaviour {
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] BukaPasarUI bukaPasarUI;

    private float elapsedTime = 0f; // Waktu yang berlalu dalam real time
    private float totalInGameMinutes = 12 * 60; // Total waktu in-game dari 06:00 sampai 18:00 dalam menit
    private float realLifeDuration = 1.5f * 60; // Durasi 2 menit di real life dalam detik
    public bool bukaPasarShown = false;
    [SerializeField] private GameObject mobilBakPrefab;
    [SerializeField] private Transform spawnPoint; // Tempat spawn MobilBak
    [SerializeField] private Transform targetPoint;
    private GameObject mobilBakInstance;

    private void Update() {
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
        if (hours == 18 && minutes == 0) {
            if (!bukaPasarShown) {
                bukaPasarUI.ShowBukaPasarUI();
                Time.timeScale = 0;
            }
            bukaPasarShown = true;
        }
    }

    // public GameObject GetMobilBakInstance()
    // {
    //     return mobilBakInstance;
    // }
}
