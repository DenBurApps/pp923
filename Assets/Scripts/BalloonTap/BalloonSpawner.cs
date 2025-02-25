using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BalloonTap
{
    public class BalloonSpawner : ObjectPool<Balloon>
    {
        [SerializeField] private float _spawnInterval;
        [SerializeField] private Balloon[] _prefabs;
        [SerializeField] private SpawnArea _spawnArea;
        [SerializeField] private int _poolCapacity;
        [SerializeField] private float _objMovingSpeed;
        [SerializeField] private int _spawnedCount;

        private List<Balloon> _spawnedObjects = new List<Balloon>();
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
            Balloon prefabToSpawn = _prefabs[randomIndex];

            if (TryGetObject(out Balloon balloon, prefabToSpawn))
            {
                balloon.transform.position = spawnPosition;
                _spawnedObjects.Add(balloon);

                balloon.MovingComponent.EnableMovement(Vector3.up);
                balloon.MovingComponent.SetSpeed(_objMovingSpeed);
            }
        }

        public void ReturnToPool(Balloon balloon)
        {
            if (balloon == null)
                return;

            balloon.MovingComponent.DisableMovement();
            PutObject(balloon);

            if (_spawnedObjects.Contains(balloon))
                _spawnedObjects.Remove(balloon);
        }

        public void IncreaseSpeed()
        {
            _objMovingSpeed += 0.1f;
        }

        public void ResetSpeed()
        {
            _objMovingSpeed = 4;
        }

        public void ReturnAllObjectsToPool()
        {
            if (_spawnedObjects.Count <= 0)
                return;

            List<Balloon> objectsToReturn = new List<Balloon>(_spawnedObjects);
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
                Balloon temp = _prefabs[i];
                int randomIndex = Random.Range(0, _prefabs.Length);
                _prefabs[i] = _prefabs[randomIndex];
                _prefabs[randomIndex] = temp;
            }
        }
    }
}
