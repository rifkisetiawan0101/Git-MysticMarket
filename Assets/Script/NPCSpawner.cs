using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NpcSpawner : MonoBehaviour {
    
    public GameObject npcPrefabs;
    public float minSpawnTime = 3f; // Waktu spawn minimum
    public float maxSpawnTime = 7f; // Waktu spawn maksimum
    public MerchantManager merchantManager; // Reference ke MerchantManager untuk akses targetMerchantNPCList

    private void Start() {
        if (SceneManager.GetActiveScene().name == "InGame") {
            StartCoroutine(SpawnNPC());
        }
    }

    IEnumerator SpawnNPC() {
        while (true) {
            if (PersistentManager.Instance.dataTotalSpawnNpc < PersistentManager.Instance.dataMaxNpc && PersistentManager.Instance.isNowMalam) {
                float spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
                yield return new WaitForSeconds(spawnTime);

                GameObject npc = Instantiate(npcPrefabs, transform.position, Quaternion.identity);
                NpcAI npcAI = npc.GetComponent<NpcAI>();
                npcAI.SetupNPC(merchantManager); // Kirim referensi MerchantManager ke NPC

                PersistentManager.Instance.dataTotalSpawnNpc++;
            }
            yield return null;
        }
    }
}
