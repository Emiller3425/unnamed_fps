using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;


[RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private Vector3 trajectory;
    private Camera playerCamera;
    private float velocity;
    private Vector3 targetDestination;
    private Vector3 spawnLocation;
    private bool trajectoryChanged = false;
    private BoxCollider boxCollider;
    private Rigidbody rigidBody;
    // Sets bullet movement variables
    public void Shoot(Vector3 direction, float speed, Vector3 target)
    {
        trajectory = direction;
        velocity = speed;
        targetDestination = target;
    }

    // sets colliders and camera for trajectory
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        rigidBody = GetComponent<Rigidbody>();
        playerCamera = Camera.main;
    }

    // sets bullet life to 3 seconds
    void Start()
    {
        spawnLocation = transform.position;
        Destroy(gameObject, 3f);
    }
    private bool IsPastTarget()
    {
        // Calculate the vector from the target point to the current bullet position.
        Vector3 targetToCurrent = transform.position - targetDestination;

        // Check the total distance traveled vs. the distance to the target.
        float targetDistance = Vector3.Distance(spawnLocation, targetDestination);
        float traveledDistance = Vector3.Distance(spawnLocation, transform.position);

        return traveledDistance > targetDistance; 
    }

    // updates bullet position
    void Update()
    {
        if (velocity > 0)
        {
            if (!trajectoryChanged)
            {
                if (IsPastTarget())
                {
                    trajectory = playerCamera.transform.forward;
                    trajectoryChanged = true;
                }
            }

            transform.position += Time.deltaTime * velocity * trajectory;
        }
    }

    // if collision occurs destroy bullet (will need to add damage logic later, potentially bullet holes in walls etc)
    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

}