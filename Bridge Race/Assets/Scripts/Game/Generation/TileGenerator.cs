using System.Collections;
using Architecture.Services.Interfaces;
using Data;
using Game.BridgeConstruction;
using Game.Generation.Interfaces;
using UnityEngine;
using Zenject;

namespace Game.Generation
{
    public class TileGenerator : MonoBehaviour, ITileGenerator
    {
        private const float RegenerationTileDelay = 3f;

        [SerializeField] private Platform _platform;
        
        private IBaseFactory _baseFactory;
        private GameSettings _gameSettings;

        private BridgeTile _currentTile;

        [Inject]
        public void Construct(IBaseFactory baseFactory, GameSettings gameSettings)
        {
            _baseFactory = baseFactory;
            _gameSettings = gameSettings;
        }
        
        public void RegenerateTile()
        {
            _platform.Tiles.Remove(_currentTile);
            
            StartCoroutine(Generate(RegenerationTileDelay));
        }

        private void Awake()
        {
            StartCoroutine(Generate(0));
        }

        private IEnumerator Generate(float generationDelay)
        {
            yield return new WaitForSeconds(generationDelay);
            
            SpawnTile();   
        }

        private async void SpawnTile()
        {
            BridgeTile createdTile = (await _baseFactory.CreateAddressableWithContainer
            (_gameSettings.Prefabs.BridgeTile, Vector3.zero, Quaternion.identity, transform))
                .GetComponent<BridgeTile>();

            createdTile.transform.position = transform.position;
            
            createdTile.Initialize(this);
            
            _platform.Tiles.Add(createdTile);
        }
    }
}