using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

// TODO: Screenshake from explosions, recoil, etc
public class PlayerCameraController : MonoBehaviour
{
    public Camera playerCamera;
    private float defaultHeight = 0.47f;
    private float crouchHeight = -0.33f;
    private Coroutine activeLerp;
    private void OnEnable()
    {
        GameEvents.current.OnCrouch += ResizeHitbox;
    }

    private void ResizeHitbox(bool isCrouched, bool isGrounded)
    {  
         // stop active coroutine if we have one
        if (activeLerp != null) StopCoroutine(activeLerp);

        if (isCrouched && isGrounded)
        {
            activeLerp = StartCoroutine(LerpCrouchRoutine(crouchHeight));
        } else
        {
            activeLerp = StartCoroutine(LerpCrouchRoutine(defaultHeight));
        }
    }

    private IEnumerator LerpCrouchRoutine(float targetHeight)
    {
        float startTime = Time.time;
        float maxDuration = 0.2f;
        Vector3 targetPosition = new Vector3(0f, targetHeight, 0f);
        while (playerCamera.transform.localPosition.y != targetHeight)
        {
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, targetPosition, 0.1f);
            if (Time.time > startTime + maxDuration)
            {
                playerCamera.transform.localPosition = targetPosition;
            }
            yield return null;
        }
    }

    // Apply screenshake by a magnitude and speeds.
    private void ApplyScreenShake(float shakeSpeed, float shakeMagnitude)
    {
        
    }

    private IEnumerator LerpShakeRoutine(float targetPosition)
    {
        yield return null;
    }
}