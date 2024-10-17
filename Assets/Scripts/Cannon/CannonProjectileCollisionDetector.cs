using System.Collections.Generic;
using System.Linq;
using Cannon.Projectile;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody))]
public class CannonProjectileCollisionDetector : MonoBehaviour
{
    private List<Projectile> _projectilesInCollision = new List<Projectile>();

    public bool HasCollision => _projectilesInCollision.Count > 0;

    private void OnTriggerEnter(Collider collider)
    {
        collider.gameObject.TryGetComponent<Projectile>(out var projectile);
        if (projectile != null) _projectilesInCollision.Add(projectile);
    }

    private void OnTriggerExit(Collider collider)
    {
        collider.gameObject.TryGetComponent<Projectile>(out var projectile);
        if (projectile == null) return;
        var outsideProjectile = _projectilesInCollision.FirstOrDefault(
            p => p.gameObject == projectile.gameObject);
        _projectilesInCollision.Remove(outsideProjectile);

    }
}