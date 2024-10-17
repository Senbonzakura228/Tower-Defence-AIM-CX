using Cannon.Projectile;

public class GuidedProjectile : Projectile
{
    protected override void MoveToTargetDirection()
    {
        var translation = target.transform.position - transform.position;
        if (translation.magnitude > speed)
        {
            translation = translation.normalized * speed;
        }
        transform.Translate(translation);
    }
}