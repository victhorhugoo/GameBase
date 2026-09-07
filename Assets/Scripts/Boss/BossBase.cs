using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Enemy;
using UnityEngine;

namespace Boss
{
    [RequireComponent(typeof(CharacterController))]
    public class BossBase : EnemyBase
    {
        protected enum BossState
        {
            PATROL,
            CHASE,
            ATTACK
        }

        [Header("Patrol")]
        public GameObject[] waypoints;
        public float minDistance = 1f;
        public float speed = 3f;
        public float rotationSpeed = 5f;
        private int _index = 0;

        [Header("Chase")]
        public float detectionRange = 6f;
        public float chaseSpeed = 4f;

        [Header("Attack")]
        public float attackRange = 2f;

        protected BossState _currentState = BossState.PATROL;
        protected BossState _previousState = BossState.PATROL;
        private Vector3 _originalScale;

        public override void Update()
        {
            base.Update();

            UpdateState();

            // Detecta transição de estado
            if (_currentState != _previousState)
            {
                OnStateExit(_previousState);
                OnStateEnter(_currentState);
                _previousState = _currentState;
            }

            switch (_currentState)
            {
                case BossState.PATROL:
                    Patrol();
                    break;
                case BossState.CHASE:
                    Chase();
                    break;
                case BossState.ATTACK:
                    LookAtPlayer();
                    OnAttack();
                    break;
            }
        }

        protected virtual void OnStateEnter(BossState state) { }
        protected virtual void OnStateExit(BossState state) { }
        protected virtual void OnAttack() { }

        #region STATE
        private void UpdateState()
        {
            if (_player == null) return;

            float dist = Vector3.Distance(transform.position, _player.transform.position);

            if (dist <= attackRange)
                _currentState = BossState.ATTACK;
            else if (dist <= detectionRange)
                _currentState = BossState.CHASE;
            else
                _currentState = BossState.PATROL;
        }
        #endregion

        #region PATROL
        private void Patrol()
        {
            if (waypoints == null || waypoints.Length == 0) return;

            Vector3 targetPos = waypoints[_index].transform.position;
            Vector3 flatPos = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 flatTarget = new Vector3(targetPos.x, 0, targetPos.z);
            Vector3 direction = flatTarget - flatPos;

            if (direction.magnitude < minDistance)
            {
                _index++;
                if (_index >= waypoints.Length)
                    _index = 0;
                return;
            }

            direction.Normalize();
            characterController.Move(direction * speed * Time.deltaTime);
            RotateTowards(direction);
        }
        #endregion

        #region CHASE
        private void Chase()
        {
            Vector3 targetPos = _player.transform.position;
            Vector3 flatPos = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 flatTarget = new Vector3(targetPos.x, 0, targetPos.z);
            Vector3 direction = flatTarget - flatPos;

            if (direction.sqrMagnitude < 0.01f) return;

            direction.Normalize();
            characterController.Move(direction * chaseSpeed * Time.deltaTime);
            RotateTowards(direction);
        }
        #endregion

        #region HELPERS
        protected void RotateTowards(Vector3 direction)
        {
            if (direction.sqrMagnitude < 0.001f) return;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        protected void LookAtPlayer()
        {
            Vector3 targetPos = _player.transform.position;
            Vector3 flatPos = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 flatTarget = new Vector3(targetPos.x, 0, targetPos.z);
            RotateTowards(flatTarget - flatPos);
        }

        private void Awake()
        {
            _originalScale = transform.localScale;
        }

        protected void PlayAttackScale(float scaleMultiplier = 1.15f, float duration = 0.1f)
        {
            if (this == null || !gameObject.activeInHierarchy) return;

            transform.DOKill();
            transform.localScale = _originalScale;
            transform.DOScale(_originalScale * scaleMultiplier, duration)
                .SetLoops(2, LoopType.Yoyo);
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }
        #endregion
    }
}