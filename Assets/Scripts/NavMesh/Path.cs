using UnityEngine;
using UnityEngine.UIElements;

public class Path : MonoBehaviour
{
    public Transform[] waypoints;
    private Transform currentWaypoint;
    private int currentWaypointIndex;

    public void Start()
    {
        currentWaypointIndex = 0;
        currentWaypoint = waypoints[currentWaypointIndex];
    }

    public Transform GetCurrentWaypoint()
    {
        return currentWaypoint;
    }

    public void SetNextWaypoint()
    {
        if (currentWaypointIndex == waypoints.Length - 1)
        {
            currentWaypointIndex = 0;
        } else
        {
            currentWaypointIndex++;
        }
        currentWaypoint = waypoints[currentWaypointIndex];
    }
}