using UnityEngine;
using UnityEngine.VFX;

[System.Serializable]
public class VisualEffectItem
{
    public string name;
    public GameObject prefab;
    public bool poolable = true;
}