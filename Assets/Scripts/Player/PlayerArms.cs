using UnityEngine;

public class PlayerArms : MonoBehaviour
{ 
    public Camera playerCamera;
    private float smoothSpeed = 10f;
    void Update()
    {
        // transform.position = Vector3.zero;
        Quaternion targetRotation = playerCamera.transform.rotation;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);
    }

}