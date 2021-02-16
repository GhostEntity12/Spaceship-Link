using System.Collections;
using System.Collections.Generic;
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
                projectile.transform.position = this.transform.position;
                projectile.transform.rotation = this.transform.rotation;
                projectile.Fire(GameManager.instance.damageMultiplier);
            }
            fireTimer = Mathf.Max(0, fireTimer - Time.deltaTime);
        }
    }

    public void OnFire(CallbackContext context)
    {
        isFiring = context.ReadValue<float>() > 0;
    }
}
