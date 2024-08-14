using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour {
    public GameObject npcPrefabs;
    public float minSpawnTime = 3f; // Waktu spawn minimum
    public float maxSpawnTime = 7f; // Waktu spawn maksimum
    public MerchantManager merchantManager; // Reference ke MerchantManager untuk akses targetMerchantNPCList

    private void Start() {
        StartCoroutine(SpawnNPC());
    }

    IEnumerator SpawnNPC() {
        float spawnTime = Random.Range(minSpawnTime, maxSpawnTime); // Tentukan waktu spawn secara random
        yield return new WaitForSeconds(spawnTime);

        GameObject npc = Instantiate(npcPrefabs, transform.position, Quaternion.identity);
        NpcAI npcAI = npc.GetComponent<NpcAI>();
        npcAI.SetupNPC(merchantManager); // Kirim referensi MerchantManager ke NPC

        StartCoroutine(SpawnNPC()); // Spawn NPC berikutnya setelah interval
    }
}
