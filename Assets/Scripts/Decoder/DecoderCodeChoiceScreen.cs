using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Injection;

public class DecoderCodeChoiceScreen : MonoBehaviour
{
    [Inject] DecoderController controller;
    [SerializeField] List<CodeSO> codes = new List<CodeSO>();
    [SerializeField] TMP_Text text;

    [SerializeField] Material baseMat;
    [SerializeField] Material rebootMat;

    MeshRenderer meshRenderer;
    private int currentCodeIndex = 0;

    public void Init()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        text.text = CreateCodeName(codes[currentCodeIndex]);
        controller.OnCodeChanged(codes[currentCodeIndex]);
    }

    private void OnEnable()
    {
        controller.rebootingStarted += Reboot;
        controller.rebootingFinished += RebootEnd;
    }


    void Reboot()
    {
        text.gameObject.SetActive(false);
    }

    void RebootEnd()
    {
        text.gameObject.SetActive(true);
    }

    public void NextCode()
    {
        if (controller.isRebooting) return;
        currentCodeIndex = (currentCodeIndex + 1) % codes.Count;
        text.text = CreateCodeName(codes[currentCodeIndex]);
        controller.OnCodeChanged(codes[currentCodeIndex]);
    }

    public void PreviousCode()
    {
        if (controller.isRebooting) return;
        currentCodeIndex--;
        if (currentCodeIndex == -1)
        {
            currentCodeIndex = codes.Count - 1;
        }
        text.text = CreateCodeName(codes[currentCodeIndex]);
        controller.OnCodeChanged(codes[currentCodeIndex]);
    }

    private string CreateCodeName(CodeSO code)
    {
        return $"{code.codeName}";
    }
}
