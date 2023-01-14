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
            if (_gameState == GameState.Perspective)
            {
                transform.rotation = Quaternion.Slerp(Quaternion.Euler(sideRotation), Quaternion.Euler(perspectiveRotation), t);
                // cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(cam.transform.position.x, cam.transform.position.y, -31), t);
                cam.orthographic = false;
            }
            else
            {
                transform.rotation = Quaternion.Slerp(Quaternion.Euler(perspectiveRotation), Quaternion.Euler(sideRotation), t);
                // cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(cam.transform.position.x, cam.transform.position.y, -10), t);
                cam.orthographic = true;
            }
        }
    }
}
