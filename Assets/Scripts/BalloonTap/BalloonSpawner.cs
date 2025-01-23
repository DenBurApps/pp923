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
                Spawn();
                yield return interval;
            }
        }

        private void Spawn()
        {
            if (ActiveObjects.Count >= _poolCapacity)
                return;

            int randomIndex = Random.Range(0, _prefabs.Length);
            Balloon prefabToSpawn = _prefabs[randomIndex];

            if (TryGetObject(out Balloon balloon, prefabToSpawn))
            {
                balloon.transform.position = _spawnArea.GetRandomXPositionToSpawn();
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