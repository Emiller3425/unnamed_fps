using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float lookSpeed = 2f;


    private CharacterController characterController;
    public Vector3 movementDirection = Vector3.zero;
    private float rotationX = 0;
    private bool canMove = true;
    void Start()
    {

    }

    void Update()
    {

    }

    void OnDestroy()
    {
        
    }

}