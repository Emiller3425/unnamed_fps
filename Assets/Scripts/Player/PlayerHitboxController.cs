using System.Collections;
using UnityEngine;

public class PlayerHitboxController : MonoBehaviour
{
    public CharacterController playerCharacterController;
    public Camera playerCamera;
    private Vector3 defaultCenter = Vector3.zero;
    private float defaultRadius = 0.5f;
    private float defaultHeight = 2f;
    private float crouchRadius = 0.5f;
    private float crouchHeight = 1.5f;
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
            activeLerp = StartCoroutine(LerpRoutine(crouchHeight, crouchRadius));
        } else
        {
            activeLerp = StartCoroutine(LerpRoutine(defaultHeight, defaultRadius));
        }
    }

    private IEnumerator LerpRoutine(float targetHeight, float targetRadius)
    {
        float startTime = Time.time;
        float maxDuration = 0.2f;
        float heightDifference = defaultHeight - targetHeight;
        Vector3 dynamicCenter = new Vector3(0f, -(heightDifference / 2f), 0f);
        while (playerCharacterController.height != targetHeight || playerCharacterController.center != dynamicCenter || playerCharacterController.radius != targetRadius)
        {
            playerCharacterController.height = Mathf.Lerp(playerCharacterController.height, targetHeight, 0.1f);
            playerCharacterController.center = Vector3.Lerp(playerCharacterController.center, dynamicCenter, 0.1f);
            playerCharacterController.radius = Mathf.Lerp(playerCharacterController.radius, targetRadius, 0.1f);
            if (Time.time > startTime + maxDuration)
            {
                playerCharacterController.height = targetHeight;
                playerCharacterController.center = dynamicCenter;
                playerCharacterController.radius = targetRadius;
            }
            yield return null;
        }
    }
}