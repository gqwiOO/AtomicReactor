using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.Transportation.WaterPipeSystem
{
    public class PipeView: SerializedMonoBehaviour
    {
        [SerializeField] private GameObject horizontal;
        [SerializeField] private GameObject vertical;
        [SerializeField] private GameObject angle_1;
        [SerializeField] private GameObject angle_2;
        [SerializeField] private GameObject angle_3;
        [SerializeField] private GameObject angle_4;
        
        [SerializeField] private Dictionary<Vector4, GameObject> states;

        public void SetState(Vector4 neighbours)
        {
            horizontal.SetActive(false);
            vertical.SetActive(false);
            angle_1.SetActive(false);
            angle_2.SetActive(false);
            angle_3.SetActive(false);
            angle_4.SetActive(false);

            if (neighbours == Vector4.zero)
                horizontal.SetActive(true);

            if (neighbours == new Vector4(1, 1, 0, 0))
            {
                angle_2.SetActive(true);
            }

            if (neighbours == new Vector4(0, 1, 1, 0))
            {
                angle_3.SetActive(true);
            }

            if (neighbours == new Vector4(0, 0, 1, 1))
            {
                angle_4.SetActive(true);
            }

            if (neighbours == new Vector4(1, 0, 0, 1))
            {
                angle_1.SetActive(true);
            }

            if (neighbours == new Vector4(0, 1, 0, 1) ||
                neighbours == new Vector4(0, 1, 0, 0) ||
                neighbours == new Vector4(0, 0, 0, 1))
            {
                horizontal.SetActive(true);
            }

            if (neighbours == new Vector4(1, 0, 1, 0) ||
                neighbours == new Vector4(0, 0, 1, 0) ||
                neighbours == new Vector4(1, 0, 0, 0))
            {
                vertical.SetActive(true);
            }

            if (states.TryGetValue(neighbours, out GameObject go))
            {
                go.SetActive(true);
            }

        // var isHorizontal = left && right;
            // var isVertical = up && down && !isHorizontal;
            // horizontal.SetActive(isHorizontal);
            // vertical.SetActive(isVertical);
            //
            // angle_1.SetActive(left && up);
            // angle_2.SetActive(right && up);
            // angle_3.SetActive(left && down);
            // angle_4.SetActive(right && down);
        }
    }
}