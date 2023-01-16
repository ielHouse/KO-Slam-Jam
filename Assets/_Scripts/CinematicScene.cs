using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Paridot
{
    public class CinematicScene : MonoBehaviour
    {
        [SerializeField] private float _sceneDuration;
        [SerializeField] private string _sceneName;
        private float _elapsedTime = 0f;

        private void Update()
        {
            if (_elapsedTime >= _sceneDuration)
            {
                SceneManager.LoadScene(_sceneName, LoadSceneMode.Single);
                Destroy(gameObject);
            }
            _elapsedTime += Time.deltaTime;
        }
    }
}
