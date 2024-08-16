using System;
using UnityEngine;

public class PersistentManager : MonoBehaviour
{
    public static PersistentManager Instance { get; private set; }

    public static event Action OnTotalKoinChanged;  // Tambahkan event ini

    public float Koins { get; private set; } = 1000;  // Nilai awal koin

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Jangan hancurkan saat berpindah scene
        }
        else
        {
            Destroy(gameObject); // Hancurkan jika instance sudah ada
        }
    }

    public void UpdateKoin(float amount)
    {
        Koins += amount;
        OnTotalKoinChanged?.Invoke();
        Debug.Log("Koin saat ini: " + Koins);
    }
}
