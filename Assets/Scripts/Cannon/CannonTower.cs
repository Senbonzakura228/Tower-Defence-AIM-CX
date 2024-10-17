using UnityEngine;

//что бы сделать упреждение по параболической траектории нужно было делать отдельный модуль атаки для башни
//и просто подменять эти модули в этом классе либо в классе наследнике в зависимости от выбранного режима, либо на этапе инициализации выбирать
//заранее, но у меня не хватило времени на этот пункт
public class CannonTower : Tower
{
    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] private CannonProjectileCollisionDetector projectileCollisionDetector;
    private bool _isReadyToPreciseAttack;

    private void Awake()
    {
        onNoTargetNotify += ChangePreciseAttackReadiness;
    }

    protected override void Update()
    {
        base.Update();
        if (!_currentTarget) return;
        AimAtTargetWithLead();
        Debug.DrawRay(shootPoint.transform.position, transform.forward * 20, Color.green);
    }

    private void AimAtTargetWithLead()
    {
        if (_currentTarget == null || projectileCollisionDetector.HasCollision) return;

        var targetPosition = _currentTarget.transform.position;
        var gunPosition = shootPoint.position;

        var distance = Vector3.Distance(gunPosition, targetPosition);

        var projectileFlyTimeToTarget = distance / projectilePrefab.Speed;

        var predictedMonsterPosition = targetPosition + _currentTarget.TargetMoveDirection * projectileFlyTimeToTarget;

        var direction = (predictedMonsterPosition - gunPosition).normalized;

        var lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        var angleDifference = Quaternion.Angle(transform.rotation, lookRotation);

        _isReadyToPreciseAttack = angleDifference < 5f;

        if (_isReadyToPreciseAttack) transform.rotation = lookRotation;
    }

    private void ChangePreciseAttackReadiness()
    {
        _isReadyToPreciseAttack = false;
    }

    protected override void CreateProjectile()
    {
        if (!_isReadyToPreciseAttack || projectileCollisionDetector.HasCollision) return;
        var projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        projectile.Init(_currentTarget);
    }
}