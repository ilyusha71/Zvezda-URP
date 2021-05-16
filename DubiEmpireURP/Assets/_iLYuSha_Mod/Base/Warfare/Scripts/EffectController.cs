using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Warfare
{
    public class EffectController : MonoBehaviour
    {
        public Transform fxFire;
        public Transform fxHit;
        private ParticleSystem[] psFire;
        private ParticleSystem[] psHit;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            psFire = fxFire.GetComponentsInChildren<ParticleSystem>();
            psHit = fxHit.GetComponentsInChildren<ParticleSystem>();
        }

        public void Fire()
        {
            // AudioClip clip = fxFire.GetComponent<AudioSource> ().clip;
            // fxFire.GetComponent<AudioSource> ().PlayOneShot (clip);

            for (int i = 0; i < psFire.Length; i++)
            {
                psFire[i].Play();
            }
        }
        public void FireSound()
        {
            fxFire.GetComponent<AudioSource>().Play();
        }

        public void Hit()
        {
            for (int i = 0; i < psHit.Length; i++)
            {
                psHit[i].Play();
            }
        }
    }
}