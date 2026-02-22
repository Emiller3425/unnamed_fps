// TODO: Define Animation Events

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameEvents : TemplateMonoBeheavior
{
    public static GameEvents current;
    private void Awake()
    {
        current = this;
    }
    public event Action<int, int> OnAmmoChanged;
    public event Action OnReloadStarted;
    public event Action OnReloadFinished;
    public event Action<string> OnPlaySFX;
    public event System.Action<string, Vector3, Vector3, Vector3, Transform> OnPlayVFX;
    public event Action<float, float, float> OnHealthAdded;
    public event Action<float, float, float> OnHealthSubtracted;
    public event Action<float, float> OnExperienceAdded;
    public event Action OnSetCrossHairActivated;
    public event Action OnSetCrossHairDeactivated;
    public event Action OnSetHitMarkerActivated;
    public event Action OnSetHitMarkerDeactivated;
    public event Action OnWeaponFired;
    public event Action OnWeaponReloaded;
    public event Action<bool> OnTogglePause;
    public event Action<bool> OnTogglePlayerInventory;
    public event Action<float> OnPlayerVelocityChanged;
    public void AmmoChanged(int currentMag, int currentAmmo)
    {
        OnAmmoChanged?.Invoke(currentMag, currentAmmo);
    }
    public void ReloadStarted()
    {
        OnReloadStarted?.Invoke();
    }
    public void ReloadFinished()
    {
        OnReloadFinished?.Invoke();
    }
    public void PlaySFX(string clip)
    {
        OnPlaySFX?.Invoke(clip);
    }
    public void PlayVFX(string shader, Vector3 position, Vector3 rotation, Vector3 velocity, Transform sourceToFollow)
    {
        OnPlayVFX?.Invoke(shader, position, rotation, velocity, sourceToFollow);
    }
    public void ExperienceAdded(float maxExperiencePoints, float currentExperiencePoints)
    {
        OnExperienceAdded?.Invoke(maxExperiencePoints, currentExperiencePoints);
    }
    public void HealthAdded(float damage, float maxHealth, float currentHealth)
    {
        OnHealthAdded?.Invoke(damage, maxHealth, currentHealth);
    }
    public void HealthSubtracted(float damage, float maxHealth, float currentHealth)
    {
        OnHealthSubtracted?.Invoke(damage, maxHealth, currentHealth);
    }
    public void SetCrossHairActivated()
    {
        OnSetCrossHairActivated?.Invoke();
    }
    public void SetCrossHairDeactivated()
    {
        OnSetCrossHairDeactivated?.Invoke();
    }
    public void SetHitMarkerActivated()
    {
        OnSetHitMarkerActivated?.Invoke();
    }
    public void SetHitMarkerDeactivated()
    {
        OnSetHitMarkerDeactivated?.Invoke();
    }
    public void WeaponFired()
    {
        OnWeaponFired?.Invoke();
    }
    public void WeaponReloaded()
    {
        OnWeaponReloaded?.Invoke();
    }
    public void TogglePause(bool isPauseToggled)
    {
        OnTogglePause?.Invoke(isPauseToggled);
    }
    public void TogglePlayerInventory(bool isPlayerInventoryToggled)
    {
        OnTogglePause?.Invoke(isPlayerInventoryToggled);
    }

    public void PlayerVelocityChanged(float velocity)
    {
        OnPlayerVelocityChanged?.Invoke(velocity);
    }
}