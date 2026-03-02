using System.Collections;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public Camera playerCamera;
    private Vector3 defaultLocalPosition;
    private Vector3 crouchLocalPosition;
    private Coroutine activeLerp;
    private void OnEnable()
    {
        GameEvents.current.OnCrouch += ResizeHitbox;
    }
    private void Start()
    {
        defaultLocalPosition = playerCamera.transform.localPosition;
        crouchLocalPosition = playerCamera.transform.localPosition - new Vector3(0f, 0.8f, 0f);
    }

    private void ResizeHitbox(bool isCrouched, bool isGrounded)
    {  
         // stop active coroutine if we have one
        if (activeLerp != null) StopCoroutine(activeLerp);

        if (isCrouched && isGrounded)
        {
            activeLerp = StartCoroutine(LerpRoutine(crouchLocalPosition));
        } else
        {
            activeLerp = StartCoroutine(LerpRoutine(defaultLocalPosition));
        }
    }

    private IEnumerator LerpRoutine(Vector3 targetLocalPosition)
    {
        float startTime = Time.time;
        float maxDuration = 0.2f;
        while (playerCamera.transform.localPosition != targetLocalPosition)
        {
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, targetLocalPosition, 0.1f);
            if (Time.time > startTime + maxDuration)
            {
                playerCamera.transform.localPosition = targetLocalPosition;
            }
            yield return null;
        }
    }
}