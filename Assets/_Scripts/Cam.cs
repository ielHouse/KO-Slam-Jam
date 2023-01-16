using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paridot
{
    public class Cam : TransitionObject
    {
        [SerializeField] private Camera cam;
        
        [SerializeField] private Vector3 perspectiveRotation;
        [SerializeField] private Vector3 sideRotation;
        
        [SerializeField] private Vector3 perspectivePosition;
        [SerializeField] private Vector3 sidePosition;

        private void Update()
        {
            
        }

        protected override void PerformTransition(float t)
        {
            if (_gameState == GameState.Perspective)
            {
                transform.rotation = Quaternion.Slerp(Quaternion.Euler(sideRotation), Quaternion.Euler(perspectiveRotation), t);
                cam.transform.localPosition = Vector3.Lerp(sidePosition, perspectivePosition, t);
                cam.orthographic = false;
            }
            else
            {
                transform.rotation = Quaternion.Slerp(Quaternion.Euler(perspectiveRotation), Quaternion.Euler(sideRotation), t);
                cam.transform.localPosition = Vector3.Lerp(perspectivePosition, sidePosition, t);
                cam.orthographic = true;
            }
        }
    }
}
