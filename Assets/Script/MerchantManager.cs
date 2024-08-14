using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;
using Unity.Mathematics;

public class MerchantManager : MonoBehaviour {
    [SerializeField] private MerchantTypeSO activeMerchantType;

    private MerchantSelectUI merchantSelectUI;
    private bool isPlacingMerchant = false;
    public bool isMerchantPlaced = true;

    private GridXY<GridObject> grid;
    private int cellSize = 60;
    private void Awake() {
        int gridWidth = 6000 / cellSize;
        int gridHeight = 6000 / cellSize;
        grid = new GridXY<GridObject>(gridWidth, gridHeight, cellSize, Vector3.zero, (GridXY<GridObject> g, int x, int y) => new GridObject(g, x, y));
        targetMerchantNPCList = new List<Vector3>();
    }

    public class GridObject {

        private GridXY<GridObject> grid;
        private int x;
        private int y;

        public GridObject(GridXY<GridObject> grid, int x, int y) {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        //Debug untuk menampilkan koordinat
        public override string ToString()
        {
            return x + ", " + y;
        }
    }

    public event Action OnMerchantPlaced;
    private void Update() {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && !isPlacingMerchant) {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            if (CanSpawnMerchant(activeMerchantType, mouseWorldPosition)) {
                PlacementInstance();
            }
        }
        merchantSelectUI = FindObjectOfType<MerchantSelectUI>();
    }

    private Transform placementInstance;

    private void PlacementInstance() {
        Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
        grid.GetXY(mouseWorldPosition, out int x, out int y);
        Vector3 spawnPosition = grid.GetWorldPosition(x, y) + new Vector3(cellSize, cellSize, 0) * 0.5f;
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

    public static event Action OnTotalMerchantChanged;
    public static int totalMerchant = 0;
    // public bool isHaveMerchant = false;
    public List<Vector3>targetMerchantNPCList;
    [SerializeField] GameObject kunchanSpawner;
    public void MerchantPlacing(Vector3 position) {
        Instantiate(activeMerchantType.merchantConstructionPrefab, position, Quaternion.identity);

        Koin.koin.updateKoin(-activeMerchantType.merchantPrice);
        SetActiveMerchantType(null); // Reset activeMerchantType setelah menaruh merchant
        OnMerchantPlaced?.Invoke(); // Panggil event ketika merchant ditempatkan
        isPlacingMerchant = false; // Reset status placement
        totalMerchant += 1;
        Debug.Log ("Total Merchant " + totalMerchant);
        OnTotalMerchantChanged?.Invoke();

        targetMerchantNPCList.Add(position);
        Debug.Log("Posisi merchant ditambahkan: " + position);
        Debug.Log("Daftar semua posisi merchant: " + string.Join(", ", targetMerchantNPCList));
        kunchanSpawner.SetActive(true);
    }

    public void CancelPlacement() {
        SetActiveMerchantType(null);
        isPlacingMerchant = false; // Reset status placement jika dibatalkan
    }

    public void SetActiveMerchantType(MerchantTypeSO merchantTypeSO) {
        activeMerchantType = merchantTypeSO;
    }

    public MerchantTypeSO GetActiveMerchantType() {
        return activeMerchantType;
    }

    private bool CanSpawnMerchant(MerchantTypeSO merchantTypeSO, Vector3 position) {
        if (activeMerchantType == null) {
            return false;
        }

        if (Koin.koin.koins < merchantTypeSO.merchantPrice) {
            return false;
        }

        BoxCollider2D merchantBoxCollider2D = merchantTypeSO.merchantPrefab.GetComponent<BoxCollider2D>();
        
        if (Physics2D.OverlapBox(position + (Vector3)merchantBoxCollider2D.offset, merchantBoxCollider2D.size, 0) != null) {
            return false;
        }

        return true;
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