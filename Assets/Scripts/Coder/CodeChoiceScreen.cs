using Injection;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CodeChoiceScreen : MonoBehaviour
{
    [Inject] DecoderController decoderController;
    [SerializeField] List<CodeSO> codes = new List<CodeSO>();
    [SerializeField] TMP_Text text;
    private int currentCodeIndex = 0;

    public void Init()
    {
        text.text = CreateCodeName(codes[currentCodeIndex]);
        OnCodeChanged(codes[currentCodeIndex]);
    }

    public static event Action<CodeSO> codeChanged;

    private void OnCodeChanged(CodeSO code)
    {
        codeChanged?.Invoke(code);
    }

    public void NextCode()
    {
        currentCodeIndex++;
        if (currentCodeIndex == codes.Count)
        {
            currentCodeIndex = 0;
        }
        text.text = CreateCodeName(codes[currentCodeIndex]);
        OnCodeChanged(codes[currentCodeIndex]);
    }

    public void PreviousCode()
    {
        currentCodeIndex--;
        if (currentCodeIndex == -1)
        {
            currentCodeIndex = codes.Count - 1;
        }
        text.text = CreateCodeName(codes[currentCodeIndex]);
        OnCodeChanged(codes[currentCodeIndex]);
    }

    private string CreateCodeName(CodeSO code)
    {
        return $"{code.codeName} / {code.codeStrength} / {code.codingTime}s";
    }

    void GetNewSignal(SignalSO signal)
    {
        OnCodeChanged(codes[currentCodeIndex]);
    }

    private void OnEnable()
    {
        decoderController.sentSignal += GetNewSignal;
    }

    private void OnDisable()
    {
        decoderController.sentSignal += GetNewSignal;
    }
}
