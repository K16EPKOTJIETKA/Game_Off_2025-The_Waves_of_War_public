using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class CodeScreen : MonoBehaviour
{
    [SerializeField] SignalsScreen signalsScreen;
    [SerializeField] TMP_Text outputText;
    [SerializeField] int outputTime;
    int symbolsCount;
    int symbolsFromPicture = 50;
    string generatedText;
    CodeSO code;
    SignalSO signal;

    bool isOutputing;

    public static event Action<CodeSO, SignalSO> canSendSignalEvent;

    private void OnCanSendSignalEvent(CodeSO code, SignalSO signal)
    {
        canSendSignalEvent?.Invoke(code, signal);
    }

    public void OutputText()
    {
        if (signalsScreen == null || signal == null)
        {
            return;
        }
        if (signalsScreen.sendingSignals != null && signalsScreen.sendingSignals.TryGetValue(signal.id, out SignalSO sendingSignal))
        {
            return;
        }
        StopAllCoroutines();
        outputText.text = "";
        if (signal == null || code == null) return;
        if (signal.isPicture)
            symbolsCount = symbolsFromPicture * (int)code.codeStrength;
        else 
            symbolsCount = signal.text.Length * (int)code.codeStrength;
        generatedText = TextGenerator.GenerateRandomText(symbolsCount);
        StartCoroutine(OutputTextRoutine(code));
        
    }

    IEnumerator OutputTextRoutine(CodeSO code)
    {
        isOutputing = true;
        for (int i = 0; i < symbolsCount; i++)
        {
            outputText.text += generatedText[i];
            yield return new WaitForSeconds(code.codingTime / symbolsCount);
        }
        OnCanSendSignalEvent(code, signal);
        isOutputing = false;
    }

    private void OnEnable()
    {
        SignalsScreen.newSignalSelected += ChangeCurrentSignal;
        SignalsScreen.signalDeletedFromList += DeleteSymbolsFromScreen;
        CodeChoiceScreen.codeChanged += ChangeCurrentCode;
    }

    private void OnDisable()
    {
        SignalsScreen.newSignalSelected -= ChangeCurrentSignal;
        SignalsScreen.signalDeletedFromList -= DeleteSymbolsFromScreen;
        CodeChoiceScreen.codeChanged -= ChangeCurrentCode;
    }

    void ChangeCurrentSignal(SignalSO newSignal)
    {
        signal = newSignal;
        StopAllCoroutines();
    }

    void DeleteSymbolsFromScreen(SignalSO deleteSignal)
    {
        if (signal != deleteSignal) return;
        Debug.Log("Delete text...");
        outputText.text = "";
        StopAllCoroutines();
    }

    void ChangeCurrentCode(CodeSO newCode)
    {
        code = newCode;
    }

}
