using UnityEngine;

namespace Game.BridgeConstruction
{
    public class Bridge : MonoBehaviour
    {
        [SerializeField] private GameObject _boxCollider;

        [SerializeField] private Transform[] _tiles;

        private int _currentColliderPosition;
        
        public void MoveColliderToNextTileOrDisable()
        {
            if (_currentColliderPosition < _tiles.Length - 1)
            {
                _currentColliderPosition++;

                SetBoxColliderCurrentPosition();
            }
            else
            {
                _boxCollider.SetActive(false);
            }      
        }

        private void Awake()
        {
            SetBoxColliderCurrentPosition();
        }

        private void SetBoxColliderCurrentPosition()
        {
            _boxCollider.transform.position = _tiles[_currentColliderPosition].position;
        }
    }
}