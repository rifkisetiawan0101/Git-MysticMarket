using UnityEngine;
using TMPro;

public class Koin : MonoBehaviour
{
    public TextMeshProUGUI koinUI;

    private void Update()
    {
        UpdateKoinUI();
    }

    public void UpdateKoin(float amount)
    {
        PersistentManager.Instance.UpdateKoin(amount);  // Update nilai koin di PersistentManager
        UpdateKoinUI();
    }

    private void UpdateKoinUI()
    {
        koinUI.text = PersistentManager.Instance.Koins.ToString("N0") + "K";  // Menampilkan nilai koin
    }
}
