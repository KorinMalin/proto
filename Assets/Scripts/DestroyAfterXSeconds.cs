using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class DestroyAfterXSeconds : MonoBehaviour
    {
        // Attributes
        [SerializeField]
        private float secondsBeforeDestruction = 3;

        // MonoBehaviour Methods
        void Update()
        {
            secondsBeforeDestruction -= Time.deltaTime;
            if (secondsBeforeDestruction <= 0)
                Destroy(gameObject);
        }
    }
}
