using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace LightingReflex
{
    public class LightDataHolder : MonoBehaviour
    {
        [SerializeField] private List<LightData> _lightDatas;
        [SerializeField] private List<LightButton> _lightButtons;

        public event Action<LightData> DataSelected;
        
        private void OnEnable()
        {
            foreach (var obj in _lightButtons)
            {
                obj.Clicked += OnDataSelected;
            }
        }

        private void OnDisable()
        {
            foreach (var obj in _lightButtons)
            {
                obj.Clicked -= OnDataSelected;
            }
        }

        public void DisableAllObjects()
        {
            foreach (var obj in _lightButtons)
            {
                obj.Disable();
            }
        }
        
        public void AssignDatas()
        {
            var list = new List<LightData>(_lightDatas);
            
            list.Shuffle();

            for (int i = 0; i < _lightButtons.Count; i++)
            {
                _lightButtons[i].Enable(list[i]);
            }
        }

        private void OnDataSelected(LightData data)
        {
            DataSelected?.Invoke(data);
        }
    }
}
