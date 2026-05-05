using UnityEngine;

public class LoopingSegment : MonoBehaviour
{
    [Header("Loop")]
    public float width = 90f;
    public Transform target;

    [Header("Índice del segmento")]
    public int segmentIndex = 0;

    [Header("Tipo de segmento")]
    public bool isCeiling = false;

    [Header("Escenarios")]
    public ScenarioManager scenarioManager;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        ApplyScenario();
    }

    private void Update()
    {
        if (target == null) return;

        if (transform.position.x + width < target.position.x)
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
        if (scenarioManager == null || sr == null) return;

        ScenarioManager.Scenario scenario = scenarioManager.GetScenarioForSegmentIndex(segmentIndex);

        Sprite spriteToUse = isCeiling
            ? scenario.ceilingSprite
            : scenario.groundSprite;

        if (spriteToUse != null)
        {
            sr.sprite = spriteToUse;
        }
    }
}