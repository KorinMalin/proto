using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    [RequireComponent(typeof(ParticleSystem))]
    public class PlayParticleSystem : MonoBehaviour
    {
        #region Attributes
        [SerializeField]
        private bool startOnEnable = true;

        private ParticleSystem pSystem;
        private float emissionRateOverTime;
        #endregion

        #region MonoBehaviour Methods
        private void Start()
        {
            pSystem = GetComponent<ParticleSystem>();
            emissionRateOverTime = pSystem.emission.rateOverTime.constant;
        }

        private void OnEnable()
        {
            if (startOnEnable)
                StartParticleSystem();
        }
        #endregion

        #region Methods
        public void StartParticleSystem()
        {
            if (!pSystem.isPlaying)
                pSystem.Play();
        }

        public void StartEmissionRateOverTime()
        {
            ParticleSystem.EmissionModule emissionModule = pSystem.emission;
            emissionModule.rateOverTime = emissionRateOverTime;
        }

        public void StopEmissionRateOverTime()
        {
            ParticleSystem.EmissionModule emissionModule = pSystem.emission;
            emissionModule.rateOverTime = 0;
        }
        #endregion
    }
}
