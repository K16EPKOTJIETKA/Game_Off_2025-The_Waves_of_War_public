using Injection;
using TMPro;
using UnityEngine;

public class DemodulatorSignalScreen : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;

    public ISignalFormula activeFormula;
    [Inject] DemodulatorController demodulatorController;
    public SignalParameters currentSignalParameters;

    [Header("Graph Settings")]
    public int resolution = 100;
    public float graphWidth = 10f;
    public float graphAmplitude = 5f;

    MeshRenderer meshRenderer;

    [SerializeField] GameObject signalLine;
    [SerializeField] Material baseSignalScreenMat;
    [SerializeField] Material baseTextScreenMat;
    [SerializeField] Material errorMat;
    [SerializeField] Material rebootMat;
    [SerializeField] TMP_Text incorrectProtocol;
    Material[] mats;

    public void Init()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        mats = meshRenderer.materials;


    }

    void CleanScreen(SignalSO signal, string str)
    {
        signalLine.SetActive(false);
    }
    void ErrorScreen()
    {
        signalLine.SetActive(false);
        mats[1] = errorMat;
        mats[2] = errorMat;
        meshRenderer.materials = mats;
    }

    void Reboot()
    {
        mats[1] = rebootMat;
        mats[2] = rebootMat;
        meshRenderer.materials = mats;
        signalLine.SetActive(false);
    }

    void RebootEnd()
    {
        mats[1] = baseSignalScreenMat;
        mats[2] = baseTextScreenMat;
        meshRenderer.materials = mats;
        if (demodulatorController.currentSignalData == null) return;
        signalLine.SetActive(true);
    }

    private void OnEnable()
    {
        demodulatorController.wasError += ErrorScreen;
        demodulatorController.rebootingStarted += Reboot;
        demodulatorController.rebootingEnded += RebootEnd;
        demodulatorController.newSignalWasSet += SetNewSignal;
        demodulatorController.sentSignal += CleanScreen;
    }

    private void OnDisable()
    {
        demodulatorController.wasError -= ErrorScreen;
        demodulatorController.rebootingStarted -= Reboot;
        demodulatorController.rebootingEnded -= RebootEnd;
        demodulatorController.newSignalWasSet -= SetNewSignal;
        demodulatorController.sentSignal -= CleanScreen;
    }
    void SetNewSignal(SignalSO signal)
    {
        if (!demodulatorController.isRebooting && !demodulatorController.isError)
            signalLine.SetActive(true);
        currentSignalParameters = signal.GetParameters(Time.time);
        activeFormula = SignalFormulaFactory.GetFormula(signal.formula);
    }

    public void UpdateGraph()
    {
        if (demodulatorController.currentSignalData == null) return;

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
