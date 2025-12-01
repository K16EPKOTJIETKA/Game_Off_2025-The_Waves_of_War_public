using Injection;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float currentSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    [SerializeField] private CharacterController _characterController;
    [SerializeField] private CinemachineCamera _cinCam;
    [Inject] GameEventBuffer eventBuffer;
    AudioSource audioSource;

    [SerializeField] List<AudioClip> audioClips = new List<AudioClip> (); 

    private Vector2 _move;
    bool isFirstStep = true;

    public void Initialize()
    {
        audioSource = GetComponent<AudioSource> ();
        currentSpeed = walkSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnMove(InputValue val)
    {
        if (isFirstStep)
        {
            isFirstStep = false;
            eventBuffer.CastNewEvent("1");
        }

        
        _move = val.Get<Vector2>();
        if (audioSource != null)
        {
            if (audioSource.isPlaying) return;
            audioSource.clip = audioClips[Random.Range(0, audioClips.Count)];
            audioSource.Play();
        }
            
    }

    public void OnSprint(InputValue val)
    {
        if (val.Get<float>() > 0.5f)
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }
    }

    private void Update()
    {
        _characterController.Move((GetForward() * _move.y + GetRight() * _move.x) * Time.deltaTime * currentSpeed);
    }

    private Vector3 GetForward()
    {
        Vector3 forward = _cinCam.transform.forward;
        forward.y = 0;

        return forward.normalized;
    }

    private Vector3 GetRight()
    {
        Vector3 right = _cinCam.transform.right;
        right.y = 0;

        return right.normalized;
    }
}
