using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public enum EnemyState
{
    IDLE,
    PATROL, 
    ATTACKING,
    RELOADING
}

// TODO: Implement a base EnemyController using nav meshes, create a new enemy prefab using the new model.

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    public float walkSpeed = 0.5f;
    public float sprintSpeed = 7f;
    public float lookSpeed = 10f;
    public EnemyGun enemyGun;
    private float rotationX = 0f;
    private float velocityY = 0f;
    private float gravity = 10f;
    private bool canJump = true;
    private float maxMeleeCooldown = 5f;
    private float currentMeleeCooldown = 0f;
    private float meleeRange = 10f;
    private float meleeDamage = 5f;

    private void Start()
    {

    }
    private void Update()
    {
 

    }
}