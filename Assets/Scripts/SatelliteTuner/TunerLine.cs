using Injection;
using UnityEngine;

public class TunerLine : MonoBehaviour
{
    [Inject] SatelliteTunerController controller;
    [SerializeField] SatelliteTunerScreen satelliteTunerScreen;

    [SerializeField] Transform firstBorder;
    [SerializeField] Transform secondBorder;
    [SerializeField] bool isHorizontal = false;
    [SerializeField] float lineSpeed;
    int moveVector = 1;
    public bool isStopped = false;
    bool onPoint = false;
    bool startLine = false;

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
                    satelliteTunerScreen.StartLine();
                return;
            }
            else if (!onPoint)
            {
                isStopped = false;
                controller.OnTuningFailed();
                
            }
            return;
        }

       
        if (isHorizontal)
        {
            gameObject.transform.Translate(Vector3.right * lineSpeed * Time.deltaTime * moveVector);
            if (gameObject.transform.position.x <= firstBorder.position.x) moveVector = 1;
            else if (gameObject.transform.position.x >= secondBorder.position.x) moveVector = -1;
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
