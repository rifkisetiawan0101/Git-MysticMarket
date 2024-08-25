using UnityEngine;
using UnityEngine.UI;

public class SerangButoButtonHandler : MonoBehaviour
{
    private PremanButoAI premanButoAI;

    private void Start()
    {
        premanButoAI = FindObjectOfType<PremanButoAI>(); // Cari objek dengan script PremanButoAI di scene

        if (premanButoAI != null)
        {
            GetComponent<Button>().onClick.AddListener(() => premanButoAI.SerangButo());
        }
    }
}
