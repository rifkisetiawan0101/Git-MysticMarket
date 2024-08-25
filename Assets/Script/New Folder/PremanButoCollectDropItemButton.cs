using UnityEngine;

public class PremanButoCollectDropItem : MonoBehaviour
{
    private PremanButoAI premanButoAI;

    private void Start()
    {
        premanButoAI = GetComponentInParent<PremanButoAI>(); // Dapatkan referensi ke script PremanButoAI
    }

    public void OnCollectButtonClicked()
    {
        BatuAkik.batuAkik.updateBatuAkik(200); // Tambah batu akik sebesar 200
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.collectCoin);
        Destroy(premanButoAI.gameObject); // Hancurkan game object ini (PremanButoController)
    }
}
