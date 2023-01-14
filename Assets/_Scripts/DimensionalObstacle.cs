using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Paridot
{
    public class DimensionalObstacle : TransitionObject
    {
        private Vector3 _perspectivePosition;
        private Vector3 _sidePosition;

        private void Start()
        {
            base.Start();

            _perspectivePosition = transform.position;
            _sidePosition = transform.position;
        }

        protected override void PerformTransition(float t)
        {
            _sidePosition.z = _z - 1;
            base.PerformTransition(t);
            if (_gameState == GameState.Perspective)
            {
                transform.position = Vector3.Lerp(_sidePosition, _perspectivePosition, t);
            }
            else
            {
                transform.position = Vector3.Lerp(_perspectivePosition, _sidePosition, t);
            }
        }
    }
}
