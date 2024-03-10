using UnityEngine;

namespace Game.BridgeConstruction
{
    public class Bridge : MonoBehaviour
    {
        public Transform Start;
        public Transform End;
        
        [SerializeField] private GameObject _boxCollider;

        [SerializeField] private Transform[] _tiles;

        private int _currentColliderPosition;

        public bool IsStopColliderEnabled => _boxCollider.activeInHierarchy;
        
        public void MoveColliderToNextTileOrDisable()
        {
            if (IsCurrentColliderPositionLessThanMax())
            {
                _currentColliderPosition++;

                SetBoxColliderCurrentPosition();
            }
            else
            {
                DisableCollider();
            }      
        }
        
        private void Awake()
        {
            SetBoxColliderCurrentPosition();
        }

        private void DisableCollider()
        {
            _boxCollider.SetActive(false);
        }

        private bool IsCurrentColliderPositionLessThanMax()
        {
            return _currentColliderPosition < _tiles.Length - 1;
        }

        private void SetBoxColliderCurrentPosition()
        {
            _boxCollider.transform.position = _tiles[_currentColliderPosition].position;
        }
    }
}