using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;

public class HiasanManager : MonoBehaviour {
    [SerializeField] private HiasanTypeSO activeHiasanType;

    private HiasanSelectUI hiasanSelectUI;

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
    public event Action OnHiasanPlaced;

    private void Update() {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            if (CanSpawnHiasan(activeHiasanType, mouseWorldPosition)) {
                grid.GetXY(mouseWorldPosition, out int x, out int y);
                Instantiate(activeHiasanType.hiasanConstructionPrefab, grid.GetWorldPosition(x, y) + new Vector3(cellSize, cellSize, 0) * 0.5f, Quaternion.identity);

                Koin.koin.updateKoin( - (activeHiasanType.hiasanPrice));
                
                SetActiveHiasanType(null); // Reset activeHiasanType setelah menaruh Hiasan
                OnHiasanPlaced?.Invoke(); // Panggil event ketika Hiasan ditempatkan
                hiasanSelectUI.DestroyCursorHiasan(); // Hapus kursor setelah hiasan ditempatkan
            }
        }
        hiasanSelectUI = FindObjectOfType<HiasanSelectUI>();
    }

    public void SetActiveHiasanType(HiasanTypeSO hiasanTypeSO) {
        activeHiasanType = hiasanTypeSO;
    }

    public HiasanTypeSO GetActiveHiasanType() {
        return activeHiasanType;
    }

    private bool CanSpawnHiasan(HiasanTypeSO hiasanTypeSO, Vector3 position) {
        if (hiasanTypeSO == null) {
            return false;
        }

        if (Koin.koin.koins < hiasanTypeSO.hiasanPrice) {
            return false;
        }

        BoxCollider2D hiasanBoxCollider2D = hiasanTypeSO.hiasanPrefab.GetComponent<BoxCollider2D>();
        
        if (Physics2D.OverlapBox(position + (Vector3)hiasanBoxCollider2D.offset, hiasanBoxCollider2D.size, 0) != null) {
            return false;
        }

        return true;
    }
}
