using Injection;
using System.Collections;
using TMPro;
using UnityEngine;

public class DemodulatorTextScreen : MonoBehaviour
{
    [Inject] DemodulatorController controller;
    [Inject] ShopManager shopManager;

    [SerializeField] TMP_Text outputText;
    [SerializeField] int outputTime;

    MeshRenderer meshRenderer;
    [SerializeField] Material baseMat;
    [SerializeField] Material errorMat;
    [SerializeField] Material rebootMat;
    string generatedText;


    Material[] mats;

    public void OutputText()
    {
        if (controller.isError || controller.currentSignalData == null) return;
        outputText.text = string.Empty;
        generatedText = TextGenerator.GenerateRandomText(controller.currentSignalData.countOfDemodulationCharacters);
        int codePlace = Random.Range(0, generatedText.Length);
        generatedText = generatedText.Insert(codePlace, controller.currentSignalData.decoderCode.name);
        StartCoroutine(OutputTextRoutine());

    }

    public void Init()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        mats = meshRenderer.materials;
    }

    void ErrorScreen()
    {
        mats[2] = errorMat;
        meshRenderer.materials = mats;
        outputText.text = "";
        StopAllCoroutines();
    }

    void Reboot()
    {
        mats[2] = rebootMat;
        meshRenderer.materials = mats;
        StopAllCoroutines();
    }

    void RebootEnd()
    {
        mats[2] = baseMat;
        meshRenderer.materials = mats;
    }

    void CleanScreen(SignalSO signal)
    {
        outputText.text = "";
    }

    void CleanScreen(SignalSO signal, string str)
    {
        outputText.text = "";
    }

    void DecreaseOutputTime(ShopItemData shopItem)
    {
        if (shopItem.itemID == 6)
        {
            outputTime -= (int)(outputTime * 0.2f);
        }
        if (shopItem.itemID == 7)
        {
            outputTime -= (int)(outputTime * 0.4f);
        }
    }

    IEnumerator OutputTextRoutine()
    {
        for (int i = 0; i < generatedText.Length; i++)
        {
            outputText.text += generatedText[i];
            yield return new WaitForSeconds((float)outputTime / generatedText.Length);
        }
        controller.outputText = generatedText;
        controller.OnCanSendSignalEvent();
    }

    private void OnEnable()
    {
        controller.activatedRightProtocol += OutputText;
        controller.wasError += ErrorScreen;
        controller.rebootingStarted += Reboot;
        controller.rebootingEnded += RebootEnd;
        controller.newSignalWasSet += CleanScreen;
        controller.sentSignal += CleanScreen;
        shopManager.productPurchased += DecreaseOutputTime;
    }

    private void OnDisable()
    {
        controller.activatedRightProtocol -= OutputText;
        controller.wasError -= ErrorScreen;
        controller.rebootingStarted -= Reboot;
        controller.rebootingEnded -= RebootEnd;
        controller.newSignalWasSet -= CleanScreen;
        controller.sentSignal -= CleanScreen;
        shopManager.productPurchased -= DecreaseOutputTime;
    }

}
