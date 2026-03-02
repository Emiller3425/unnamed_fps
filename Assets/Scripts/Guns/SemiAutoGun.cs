using UnityEngine.InputSystem;

public class SemiAutoGun : Gun
{
    protected override void OnShoot(InputAction.CallbackContext context)
    {
        AttemptShoot();
    }

}