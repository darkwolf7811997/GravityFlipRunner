using UnityEngine;

public class LoopingScenarioSegment : MonoBehaviour
{
    [Header("Loop")]
    [SerializeField] private float width = 90f;
    [SerializeField] private Transform target;
    [SerializeField] private float recycleExtraDistance = 30f;

    [Header("Índice del segmento")]
    [SerializeField] private int segmentIndex = 0;

    [Header("Escenarios")]
    [SerializeField] private ScenarioManager scenarioManager;

    private ScenarioSegmentVisual visual;

    private void Awake()
    {
        visual = GetComponent<ScenarioSegmentVisual>();
    }

    private void Start()
    {
        // Coloca automáticamente los 3 segmentos en fila:
        // 0, 90, 180
        transform.position = new Vector3(
            segmentIndex * width,
            transform.position.y,
            transform.position.z
        );

        ApplyScenario();
    }

    private void Update()
    {
        if (target == null) return;

        float rightEdge = transform.position.x + (width * 0.5f);

        if (rightEdge < target.position.x - recycleExtraDistance)
        {
            Reposition();
        }
    }

    private void Reposition()
    {
        transform.position = new Vector3(
            transform.position.x + width * 3f,
            transform.position.y,
            transform.position.z
        );

        segmentIndex += 3;

        ApplyScenario();
    }

    private void ApplyScenario()
    {
        if (scenarioManager == null || visual == null) return;

        ScenarioManager.Scenario scenario = scenarioManager.GetScenarioForSegmentIndex(segmentIndex);

        visual.Setup(scenario, segmentIndex);
    }
}