using System;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemManager: MonoBehaviour
{
    [Serializable]
    public struct ParticlePrefabMapping
    {
        public string key;
        public GameObject prefab;
    }
    [Header("Particle Config")]
    [SerializeField] private List<ParticlePrefabMapping> particleEntries = new List<ParticlePrefabMapping>();

    // Fast O(1) lookup created at runtime
    private Dictionary<string, GameObject> particleDictionary;
    private void Awake()
    {
        particleDictionary = new Dictionary<string, GameObject>();

        foreach(var entry in particleEntries)
        {
            if (string.IsNullOrEmpty(entry.key) || entry.prefab == null)
            continue;

            if (!particleDictionary.ContainsKey(entry.key))
            {
                particleDictionary.Add(entry.key, entry.prefab);
            } else
            {
                Debug.LogWarning($"Duplicate particle key {entry.key}");
            }
        }
    }

    private void OnEnable()
    {
        GameEvents.current.OnPlayParticleSystem += InstantiateParticleSystem;
    }
    private void InstantiateParticleSystem(string particlePrefabName, Vector3 position, Quaternion rotation)
    {
        if (particleDictionary.TryGetValue(particlePrefabName, out GameObject prefab))
        {
            Instantiate(prefab, position, rotation);
        } else
        {
            Debug.LogError($"Particle key {particlePrefabName} not found");
        }
    }
    private void OnDisable()
    {
        GameEvents.current.OnPlayParticleSystem -= InstantiateParticleSystem;
    }
}