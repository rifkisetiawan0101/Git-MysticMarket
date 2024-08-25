using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PremanSpawner : MonoBehaviour
{
    public GameObject premanPrefabs;
    public float spawnTime = 5;
    public MerchantManager merchantManager; // Reference ke MerchantManager untuk akses targetMerchantNPCList

    private GameObject currentNPC; // Simpan referensi ke NPC yang sudah di-spawn

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "InGame") {
            StartCoroutine(SpawnNPC());
        }
        
    }

    IEnumerator SpawnNPC()
    {
        while (true)
        {
            if (currentNPC == null && PersistentManager.Instance.isNowMalam)
            {
                yield return new WaitForSeconds(spawnTime);
                currentNPC = Instantiate(premanPrefabs, transform.position, Quaternion.identity);
                PremanButoAI premanButoAI = currentNPC.GetComponent<PremanButoAI>();
                premanButoAI.SetupNPC(merchantManager); // Kirim referensi MerchantManager ke NPC
            }
        }
    }
}
