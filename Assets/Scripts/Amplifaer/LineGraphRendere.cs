using UnityEngine;
using Injection;

[RequireComponent(typeof(LineRenderer))]
public class LineGraphRendere : MonoBehaviour
{
    private LineRenderer lineRenderer;

    public ISignalFormula activeFormula; 
    public SignalSO currentSignalData;
    public SignalParameters currentSignalParameters;

    [Inject] SignalManager signalManager;

    [Header("Graph Settings")]
    public int resolution = 100;
    public float graphWidth = 10f; 
    public float graphAmplitude = 5f; 

    public void InitAwake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        signalManager.newSignalWasSet += SetNewSignal;
    }

    private void OnDisable()
    {
        signalManager.newSignalWasSet -= SetNewSignal;
    }

    void SetNewSignal(SignalSO newSignal)
    {
        currentSignalData = newSignal;
        currentSignalParameters = currentSignalData.GetParameters(Time.time);
        activeFormula = SignalFormulaFactory.GetFormula(currentSignalData.formula);
    }

    public void UpdateGraph()
    {
        if (currentSignalData == null) return;

        if (activeFormula == null)
        {
            return;
        }

        lineRenderer.positionCount = resolution;

        for (int i = 0; i < resolution; i++)
        {
            float t = (float)i / (resolution - 1);
            float x = t * graphWidth;

            float y = activeFormula.GetValue(Time.time + t, currentSignalParameters) * graphAmplitude;

            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }

    void Update()
    {
        UpdateGraph();
    }
}
