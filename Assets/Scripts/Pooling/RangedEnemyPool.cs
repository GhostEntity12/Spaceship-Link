public class RangedEnemyPool : EnemyPool
{
    public ProjectilePool projectilePool;

    public override Poolable CreateNewPooledObject()
    {
        RangedEnemy enemy = base.CreateNewPooledObject() as RangedEnemy;
        enemy.projectilePool = projectilePool;
        return enemy as Poolable;
    }
}
