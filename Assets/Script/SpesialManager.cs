using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;

public class SpesialManager : MonoBehaviour {
    [SerializeField] private SpesialTypeSO activeSpesialType;
    [SerializeField] private LayerMask ignoreLayerMask;

    private SpesialSelectUI spesialSelectUI;
    private bool isPlacingSpesial = false;
    public bool isSpesialPlaced = true;

    private GridXY<GridObject> grid;
    private int cellSize = 60;
    private void Awake() {
        int gridWidth = 6000 / cellSize;
        int gridHeight = 6000 / cellSize;
        grid = new GridXY<GridObject>(gridWidth, gridHeight, cellSize, Vector3.zero, (GridXY<GridObject> g, int x, int y) => new GridObject(g, x, y));
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

    public event Action OnSpesialPlaced;

    private void Update() {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && !isPlacingSpesial) {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            if (CanSpawnSpesial(activeSpesialType, mouseWorldPosition)) {
                PlacementInstance();
            }
        }
        spesialSelectUI = FindObjectOfType<SpesialSelectUI>();
    }

    private Transform placementInstance;
    private void PlacementInstance() {
        Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
        grid.GetXY(mouseWorldPosition, out int x, out int y);
        Vector3 spawnPosition = grid.GetWorldPosition(x, y) + new Vector3(cellSize, cellSize, 0) * 0.5f;
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

    public static event Action OnTotalSpesialChanged;
    public static int totalSpesial = 0;
    public void SpesialPlacing(Vector3 position) {
        Instantiate(activeSpesialType.spesialConstructionPrefab, position, Quaternion.identity);

        PersistentManager.Instance.UpdateKoin(-activeSpesialType.spesialPrice);
        SetActiveSpesialType(null); // Reset activeSpesialType setelah menaruh spesial
        OnSpesialPlaced?.Invoke(); // Panggil event ketika spesial ditempatkan
        isPlacingSpesial = false; // Reset status placement
        totalSpesial += 1;
        Debug.Log ("Total Spesial " + totalSpesial);
        OnTotalSpesialChanged?.Invoke();
    }

    public void CancelPlacement() {
        SetActiveSpesialType(null);
        isPlacingSpesial = false; // Reset status placement jika dibatalkan
    }

    public void SetActiveSpesialType(SpesialTypeSO spesialTypeSO) {
        activeSpesialType = spesialTypeSO;
    }

    public SpesialTypeSO GetActiveSpesialType() {
        return activeSpesialType;
    }

    private bool CanSpawnSpesial(SpesialTypeSO spesialTypeSO, Vector3 position) {
        if (activeSpesialType == null) {
            return false;
        }

        if (PersistentManager.Instance.Koins < spesialTypeSO.spesialPrice) {
            return false;
        }

        BoxCollider2D spesialBoxCollider2D = spesialTypeSO.spesialPrefab.GetComponent<BoxCollider2D>();
        
        if (Physics2D.OverlapBox(position + (Vector3)spesialBoxCollider2D.offset, spesialBoxCollider2D.size, 0, ~ignoreLayerMask) != null) {
            return false;
        }

        return true;
    }

    public IEnumerator ActivateIsSpesialPlaced(float delay) {
        yield return new WaitForSeconds(delay);
        isSpesialPlaced = true;
    }

    public IEnumerator DeactivateIsSpesialPlaced(float delay) {
        yield return new WaitForSeconds(delay);
        isSpesialPlaced = false;
    }
}