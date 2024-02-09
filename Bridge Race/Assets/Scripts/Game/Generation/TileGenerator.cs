using System.Collections;
using System.Linq;
using Architecture.Services.Interfaces;
using Data;
using Game.BridgeConstruction;
using Game.Generation.Interfaces;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;
using Random = UnityEngine.Random;

namespace Game.Generation
{
    public class TileGenerator : MonoBehaviour, ITileGenerator
    {
        private const float GenerationFrequency = 1f;
        
        private const int MaxTriesToStopGeneration = 200;
        
        private const float MinDistanceBetweenTiles = 3f;

        private readonly ObservableList<BridgeTile> _createdTiles = new();
        
        [SerializeField] private BoxCollider _boxCollider;

        [SerializeField] private int _maxTiles;
        
        private IBaseFactory _baseFactory;
        private GameSettings _gameSettings;

        private bool _isGenerationEnabled;

        private Coroutine _generationCoroutine;

        [Inject]
        public void Construct(IBaseFactory baseFactory, GameSettings gameSettings)
        {
            _baseFactory = baseFactory;
            _gameSettings = gameSettings;
        }
        
        public void RemoveTile(BridgeTile tile)
        {
            _createdTiles.Remove(tile);
        }

        private void Awake()
        {
            StartGeneration(0);

            _createdTiles.ItemRemoved += TryToRestartGeneration;
        }

        private void OnDestroy()
        {
            _createdTiles.ItemRemoved -= TryToRestartGeneration;
        }

        private IEnumerator Generate(float generationFrequency)
        {
            _isGenerationEnabled = true;
            
            while (true)
            {
                SpawnTile();

                if (_createdTiles.Count >= _maxTiles)
                {
                    _isGenerationEnabled = false;
                    
                    yield break;
                }
                
                yield return new WaitForSeconds(generationFrequency);
            }  
        }

        private async void SpawnTile()
        {
            BridgeTile createdTile = (await _baseFactory.CreateAddressableWithContainer(_gameSettings.BridgeTile, 
                GetSpawnPosition(), Quaternion.identity, transform)).GetComponent<BridgeTile>();
                         
            createdTile.Initialize(this);

            _createdTiles.Add(createdTile);
        }
        
        private Vector3 GetSpawnPosition()
        {
            int tries = 0;
            
            while (true)
            {
                Vector3 bounds = _boxCollider.bounds.extents;

                Vector3 tileSize = _gameSettings.BridgeTileScript.Size;
                
                Vector3 randomSpawnPoint = new Vector3(Random.Range(-bounds.x + tileSize.x,
                    bounds.x - tileSize.x), bounds.y * 1.5f, Random.Range(-bounds.z + tileSize.z, bounds.z - tileSize.z));
                
                int closeTilesCount = 0;
                
                foreach (BridgeTile generatedTile in _createdTiles.ToList())
                {
                    if (generatedTile != null)
                    {
                        if (Vector2.Distance(randomSpawnPoint, generatedTile.transform.position) < MinDistanceBetweenTiles) 
                            closeTilesCount++;    
                    }
                }

                if (closeTilesCount == 0)
                    return randomSpawnPoint;
                
                tries++;

                if (tries >= MaxTriesToStopGeneration)
                {
                    StopGeneration();

                    return Vector3.zero;
                }
            }
        }

        private void StopGeneration()
        {
            if (_generationCoroutine != null)
            {
                _isGenerationEnabled = false;
                        
                StopCoroutine(_generationCoroutine);
            }
        }

        private void TryToRestartGeneration(ObservableList<BridgeTile> sender, ListChangedEventArgs<BridgeTile> e)
        {
            if (_createdTiles.Count < _maxTiles & !_isGenerationEnabled)
                StartGeneration(GenerationFrequency);
        }

        private void StartGeneration(float generationFrequency)
        {
            _generationCoroutine = StartCoroutine(Generate(generationFrequency));
        }
    }
}