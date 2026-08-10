using NUnit.Framework;
using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshController : MonoBehaviour
{
    private NavMeshSurface navMeshSurface;
    private bool isRecalculating = false;
    private float maxRecalcTimer = 1f;
    private float currentRecalcTimer;
    private void Start()
    {
        currentRecalcTimer = maxRecalcTimer;
        navMeshSurface = GetComponent<NavMeshSurface>();
    }
    private void Update()
    {
        if (!isRecalculating)
        {
            StartCoroutine(RecalculateNavMeshSurfaceRoutine());
        }
    }

    private IEnumerator RecalculateNavMeshSurfaceRoutine()
    {
        isRecalculating = true;
        while (currentRecalcTimer >= 0)
        {
            currentRecalcTimer -= Time.deltaTime;
            yield return null;
        }
        yield return navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);
        // ShouldUpdateNavMesh(navMeshSurface.navMeshData);
        currentRecalcTimer = maxRecalcTimer;
        isRecalculating = false;
    }

    private async void ShouldUpdateNavMesh(NavMeshData navMeshData)
    {
        // TODO: Only update navmesh if data has changed
    }


}