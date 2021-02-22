using UnityEngine;

public class ParticlePool : Pool
{
    // Start is called before the first frame update
    void Start()
    {
        if (!(sourceObject is Particle))
        {
            Debug.LogError($"ParticlePool {gameObject.name} has a prefab that is not a particle", this);
            enabled = false;
        }
    }

    public Particle GetPooledParticle() => base.GetPooledObject() as Particle;
}
