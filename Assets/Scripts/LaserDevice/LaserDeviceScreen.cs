using Injection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDeviceScreen : MonoBehaviour
{
    [Inject] LaserDeviceController controller;
    [Inject] ShopManager shopManager;

    [SerializeField] GameObject tunerPoint;
    [SerializeField] GameObject verticalLine;
    [SerializeField] GameObject horizontalLine;
    [SerializeField] GameObject firstDiagonalLine;
    [SerializeField] GameObject secondDiagonalLine;

    [SerializeField] GameObject okImage;
    [SerializeField] GameObject failImage;


    [SerializeField] Transform rigntXBorder;
    [SerializeField] Transform leftXBorder;
    [SerializeField] Transform upYBorder;
    [SerializeField] Transform downYBorder;

    [SerializeField] Transform rightUpCorner;
    [SerializeField] Transform rightDownCorner;
    [SerializeField] Transform leftUpCorner;
    [SerializeField] Transform leftDownCorner;

    [SerializeField] Material baseMat;
    [SerializeField] Material correctMat;
    [SerializeField] Material failMat;

    [SerializeField] int finishBlimingCount = 3;
    [SerializeField] float finishBlimingTime = 1;
    [SerializeField] float finishScreenTime = 2;


    Vector3 tunerPointScale;
    MeshRenderer meshRenderer;
    Material[] mats;
    public void Init()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        mats = meshRenderer.materials;
        tunerPointScale = tunerPoint.transform.localScale;
    }
    Vector3 ChooseSpawnPosition()
    {
        tunerPoint.transform.localScale = new Vector3(tunerPointScale.x * controller.tunerPointSizeKoef, tunerPointScale.y * controller.tunerPointSizeKoef, tunerPointScale.z);
        float x = Random.Range(leftXBorder.position.x, rigntXBorder.position.x);
        float y = Random.Range(downYBorder.position.y, upYBorder.position.y);
        return new Vector3(x, y, tunerPoint.transform.position.z);
    }

    void SpawnTunerPoint()
    {
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

    void StartFirstDiagonalLine()
    {
        int corner = Random.Range(0, 2);
        if (corner == 0)
            firstDiagonalLine.transform.position = new Vector3(leftUpCorner.position.x, leftUpCorner.position.y, firstDiagonalLine.transform.position.z);
        else firstDiagonalLine.transform.position = new Vector3(rightDownCorner.position.x, rightDownCorner.position.y, firstDiagonalLine.transform.position.z);
        firstDiagonalLine.SetActive(true);
    }

    void StartSecondDiagonalLine()
    {
        int corner = Random.Range(0, 2);
        if (corner == 0)
            secondDiagonalLine.transform.position = new Vector3(leftDownCorner.position.x, leftDownCorner.position.y, secondDiagonalLine.transform.position.z);
        else secondDiagonalLine.transform.position = new Vector3(rightUpCorner.position.x, rightUpCorner.position.y, secondDiagonalLine.transform.position.z);
        secondDiagonalLine.SetActive(true);
    }

    int buffer = -1;
    private List<int> availableLines = new List<int>();
    public void StartLine()
    {

        if (availableLines.Count == 0)
        {
            availableLines.Add(0);
            availableLines.Add(1);
            availableLines.Add(2);
            availableLines.Add(3);
        }

        int randomIndex = Random.Range(0, availableLines.Count);

        int line = availableLines[randomIndex];

        availableLines.RemoveAt(randomIndex);

        switch (line)
        {
            case 0:
                StartHorizontalLine();
                break;
            case 1:
                StartVerticalLine();
                break;
            case 2:
                StartFirstDiagonalLine();
                break;
            case 3:
                StartSecondDiagonalLine();
                break;
        }


    }

    void TuningFinish()
    {
        StartCoroutine(TuningFinishingRoutine());
    }

    void SetLinesVisibility(bool flag)
    {
        tunerPoint.SetActive(flag);
        verticalLine.SetActive(flag);
        horizontalLine.SetActive(flag);
        firstDiagonalLine.SetActive(flag);
        secondDiagonalLine.SetActive(flag);
    }

    IEnumerator TuningFinishingRoutine()
    {
        for (int i = 0; i < finishBlimingCount; i++)
        {
            SetLinesVisibility(false);
            yield return new WaitForSeconds(finishBlimingTime);
            SetLinesVisibility(true);
            yield return new WaitForSeconds(finishBlimingTime);
        }
        SetLinesVisibility(false);

        mats[1] = correctMat;
        meshRenderer.materials = mats;
        okImage.SetActive(true);
        yield return new WaitForSeconds(finishScreenTime);
        controller.isFinishing = false;
        mats[1] = baseMat;
        meshRenderer.materials = mats;
        okImage.SetActive(false);

    }

    IEnumerator TuninngFailingRoutine()
    {
        mats[1] = failMat;
        meshRenderer.materials = mats;
        failImage.SetActive(true);
        yield return new WaitForSeconds(finishScreenTime);
        controller.isFailing = false;
        mats[1] = baseMat;
        meshRenderer.materials = mats;
        failImage.SetActive(false);
    }

    void TuningFail()
    {
        availableLines.Clear();
        SetLinesVisibility(false); 
        StartCoroutine(TuninngFailingRoutine());

    }

    void DeviceOn()
    {
        mats[1] = baseMat;
        meshRenderer.materials = mats;
    }

    
    
    private void OnEnable()
    {
        controller.deviceWasOn += DeviceOn;
        controller.tuningStarted += SpawnTunerPoint;
        controller.tuningStarted += StartLine;
        controller.tuningFailed += TuningFail;
        controller.tuningFinished += TuningFinish;


    }

   

    private void OnDisable()
    {
        controller.deviceWasOn -= DeviceOn;
        controller.tuningStarted -= SpawnTunerPoint;
        controller.tuningStarted -= StartLine;
        controller.tuningFailed -= TuningFail;
        controller.tuningFinished -= TuningFinish;
    }

}
