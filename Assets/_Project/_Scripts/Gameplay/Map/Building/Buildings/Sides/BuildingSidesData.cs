using System;
using System.Collections.Generic;
using System.Numerics;
using JetBrains.Annotations;
using UnityEngine;

namespace Gameplay.Map.Building
{
    [Serializable]
    public class BuildingSidesData
    {
        private SideType _left;
        public SideType Left
        {
            get => _left;
            set
            {
                _left = value;
                OnAnySideChanged?.Invoke();
            }
        }
        private SideType _right;
        public SideType Right
        {
            get => _right;
            set
            {
                _right = value;
                OnAnySideChanged?.Invoke();
            }
        }
        private SideType _front;
        public SideType Front
        {
            get => _front;
            set
            {
                _front = value;
                OnAnySideChanged?.Invoke();
            }
        }
        private SideType _back;
        public SideType Back
        {
            get => _back;
            set
            {
                _back = value;
                OnAnySideChanged?.Invoke();
            }
        }

        public List<SideType> AllowedTypes;

        public event Action OnAnySideChanged;

        public BuildingSidesData(SideType left, SideType right, SideType front, SideType back,
            List<SideType> allowedTypes = null)
        {
            Left = left;
            Right = right;
            Front = front;
            Back = back;
            
            AllowedTypes = allowedTypes;
        }

        public SideType GetSide(Vector2Int vector2)
        {
            if (vector2 == Vector2Int.right)
                return Right;
            if (vector2 == Vector2Int.left)
                return Left;
            if (vector2 == Vector2Int.up)
                return Back;
            if (vector2 == Vector2Int.down)
                return Front;
            
            return SideType.None;
        }
    }
}