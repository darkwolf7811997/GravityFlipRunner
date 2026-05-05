using UnityEngine;

public class GeneralSpawnDirector : MonoBehaviour
{
    [Header("Spawn Points Obstacles")]
    [SerializeField] private Transform groundSpawnPoint;
    [SerializeField] private Transform ceilingSpawnPoint;

    [Header("Spawn Points Coins")]
    [SerializeField] private Transform groundCoinPoint;
    [SerializeField] private Transform middleCoinPoint;
    [SerializeField] private Transform ceilingCoinPoint;

    [Header("Prefabs Ground")]
    [SerializeField] private GameObject[] groundObstacles;

    [Header("Prefabs Ceiling")]
    [SerializeField] private GameObject[] ceilingObstacles;

    [Header("Coin Prefab")]
    [SerializeField] private GameObject coinPrefab;

    [Header("Seguimiento")]
    [SerializeField] private Transform player;
    [SerializeField] private float followOffsetX = 18f;

    [Header("Tiempo de spawn")]
    [SerializeField] private float spawnIntervalMin = 1.2f;
    [SerializeField] private float spawnIntervalMax = 2.2f;

    [Header("Dificultad progresiva")]
    [SerializeField] private float intervalReduction = 0.02f;
    [SerializeField] private float minAllowedInterval = 0.75f;

    [Header("Control de lados")]
    [SerializeField] private bool allowSameSideTwice = true;
    [SerializeField] private float sameSideChance = 0.35f;

    [Header("Coins")]
    [SerializeField] private bool enableCoins = true;
    [SerializeField] private float coinSpawnChance = 0.65f;
    [SerializeField] private int minCoinsPerLine = 3;
    [SerializeField] private int maxCoinsPerLine = 8;
    [SerializeField] private float coinSpacing = 1.0f;
    [SerializeField] private float coinStartOffsetFromObstacle = 1.2f;

    [Header("Inicio de partida")]
    [SerializeField] private float coinStartDelay = 2.5f;

    [Header("Shield Power-Up")]
    [SerializeField] private bool enableShield = true;
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private float shieldSpawnEveryMeters = 1000f;
    [SerializeField] private float shieldStartDistance = 300f;

    private float nextShieldSpawnX;

    [Header("Speed Zones")]
    [SerializeField] private bool enableSpeedZones = true;
    [SerializeField] private GameObject speedZonePrefab;
    [SerializeField] private GameObject slowZonePrefab;
    [SerializeField] private float speedZoneStartDistance = 500f;
    [SerializeField] private float speedZoneMinDistance = 350f;
    [SerializeField] private float speedZoneMaxDistance = 600f;
    [SerializeField] private float boostZoneChance = 0.7f;

    private float nextSpeedZoneSpawnX;

    private float timer;
    private float currentSpawnInterval;
    private float runTime;
    private int lastSide = -1;

    private void Start()
    {
        FollowPlayer();
        SetNextSpawnTime();
        if (player != null)
            nextShieldSpawnX = player.position.x + shieldStartDistance;

        if (player != null)
        {
            nextSpeedZoneSpawnX = player.position.x + speedZoneStartDistance;
        }
    }

