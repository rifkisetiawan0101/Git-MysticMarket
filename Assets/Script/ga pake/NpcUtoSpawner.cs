using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcUtoSpawner : MonoBehaviour {
    
    public GameObject npcPrefabs;
    public float minSpawnTime = 30f; // Waktu spawn minimum
    public float maxSpawnTime = 30.1f; // Waktu spawn maksimum
    public MerchantManager merchantManager; // Reference ke MerchantManager untuk akses targetMerchantNPCList

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        StartCoroutine(SpawnNPC());
    }

    IEnumerator SpawnNPC() {
        while (true) {
            if (PersistentManager.Instance.nightCounter >= 2) {
                float spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
                yield return new WaitForSeconds(spawnTime);

                GameObject npcUto = Instantiate(npcPrefabs, transform.position, Quaternion.identity);
                NpcUtoAI npcUtoAI = npcUto.GetComponent<NpcUtoAI>();
                npcUtoAI.SetupUto(merchantManager); // Kirim referensi MerchantManager ke NPC

            }
            yield return null;
        }
    }
}
