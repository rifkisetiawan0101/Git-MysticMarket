using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PremanButoAttackTrigger : MonoBehaviour
{
    private GameObject serangButoButton;
    private GameObject collectDropItemButton;
    private PremanButoAI premanButoAI;

    public bool isPlayerEnterTrigger = false;

    void Start()
    {
        // Cari GameObject dengan nama "SerangButoButton" di dalam scene
        serangButoButton = GameObject.Find("SerangButoButton");
        collectDropItemButton = GameObject.Find("CollectDropItemButton");
        premanButoAI = GetComponentInParent<PremanButoAI>(); // Referensi ke PremanButoAI

        if (serangButoButton != null)
        {
            serangButoButton.SetActive(false); // Awalnya dimatikan
            collectDropItemButton.SetActive(false);
            premanButoAI.overlay.SetActive(false);
        }
        Debug.Log("isPlayerEnterTrigger: " + isPlayerEnterTrigger);
    }

    // Fungsi untuk mengaktifkan tombol serang
    public void ActivateSerangButton()
    {
        if (serangButoButton != null && premanButoAI.buttoHealth > 0 && premanButoAI.buttoHealth != premanButoAI.maxHealth && PersistentManager.Instance.isUIOpen == false)
        {
            serangButoButton.SetActive(true);
            premanButoAI.overlay.gameObject.SetActive(true);
            AudioManager.audioManager.ChangeMusic(AudioManager.audioManager.battleBacksound, 0.5f);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Mengecek jika yang menyentuh collider ini adalah player
        if (collision.gameObject.CompareTag("Player"))
        {
            if (premanButoAI.buttoHealth > 0)
            {
                isPlayerEnterTrigger = true;
                premanButoAI.StopMovement();
            }

            if (premanButoAI.isPremanArrived || !premanButoAI.isPremanArrived) // Cek apakah Buto sudah sampai di target atau belum
            {
                ActivateSerangButton(); // Aktifkan tombol serang
                // premanButoAI.StartIncreasingHealth(); // Mulai penambahan health
            }

            premanButoAI.CheckAndActivateCollectButton();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // Mengecek jika yang meninggalkan collider ini adalah player
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerEnterTrigger = false;
            serangButoButton.SetActive(false); // Menonaktifkan GameObject SerangButoButton
            premanButoAI.overlay.gameObject.SetActive(false);

            if (premanButoAI.buttoHealth <= 0 && premanButoAI.premanButoDropItem.activeSelf)
            {
                Debug.Log("OnTriggerExit2D: Menonaktifkan tombol collect");
                premanButoAI.collectDropItemButton.SetActive(false);
            }
        }
    }
}