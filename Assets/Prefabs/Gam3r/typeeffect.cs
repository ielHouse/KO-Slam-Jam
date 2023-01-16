using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Paridot
{
    public class typeeffect : MonoBehaviour
    {
        public float delay = 0.1f;
        public string fulltext;
        private string currenttext = "";

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(ShowText());
        }
        IEnumerator ShowText()
            {
                for(int ii = 0; ii< fulltext.Length; ii++)
                {
                    currenttext = fulltext.Substring(0, ii);
                    this.GetComponent<TextMeshProUGUI>().text = currenttext;
                    yield return new WaitForSeconds(delay);
                }
            }
        

        
    }
}
