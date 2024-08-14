using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Merchants", menuName = "Scriptable Object/Merchants")]
public class MerchantTypeSO : ScriptableObject {
    public string merchantName;
    public float merchantPrice;
    public Transform merchantPrefab;
    public Transform merchantConstructionPrefab;
    public Sprite merchantButton;
    public Sprite selectedMerchantButton;
    public Sprite merchantWindow;
    public GameObject merchantCursor;
    public Transform merchantPlacementPrefab;

    // private void Awake() {
    //     merchantPrefab = new List<Transform>();
    //     merchantConstructionPrefab = new List<Transform>();
    // }
}
