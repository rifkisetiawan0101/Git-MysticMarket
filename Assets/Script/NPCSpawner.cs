using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour {
    public GameObject npcPrefabs;
    public float spawnTime;

    private void Start() {
        StartCoroutine(spawnNPC());
    }

    IEnumerator spawnNPC() {
        yield return new WaitForSeconds(spawnTime);
        Instantiate(npcPrefabs, transform.position, Quaternion.identity);
        StartCoroutine(spawnNPC());
    }

}