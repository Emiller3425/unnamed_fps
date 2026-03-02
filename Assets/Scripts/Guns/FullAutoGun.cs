using UnityEngine.InputSystem;

public class FullAutoGun : Gun
{
    protected override void Update()
    {
        base.Update();
        if (shootAction.IsPressed())
        {
            AttemptShoot();
        }
    }
    protected override void OnShoot(InputAction.CallbackContext context)
    {
        // do nothing because we are handling shoot logic in Update();
    }
} 