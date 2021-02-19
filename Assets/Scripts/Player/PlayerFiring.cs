using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerFiring : MonoBehaviour
{
    bool isFiring;

    public ProjectilePool projectilePool;

    public float fireSpeed = 0.1f;
    float fireTimer = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isFiring)
        {
            fireTimer = 0;
        }
        else
        {
            if (fireTimer == 0)
            {
                fireTimer = fireSpeed;
                Projectile projectile = projectilePool.GetPooledProjectile();
                projectile.Fire(GameManager.instance.damageMultiplier, transform.position, transform.rotation);
            }
            fireTimer = Mathf.Max(0, fireTimer - Time.fixedDeltaTime);
        }
    }

    public void OnFire(CallbackContext context)
    {
        isFiring = context.ReadValue<float>() > 0;
    }
}
