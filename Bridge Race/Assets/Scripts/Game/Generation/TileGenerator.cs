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

        private Vector3 _minBounds;
        private Vector3 _maxBounds;

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
            SetBoundsSize();
            StartGeneration(0);

            _createdTiles.ItemRemoved += TryToRestartGeneration;
        }

        private void SetBoundsSize()
        {
            Vector3 colliderBounds = _boxCollider.bounds.extents;
            
            _minBounds = new Vector3(transform.position.x - (colliderBounds.x / 2),
                transform.position.y + (colliderBounds.y / 2), transform.position.z - (colliderBounds.z / 2));
            
            _maxBounds = new Vector3(transform.position.x + (colliderBounds.x / 2),
                transform.position.y + (colliderBounds.y / 2), transform.position.z + (colliderBounds.z / 2));
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
                Vector3.zero, Quaternion.identity, transform)).GetComponent<BridgeTile>();

            createdTile.transform.position = GetSpawnPosition(createdTile
                .GetComponent<BoxCollider>().bounds.extents * 2);
            
            createdTile.Initialize(this);

            _createdTiles.Add(createdTile);
        }
        
        private Vector3 GetSpawnPosition(Vector3 tileSize)
        {
            int tries = 0;
            
            while (true)
            {
                Vector3 randomSpawnPoint = new Vector3(Random.Range(_minBounds.x + tileSize.x,
                    _maxBounds.x - tileSize.x), _maxBounds.y + tileSize.y, Random.Range
                    (_minBounds.z + tileSize.z, _maxBounds.z - tileSize.z));
                
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