using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionProgressShader : MonoBehaviour {

    [SerializeField] private Transform merchantPrefab;

    private float contructionTimer;
    private Material material;
    
    private void Awake() {
        material = GetComponent<SpriteRenderer>().material;
    }
    
    private void Start() {
        material.SetFloat("_Progress", 0f);
    }

    private void Update() {
        float timeToConstruct = 5f;
        contructionTimer += Time.deltaTime / timeToConstruct;
        material.SetFloat("_Progress", contructionTimer);

        if (contructionTimer >= 1f) {
            Instantiate(merchantPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }
}
