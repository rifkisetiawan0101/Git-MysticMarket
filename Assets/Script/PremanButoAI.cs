using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PremanButoAI : MonoBehaviour
{
    [SerializeField] private GameObject premanButoGFX; // Menyimpan referensi ke GFX
    [SerializeField] private GameObject winFlash;
    [SerializeField] private TextMeshProUGUI premanButoHealthUI;
    [SerializeField] private Image blueBarImage;
    public GameObject overlay;
    public GameObject serangButoButton;
    public GameObject collectDropItemButton; // Tambahkan referensi ke CollectDropItemButton
    public GameObject premanButoDropItem;
    public float buttoHealth = 60f; // Health awal 30
    public float maxHealth = 100f; // Health maksimal

    public float speed = 40000f;
    public float smoothTime = 0.1f;
    public float pauseTime = 3f; // Waktu berhenti di target
    public bool isPremanArrived = false;

    private MerchantManager merchantManager;
    private Vector3 targetPosition;
    private Vector3 spawnPosition; // Posisi awal spawn NPC
    private PremanButoAttackTrigger premanButoAttackTrigger;

    Rigidbody2D rb;
    Animator animator;
    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _lastHorizontal = "LastHorizontal";
    private const string _lastVertical = "LastVertical";

    private Vector2 velocity;
    private Vector2 _oldPosition;
    private Coroutine increaseHealthCoroutine; // Coroutine untuk menambah health


    public void SetupNPC(MerchantManager manager)
    {
        // Inisialisasi MerchantManager dan posisi spawn
        merchantManager = manager;
        spawnPosition = transform.position;
        SetRandomTarget();
    }

    private void Start()
    {
        premanButoAttackTrigger = GetComponentInChildren<PremanButoAttackTrigger>();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        _oldPosition = rb.position;

        if (merchantManager != null)
        {
            SetRandomTarget();
            StartCoroutine(ButoLoop());
        }
        else
        {
            Debug.LogWarning("MerchantManager belum di-set.");
        }

        premanButoDropItem.gameObject.SetActive(false); // Nonaktifkan DropItem di awal
        UpdateHealthUI(); // Update UI saat starts
    }

    private void Update()
    {
        NpcAnimation();
    }
    private bool isUtoMove = false;

    private void NpcAnimation() {
        Vector2 newPosition = transform.position;
        Vector2 movement = (newPosition - _oldPosition).normalized;
        _oldPosition = newPosition;

        if (movement != Vector2.zero) {
            animator.SetFloat(_horizontal, movement.x);
            animator.SetFloat(_vertical, movement.y);
            animator.SetFloat(_lastHorizontal, movement.x);
            animator.SetFloat(_lastVertical, movement.y);
            
            if (isUtoMove) {
                animator.Play("Move");
            }
        } else {
            if (!isUtoMove) {
                animator.Play("Idle");
            }
        }
    }

    private void SetRandomTarget()
    {
        if (PersistentManager.Instance.dataMerchantList.Count > 0)
        {
            int randomIndex = Random.Range(0, PersistentManager.Instance.dataMerchantList.Count);
            targetPosition = PersistentManager.Instance.dataMerchantList[randomIndex].merchantPosition;
        }
    }

    private IEnumerator MoveToTarget()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            Vector2 targetPos = Vector2.SmoothDamp(rb.position, targetPosition, ref velocity, smoothTime, speed * Time.deltaTime);
            rb.MovePosition(targetPos);
            isUtoMove = true;

            yield return null;
        }

        isPremanArrived = true; // Menandakan bahwa Buto sudah sampai di lokasi target
        HandleArrivalAtTarget(); // Memanggil fungsi untuk menangani setelah Buto sampai di target
        StartCoroutine(PauseAtTargetAndCollectCoins());
    }

    private void ReturnToSpawn()
    {
        StartCoroutine(MoveToSpawn());
    }

    private IEnumerator MoveToSpawn()
    {
        while (Vector3.Distance(transform.position, spawnPosition) > 0.1f)
        {
            Vector2 targetPos = Vector2.SmoothDamp(rb.position, spawnPosition, ref velocity, smoothTime, speed * Time.deltaTime);
            rb.MovePosition(targetPos);
            isUtoMove = true;
            
            yield return null;
        }

        Destroy(gameObject); // Menghancurkan NPC setelah kembali ke posisi awal
    }

    // ------------------------------------- BARU ------------------------------------------------------------ //

    public void UpdateButoHealth(float amount)
    {
        // Update health dan pastikan tidak kurang dari 0 atau lebih dari maxHealth
        buttoHealth = Mathf.Clamp(buttoHealth + amount, 0, maxHealth);

        Debug.Log("Update Health UI: " + buttoHealth);

        UpdateHealthUI();

        if (buttoHealth <= 0)
        {
            HandleButoDefeated();
        }
        else if (buttoHealth >= maxHealth) // preman buto menang
        {
            StartCoroutine(PlayWInFlash());
            AudioManager.audioManager.ChangeMusic(AudioManager.audioManager.inGameBacksound, 0.5f);
            ResetHealthAndStop();
            buttoHealth = 60; // Reset darah ke 60
            overlay.SetActive(false);
            serangButoButton.SetActive(false); 
            StartCoroutine(ButoLoop());
        }
    }

    private void UpdateHealthUI()
    {
        premanButoHealthUI.text = "HP: " + buttoHealth.ToString("F0");
        blueBarImage.fillAmount = buttoHealth / maxHealth;
    }

    private void HandleArrivalAtTarget()
    {
        if (premanButoAttackTrigger.isPlayerEnterTrigger)
        {
            premanButoAttackTrigger.ActivateSerangButton(); // Aktifkan tombol serang
            // StartIncreasingHealth(); // Mulai penambahan health jika pemain berada di dalam trigger
        }
    }

    private void HandleButoDefeated()
    {
        premanButoGFX.SetActive(false);
        premanButoDropItem.SetActive(true);
        serangButoButton.SetActive(false);
        premanButoHealthUI.text = " ";
        CheckAndActivateCollectButton();
        buttoHealth = 0;
        premanButoAttackTrigger.isPlayerEnterTrigger = false;
        overlay.SetActive(false);
        StartCoroutine(PlayWInFlash());
        
        PersistentManager.Instance.UpdateUtoDefeated(true);

        // Ubah musik kembali menjadi in-game music
        AudioManager.audioManager.ChangeMusic(AudioManager.audioManager.inGameBacksound, 0.5f);
    }

    public void CheckAndActivateCollectButton()
    {
        collectDropItemButton.SetActive(buttoHealth <= 0 && premanButoDropItem.activeSelf);
    }

    public void SerangButo()
    {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.attackButtonSound);
        if (buttoHealth > 0 && buttoHealth < maxHealth)
        {
            StartIncreasingHealth();
        }
        UpdateButoHealth(-3);
    }

    // Fungsi untuk menangani klik pada PremanButoDropItem
    [SerializeField] private GameObject collectFeedback;
    [SerializeField] private Image collectFeedbackImage;
    [SerializeField] private Sprite[] collectFrames;
    private float collectDelay = 0.055f;

    public void CollectDropItem()
    {   
        premanButoDropItem.SetActive(false);
        collectDropItemButton.SetActive(false);
        StartCoroutine(PlayCollectBatuAkik());
        PersistentManager.Instance.UpdateBatuAkik(1);
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.collectCoin);
        StartCoroutine(DestroyButoAfterDelay());
    }
    private IEnumerator PlayCollectBatuAkik() {
        collectFeedback.SetActive(true);
        for (int i = 0; i < collectFrames.Length; i++) {
            collectFeedbackImage.sprite = collectFrames[i];
            yield return new WaitForSeconds(collectDelay);
        }
        collectFeedback.SetActive(false);
    }

    private IEnumerator DestroyButoAfterDelay() {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    // Coroutine untuk menambah health ketika player berada di dalam trigger
    public void StartIncreasingHealth()
    {
        if (increaseHealthCoroutine == null)
        {
            Debug.Log("Memulai Coroutine Penambahan Darah");
            increaseHealthCoroutine = StartCoroutine(IncreaseHealthOverTime());
        }
    }

    public void StopIncreasingHealth()
    {
        if (increaseHealthCoroutine != null)
        {
            StopCoroutine(increaseHealthCoroutine);
            increaseHealthCoroutine = null;

            buttoHealth = 0;
        }
    }

    public void ResetHealthAndStop()
    {
        if (increaseHealthCoroutine != null)
        {
            StopCoroutine(increaseHealthCoroutine);
            increaseHealthCoroutine = null;

            buttoHealth = 60;

            UpdateHealthUI();
        }
    }

    private IEnumerator IncreaseHealthOverTime()
    {
        while (buttoHealth < maxHealth && buttoHealth != 0)
        {
            UpdateButoHealth(5);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator PlayWInFlash()
    {
        if (winFlash.activeSelf != true)
        {
            winFlash.SetActive(true);
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.winFlashSound);
        }
        else
        {
            winFlash.SetActive(false);
        }

        yield return new WaitForSeconds(1f);

        winFlash.SetActive(false);
    }

    public void StopMovement()
    {
        StopAllCoroutines(); // Menghentikan semua coroutine, termasuk pergerakan
        isUtoMove = false;
    }

    // baru
    private IEnumerator PauseAtTargetAndCollectCoins()
    {
        // Berhenti selama 3 detik
        isUtoMove = false;
        yield return new WaitForSeconds(3f);
        // Mengambil koin sebanyak 500
        // PersistentManager.Instance.UpdateKoin(-500);

        if (premanButoAttackTrigger.isPlayerEnterTrigger)
        {
            HandleArrivalAtTarget(); // Jika pemain sudah menyentuh trigger, mulai pertarungan
        }
    }

    private IEnumerator ButoLoop()
    {
        while (buttoHealth > 0) // Buto terus bergerak sampai dikalahkan
        {
            SetRandomTarget();
            yield return StartCoroutine(MoveToTarget());

            if (buttoHealth > 0) // Cek jika Buto belum kalah
            {
                yield return StartCoroutine(PauseAtTargetAndCollectCoins());
            }
        }
    }

    [SerializeField] private Sprite normalAttack;
    [SerializeField] private Sprite downAttack;
    [SerializeField] private Button attackButton;

    public void OnDownAttack() {
        attackButton.image.sprite = downAttack;
    }

    public void OnNormalAttack() {
        attackButton.image.sprite = normalAttack;
    }


    [SerializeField] private Sprite normalcollect;
    [SerializeField] private Sprite downcollect;
    [SerializeField] private Button collecDropButton;

    public void OnDownCollect() {
        collecDropButton.image.sprite = downcollect;
    }

    public void OnNormalCollect() {
        collecDropButton.image.sprite = normalcollect;
    }
}
