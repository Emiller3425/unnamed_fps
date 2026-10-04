using UnityEngine;

public class BreakMesh : MonoBehaviour
{
    public GameObject brokenMesh;
    protected EnvironmentObjectStatsManager statsManager;
    protected int parentInstanceId;
    protected float breakForce = 2f;

    private void OnEnable()
    {
        GameEvents.current.OnBreakMesh += BreakObjectMesh;
    }
    private void Start()
    {
        statsManager = GetComponentInParent<EnvironmentObjectStatsManager>();
        parentInstanceId = statsManager.gameObject.GetInstanceID();
    }
    private void BreakObjectMesh(int instanceId)
    {
        if (instanceId == parentInstanceId) {
            GameObject brokenMeshObject = Instantiate(brokenMesh, transform.position, transform.rotation);
            Destroy(gameObject);
            foreach(Rigidbody rb in brokenMeshObject.GetComponentsInChildren<Rigidbody>())
            {
                Vector3 force = (rb.transform.position - transform.position).normalized * breakForce;
                rb.AddForce(force, ForceMode.Impulse);
            }
        }
        Explosive e = GetComponent<Explosive>();
        if (e)
        {
            e.Detonate();
        }
    }

    private void OnDisable()
    {
        GameEvents.current.OnBreakMesh -= BreakObjectMesh;
    }
}