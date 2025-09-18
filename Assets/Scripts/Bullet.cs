using System;
using System.Numerics;
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
    private UnityEngine.Vector3 trajectory;
    private float velocity;
    private BoxCollider boxCollider ;
    private Rigidbody rigidBody;

    // Sets bullet movement variables
    public void Shoot(UnityEngine.Vector3 direction, float speed)
    {
        trajectory = direction;
        velocity = speed;
    }

    // sets colliders
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        rigidBody = GetComponent<Rigidbody>();
    }

    // sets bullet life to 3 seconds
    void Start()
    {
        Destroy(gameObject, 3f);
    }

    // updates bullet position
    void Update()
    {
        if (velocity > 0)
        {
            transform.position += Time.deltaTime * velocity * trajectory;
        }
    }

    // if collision occurs destryo bullet (will need to add damage logic later, potentially bullet holes in walls etc)
    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        // do nothing
    }

}