using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

// TODO: Screenshake from explosions, recoil, etc
public class PlayerCameraController : MonoBehaviour
{
    public Camera playerCamera;
    private float defaultHeight = 0.47f;
    private float crouchHeight = -0.33f;
    private float recoilOffset = 0f;
    private float recoilOffSetMax = -10f;
    private Vector3 currentRecoilRotation = Vector3.zero;
    private Coroutine activeLerp;
    private void OnEnable()
    {
        GameEvents.current.OnCrouch += ResizeHitbox;
        GameEvents.current.OnWeaponFired += Recoil;
        GameEvents.current.OnPlayerRotation += Rotate;
    }

    private void Update ()
    {
        // Remove recoil offset based on deltatime if it exists
        if (recoilOffset <= 0)
        {
            recoilOffset = Mathf.MoveTowards(recoilOffset, 0f, Time.deltaTime * 20);
        }
        // lerp the current recoil rotation towards the target
        currentRecoilRotation = Vector3.Lerp(currentRecoilRotation, new Vector3(recoilOffset, 0f, 0f), 0.4f);
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

    private void Rotate(float rotation)
    {
        playerCamera.transform.localRotation = Quaternion.Euler(rotation, 0f, 0f) * Quaternion.Euler(currentRecoilRotation);
    }

    private void Recoil()
    {
        if (recoilOffset > recoilOffSetMax)
            recoilOffset += -2f;
    }

    // Apply screenshake by a magnitude and speeds.
    private void ApplyScreenShake(float shakeSpeed, float shakeMagnitude)
    {
        
    }

    private IEnumerator LerpShakeRoutine(float targetPosition)
    {
        yield return null;
    }

    private void OnDisable()
    {
        GameEvents.current.OnCrouch -= ResizeHitbox;
        GameEvents.current.OnWeaponFired -= Recoil;
        GameEvents.current.OnPlayerRotation -= Rotate;
    }
}