using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Paridot
{
    public class Spin : MonoBehaviour
    {
        [SerializeField] Vector3 _spinVector;
        [SerializeField] private float _rotationSpeed;


        private void Update()
        {
            transform.Rotate(_spinVector);
        }
    }
}
