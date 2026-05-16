using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    [System.Serializable]
    public class Scenario
    {
        public string scenarioName;
        public Sprite groundSprite;
        public Sprite ceilingSprite;
        public Sprite[] backgroundSprites;
    }

    [Header("Escenario inicial")]
    [SerializeField] private Scenario firstScenario;

    [Header("Escenarios aleatorios")]
    [SerializeField] private Scenario[] randomScenarios;

    [Header("Duración por escenario")]
    [SerializeField] private int minSegments = 2;
    [SerializeField] private int maxSegments = 4;

    private List<Scenario> scenarioBySegmentIndex = new List<Scenario>();

    private Scenario currentScenario;
    private Scenario previousScenario;
    private int remainingSegments;

    private void Awake()
    {
        currentScenario = firstScenario;
        previousScenario = firstScenario;
        remainingSegments = Random.Range(minSegments, maxSegments + 1);
    }

    public Scenario GetScenarioForSegmentIndex(int segmentIndex)
    {
        while (scenarioBySegmentIndex.Count <= segmentIndex)
        {
            if (remainingSegments <= 0)
            {
                ChooseNextScenario();
            }

            scenarioBySegmentIndex.Add(currentScenario);
            remainingSegments--;
        }

        return scenarioBySegmentIndex[segmentIndex];
    }

    private void ChooseNextScenario()
    {
        if (randomScenarios == null || randomScenarios.Length == 0)
        {
            currentScenario = firstScenario;
            remainingSegments = Random.Range(minSegments, maxSegments + 1);
            return;
        }

        Scenario newScenario = currentScenario;
        int safety = 20;

        while (safety > 0)
        {
            int index = Random.Range(0, randomScenarios.Length);
            newScenario = randomScenarios[index];

            if (newScenario != previousScenario)
            {
                break;
            }

            safety--;
        }

        currentScenario = newScenario;
        previousScenario = currentScenario;
        remainingSegments = Random.Range(minSegments, maxSegments + 1);
    }
}