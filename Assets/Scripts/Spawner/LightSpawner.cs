using UnityEngine;

public class LightSpawner : MonoBehaviour
{
    private void OnEnable()
    {
        GameEvents.current.OnSpawnLight += SpawnLightObject;
    }
    private void SpawnLightObject(GameObject light, Vector3 position, Vector3 rotation, float destroyTimer)
    {
        var vfxLight = Instantiate(light, position, Quaternion.Euler(rotation));

        Destroy(vfxLight, destroyTimer);
    }

    private void OnDisable()
    {
        GameEvents.current.OnSpawnLight -= SpawnLightObject;
    }
}