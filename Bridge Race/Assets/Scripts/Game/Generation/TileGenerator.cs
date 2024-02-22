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

        private IBaseFactory _baseFactory;
        private GameSettings _gameSettings;

        [Inject]
        public void Construct(IBaseFactory baseFactory, GameSettings gameSettings)
        {
            _baseFactory = baseFactory;
            _gameSettings = gameSettings;
        }
        
        public void RegenerateTile()
        {
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
            BridgeTile createdTile = (await _baseFactory.CreateAddressableWithContainer(_gameSettings.BridgeTile, 
                Vector3.zero, Quaternion.identity, transform)).GetComponent<BridgeTile>();

            createdTile.transform.position = transform.position;
            
            createdTile.Initialize(this);
        }
    }
}