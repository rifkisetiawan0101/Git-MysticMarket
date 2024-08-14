using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PenghargaanManager : MonoBehaviour {
    [Header("----- ALL -----")]
    [SerializeField] private Sprite ceklisOn;

    [Header("----- Achievment 1 -----")]
    [SerializeField] private Image ceklis_1;
    [SerializeField] private GameObject buttonCollectPenghargaan_1;
    [SerializeField] private GameObject dateTextGO_1;

    private void OnEnable() {
        MerchantManager.OnTotalMerchantChanged += PenghargaanDuaToko;
        Koin.OnTotalKoinChanged += PenghargaanKoin2000K;
    }

    private void OnDisable() {
        MerchantManager.OnTotalMerchantChanged -= PenghargaanDuaToko;
        Koin.OnTotalKoinChanged -= PenghargaanKoin2000K;
    }
    private void PenghargaanDuaToko() {
        if (MerchantManager.totalMerchant == 2) {
            buttonCollectPenghargaan_1.SetActive(true);

            buttonCollectPenghargaan_1.GetComponent<Button>().onClick.AddListener(() => {
                buttonCollectPenghargaan_1.SetActive(false);
                dateTextGO_1.SetActive(true);
                TextMeshProUGUI dateText = dateTextGO_1.GetComponent<TextMeshProUGUI>();
                dateText.text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                ceklis_1.sprite = ceklisOn;
                Koin.koin.updateKoin(150f);
            });
        }
    }

    [Header("----- Achievment 2 -----")]
    [SerializeField] private Image ceklis_2;
    [SerializeField] private GameObject buttonCollectPenghargaan_2;
    [SerializeField] private GameObject dateTextGO_2;

    private void PenghargaanEmpatToko() {

    }

    [Header("----- Achievment 3 -----")]
    [SerializeField] private Image ceklis_3;
    [SerializeField] private GameObject buttonCollectPenghargaan_3;
    [SerializeField] private GameObject dateTextGO_3;

    private void PenghargaanSemuaJenisToko() {
    
    }

    [Header("----- Achievment 4 -----")]
    [SerializeField] private Image ceklis_4;
    [SerializeField] private GameObject buttonCollectPenghargaan_4;
    [SerializeField] private GameObject dateTextGO_4;

    private void PenghargaanKoin2000K(){
        if (Koin.koin.koins >= 2000) {
            buttonCollectPenghargaan_4.SetActive(true);

            buttonCollectPenghargaan_4.GetComponent<Button>().onClick.AddListener(() => {
                buttonCollectPenghargaan_4.SetActive(false);
                dateTextGO_4.SetActive(true);
                TextMeshProUGUI dateText = dateTextGO_4.GetComponent<TextMeshProUGUI>();
                dateText.text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                ceklis_4.sprite = ceklisOn;
                Koin.koin.updateKoin(3333f);
            });
        }
    }
}