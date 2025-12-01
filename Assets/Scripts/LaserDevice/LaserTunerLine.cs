using Injection;
using System.Collections;
using UnityEngine;

public class LaserTunerLine : MonoBehaviour
{
    [Inject] LaserDeviceController controller;
    [SerializeField] LaserDeviceScreen laserDeviceScreen;

    [SerializeField] Transform firstBorder;
    [SerializeField] Transform secondBorder;
    [SerializeField] bool isHorizontal = false;
    [SerializeField] bool isDiagonal = false;
    [SerializeField] float lineSpeed;
    int moveVector = 1;
    public bool isStopped = false;
    bool onPoint = false;
    bool startLine = false;

    IEnumerator StartLineRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        laserDeviceScreen.StartLine();
    }

    void MoveLine()
    {
        if (!controller.isTuning) return;
        if (isStopped)
        {
            if (onPoint && !startLine && controller.isTuning)
            {
                startLine = true;
                controller.CorrectTuning();
                if (controller.isTuning)
                    StartCoroutine(StartLineRoutine());
                return;
            }
            else if (!onPoint)
            {
                controller.OnTuningFailed();
                isStopped = false;
            }
            return;
        }


        if (isHorizontal)
        {
            gameObject.transform.Translate(Vector3.right * lineSpeed * Time.deltaTime * moveVector);
            if (gameObject.transform.position.x <= firstBorder.position.x) moveVector = 1;
            else if (gameObject.transform.position.x >= secondBorder.position.x) moveVector = -1;
        }
        else if (isDiagonal)
        {
            transform.Translate((secondBorder.position - firstBorder.position).normalized * lineSpeed * Time.deltaTime * moveVector, Space.World);

            if (moveVector == -1 && Vector3.Distance(transform.position, firstBorder.position) < 0.8f)
            {
                moveVector = 1;
            }
            else if (moveVector == 1 && Vector3.Distance(transform.position, secondBorder.position) < 0.8f)
            {
                moveVector = -1;
            }
        }
        else
        {
            gameObject.transform.Translate(Vector3.up * lineSpeed * Time.deltaTime * moveVector, Space.World);
            if (gameObject.transform.position.y <= firstBorder.position.y) moveVector = 1;
            else if (gameObject.transform.position.y >= secondBorder.position.y) moveVector = -1;
        }

    }

    public void Stop()
    {
        isStopped = true;
    }

    private void OnDisable()
    {
        onPoint = false;
        isStopped = false;
        startLine = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TuningPoint"))
        {
            onPoint = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TuningPoint"))
        {
            onPoint = false;
        }
    }


    private void Update()
    {
        MoveLine();
    }
}
