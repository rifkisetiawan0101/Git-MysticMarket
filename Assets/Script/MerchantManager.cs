using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;

public class MerchantManager : MonoBehaviour {
    public static MerchantManager Instance { get; private set; }

    [SerializeField] private MerchantTypeSO activeMerchantType;
    [SerializeField] private MerchantTerkunciSO activeTerkunci;
    [SerializeField] private LayerMask ignoreLayerMask;

    private MerchantSelectUI merchantSelectUI;
    private bool isPlacingMerchant = false;
    public bool isMerchantPlaced = true;

    [SerializeField] private GameObject LessKoinNotif;
    [SerializeField] private GameObject LahanBurukNotif;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Jangan hancurkan saat berpindah scene
        } else {
            Destroy(gameObject); // Hancurkan jika instance sudah ada
        }
    }

    public event Action OnMerchantPlaced;
    private void Update() {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && !isPlacingMerchant) {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            if (CanSpawnMerchant(activeMerchantType, mouseWorldPosition)) {
                PlacementInstance();
                UIManager.Instance.DeactivateUI();
            } else if (LessKoin(activeMerchantType)) {
                StartCoroutine(PlayLessKoin());
                UIManager.Instance.ActivateUI();
            } else if (LahanBuruk(activeMerchantType, mouseWorldPosition)) {
                StartCoroutine(PlayLahanBuruk());
                UIManager.Instance.ActivateUI();
            }
        }

        merchantSelectUI = FindObjectOfType<MerchantSelectUI>();
    }

    private Transform placementInstance;
    private void PlacementInstance() {
        Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();

        Vector3 spawnPosition = mouseWorldPosition;
        placementInstance = Instantiate(activeMerchantType.merchantPlacementPrefab, spawnPosition, Quaternion.identity);

        // Set status isPlacingMerchant menjadi true setelah memanggil placement
        isPlacingMerchant = true;

        // Setup merchant placement instance
        MerchantPlacement merchantPlacement = placementInstance.GetComponent<MerchantPlacement>();
        merchantPlacement.Setup(spawnPosition, this);
        merchantSelectUI.DestroyCursorMerchant();

        StartCoroutine (ActivateIsMerchantPlaced (0.5f));
    }

    public void DestroyPlacementInstance() {
        if (placementInstance != null) {
            Destroy(placementInstance.gameObject);
            placementInstance = null;
            isPlacingMerchant = false;
        }
    }

    [SerializeField] GameObject kunchanSpawner;
    [SerializeField] GameObject pocinSpawner;
    [SerializeField] GameObject ayangSpawner;
    [SerializeField] GameObject utoSpawner;
    private int merchantIndex = -1;

    public void MerchantPlacing(Vector3 position) {  
        Instantiate(activeMerchantType.merchantConstructionPrefab, position, Quaternion.identity);

        PersistentManager.Instance.UpdateKoin(-activeMerchantType.merchantPrice);

        merchantIndex++;
        
        PersistentManager.MerchantData newMerchantData = new PersistentManager.MerchantData {
            merchantTypeSO = activeMerchantType, // Atur merchantTypeSO
            merchantPosition = position
        };

        HandleJuraganPasarEvent();

        PersistentManager.Instance.dataMerchantList.Add(newMerchantData);        

        pocinSpawner.SetActive(true);
        kunchanSpawner.SetActive(true);
        ayangSpawner.SetActive(true);
        utoSpawner.SetActive(true);

        SetActiveMerchantType(null);
        OnMerchantPlaced?.Invoke(); // Panggil event ketika merchant ditempatkan
        isPlacingMerchant = false; // Reset status placement
        PersistentManager.Instance.UpdateTotalMerchant(1f);
        PersistentManager.Instance.UpdateTotalNpc(3f);

        UIManager.Instance.ActivateUI();
    }

    public int GetCurrentMerchantIndex() {
        return merchantIndex;
    }

    public static event Action OnJuraganPasar;
    public void HandleJuraganPasarEvent() {
        if (activeMerchantType.merchantName == "Pedagang Sayur") {
            PersistentManager.Instance.isSayurPlaced = true;
        }

        if (activeMerchantType.merchantName == "Pemasok Rempah") {
            PersistentManager.Instance.isRempahPlaced = true;
        }

        if (activeMerchantType.merchantName == "Penjual Daging") {
            PersistentManager.Instance.isDagingPlaced = true;
        }
        OnJuraganPasar?.Invoke();
    }

    public void CancelPlacement() {
        SetActiveMerchantType(null);
        isPlacingMerchant = false; // Reset status placement jika dibatalkan
        UIManager.Instance.ActivateUI();
    }

    public void SetActiveMerchantType(MerchantTypeSO merchantTypeSO) {
        activeMerchantType = merchantTypeSO;
    }

    public MerchantTypeSO GetActiveMerchantType() {
        return activeMerchantType;
    }

    public void SetActiveTerkunci(MerchantTerkunciSO merchantTerkunciSO) {
        activeTerkunci = merchantTerkunciSO;
    }

    public MerchantTerkunciSO GetActiveTerkunci() {
        return activeTerkunci;
    }

    private bool CanSpawnMerchant(MerchantTypeSO merchantTypeSO, Vector3 position) {
        if (activeMerchantType == null) {
            return false;
        }

        if (PersistentManager.Instance.dataKoin < merchantTypeSO.merchantPrice) {
            return false;
        }

        PolygonCollider2D merchantCollider = merchantTypeSO.merchantPrefab.GetComponent<PolygonCollider2D>();

        if (merchantCollider == null) {
            Debug.LogError("PolygonCollider2D tidak ditemukan pada prefab merchant!");
            return false;
        }

        Vector2[] worldSpacePoints = new Vector2[merchantCollider.points.Length];

        for (int i = 0; i < merchantCollider.points.Length; i++) {
            worldSpacePoints[i] = (Vector2)position + merchantCollider.points[i];
        
            if (Physics2D.OverlapPoint(worldSpacePoints[i], ~ignoreLayerMask) != null) {
                return false;
            }
        }

        return true;
    }

    private bool LessKoin(MerchantTypeSO merchantTypeSO) {
        if (PersistentManager.Instance.dataKoin < merchantTypeSO.merchantPrice) {
            return true;
        }
        return false;
    }

    private IEnumerator PlayLessKoin() {
        LessKoinNotif.SetActive(true);
        SetActiveMerchantType(null);
        Destroy(merchantSelectUI.cursorInstance);
        yield return new WaitForSeconds(1.277f);
        isMerchantPlaced = true;
        LessKoinNotif.SetActive(false);
    }

    private bool LahanBuruk(MerchantTypeSO merchantTypeSO, Vector3 position) {
        PolygonCollider2D merchantCollider = merchantTypeSO.merchantPrefab.GetComponent<PolygonCollider2D>();

        Vector2[] worldSpacePoints = new Vector2[merchantCollider.points.Length];

        for (int i = 0; i < merchantCollider.points.Length; i++) {
            worldSpacePoints[i] = (Vector2)position + merchantCollider.points[i];
        
            if (Physics2D.OverlapPoint(worldSpacePoints[i], ~ignoreLayerMask) != null) {
                return true;
            }
        }
        return false;
    }

    private IEnumerator PlayLahanBuruk() {
        LahanBurukNotif.SetActive(true);
        SetActiveMerchantType(null);
        Destroy(merchantSelectUI.cursorInstance);
        yield return new WaitForSeconds(1.277f);
        isMerchantPlaced = true;
        LahanBurukNotif.SetActive(false);
    }

    public IEnumerator ActivateIsMerchantPlaced(float delay) {
        yield return new WaitForSeconds(delay);
        isMerchantPlaced = true;
    }

    public IEnumerator DeactivateIsMerchantPlaced(float delay) {
        yield return new WaitForSeconds(delay);
        isMerchantPlaced = false;
    }
}