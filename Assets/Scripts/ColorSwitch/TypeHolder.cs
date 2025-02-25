using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ColorSwitch
{
    public class TypeHolder : MonoBehaviour
    {
        [SerializeField] private List<TypeData> _typeDatas;

        public TypeData GetRandomType()
        {
            var randomIndex = Random.Range(0, _typeDatas.Count);

            return _typeDatas[randomIndex];
        }
        
        public List<TypeData> GetAllTypes()
        {
            return new List<TypeData>(_typeDatas);
        }
    }

    [Serializable]
    public class TypeData
    {
        public Sprite Sprite;
        public FigureType FigureType;
        public ColorType ColorType;
    }

    public enum FigureType
    {
        Square,
        Circle,
        Star,
        Triangle
    }

    public enum ColorType
    {
        Yellow,
        Red,
        Green,
        Blue
    }
}