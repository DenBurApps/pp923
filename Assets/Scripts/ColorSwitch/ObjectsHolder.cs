using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace ColorSwitch
{
    public class ObjectsHolder : MonoBehaviour
    {
        [SerializeField] private List<ClickableObject> _clickableObjects;
        [SerializeField] private TypeHolder _typeHolder;

        private List<TypeData> _selectedDatas = new();

        public event Action<TypeData> DataSelected;

        public IReadOnlyList<TypeData> SelectedDatas => _selectedDatas;

        private void OnEnable()
        {
            foreach (var obj in _clickableObjects)
            {
                obj.Clicked += OnDataSelected;
            }
        }

        private void OnDisable()
        {
            foreach (var obj in _clickableObjects)
            {
                obj.Clicked -= OnDataSelected;
            }
        }

        public void DisableAllObjects()
        {
            foreach (var obj in _clickableObjects)
            {
                obj.Disable();
            }

            _selectedDatas.Clear();
        }

        public void AssignAllObjects()
        {
            for (int i = 0; i < _clickableObjects.Count; i++)
            {
                _selectedDatas.Add(_typeHolder.GetRandomType()); 
            }

            for (int i = 0; i < _clickableObjects.Count; i++)
            {
                _clickableObjects[i].Enable(_selectedDatas[i]);
            }
        }

        private void OnDataSelected(TypeData data)
        {
            DataSelected?.Invoke(data);
        }
    }
}