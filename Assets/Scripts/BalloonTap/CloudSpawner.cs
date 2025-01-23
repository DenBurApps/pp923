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
                Spawn();
                yield return interval;
            }
        }

        private void Spawn()
        {
            if (ActiveObjects.Count >= _poolCapacity)
                return;

            int randomIndex = Random.Range(0, _prefabs.Length);
            Cloud prefabToSpawn = _prefabs[randomIndex];

            if (TryGetObject(out Cloud cloud, prefabToSpawn))
            {
                cloud.transform.position = _spawnArea.GetRandomXPositionToSpawn();
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