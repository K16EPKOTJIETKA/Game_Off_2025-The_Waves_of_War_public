using Injection;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DecoderInfoScreen : MonoBehaviour
{
    [Inject] DecoderController controller;
    [SerializeField] TMP_Text text;
    [SerializeField] Image image;
    [SerializeField] float imagePercent = 0.01f;

    [SerializeField] Material baseInfoMat;
    [SerializeField] Material baseOutputMat;
    [SerializeField] Material baseCodeMat;
    [SerializeField] Material errorMat;
    [SerializeField] Material rebootMat;

    MeshRenderer meshRenderer;
    Material[] mats;

    public void Init()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        text.text = string.Empty;
        image.fillAmount = 0;
        mats = meshRenderer.materials;
    }

    void CleanScreen(SignalSO signal)
    {
        text.text = string.Empty;
        image.gameObject.SetActive(false);
        image.fillAmount = 0;
    }
    void Error()
    {
        mats[3] = errorMat;
        meshRenderer.materials = mats;
        //meshRenderer.material = errorMat;
        text.text = string.Empty;
        image.gameObject.SetActive(false);
        image.fillAmount = 0;
        StopAllCoroutines();
    }

    void Reboot()
    {
        mats[1] = rebootMat;
        mats[2] = rebootMat;
        mats[3] = rebootMat;
        meshRenderer.materials = mats;
        //meshRenderer.material = rebootMat;
        text.text = string.Empty;
        image.gameObject.SetActive(false);
        image.fillAmount = 0;
        StopAllCoroutines();
    }

    void RebootEnd()
    {
        mats[1] = baseOutputMat;
        mats[2] = baseCodeMat;
        mats[3] = baseInfoMat;
        meshRenderer.materials = mats;
        //meshRenderer.material = baseInfoMat;

    }

    void OutputInfo()
    {
        if (controller.currentSignalData == null) return;
        if (controller.currentSignalData.isPicture)
        {
            text.text = string.Empty;
            image.sprite = controller.currentSignalData.image;
            image.gameObject.SetActive(true);
            StartCoroutine(OutputImageRoutine());
            return;
        }
        image.gameObject.SetActive(false);
        text.text = string.Empty;
        StartCoroutine(OutputTextRoutine());
    }

    IEnumerator OutputImageRoutine()
    {
        bool isOutputingEnd = false;
        while (!isOutputingEnd)
        {
            image.fillAmount += imagePercent;
            yield return new WaitForSeconds(controller.currentSignalData.decoderCode.codingTime / (1 / imagePercent));
            if (image.fillAmount >= 1)
            {
                isOutputingEnd = true;
            }
        }
        controller.OnCanSendSignalEvent();
    }

    IEnumerator OutputTextRoutine()
    {
        for (int i = 0; i < controller.currentSignalData.text.Length; i++)
        {
            text.text += controller.currentSignalData.text[i];
            yield return new WaitForSeconds((float)controller.currentSignalData.decoderCode.codingTime / controller.currentSignalData.text.Length);
        }
        controller.OnCanSendSignalEvent();
    }

    private void OnEnable()
    {
        controller.wasError += Error;
        controller.rebootingStarted += Reboot;
        controller.rebootingFinished += RebootEnd;
        controller.decodingStarted += OutputInfo;
        controller.sentSignal += CleanScreen;
    }

    private void OnDisable()
    {
        controller.wasError -= Error;
        controller.rebootingStarted -= Reboot;
        controller.rebootingFinished -= RebootEnd;
        controller.decodingStarted -= OutputInfo;
        controller.sentSignal -= CleanScreen;
    }

}
