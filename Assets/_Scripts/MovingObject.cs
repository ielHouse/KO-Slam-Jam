using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Paridot
{
    public class MovingObject : MonoBehaviour
    {
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        [SerializeField] private Vector3 _changeVector;

        private float _zChange;

        [SerializeField] private float _cycleTime;
        private float _elapsedTime;

        private bool forward = true;


        private void Start()
        {
            _startPosition = transform.position;
            _endPosition = _startPosition + _changeVector;

            _elapsedTime = 0;
        }

        private void Update()
        {
            float t = _elapsedTime / _cycleTime;
            if (forward)
            {
                _elapsedTime += Time.deltaTime;
            }
            else
            {
                _elapsedTime -= Time.deltaTime;
            }

            if ((forward && t >= 1) || (!forward && t <= 0))
            {
                forward = !forward;
            }

            transform.position = Vector3.Lerp(_startPosition, _endPosition, t);
        }
    }
}
