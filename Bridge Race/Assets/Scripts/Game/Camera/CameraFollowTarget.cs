using System;
using UnityEngine;

namespace Game.Camera
{
    public class CameraFollowTarget : MonoBehaviour
    {
        [SerializeField] private Vector3 _offsetPosition;
        [SerializeField] private Vector3 _startRotation;
        
        private Transform _target;
        
        public void Initialize(Transform target)
        {
            _target = target;
        }

        private void Awake()
        {
            transform.rotation = Quaternion.Euler(_startRotation);
        }

        private void LateUpdate()
        {
            if (_target != null)
                transform.position = _target.position + _offsetPosition;
        }
    }
}