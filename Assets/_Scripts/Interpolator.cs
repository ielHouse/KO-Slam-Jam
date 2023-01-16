using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paridot
{
    public class Interpolator : MonoBehaviour
    {
        private Quaternion _startRotation;
        [SerializeField] private Vector3 _endRotation;
        
        private Vector3 _startPosition;
        [SerializeField] private Vector3 _endPosition;

        [SerializeField] private float _duration;

        private void Start()
        {
            _startPosition = transform.position;
            _startRotation = transform.rotation;
            
            StartCoroutine(Interpolate());
        }

        private IEnumerator Interpolate()
        {
            float elapsedTime = 0f;

            while (elapsedTime <= _duration)
            {
                float t = elapsedTime / _duration;
                transform.position = Vector3.Lerp(_startPosition, _endPosition, t);
                transform.rotation = Quaternion.Lerp(_startRotation, Quaternion.Euler(_endRotation), t);
                
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            transform.position = _endPosition;
            transform.rotation = Quaternion.Euler(_endRotation);
        }
    }
}
