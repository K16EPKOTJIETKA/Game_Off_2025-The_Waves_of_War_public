using System.Collections;
using UnityEngine;
using Injection;

public class SatelliteTunerScreen : MonoBehaviour
{
    [Inject] SatelliteTunerController controller;

    [SerializeField] GameObject tunerPoint;
    [SerializeField] GameObject verticalLine;
    [SerializeField] GameObject horizontalLine;

    [SerializeField] GameObject okImage;
    [SerializeField] GameObject failImage;

    [SerializeField] Transform rigntXBorder;
    [SerializeField] Transform leftXBorder;
    [SerializeField] Transform upYBorder;
    [SerializeField] Transform downYBorder;

    [SerializeField] Material baseMat;
    [SerializeField] Material correctMat;
    [SerializeField] Material failMat;

    [SerializeField] int finishBlimingCount = 3;
    [SerializeField] float finishBlimingTime = 1;
    [SerializeField] float finishScreenTime = 2;



    Vector3 tunerPointScale;
    MeshRenderer meshRenderer;
    Material[] mats;
    [SerializeField] AudioSource completeSound;
    [SerializeField] AudioSource failSound;
    void PlayCompleteSound()
    {
        if (completeSound == null) return;
        completeSound.Play();
    }

    void PlayFailSound()
    {
        if (failSound == null) return;
        failSound.Play();
    }
    public void Init()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        mats = meshRenderer.materials;
        tunerPointScale = tunerPoint.transform.localScale;
    }
    Vector3 ChooseSpawnPosition()
    {
        float x = Random.Range(leftXBorder.position.x,rigntXBorder.position.x);
        float y = Random.Range(downYBorder.position.y, upYBorder.position.y);
        return new Vector3(x,y, tunerPoint.transform.position.z);
    }
    
    void SpawnTunerPoint()
    {
        tunerPoint.transform.localScale = new Vector3(tunerPointScale.x * controller.tunerPointSizeKoef, tunerPointScale.y * controller.tunerPointSizeKoef, tunerPointScale.z);
        tunerPoint.transform.position = ChooseSpawnPosition();
        tunerPoint.SetActive(true);
    }

    void StartHorizontalLine()
    {
        int side = Random.Range(0, 2);
        if (side == 0)
            horizontalLine.transform.position = new Vector3(leftXBorder.position.x, horizontalLine.transform.position.y, horizontalLine.transform.position.z);
        else horizontalLine.transform.position = new Vector3(rigntXBorder.position.x, horizontalLine.transform.position.y, horizontalLine.transform.position.z);
        horizontalLine.SetActive(true);
    }

    void StartVerticalLine()
    {
        int side = Random.Range(0, 2);
        if (side == 0)
            verticalLine.transform.position = new Vector3(verticalLine.transform.position.x, downYBorder.position.y, verticalLine.transform.position.z);
        else verticalLine.transform.position = new Vector3(verticalLine.transform.position.x, upYBorder.position.y, verticalLine.transform.position.z);
        verticalLine.SetActive(true);
    }

    int buffer = -1;
    public void StartLine()
    {

        int line = Random.Range(0, 2);

        if (line == buffer)
        {
            line = 1 - line;
        }


        if (line == 0)
        {
            StartHorizontalLine();
            buffer = 0;
        }
        else 
        {
            StartVerticalLine();
            buffer = 1; 
        }
    }

    void TuningFinish()
    {
        StartCoroutine(TuningFinishingRoutine());
    }

    IEnumerator TuningFinishingRoutine()
    {
        for (int i = 0; i < finishBlimingCount; i++)
        {
            tunerPoint.SetActive(false);
            verticalLine.SetActive(false);
            horizontalLine.SetActive(false);
            yield return new WaitForSeconds(finishBlimingTime);
            tunerPoint.SetActive(true);
            verticalLine.SetActive(true);
            horizontalLine.SetActive(true);
            yield return new WaitForSeconds(finishBlimingTime);
        }
        tunerPoint.SetActive(false);
        verticalLine.SetActive(false);
        horizontalLine.SetActive(false);

        PlayCompleteSound();
        mats[2] = correctMat;
        meshRenderer.materials = mats;
        okImage.SetActive(true);
        yield return new WaitForSeconds(finishScreenTime);
        controller.isFinishing = false;
        mats[2] = baseMat;
        meshRenderer.materials = mats;
        okImage.SetActive(false);

    }

    IEnumerator TuninngFailingRoutine()
    {
        mats[2] = failMat;
        meshRenderer.materials = mats;
        failImage.SetActive(true);
        PlayFailSound();
        yield return new WaitForSeconds(finishScreenTime);
        controller.isFailing = false;
        mats[2] = baseMat;
        meshRenderer.materials = mats;
        failImage.SetActive(false);
    }

    void TuningFail()
    {
        tunerPoint.SetActive(false);
        verticalLine.SetActive(false);
        horizontalLine.SetActive(false);
        StartCoroutine(TuninngFailingRoutine());

    }

    private void OnEnable()
    {
        controller.tuningStarted += SpawnTunerPoint;
        controller.tuningStarted += StartLine;
        controller.tuningFailed += TuningFail;
        controller.tuningFinished += TuningFinish;

        
    }

    private void OnDisable()
    {
        controller.tuningStarted -= SpawnTunerPoint;
        controller.tuningStarted -= StartLine;
        controller.tuningFailed -= TuningFail;
        controller.tuningFinished -= TuningFinish;
    }


}
