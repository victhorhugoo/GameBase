using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Animation;

namespace Enemy
{
    public class EnemyBase : MonoBehaviour, IDamageable
    {
        public float startLife = 10f;
        public Collider myCollider;
        public FlashColor flashColor;
        public ParticleSystem myParticleSystem;

        [SerializeField] private float _currentLife;

        [Header("Animation")]
        [SerializeField] private AnimationBase _animationBase;

        [Header("Start Animation")]
        public float startAnimationDuration = .2f;
        public Ease startAnimationEase = Ease.OutBack;
        public bool startWithBornAnimation = true;

        private void Awake()
        {
            Init();
            
        }

        protected void ResetLife()
        {
            _currentLife = startLife;
        }

        protected virtual void Init()
        {
            ResetLife();
            if(startWithBornAnimation)
                BornAnimation();
        }

        protected virtual void Kill()
        {
            OnKill();
        }

        protected virtual void OnKill()
        {
            if (myCollider != null) myCollider.enabled = false;
            Destroy(gameObject, 3f);
            PlayAnimationByTrigger(AnimationType.DEATH);
        }

        public void OnDamage(float f)
        {
            if (flashColor != null) flashColor.Flash();
            if (myParticleSystem != null) myParticleSystem.Emit(15);
            _currentLife -= f;

            if(_currentLife <= 0)
            {
                Kill();
            }
        }

        #region ANIMATION
        private void BornAnimation()
        {
            transform.DOScale(0, startAnimationDuration).SetEase(startAnimationEase).From();
        }

        public void PlayAnimationByTrigger(AnimationType animationType)
        {
            _animationBase.PlayAnimationByTrigger(animationType);
        }

        #endregion
        // Debug
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                OnDamage(5f);
            }
        }

        public void Damage(float damage)
        {
            OnDamage(damage);
        }
    }
}