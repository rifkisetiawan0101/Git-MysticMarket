using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;

public class SpesialManager : MonoBehaviour {
    public static SpesialManager Instance { get; private set; }

    [SerializeField] private SpesialTypeSO activeSpesialType;
    [SerializeField] private SpesialTerkunciSO activeTerkunciType;
    [SerializeField] private SpesialTerpasangSO activeTerpasangType;
    [SerializeField] private LayerMask ignoreLayerMask;

    private SpesialSelectUI spesialSelectUI;
    private bool isPlacingSpesial = false;
    public bool isSpesialPlaced = true;
    [SerializeField] private GameObject LahanBurukNotif;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Jangan hancurkan saat berpindah scene
        } else {
            Destroy(gameObject); // Hancurkan jika instance sudah ada
        }
    }

    public event Action OnSpesialPlaced;
    private void Update() {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && !isPlacingSpesial) {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            if (CanSpawnSpesial(activeSpesialType, mouseWorldPosition)) {
                PlacementInstance();
                UIManager.Instance.DeactivateUI();
            }  else if (LahanBuruk(activeSpesialType, mouseWorldPosition)) {
                StartCoroutine(PlayLahanBuruk());
                UIManager.Instance.ActivateUI();
            }
        }
        spesialSelectUI = FindObjectOfType<SpesialSelectUI>();

        UpdateSpesialPlacementStatus();
    }

    private Transform placementInstance;
    private void PlacementInstance() {
        Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();

        Vector3 spawnPosition = mouseWorldPosition;
        placementInstance = Instantiate(activeSpesialType.spesialPlacementPrefab, spawnPosition, Quaternion.identity);

        // Set status isPlacingSpesial menjadi true setelah memanggil placement
        isPlacingSpesial = true;

        // Setup spesial placement instance
        SpesialPlacement spesialPlacement = placementInstance.GetComponent<SpesialPlacement>();
        spesialPlacement.Setup(spawnPosition, this);
        spesialSelectUI.DestroyCursorSpesial();

        StartCoroutine (ActivateIsSpesialPlaced (0.5f));
    }

    public void DestroyPlacementInstance() {
        if (placementInstance != null) {
            Destroy(placementInstance.gameObject);
            placementInstance = null;
            isPlacingSpesial = false;
        }
    }
    
    public void SpesialPlacing(Vector3 position) {
        int index = spesialSelectUI.spesialTypeSOList.IndexOf(activeSpesialType);
        if (index >= 0 && index < IsSpesialPlaced.Length) {
            IsSpesialPlaced[index] = true;
            spesialSelectUI.terpasangButtonList[index].gameObject.SetActive(true);
        }

        Instantiate(activeSpesialType.spesialConstructionPrefab, position, Quaternion.identity);

        PersistentManager.SpesialData newSpesialData = new PersistentManager.SpesialData {
            spesialTypeSO = activeSpesialType, // Atur spesialTypeSO
            spesialPosition = position
        };

        PersistentManager.Instance.dataSpesialList.Add(newSpesialData);
        
        SetActiveSpesialType(null); // Reset activeSpesialType setelah menaruh spesial
        OnSpesialPlaced?.Invoke(); // Panggil event ketika spesial ditempatkan
        isPlacingSpesial = false; // Reset status placement

        UIManager.Instance.ActivateUI();
    }

    public void CancelPlacement() {
        SetActiveSpesialType(null);
        isPlacingSpesial = false; // Reset status placement jika dibatalkan
        UIManager.Instance.ActivateUI();
    }

    public void SetActiveSpesialType(SpesialTypeSO spesialTypeSO) {
        activeSpesialType = spesialTypeSO;
    }

    public SpesialTypeSO GetActiveSpesialType() {
        return activeSpesialType;
    }

    public void SetActiveTerkunciType(SpesialTerkunciSO spesialTerkunciSO) {
        activeTerkunciType = spesialTerkunciSO;
    }

    public SpesialTerkunciSO GetActiveTerkunciType() {
        return activeTerkunciType;
    }

    public void SetActiveTerpasangType(SpesialTerpasangSO spesialTerpasangSO) {
        activeTerpasangType = spesialTerpasangSO;
    }

    public SpesialTerpasangSO GetActiveTerpasangType() {
        return activeTerpasangType;
    }

    private bool CanSpawnSpesial(SpesialTypeSO spesialTypeSO, Vector3 position) {
        if (activeSpesialType == null) {
            return false;
        }

        
        PolygonCollider2D spesialCollider = spesialTypeSO.spesialPrefab.GetComponent<PolygonCollider2D>();

        if (spesialCollider == null) {
            Debug.LogError("PolygonCollider2D tidak ditemukan pada prefab spesial!");
            return false;
        }

        Vector2[] worldSpacePoints = new Vector2[spesialCollider.points.Length];

        for (int i = 0; i < spesialCollider.points.Length; i++) {
            worldSpacePoints[i] = (Vector2)position + spesialCollider.points[i];
        
            if (Physics2D.OverlapPoint(worldSpacePoints[i], ~ignoreLayerMask) != null) {
                return false;
            }
        }

        return true;
    }

    private bool LahanBuruk(SpesialTypeSO spesialTypeSO, Vector3 position) {
        PolygonCollider2D spesialCollider = spesialTypeSO.spesialPrefab.GetComponent<PolygonCollider2D>();

        Vector2[] worldSpacePoints = new Vector2[spesialCollider.points.Length];

        for (int i = 0; i < spesialCollider.points.Length; i++) {
            worldSpacePoints[i] = (Vector2)position + spesialCollider.points[i];
        
            if (Physics2D.OverlapPoint(worldSpacePoints[i], ~ignoreLayerMask) != null) {
                return true;
            }
        }
        return false;
    }

    private IEnumerator PlayLahanBuruk() {
        LahanBurukNotif.SetActive(true);
        SetActiveSpesialType(null);
        Destroy(spesialSelectUI.cursorInstance);
        yield return new WaitForSeconds(1.277f);
        isSpesialPlaced = true;
        LahanBurukNotif.SetActive(false);
    }

    public IEnumerator ActivateIsSpesialPlaced(float delay) {
        yield return new WaitForSeconds(delay);
        isSpesialPlaced = true;
    }

    public IEnumerator DeactivateIsSpesialPlaced(float delay) {
        yield return new WaitForSeconds(delay);
        isSpesialPlaced = false;
    }

    private bool[] IsSpesialPlaced = new bool[6]; 
        void UpdateSpesialPlacementStatus() {
        for (int i = 0; i < IsSpesialPlaced.Length; i++) {
            if (IsSpesialPlaced[i]) {
                spesialSelectUI.terpasangButtonList[i].gameObject.SetActive(true);
            }
        }
    }
}