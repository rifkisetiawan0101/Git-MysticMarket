using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private Canvas collectCanvas; // Referensi ke Canvas collect
    [SerializeField] private float holdTime = 1f; // Waktu yang dibutuhkan untuk menahan tombol
    private bool isPlayerInRange = false;
    private float holdTimer = 0f;
    private ItemType itemType; // Jenis item yang dikumpulkan

    public enum ItemType
    {
        Batu,
        Kayu,
        TanahLiat
    }

    // Set jenis item
    public void SetItemType(ItemType type)
    {
        itemType = type;
    }

    private void Start()
    {
        if (collectCanvas != null)
        {
            collectCanvas.gameObject.SetActive(false); // Pastikan canvas tidak aktif saat awal
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetMouseButton(0)) // Cek jika mouse button ditekan
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= holdTime)
            {
                CollectItem();
            }
        }
        else
        {
            holdTimer = 0f; // Reset timer jika tidak menahan tombol
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger");
            isPlayerInRange = true;
            if (collectCanvas != null)
            {
                collectCanvas.gameObject.SetActive(true); // Tampilkan canvas collect
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (collectCanvas != null)
            {
                collectCanvas.gameObject.SetActive(false); // Sembunyikan canvas collect
            }
        }
    }

    private void CollectItem()
    {
        CollectableManager.Instance.AddItem(itemType, 1);
        Destroy(gameObject); // Hancurkan collectable setelah diambil
    }
}
