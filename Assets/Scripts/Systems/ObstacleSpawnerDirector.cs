using UnityEngine;

public class ObstacleSpawnerDirector : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] private Transform groundSpawnPoint;
    [SerializeField] private Transform ceilingSpawnPoint;

    [Header("Prefabs Ground")]
    [SerializeField] private GameObject[] groundObstacles;

    [Header("Prefabs Ceiling")]
    [SerializeField] private GameObject[] ceilingObstacles;

    [Header("Seguimiento")]
    [SerializeField] private Transform player;
    [SerializeField] private float followOffsetX = 14f;

    [Header("Tiempo de spawn")]
    [SerializeField] private float spawnIntervalMin = 1.2f;
    [SerializeField] private float spawnIntervalMax = 2.2f;

    [Header("Dificultad progresiva")]
    [SerializeField] private float intervalReduction = 0.02f;
    [SerializeField] private float minAllowedInterval = 0.75f;

    [Header("Control de lados")]
    [SerializeField] private bool allowSameSideTwice = true;
    [SerializeField] private float sameSideChance = 0.35f;

    private float timer;
    private float currentSpawnInterval;
    private int lastSide = -1; // 0 = ground, 1 = ceiling

    private void Start()
    {
        SetNextSpawnTime();
    }

    private void Update()
    {
        FollowPlayer();

        timer += Time.deltaTime;

        if (timer >= currentSpawnInterval)
        {
            SpawnNextObstacle();
            timer = 0f;
            SetNextSpawnTime();
        }
    }

    private void FollowPlayer()
    {
        if (player == null) return;

        transform.position = new Vector3(
            player.position.x + followOffsetX,
            transform.position.y,
            transform.position.z
        );
    }

    private void SpawnNextObstacle()
    {
        int side = ChooseSide();

        if (side == 0)
        {
            SpawnFromArray(groundObstacles, groundSpawnPoint);
        }
        else
        {
            SpawnFromArray(ceilingObstacles, ceilingSpawnPoint);
        }

        lastSide = side;
    }

    private int ChooseSide()
    {
        if (lastSide == -1)
        {
            return Random.Range(0, 2);
        }

        if (allowSameSideTwice)
        {
            bool repeatSameSide = Random.value < sameSideChance;
            if (repeatSameSide)
            {
                return lastSide;
            }
        }

        return lastSide == 0 ? 1 : 0;
    }

    private void SpawnFromArray(GameObject[] obstacleArray, Transform point)
    {
        if (obstacleArray == null || obstacleArray.Length == 0 || point == null) return;

        int index = Random.Range(0, obstacleArray.Length);
        Instantiate(obstacleArray[index], point.position, Quaternion.identity);
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