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
        
        protected override void PerformTransition(float t)
        {
            print($"Gamestate: {_gameState}, Cam Rotation: {transform.rotation}, Side: {sideRotation}, Persp: {perspectiveRotation}, t: {t}");
            if (_gameState == GameState.Perspective)
            {
                transform.rotation = Quaternion.Slerp(Quaternion.Euler(sideRotation), Quaternion.Euler(perspectiveRotation), t);
                cam.orthographic = false;
            }
            else
            {
                transform.rotation = Quaternion.Slerp(Quaternion.Euler(perspectiveRotation), Quaternion.Euler(sideRotation), t);
                cam.orthographic = true;
            }
        }
    }
}
