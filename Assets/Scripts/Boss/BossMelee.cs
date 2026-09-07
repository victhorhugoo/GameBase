using UnityEngine;

namespace Boss
{
    public class BossMelee : BossBase
    {
        [Header("Melee Attack")]
        public float damage = 1f;
        public float timeBetweenHits = 1f;
        private float _hitTimer;

        protected override void OnStateEnter(BossState state)
        {
            if (state == BossState.ATTACK)
            {
                // Ataca imediatamente ao entrar no alcance
                _hitTimer = timeBetweenHits;
            }
        }

        protected override void OnAttack()
        {
            _hitTimer += Time.deltaTime;

            if (_hitTimer >= timeBetweenHits)
            {
                _hitTimer = 0f;
                Hit();
            }
        }

        private void Hit()
        {
            if (_player == null) return;

            Vector3 dir = (_player.transform.position - transform.position).normalized;
            _player.Damage(damage, dir);

            PlayAttackScale(1.25f, 0.15f); // golpe um pouco mais forte
        }
    }
}