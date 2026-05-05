using UnityEngine;

public class CoinSpawnDirector : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] private Transform groundSpawnPoint;
    [SerializeField] private Transform ceilingSpawnPoint;

    [Header("Coin Prefab")]
    [SerializeField] private GameObject coinPrefab;

    [Header("Spawn Timing")]
    [SerializeField] private float minSpawnInterval = 1.2f;
    [SerializeField] private float maxSpawnInterval = 2.5f;

    [Header("Coin Line Settings")]
    [SerializeField] private int minCoinsPerLine = 2;
    [SerializeField] private int maxCoinsPerLine = 5;
    [SerializeField] private float horizontalSpacing = 1.2f;

    [Header("Vertical Offset")]
    [SerializeField] private float groundYOffset = 0.8f;
    [SerializeField] private float ceilingYOffset = -0.8f;

    private float timer;
    private float nextSpawnTime;

    private void Start()
    {
        SetNextSpawnTime();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= nextSpawnTime)
        {
            SpawnCoinLine();
            timer = 0f;
            SetNextSpawnTime();
        }
    }

    private void SetNextSpawnTime()
    {
        nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void SpawnCoinLine()
    {
        bool spawnOnGround = Random.value > 0.5f;

        Transform spawnReference = spawnOnGround ? groundSpawnPoint : ceilingSpawnPoint;
        float yOffset = spawnOnGround ? groundYOffset : ceilingYOffset;

        int amount = Random.Range(minCoinsPerLine, maxCoinsPerLine + 1);

        Vector3 startPos = spawnReference.position + new Vector3(0f, yOffset, 0f);

        for (int i = 0; i < amount; i++)
        {
            Vector3 spawnPos = startPos + new Vector3(i * horizontalSpacing, 0f, 0f);
            Instantiate(coinPrefab, spawnPos, Quaternion.identity);
        }
    }
}