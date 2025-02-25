using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BalloonTap
{
    public class CloudSpawner : ObjectPool<Cloud>
    {
        [SerializeField] private float _spawnInterval;
        [SerializeField] private Cloud[] _prefabs;
        [SerializeField] private SpawnArea _spawnArea;
        [SerializeField] private int _poolCapacity;
        [SerializeField] private float _objMovingSpeed;
        [SerializeField] private int _spawnedCount;

        private List<Cloud> _spawnedObjects = new List<Cloud>();
        private IEnumerator _spawnCoroutine;

        private void Awake()
        {
            for (int i = 0; i <= _poolCapacity; i++)
            {
                ShuffleArray();

                foreach (var prefab in _prefabs)
                {
                    Initalize(prefab);
                }
            }
        }

        public void StartSpawn()
        {
            if (_spawnCoroutine != null) return;
            _spawnCoroutine = StartSpawning();
            StartCoroutine(_spawnCoroutine);
        }

        public void StopSpawn()
        {
            if (_spawnCoroutine == null) return;

            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }

        private IEnumerator StartSpawning()
        {
            WaitForSeconds interval = new WaitForSeconds(_spawnInterval);

            while (true)
            {
                List<Vector3> spawnPositions = GenerateSpawnPositions();

                for (int i = 0; i < spawnPositions.Count; i++)
                {
                    Spawn(spawnPositions[i]);
                }

                yield return interval;
            }
        }

        private List<Vector3> GenerateSpawnPositions()
        {
            const float minHorizontalSpacing = 1.5f;
            const float minVerticalDifference = 0.5f;

            List<Vector3> positions = new List<Vector3>();

            for (int i = 0; i < _spawnedCount; i++)
            {
                Vector3 spawnPosition;
                bool validPosition;

                do
                {
                    validPosition = true;

                    float randomX = _spawnArea.GetRandomXPosition();
                    float randomY = _spawnArea.GetRandomYPosition();
                    spawnPosition = new Vector3(randomX, randomY, 0);

                    foreach (var existingPosition in positions)
                    {
                        if (Mathf.Abs(existingPosition.x - spawnPosition.x) < minHorizontalSpacing ||
                            Mathf.Abs(existingPosition.y - spawnPosition.y) < minVerticalDifference)
                        {
                            validPosition = false;
                            break;
                        }
                    }
                } while (!validPosition);

                positions.Add(spawnPosition);
            }

            return positions;
        }


        private void Spawn(Vector3 spawnPosition)
        {
            if (ActiveObjects.Count >= _poolCapacity)
                return;

            int randomIndex = Random.Range(0, _prefabs.Length);
            Cloud prefabToSpawn = _prefabs[randomIndex];

            if (TryGetObject(out Cloud cloud, prefabToSpawn))
            {
                cloud.transform.position = spawnPosition;
                _spawnedObjects.Add(cloud);

                cloud.MovingComponent.EnableMovement(Vector3.down);
                cloud.MovingComponent.SetSpeed(_objMovingSpeed);
            }
        }
        
        public void ReturnToPool(Cloud cloud)
        {
            if (cloud == null)
                return;

            cloud.MovingComponent.DisableMovement();
            PutObject(cloud);

            if (_spawnedObjects.Contains(cloud))
                _spawnedObjects.Remove(cloud);
        }

        public void IncreaseSpeed()
        {
            _objMovingSpeed += 0.5f;
        }

        public void ReturnAllObjectsToPool()
        {
            if (_spawnedObjects.Count <= 0)
                return;

            List<Cloud> objectsToReturn = new List<Cloud>(_spawnedObjects);
            foreach (var @object in objectsToReturn)
            {
                @object.MovingComponent.DisableMovement();
                ReturnToPool(@object);
            }
        }

        private void ShuffleArray()
        {
            for (int i = 0; i < _prefabs.Length - 1; i++)
            {
                Cloud temp = _prefabs[i];
                int randomIndex = Random.Range(0, _prefabs.Length);
                _prefabs[i] = _prefabs[randomIndex];
                _prefabs[randomIndex] = temp;
            }
        }
    }
}