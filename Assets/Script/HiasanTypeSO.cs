using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hiasan", menuName = "Scriptable Object/Hiasan")]
public class HiasanTypeSO : ScriptableObject {
    public string hiasanName;
    public float hiasanPrice;
    public Transform hiasanPrefab;
    public Transform hiasanConstructionPrefab;
    public Sprite hiasanButton;
    public Sprite selectedHiasanButton;
    public Sprite hiasanWindow;
    public GameObject hiasanCursor;
}
