using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject[] obstacles;
    [SerializeField] private Transform spawnPoint;

    [Header("Tiempo de spawn")]
    [SerializeField] private float spawnIntervalMin = 1.2f;
    [SerializeField] private float spawnIntervalMax = 2.2f;

    [Header("Dificultad progresiva")]
    [SerializeField] private float intervalReduction = 0.02f;
    [SerializeField] private float minAllowedInterval = 0.75f;

    [Header("Separación")]
    [SerializeField] private float minDistanceFromAnySpawn = 4f;

    private float timer;
    private float currentSpawnInterval;

    // Compartido entre TODOS los spawners
    public static float lastGlobalSpawnX = -999f;

    private void Start()
    {
        SetNextSpawnTime();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentSpawnInterval)
        {
            TrySpawn();
            timer = 0f;
            SetNextSpawnTime();
        }
    }

    private void TrySpawn()
    {
        float currentX = spawnPoint.position.x;

        // Evita que dos spawners generen demasiado cerca en X
        if (Mathf.Abs(currentX - lastGlobalSpawnX) < minDistanceFromAnySpawn)
        {
            return;
        }

        SpawnObstacle();
        lastGlobalSpawnX = currentX;
    }

    private void SpawnObstacle()
    {
        if (obstacles == null || obstacles.Length == 0) return;

        int index = Random.Range(0, obstacles.Length);
        Instantiate(obstacles[index], spawnPoint.position, Quaternion.identity);
    }

    private void SetNextSpawnTime()
    {
        float min = Mathf.Max(minAllowedInterval, spawnIntervalMin);
        float max = Mathf.Max(min, spawnIntervalMax);

        currentSpawnInterval = Random.Range(min, max);

        spawnIntervalMin = Mathf.Max(minAllowedInterval, spawnIntervalMin - intervalReduction);
        spawnIntervalMax = Mathf.Max(minAllowedInterval, spawnIntervalMax - intervalReduction);
    }
}