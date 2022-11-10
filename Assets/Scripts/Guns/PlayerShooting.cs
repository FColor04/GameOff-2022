using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private PlayerEquipment equipment;
    private float lastShotFired;

    public void OnFireButtonPressed(InputAction.CallbackContext context)
    {
        if (!equipment || !equipment.equippedGun || !equipment.equippedGun.GunData) return;
        var gunInstance = equipment.equippedGun;
        var gunData = equipment.equippedGun.GunData;
        if ((context.phase != InputActionPhase.Started) && !(context.phase == InputActionPhase.Performed && gunData.isAutomatic)) return;
        if(Time.time < lastShotFired + 1f / gunData.fireRate) return;
        
    }
}
