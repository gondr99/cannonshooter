using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    private ParticleSystem _mainExp;
    private ParticleSystem _debriExp;

    private void Awake()
    {
        _mainExp = GetComponent<ParticleSystem>();
        _debriExp = transform.Find("DebriParticle").GetComponent<ParticleSystem>();
        _debriExp.gameObject.SetActive(false);
    }

    public void PlayParticle(bool isDebri)
    {
        if (isDebri == true)
        {
            _debriExp.gameObject.SetActive(true);
        }
        _mainExp.Play();            
    }
}
