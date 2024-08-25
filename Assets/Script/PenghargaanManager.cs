using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PenghargaanManager : MonoBehaviour {
    [SerializeField] private SpesialSelectUI spesialSelectUI;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Jangan hancurkan saat berpindah scene
        } else {
            Destroy(gameObject); // Hancurkan jika instance sudah ada
        }
    }
    
    private void OnEnable() { // JANGAN PERNAH DIUBAH KE UPDATE
        PersistentManager.OnTotalMerchantChanged += AwalEkonomi;
        PersistentManager.OnTotalMerchantChanged += ProyekBesar;
    
        MerchantManager.OnJuraganPasar += JuraganPasar;
        
        PersistentManager.OnTotalKoinChanged += CuanDiHutan;

        PersistentManager.OnDayCounterChanged += HinggaTerbitFajar;

        PersistentManager.OnUtoDefeated += AkuDukun;

        PersistentManager.OnTotalCollectableChanged += PengepulAlam;
        PersistentManager.OnTotalCollectableChanged += KuliSakti;

        PersistentManager.OnNightCounterChanged += MalamKliwon;

        PersistentManager.OnTotalKoinChanged += PebisnisGhaib;
    }

    public static PenghargaanManager Instance { get; private set; }

    [Header("--- 1. Memiliki 2 pedagang pertama ---")]
    [SerializeField] private GameObject buttonCollectPenghargaan_1;
    [SerializeField] private GameObject dateTextGO_1;
    [SerializeField] private GameObject notif_1;
    

    private void AwalEkonomi() {
        if (PersistentManager.Instance.dataTotalMerchant == 2) {
            if (PersistentManager.Instance.isAwalEkonomiPlayed == false) {
                StartCoroutine(PlayNotif_1());
            }
            PersistentManager.Instance.isAwalEkonomiPlayed = true;

            buttonCollectPenghargaan_1.SetActive(true);
            buttonCollectPenghargaan_1.GetComponent<Button>().onClick.AddListener(() => {
                buttonCollectPenghargaan_1.SetActive(false);
                dateTextGO_1.SetActive(true);
                TextMeshProUGUI dateText = dateTextGO_1.GetComponent<TextMeshProUGUI>();
                dateText.text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                
                PersistentManager.Instance.UpdateKoin(200f);
                PersistentManager.Instance.isAwalEkonomiClaimed = true;

                spesialSelectUI.terkunciButtonList[0].gameObject.SetActive(false);
            });

            if (PersistentManager.Instance.isAwalEkonomiClaimed == true) {
                buttonCollectPenghargaan_1.SetActive(false);
            } else {
                Debug.LogError("List terkunciButtonList kosong atau tidak memiliki elemen di index 0!");
            }
        }
    }

    private IEnumerator PlayNotif_1() {
        notif_1.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        notif_1.SetActive(false);
    }

    [Header("--- 2. Memiliki 4 pedagang pertama ---")]
    // [SerializeField] private Image ceklis_2;
    [SerializeField] private GameObject buttonCollectPenghargaan_2;
    [SerializeField] private GameObject dateTextGO_2;
    [SerializeField] private GameObject notif_2;
    

    private void ProyekBesar() {
        if (PersistentManager.Instance.dataTotalMerchant == 4) {
            if (PersistentManager.Instance.isProyekBesarPlayed == false) {
                StartCoroutine(PlayNotif_2());
            }
            PersistentManager.Instance.isProyekBesarPlayed = true;

            buttonCollectPenghargaan_2.SetActive(true);
            buttonCollectPenghargaan_2.GetComponent<Button>().onClick.AddListener(() => {
                buttonCollectPenghargaan_2.SetActive(false);
                dateTextGO_2.SetActive(true);
                TextMeshProUGUI dateText = dateTextGO_2.GetComponent<TextMeshProUGUI>();
                dateText.text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                
                PersistentManager.Instance.UpdateKoin(400f);
                PersistentManager.Instance.isProyekBesarClaimed = true;

                spesialSelectUI.terkunciButtonList[1].gameObject.SetActive(false);
            });

            if (PersistentManager.Instance.isProyekBesarClaimed == true) {
                buttonCollectPenghargaan_2.SetActive(false);
            }
        }
    }

    private IEnumerator PlayNotif_2() {
        notif_2.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        notif_2.SetActive(false);
    }

    [Header("--- 3. Memiliki semua jenis pedagang ---")]
    [SerializeField] private GameObject buttonCollectPenghargaan_3;
    [SerializeField] private GameObject dateTextGO_3;
    [SerializeField] private GameObject notif_3;
    

    private void JuraganPasar() {
        if (PersistentManager.Instance.isSayurPlaced && PersistentManager.Instance.isRempahPlaced && PersistentManager.Instance.isDagingPlaced) {
            if (PersistentManager.Instance.isJuraganPasarPlayed == false) {
                StartCoroutine(PlayNotif_3());
            }
            PersistentManager.Instance.isJuraganPasarPlayed = true;

            buttonCollectPenghargaan_3.SetActive(true);
            buttonCollectPenghargaan_3.GetComponent<Button>().onClick.AddListener(() => {
                buttonCollectPenghargaan_3.SetActive(false);
                dateTextGO_3.SetActive(true);
                TextMeshProUGUI dateText = dateTextGO_3.GetComponent<TextMeshProUGUI>();
                dateText.text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                PersistentManager.Instance.UpdateKoin(500f);
                PersistentManager.Instance.isJuraganPasarClaimed = true;
            });

            if (PersistentManager.Instance.isJuraganPasarClaimed == true) {
                buttonCollectPenghargaan_3.SetActive(false);
            }
        }
    }

    private IEnumerator PlayNotif_3() {
        notif_3.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        notif_3.SetActive(false);
    }

    [Header("--- 4. Memiliki uang senilai 2000k ---")]
    // [SerializeField] private Image ceklis_4;
    [SerializeField] private GameObject buttonCollectPenghargaan_4;
    [SerializeField] private GameObject dateTextGO_4;
    [SerializeField] private GameObject notif_4;
    

    private void CuanDiHutan() {
        if (PersistentManager.Instance.dataKoin >= 2000) {
            if (PersistentManager.Instance.isCuanDiHutanPlayed == false) {
                StartCoroutine(PlayNotif_4());
            }
            PersistentManager.Instance.isCuanDiHutanPlayed = true;

            buttonCollectPenghargaan_4.SetActive(true);
            buttonCollectPenghargaan_4.GetComponent<Button>().onClick.AddListener(() => {
                buttonCollectPenghargaan_4.SetActive(false);
                dateTextGO_4.SetActive(true);
                TextMeshProUGUI dateText = dateTextGO_4.GetComponent<TextMeshProUGUI>();
                dateText.text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                PersistentManager.Instance.UpdateKoin(500f);
                PersistentManager.Instance.isCuanDiHutanClaimed = true;
            });

            if (PersistentManager.Instance.isCuanDiHutanClaimed == true) {
                buttonCollectPenghargaan_4.SetActive(false);
            }
        }
    }
    
    private IEnumerator PlayNotif_4() {
        notif_4.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        notif_4.SetActive(false);
    }

    [Header("--- 5. Memasuki pagi hari pertama ---")]
    [SerializeField] private GameObject buttonCollectPenghargaan_5;
    [SerializeField] private GameObject dateTextGO_5;
    [SerializeField] private GameObject notif_5;

    private void HinggaTerbitFajar() {
        if (PersistentManager.Instance.dayCounter == 1) {
            if (PersistentManager.Instance.isHinggaTerbitFajarPlayed == false) {
                StartCoroutine(PlayNotif_5());
            }
            PersistentManager.Instance.isHinggaTerbitFajarPlayed = true;

            buttonCollectPenghargaan_5.SetActive(true);
            buttonCollectPenghargaan_5.GetComponent<Button>().onClick.AddListener(() => {
                buttonCollectPenghargaan_5.SetActive(false);
                dateTextGO_5.SetActive(true);
                TextMeshProUGUI dateText = dateTextGO_5.GetComponent<TextMeshProUGUI>();
                dateText.text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                PersistentManager.Instance.UpdateKoin(400f);
                PersistentManager.Instance.isHinggaTerbitFajarClaimed = true;
            });

            if (PersistentManager.Instance.isHinggaTerbitFajarClaimed == true) {
                buttonCollectPenghargaan_5.SetActive(false);
            }
        }
    }

    private IEnumerator PlayNotif_5() {
        notif_5.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        notif_5.SetActive(false);
    }

    [Header("--- 6. Mengalahkan Uto Ijo pertama kali ---")]
    [SerializeField] private GameObject buttonCollectPenghargaan_6;
    [SerializeField] private GameObject dateTextGO_6;
    [SerializeField] private GameObject notif_6;
    
    private void AkuDukun() {
        if (PersistentManager.Instance.isUtoDefeated == true) {
            if (PersistentManager.Instance.isAkuDukunPlayed == false) {
                StartCoroutine(PlayNotif_6());
            }
            PersistentManager.Instance.isAkuDukunPlayed = true;

            buttonCollectPenghargaan_6.SetActive(true);
            buttonCollectPenghargaan_6.GetComponent<Button>().onClick.AddListener(() => {
                buttonCollectPenghargaan_6.SetActive(false);
                dateTextGO_6.SetActive(true);
                TextMeshProUGUI dateText = dateTextGO_6.GetComponent<TextMeshProUGUI>();
                dateText.text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                PersistentManager.Instance.UpdateKoin(500f);
                PersistentManager.Instance.isAkuDukunClaimed = true;
                spesialSelectUI.terkunciButtonList[4].gameObject.SetActive(false);
            });

            if (PersistentManager.Instance.isAkuDukunClaimed == true) {
                buttonCollectPenghargaan_6.SetActive(false);
            }
        }
    }

    private IEnumerator PlayNotif_6() {
        notif_6.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        notif_6.SetActive(false);
    }

    [Header("--- 7. Mengoleksi bahan pertama kali ---")]
    [SerializeField] private GameObject buttonCollectPenghargaan_7;
    [SerializeField] private GameObject dateTextGO_7;
    [SerializeField] private GameObject notif_7;
    
    private void PengepulAlam(string amount, float namaCollectable) {
        if (PersistentManager.Instance.isBatuCollected || PersistentManager.Instance.isKayuCollected || PersistentManager.Instance.isTanahLiatCollected) {
            if (PersistentManager.Instance.isPengepulAlamPlayed == false) {
                StartCoroutine(PlayNotif_7());
            }
            PersistentManager.Instance.isPengepulAlamPlayed = true;

            buttonCollectPenghargaan_7.SetActive(true);
            buttonCollectPenghargaan_7.GetComponent<Button>().onClick.AddListener(() => {
                buttonCollectPenghargaan_7.SetActive(false);
                dateTextGO_7.SetActive(true);
                TextMeshProUGUI dateText = dateTextGO_7.GetComponent<TextMeshProUGUI>();
                dateText.text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                PersistentManager.Instance.UpdateKoin(500f);
                PersistentManager.Instance.isPengepulAlamClaimed = true;
            });

            if (PersistentManager.Instance.isPengepulAlamClaimed == true) {
                buttonCollectPenghargaan_7.SetActive(false);
            }
        }
    }

    private IEnumerator PlayNotif_7() {
        notif_7.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        notif_7.SetActive(false);
    }

    [Header("--- 8. Mengoleksi semua bahan ---")]
    [SerializeField] private GameObject buttonCollectPenghargaan_8;
    [SerializeField] private GameObject dateTextGO_8;
    [SerializeField] private GameObject notif_8;
    
    private void KuliSakti(string amount, float namaCollectable) {
        if (PersistentManager.Instance.isBatuCollected && PersistentManager.Instance.isKayuCollected && PersistentManager.Instance.isTanahLiatCollected) {
            if (PersistentManager.Instance.isKuliSaktiPlayed == false) {
                StartCoroutine(PlayNotif_8());
            }
            PersistentManager.Instance.isKuliSaktiPlayed = true;

            buttonCollectPenghargaan_8.SetActive(true);
            buttonCollectPenghargaan_8.GetComponent<Button>().onClick.AddListener(() => {
                buttonCollectPenghargaan_8.SetActive(false);
                dateTextGO_8.SetActive(true);
                TextMeshProUGUI dateText = dateTextGO_8.GetComponent<TextMeshProUGUI>();
                dateText.text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                PersistentManager.Instance.UpdateKoin(500f);
                PersistentManager.Instance.isKuliSaktiClaimed = true;

                spesialSelectUI.terkunciButtonList[3].gameObject.SetActive(false);
            });

            if (PersistentManager.Instance.isKuliSaktiClaimed == true) {
                buttonCollectPenghargaan_8.SetActive(false);
            }
        }
    }

    private IEnumerator PlayNotif_8() {
        notif_8.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        notif_8.SetActive(false);
    }

    [Header("--- 9. Memasuki malam kedua ---")]
    [SerializeField] private GameObject buttonCollectPenghargaan_9;
    [SerializeField] private GameObject dateTextGO_9;
    [SerializeField] private GameObject notif_9;
    
    private void MalamKliwon() {
        if (PersistentManager.Instance.nightCounter == 2) {
            if (PersistentManager.Instance.isMalamKliwonPlayed == false) {
                StartCoroutine(PlayNotif_9());
            }
            PersistentManager.Instance.isMalamKliwonPlayed = true;

            buttonCollectPenghargaan_9.SetActive(true);
            buttonCollectPenghargaan_9.GetComponent<Button>().onClick.AddListener(() => {
                buttonCollectPenghargaan_9.SetActive(false);
                dateTextGO_9.SetActive(true);
                TextMeshProUGUI dateText = dateTextGO_9.GetComponent<TextMeshProUGUI>();
                dateText.text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                PersistentManager.Instance.UpdateKoin(500f);
                PersistentManager.Instance.isMalamKliwonClaimed = true;

                spesialSelectUI.terkunciButtonList[2].gameObject.SetActive(false);
            });

            if (PersistentManager.Instance.isMalamKliwonClaimed == true) {
                buttonCollectPenghargaan_9.SetActive(false);
            }
        }
    }

    private IEnumerator PlayNotif_9() {
        notif_9.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        notif_9.SetActive(false);
    }

    [Header("--- 10. Memiliki 4000K ---")]
    [SerializeField] private GameObject buttonCollectPenghargaan_10;
    [SerializeField] private GameObject dateTextGO_10;
    [SerializeField] private GameObject notif_10;

    private void PebisnisGhaib() {
        if (PersistentManager.Instance.dataKoin >= 4000) {
            if (PersistentManager.Instance.isPebisnisGhaibPlayed == false) {
                StartCoroutine(PlayNotif_10());
            }
            PersistentManager.Instance.isPebisnisGhaibPlayed = true;

            buttonCollectPenghargaan_10.SetActive(true);
            buttonCollectPenghargaan_10.GetComponent<Button>().onClick.AddListener(() => {
                buttonCollectPenghargaan_10.SetActive(false);
                dateTextGO_10.SetActive(true);
                TextMeshProUGUI dateText = dateTextGO_10.GetComponent<TextMeshProUGUI>();
                dateText.text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                PersistentManager.Instance.UpdateKoin(500f);
                PersistentManager.Instance.isPebisnisGhaibClaimed = true;

                spesialSelectUI.terkunciButtonList[5].gameObject.SetActive(false);
            });

            if (PersistentManager.Instance.isPebisnisGhaibClaimed == true) {
                buttonCollectPenghargaan_10.SetActive(false);
            }
        }
    }

    private IEnumerator PlayNotif_10() {
        notif_10.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        notif_10.SetActive(false);
    }
}