    private void Update()
    {
        FollowPlayer();

        runTime += Time.deltaTime;
        timer += Time.deltaTime;

        if (timer >= currentSpawnInterval)
        {
            SpawnEvent();
            timer = 0f;
            SetNextSpawnTime();
        }

        if (enableShield && shieldPrefab != null && player != null)
        {
            if (player.position.x >= nextShieldSpawnX)
            {
                SpawnShield();
                nextShieldSpawnX += shieldSpawnEveryMeters;
            }
        }

        if (enableSpeedZones && player != null)
        {
            if (player.position.x >= nextSpeedZoneSpawnX)
            {
                SpawnSpeedZone();
                nextSpeedZoneSpawnX += Random.Range(speedZoneMinDistance, speedZoneMaxDistance);
            }
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

    private void SpawnEvent()
    {
        int obstacleSide = ChooseSide();

        Transform obstaclePoint = null;

        if (obstacleSide == 0)
        {
            obstaclePoint = groundSpawnPoint;
            SpawnFromArray(groundObstacles, groundSpawnPoint);
        }
        else
        {
            obstaclePoint = ceilingSpawnPoint;
            SpawnFromArray(ceilingObstacles, ceilingSpawnPoint);
        }

        lastSide = obstacleSide;

        if (enableCoins && coinPrefab != null && runTime >= coinStartDelay && Random.value <= coinSpawnChance)
        {
            SpawnCoinsForObstacle(obstacleSide, obstaclePoint);
        }
    }

    private int ChooseSide()
    {
        if (lastSide == -1)
            return Random.Range(0, 2);

        if (allowSameSideTwice)
        {
            bool repeatSameSide = Random.value < sameSideChance;
            if (repeatSameSide)
                return lastSide;
        }

        return lastSide == 0 ? 1 : 0;
    }

    private void SpawnFromArray(GameObject[] obstacleArray, Transform point)
    {
        if (obstacleArray == null || obstacleArray.Length == 0 || point == null) return;

        int index = Random.Range(0, obstacleArray.Length);
        Instantiate(obstacleArray[index], point.position, Quaternion.identity);
    }

    private void SpawnCoinsForObstacle(int obstacleSide, Transform obstaclePoint)
    {
        Transform lanePoint = ChooseCoinLane(obstacleSide);
        if (lanePoint == null) return;

        int amount = Random.Range(minCoinsPerLine, maxCoinsPerLine + 1);

        float obstacleX = obstaclePoint.position.x;
        float y = lanePoint.position.y;

        for (int i = 0; i < amount; i++)
        {
            float x = obstacleX - coinStartOffsetFromObstacle - (i * coinSpacing);
            Vector3 spawnPos = new Vector3(x, y, 0f);
            Instantiate(coinPrefab, spawnPos, Quaternion.identity);
        }
    }

    private Transform ChooseCoinLane(int obstacleSide)
    {
        if (obstacleSide == 0)
        {
            int option = Random.Range(0, 2);
            return option == 0 ? middleCoinPoint : ceilingCoinPoint;
        }
        else
        {
            int option = Random.Range(0, 2);
            return option == 0 ? middleCoinPoint : groundCoinPoint;
        }
    }

    private void SetNextSpawnTime()
    {
        float min = Mathf.Max(minAllowedInterval, spawnIntervalMin);
        float max = Mathf.Max(min, spawnIntervalMax);

        currentSpawnInterval = Random.Range(min, max);

        spawnIntervalMin = Mathf.Max(minAllowedInterval, spawnIntervalMin - intervalReduction);
        spawnIntervalMax = Mathf.Max(minAllowedInterval, spawnIntervalMax - intervalReduction);
    }

    private void SpawnShield()
    {
        Transform lanePoint = Random.value > 0.5f ? groundCoinPoint : ceilingCoinPoint;

        if (lanePoint == null) return;

        Vector3 spawnPos = new Vector3(
            transform.position.x,
            lanePoint.position.y,
            0f
        );

        Instantiate(shieldPrefab, spawnPos, Quaternion.identity);
    }

    private void SpawnSpeedZone()
    {
        GameObject prefabToSpawn = null;

        if (Random.value <= boostZoneChance)
        {
            prefabToSpawn = speedZonePrefab;
        }
        else
        {
            prefabToSpawn = slowZonePrefab;
        }

        if (prefabToSpawn == null) return;

        Transform lanePoint = Random.value > 0.5f ? groundCoinPoint : ceilingCoinPoint;

        if (lanePoint == null) return;

        Vector3 spawnPos = new Vector3(
            transform.position.x,
            lanePoint.position.y,
            0f
        );

        Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
    }
}