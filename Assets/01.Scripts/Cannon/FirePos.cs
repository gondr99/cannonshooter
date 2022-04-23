using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePos : MonoBehaviour
{
    private ParticleSystem _fireParticle;
    private void Awake()
    {
        _fireParticle = transform.Find("FireEffect").GetComponent<ParticleSystem>();
        _fireParticle.gameObject.SetActive(false);
    }

    public void PlayParticle()
    {
        _fireParticle.gameObject.SetActive(true);
        _fireParticle.Play();
    }
}
