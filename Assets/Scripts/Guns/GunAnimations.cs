using UnityEngine;

public class GunAnimations : MonoBehaviour
{
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private PlayerCamera pc;
    [SerializeField] private VelocityController vc;
    public RuntimeAnimatorController AnimationSet { get => gunAnimator.runtimeAnimatorController; set => gunAnimator.runtimeAnimatorController = value; }
    public float FallSpeed { set => gunAnimator.SetFloat("FallSpeed", value); }
    public Vector2 LookVelocity
    {
        set
        {
            gunAnimator.SetFloat("DeltaAngleX", value.x);
            gunAnimator.SetFloat("DeltaAngleY", value.y);
        }
        get => new(gunAnimator.GetFloat("DeltaAngleX"), gunAnimator.GetFloat("DeltaAngleY"));
    }
    void FixedUpdate()
    {
        var playerLocalVelocity = Quaternion.Euler(-pc.Camera.transform.eulerAngles._0y0()) * vc.CurrentVelocity;
        LookVelocity = Vector2.Lerp(pc.TurnSpeed + playerLocalVelocity._y0() + playerLocalVelocity._0x() * .75f, LookVelocity, 0.5f);
    }

    void LateUpdate()
    {
        gunAnimator.Update(Time.deltaTime);
    }

    public void PlayRecoil(float strength = 1f)
    {
        gunAnimator.SetFloat("Recoil Strength", strength);
        gunAnimator.SetTrigger("Recoil");
    }

}
