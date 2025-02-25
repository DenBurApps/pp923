using System;
using UnityEngine;
using UnityEngine.UI;

namespace ColorSwitch
{
    public class ClickableObject : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;

        public event Action<TypeData> Clicked; 
        
        public TypeData TypeData { get; private set; }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClicked);
        }

        public void Enable(TypeData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            
            gameObject.SetActive(true);
            TypeData = data;
            _image.sprite = TypeData.Sprite;
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        private void OnClicked()
        {
            Clicked?.Invoke(TypeData);
        }
    }
}