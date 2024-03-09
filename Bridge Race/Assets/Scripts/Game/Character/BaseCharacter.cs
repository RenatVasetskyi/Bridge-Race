using UnityEngine;

namespace Game.Character
{
    public abstract class BaseCharacter : MonoBehaviour
    {
        private const float StepRayDistance = 3f;

        [SerializeField] protected Rigidbody _rigidbody;
        
        [SerializeField] private Transform _climbRaycastOrigin;
        
        [Header("Layers")]
        
        [SerializeField] private LayerMask _stepLayer;
        [SerializeField] private LayerMask _groundLayer;
        
        protected void Climb()
        {
            RaycastHit hit;

            if (Physics.Raycast(_climbRaycastOrigin.position, transform.TransformDirection(-Vector3.up),
                    out hit, StepRayDistance, _stepLayer | _groundLayer))
            {
                Vector3 targetVector = new Vector3(_rigidbody.position.x, hit.point.y, _rigidbody.position.z);   

                _rigidbody.position = targetVector;
                
                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
            }
        }
    }
}