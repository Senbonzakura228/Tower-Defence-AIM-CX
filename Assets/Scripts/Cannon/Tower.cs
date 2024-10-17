using System;
using System.Collections;
using System.Linq;
using Cannon.Projectile;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] protected float shootInterval = 0.5f;
    [SerializeField] protected Projectile projectilePrefab;
    [SerializeField] protected Vector3 overlapCenter;
    [SerializeField] protected Vector3 overlapSize;
    [SerializeField] protected Transform shootPoint;

    protected Action onNoTargetNotify;
    protected Monster _currentTarget;
    private Coroutine _attackRoutine;

    protected virtual void Update()
    {
        Attack();
    }

    protected void Attack()
    {
        if (_currentTarget != null) return;
        var target = TryDetectTarget();
        if (target == null) return;
        _currentTarget = target;
        _attackRoutine = StartCoroutine(AttackTarget(_currentTarget));
    }

    protected Monster TryDetectTarget()
    {
        var colliders = DetectColliders();

        if (colliders.Length == 0) return null;
        var leftmostCollider = colliders.OrderBy(c => c.transform.position.x).FirstOrDefault();
        leftmostCollider.TryGetComponent<Monster>(out var monster);
        return monster == null ? null : monster;
    }

    private Collider[] DetectColliders()
    {
        return Physics.OverlapBox(transform.position + overlapCenter, overlapSize / 2, Quaternion.identity);
    }

    protected IEnumerator AttackTarget(Monster target)
    {
        while (true)
        {
            var isTargetOnRange = CheckTargetIsOnRange(target);
            if (!isTargetOnRange)
            {
                _currentTarget = null;
                onNoTargetNotify?.Invoke();
                yield break;
            }
            CreateProjectile();
            yield return new WaitForSeconds(shootInterval);
        }
    }

    protected bool CheckTargetIsOnRange(Monster target)
    {
        var collidersInRange = DetectColliders();
        return
            collidersInRange.FirstOrDefault(c => target.gameObject == c.gameObject) != null;
    }

    protected virtual void CreateProjectile()
    {
        var projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        projectile.Init(_currentTarget);
    }

    //для удобной настройки оверлапа
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.matrix = Matrix4x4.TRS(transform.position + overlapCenter, Quaternion.identity, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, overlapSize);
    }

    protected virtual void OnDestroy()
    {
        if (_attackRoutine != null)
            StopCoroutine(_attackRoutine);
        onNoTargetNotify = null;
    }
}