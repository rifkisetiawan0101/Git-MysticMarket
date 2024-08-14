using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionProgressShader : MonoBehaviour {

    [SerializeField] private Transform targetPrefab;
    [SerializeField] private float timeToConstruct = 5f;

    private float contructionTimer;
    private Material material;
    
    private void Awake() {
        material = GetComponent<SpriteRenderer>().material;
    }
    
    private void Start() {
        material.SetFloat("_Progress", 0f);
    }

    private void Update() {
        contructionTimer += Time.deltaTime / timeToConstruct;
        material.SetFloat("_Progress", contructionTimer);

        if (contructionTimer >= 1f) {
            Instantiate(targetPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }
}
