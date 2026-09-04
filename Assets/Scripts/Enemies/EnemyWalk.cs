/*
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

namespace Enemy
{
    public class EnemyWalk : EnemyBase
    {
        public GameObject[] waypoints;
        public float minDistance = 1f;
        public float speed = 1f;

        private int _index = 0;

        private void Update()
        {
            if (Vector3.Distance(transform.position, waypoints[_index].transform.position) < minDistance)
            {
                _index++;
                if (_index >= waypoints.Length)
                {
                    _index = 0;
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, waypoints[_index].transform.position, Time.deltaTime * speed);
            transform.LookAt(waypoints[_index].transform.position);
        }
    }
}
*/


using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;
namespace Enemy
{
    [RequireComponent(typeof(CharacterController))]
    public class EnemyWalk : EnemyBase
    {
        public GameObject[] waypoints;
        public float minDistance = 1f;
        public float speed = 1f;
        private int _index = 0;
        public float rotationSpeed = 5f;

        public override void Update()
        {
            base.Update();
            MoveTowardsWaypoint();
            ApplyGravity();
        }
        private void MoveTowardsWaypoint()
        {
            Vector3 targetPos = waypoints[_index].transform.position;
            // Direção só no plano XZ
            Vector3 flatPos = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 flatTarget = new Vector3(targetPos.x, 0, targetPos.z);
            Vector3 direction = (flatTarget - flatPos);
            if (direction.magnitude < minDistance)
            {
                _index++;
                if (_index >= waypoints.Length)
                {
                    _index = 0;
                }
                return;
            }
            direction.Normalize();
            characterController.Move(direction * speed * Time.deltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}