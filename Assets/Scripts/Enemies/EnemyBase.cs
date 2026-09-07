
/*using System.Collections;
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
        public float gravity = -9.8f;

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
            Destroy(gameObject, 1f);
            PlayAnimationByTrigger(AnimationType.DEATH);
        }

        public void OnDamage(float f)
        {
            if (flashColor != null) flashColor.Flash();
            if (myParticleSystem != null) myParticleSystem.Emit(15);

            transform.position -= transform.forward;
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
        

        public void Damage(float damage)
        {
            OnDamage(damage);
        }

        public void Damage(float damage, Vector3 dir)
        {
            //OnDamage(damage);
            transform.DOMove(transform.position - dir, .1f);
        }
    }
}
*/


using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Animation;

namespace Enemy
{
    [RequireComponent(typeof(CharacterController))]
    public class EnemyBase : MonoBehaviour, IDamageable
    {
        public float startLife = 10f;
        public Collider myCollider;
        public FlashColor flashColor;
        public ParticleSystem myParticleSystem;
        [Header("Gravity")]
        public CharacterController characterController;
        public float gravity = -9.8f;
        public float VerticalSpeed { get; protected set; }
        public bool IsGrounded => characterController.isGrounded;
        public bool lookAtPlayer = false;

        [SerializeField] private float _currentLife;
        [Header("Animation")]
        [SerializeField] private AnimationBase _animationBase;
        [Header("Start Animation")]
        public float startAnimationDuration = .2f;
        public Ease startAnimationEase = Ease.OutBack;
        public bool startWithBornAnimation = true;

        [Header("Knockback")]
        public float knockbackDrag = 5f;
        private Vector3 _knockbackVelocity;

        protected PlayerMoviment _player;

        private void Awake()
        {
            if (characterController == null)
                characterController = GetComponent<CharacterController>();
            Init();
        }

        private void Start()
        {
            _player = FindObjectOfType<PlayerMoviment>();
        }

        public virtual void Update()
        {
            ApplyGravity();
            ApplyKnockback();

            if (lookAtPlayer)
            {
                transform.LookAt(_player.transform.position);
            }
        }
        protected void ApplyGravity()
        {
            if (IsGrounded)
            {
                if (VerticalSpeed < 0)
                    VerticalSpeed = -1f;
            }
            else
            {
                VerticalSpeed += gravity * Time.deltaTime;
            }
            characterController.Move(Vector3.up * VerticalSpeed * Time.deltaTime);
        }
        private void ApplyKnockback()
        {
            if (_knockbackVelocity.sqrMagnitude > 0.01f)
            {
                characterController.Move(_knockbackVelocity * Time.deltaTime);
                _knockbackVelocity = Vector3.Lerp(_knockbackVelocity, Vector3.zero, knockbackDrag * Time.deltaTime);
            }
        }
        protected void ResetLife()
        {
            _currentLife = startLife;
        }
        protected virtual void Init()
        {
            ResetLife();
            if (startWithBornAnimation)
                BornAnimation();
        }
        protected virtual void Kill()
        {
            OnKill();
        }
        protected virtual void OnKill()
        {
            if (myCollider != null) myCollider.enabled = false;
            Destroy(gameObject, 1f);
            PlayAnimationByTrigger(AnimationType.DEATH);
        }
        public void OnDamage(float f)
        {
            if (flashColor != null) flashColor.Flash();
            if (myParticleSystem != null) myParticleSystem.Emit(15);
            transform.position -= transform.forward;
            _currentLife -= f;
            if (_currentLife <= 0)
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
        public void Damage(float damage)
        {
            OnDamage(damage);
        }
        public void Damage(float damage, Vector3 dir)
        {
            OnDamage(damage);

            _knockbackVelocity = dir.normalized * 5f;
        }

        private void OnCollisionEnter(Collision collision)
        {
            PlayerMoviment p = collision.transform.GetComponent<PlayerMoviment>();

            if(p != null)
            {
                p.Damage(1);
            }
        }
    }
}