using System;
using UnityEngine;
using System.Collections;
using Cannon.Projectile;

public class CannonProjectile : Projectile
{
    protected override void Update()
    {
        base.Update();
        Debug.DrawRay(transform.position, transform.forward * 20, Color.red);
    }

    protected override void MoveToTargetDirection()
    {
        var translation = transform.forward * speed;
        transform.Translate(translation, Space.World);
    }
}