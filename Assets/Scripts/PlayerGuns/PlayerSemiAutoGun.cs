using UnityEngine.InputSystem;

public class PlayerSemiAutoGun : PlayerGun
{
    protected override void OnShoot(InputAction.CallbackContext context)
    {
        AttemptShoot();
    }

}