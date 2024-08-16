using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    public static CollectableManager Instance { get; private set; }

    [SerializeField] private GameObject batuPrefab;
    [SerializeField] private GameObject kayuPrefab;
    [SerializeField] private GameObject tanahLiatPrefab;
    [SerializeField] private float spawnRadius = 3000f; // Radius spawn dalam unit
    [SerializeField] private int minSpawnCount = 10; // Jumlah minimal spawn
    [SerializeField] private int maxSpawnCount = 20; // Jumlah maksimal spawn

    private int batuCount = 0;
    private int kayuCount = 0;
    private int tanahLiatCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: jika ingin inventory persist melalui scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SpawnCollectables();
    }

    private void SpawnCollectables()
    {
        int batuSpawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);
        int kayuSpawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);
        int tanahLiatSpawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);

        SpawnCollectable(batuPrefab, batuSpawnCount, Collectable.ItemType.Batu);
        SpawnCollectable(kayuPrefab, kayuSpawnCount, Collectable.ItemType.Kayu);
        SpawnCollectable(tanahLiatPrefab, tanahLiatSpawnCount, Collectable.ItemType.TanahLiat);
    }

    private void SpawnCollectable(GameObject prefab, int count, Collectable.ItemType itemType)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 randomPosition = GetRandomPositionWithinCircle();
            GameObject collectable = Instantiate(prefab, randomPosition, Quaternion.identity);
            Collectable collectableComponent = collectable.GetComponent<Collectable>();
            if (collectableComponent != null)
            {
                collectableComponent.SetItemType(itemType);
            }
        }
    }

    private Vector3 GetRandomPositionWithinCircle()
    {
        float angle = Random.Range(0f, 2f * Mathf.PI); // Random angle in radians
        float radius = Random.Range(0f, spawnRadius); // Random distance from the center
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;
        return new Vector3(x, y, 0) + transform.position;
    }

    public void AddItem(Collectable.ItemType itemType, int amount)
    {
        switch (itemType)
        {
            case Collectable.ItemType.Batu:
                batuCount += amount;
                Debug.Log("Batu: " + batuCount);
                break;
            case Collectable.ItemType.Kayu:
                kayuCount += amount;
                Debug.Log("Kayu: " + kayuCount);
                break;
            case Collectable.ItemType.TanahLiat:
                tanahLiatCount += amount;
                Debug.Log("Tanah Liat: " + tanahLiatCount);
                break;
        }
    }

    public bool RemoveItem(Collectable.ItemType itemType, int amount)
    {
        switch (itemType)
        {
            case Collectable.ItemType.Batu:
                if (batuCount >= amount)
                {
                    batuCount -= amount;
                    Debug.Log("Batu: " + batuCount);
                    return true;
                }
                break;
            case Collectable.ItemType.Kayu:
                if (kayuCount >= amount)
                {
                    kayuCount -= amount;
                    Debug.Log("Kayu: " + kayuCount);
                    return true;
                }
                break;
            case Collectable.ItemType.TanahLiat:
                if (tanahLiatCount >= amount)
                {
                    tanahLiatCount -= amount;
                    Debug.Log("Tanah Liat: " + tanahLiatCount);
                    return true;
                }
                break;
        }
        return false;
    }

    public int GetItemCount(Collectable.ItemType itemType)
    {
        switch (itemType)
        {
            case Collectable.ItemType.Batu:
                return batuCount;
            case Collectable.ItemType.Kayu:
                return kayuCount;
            case Collectable.ItemType.TanahLiat:
                return tanahLiatCount;
            default:
                return 0;
        }
    }
}
