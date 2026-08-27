using UnityEngine.InputSystem;

public class PlayerFullAutoGun : PlayerGun
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