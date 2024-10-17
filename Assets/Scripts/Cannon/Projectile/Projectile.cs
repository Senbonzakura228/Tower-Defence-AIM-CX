using System.Collections;
using UnityEngine;

namespace Cannon.Projectile
{
    [RequireComponent(typeof(SphereCollider))]
    public abstract class Projectile : MonoBehaviour
    {
        [SerializeField] protected float speed = 0.2f;
        [SerializeField] protected int damage = 10;
        [SerializeField] protected float selfDestructTimer = 5;
        protected Monster target;
        private Coroutine _selfDestructRoutine;

        public float Speed => speed;

        public virtual void Init(Monster target)
        {
            this.target = target;
            _selfDestructRoutine = StartCoroutine(SelfDestruct());
        }

        protected virtual void Update()
        {
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }

            MoveToTargetDirection();
        }

        protected virtual void MoveToTargetDirection()
        {
        }

        protected void OnTriggerEnter(Collider other)
        {
            var isMonster = target.gameObject == other.gameObject;
            if (!isMonster) return;
            if (target.gameObject == null) return;
            target.TakeDamage(damage);
            Destroy(gameObject);
        }

        private IEnumerator SelfDestruct()
        {
            yield return new WaitForSeconds(selfDestructTimer);
        }

        private void OnDestroy()
        {
            StopCoroutine(_selfDestructRoutine);
        }
    }
}