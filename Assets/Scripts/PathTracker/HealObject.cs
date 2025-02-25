using System.Collections;
using UnityEngine;

namespace PathTracker
{
    public class HealObject : MonoBehaviour
    {
        [SerializeField] private float _disableDelay = 5f;
        
        public void Enable()
        {
            gameObject.SetActive(true);
            StartCoroutine(AutoDisable());
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            StopCoroutine(AutoDisable());
        }
        
        private IEnumerator AutoDisable()
        {
            yield return new WaitForSeconds(_disableDelay);
            Disable();
        }

        private void Start()
        {
            StartCoroutine(AutoDisable());
        }
    }
}