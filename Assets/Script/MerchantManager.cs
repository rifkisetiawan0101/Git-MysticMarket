using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;

public class MerchantManager : MonoBehaviour {
    [SerializeField] private MerchantTypeSO activeMerchantType;

    private MerchantSelectUI merchantSelectUI;

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

    private void Start() {
        
    }
    public event Action OnMerchantPlaced;

    private void Update() {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            if (CanSpawnMerchant(activeMerchantType, mouseWorldPosition)) {
                grid.GetXY(mouseWorldPosition, out int x, out int y);
                Instantiate(activeMerchantType.merchantConstructionPrefab, grid.GetWorldPosition(x, y) + new Vector3(cellSize, cellSize, 0) * 0.5f, Quaternion.identity);

                Koin.koin.updateKoin( - (activeMerchantType.merchantPrice));
                
                SetActiveMerchantType(null); // Reset activeMerchantType setelah menaruh merchant
                OnMerchantPlaced?.Invoke(); // Panggil event ketika merchant ditempatkan
                merchantSelectUI.DestroyCursorMerchant(); // Hapus kursor setelah merchant ditempatkan
            }
        }
        merchantSelectUI = FindObjectOfType<MerchantSelectUI>();
    }

    public void SetActiveMerchantType(MerchantTypeSO merchantTypeSO) {
        activeMerchantType = merchantTypeSO;
    }

    public MerchantTypeSO GetActiveMerchantType() {
        return activeMerchantType;
    }

    private bool CanSpawnMerchant(MerchantTypeSO merchantTypeSO, Vector3 position) {
        if (merchantTypeSO == null) {
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
}
