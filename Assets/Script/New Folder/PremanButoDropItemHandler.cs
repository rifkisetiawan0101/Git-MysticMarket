using UnityEngine;

public class PremanButoDropItemHandler : MonoBehaviour
{
    private PremanButoAI premanButoAI;

    private void Start()
    {
        premanButoAI = GetComponentInParent<PremanButoAI>(); // Mendapatkan referensi ke parent

        if (premanButoAI == null)
        {
            Debug.LogWarning("PremanButoAI tidak ditemukan pada parent.");
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("Drop item diklik");
        if (premanButoAI != null)
        {
            premanButoAI.CollectDropItem(); // Memanggil fungsi untuk mengumpulkan drop item
        }
    }
}
