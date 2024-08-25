using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurniturConstruction : MonoBehaviour {
    [SerializeField] private Transform targetPrefab;
    [SerializeField] private float timeToConstruct = 1.916f;
    private float contructionTimer;
    // private Animator animator;

    // private void Start() {
    //     animator = GetComponent<Animator>();
    // }

    private void Update() {
        contructionTimer += Time.deltaTime;

        if (contructionTimer >= timeToConstruct) {
            Instantiate(targetPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
