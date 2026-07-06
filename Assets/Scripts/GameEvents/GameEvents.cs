// TODO: Define Animation Events
using System;
using UnityEngine;

public class GameEvents : TemplateMonoBeheavior
{
    public static GameEvents current;
    private void Awake()
    {
        current = this;
    }
    public event Action<int, int> OnAmmoChanged;
    public event Action<int> OnEquipmentChanged;
    public event Action<int> OnLevelChanged;
    public event Action OnReloadStarted;
    public event Action OnReloadFinished;
    public event Action<string> OnPlaySFX;
    public event System.Action<string, Vector3, Vector3, Vector3, Transform> OnPlayVFX;
    public event Action<float, float, float> OnHealthAdded;
    public event Action<float, float, float> OnHealthSubtracted;
    public event Action<float, float, float, int> OnExperienceAdded;
    public event Action OnSetCrossHairActivated;
    public event Action OnSetCrossHairDeactivated;
    public event Action OnSetHitMarkerActivated;
    public event Action OnSetHitMarkerDeactivated;
    public event Action OnWeaponFired;
    public event Action OnWeaponReloaded;
    public event Action OnEquipmentThrown;
    public event Action<bool> OnTogglePause;
    public event Action<bool> OnTogglePlayerInventory;
    public event Action<float, bool> OnBloom;
    public event Action<bool, bool> OnCrouch;
    public event Action<GameObject> OnWeaponPickup;
    public event Action<GameObject> OnEquipmentPickup;
    public event Action OnScreenResize;
    public event Action<int> OnEntityDeath;
    public void AmmoChanged(int currentMag, int currentAmmo)
    {
        OnAmmoChanged?.Invoke(currentMag, currentAmmo);
    }

    public void EquipmentCountChanged(int currentEquipment)
    {
        OnEquipmentChanged?.Invoke(currentEquipment);
    }
    public void LevelChanged(int level)
    {
        OnLevelChanged?.Invoke(level);
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
    public void ExperienceAdded(float maxExperiencePoints, float currentExperiencePoints, float previousExperiencePoints, int currentLevel)
    {
        OnExperienceAdded?.Invoke(maxExperiencePoints, currentExperiencePoints, previousExperiencePoints, currentLevel);
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
    public void EquipmentThrown()
    {
        OnEquipmentThrown?.Invoke();
    }
    public void TogglePause(bool isPauseToggled)
    {
        OnTogglePause?.Invoke(isPauseToggled);
    }
    public void TogglePlayerInventory(bool isPlayerInventoryToggled)
    {
        OnTogglePause?.Invoke(isPlayerInventoryToggled);
    }

    public void Bloom(float velocity, bool fromShotBullet)
    {
        OnBloom?.Invoke(velocity, fromShotBullet);
    }

    public void Crouch(bool isCrouched, bool isGrounded)
    {
        OnCrouch?.Invoke(isCrouched, isGrounded);
    }

    public void WeaponPickup(GameObject weapon)
    {
        OnWeaponPickup?.Invoke(weapon);
    }

    public void EquipmentPickup(GameObject equipment)
    {
        OnEquipmentPickup?.Invoke(equipment);
    }

    public void ScreenResize()
    {
        OnScreenResize?.Invoke();
    }

    public void EntityDeath(int instanceId)
    {
        OnEntityDeath?.Invoke(instanceId);
    }
}