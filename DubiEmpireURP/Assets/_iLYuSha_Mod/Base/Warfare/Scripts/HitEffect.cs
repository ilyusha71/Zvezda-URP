using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Warfare
{
    public class HitEffect : MonoBehaviour
    {
        private ParticleSystem[] particleSystems;
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            particleSystems = GetComponentsInChildren<ParticleSystem>();
        }

        public void Hit()
        {
            for (int i = 0; i < particleSystems.Length; i++)
            {
                particleSystems[i].Play();
            }
        }
    }
}