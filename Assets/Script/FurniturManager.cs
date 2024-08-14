using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;

public class FurniturManager : MonoBehaviour {
    [SerializeField] private FurniturTypeSO activeFurniturType;

    private FurniturSelectUI furniturSelectUI;
    private bool isPlacingFurnitur = false;
    public bool isFurniturPlaced = true;

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

    public event Action OnFurniturPlaced;

    private void Update() {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && !isPlacingFurnitur) {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            if (CanSpawnFurnitur(activeFurniturType, mouseWorldPosition)) {
                PlacementInstance();
            }
        }
        furniturSelectUI = FindObjectOfType<FurniturSelectUI>();
    }

    private Transform placementInstance;
    private void PlacementInstance() {
        Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
        grid.GetXY(mouseWorldPosition, out int x, out int y);
        Vector3 spawnPosition = grid.GetWorldPosition(x, y) + new Vector3(cellSize, cellSize, 0) * 0.5f;
        placementInstance = Instantiate(activeFurniturType.furniturPlacementPrefab, spawnPosition, Quaternion.identity);

        // Set status isPlacingFurnitur menjadi true setelah memanggil placement
        isPlacingFurnitur = true;

        // Setup furnitur placement instance
        FurniturPlacement furniturPlacement = placementInstance.GetComponent<FurniturPlacement>();
        furniturPlacement.Setup(spawnPosition, this);
        furniturSelectUI.DestroyCursorFurnitur();

        StartCoroutine (ActivateIsFurniturPlaced (0.5f));
    }

    public void DestroyPlacementInstance() {
        if (placementInstance != null) {
            Destroy(placementInstance.gameObject);
            placementInstance = null;
            isPlacingFurnitur = false;
        }
    }

    public static event Action OnTotalFurniturChanged;
    public static int totalFurnitur = 0;
    public void FurniturPlacing(Vector3 position) {
        Instantiate(activeFurniturType.furniturConstructionPrefab, position, Quaternion.identity);

        Koin.koin.updateKoin(-activeFurniturType.furniturPrice);
        SetActiveFurniturType(null); // Reset activeFurniturType setelah menaruh furnitur
        OnFurniturPlaced?.Invoke(); // Panggil event ketika furnitur ditempatkan
        isPlacingFurnitur = false; // Reset status placement
        totalFurnitur += 1;
        Debug.Log ("Total Furnitur " + totalFurnitur);
        OnTotalFurniturChanged?.Invoke();
    }

    public void CancelPlacement() {
        SetActiveFurniturType(null);
        isPlacingFurnitur = false; // Reset status placement jika dibatalkan
    }

    public void SetActiveFurniturType(FurniturTypeSO furniturTypeSO) {
        activeFurniturType = furniturTypeSO;
    }

    public FurniturTypeSO GetActiveFurniturType() {
        return activeFurniturType;
    }

    private bool CanSpawnFurnitur(FurniturTypeSO furniturTypeSO, Vector3 position) {
        if (activeFurniturType == null) {
            return false;
        }

        if (Koin.koin.koins < furniturTypeSO.furniturPrice) {
            return false;
        }

        BoxCollider2D furniturBoxCollider2D = furniturTypeSO.furniturPrefab.GetComponent<BoxCollider2D>();
        
        if (Physics2D.OverlapBox(position + (Vector3)furniturBoxCollider2D.offset, furniturBoxCollider2D.size, 0) != null) {
            return false;
        }

        return true;
    }

    public IEnumerator ActivateIsFurniturPlaced(float delay) {
        yield return new WaitForSeconds(delay);
        isFurniturPlaced = true;
    }

    public IEnumerator DeactivateIsFurniturPlaced(float delay) {
        yield return new WaitForSeconds(delay);
        isFurniturPlaced = false;
    }
}
