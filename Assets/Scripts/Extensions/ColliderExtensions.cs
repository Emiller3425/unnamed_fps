using System.Security.Cryptography;
using UnityEngine;

public static class ColliderExtensions
{
    public static bool IsPlayer(this Collider c)
    {
        return c.CompareTag("Player");
    }
}