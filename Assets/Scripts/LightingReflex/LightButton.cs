using System;
using UnityEngine;
using UnityEngine.UI;

namespace LightingReflex
{
    public class LightButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _greenSprite;

        public event Action<LightData> Clicked;
        
        public LightData LightData { get; private set; }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClicked);
        }

        public void Enable(LightData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            _button.enabled = true;
            LightData = data;
            _image.sprite = LightData.Sprite;
        }

        public void Disable()
        {
            _image.sprite = _greenSprite;
            _button.enabled = false;
        }
        
        private void OnClicked()
        {
            Clicked?.Invoke(LightData);
        }
    }

    [Serializable]
    public class LightData
    {
        public Sprite Sprite;
        public LightType LightType;
    }

    public enum LightType
    {
        Green,
        Red,
        Blue,
        Yellow
    }
}