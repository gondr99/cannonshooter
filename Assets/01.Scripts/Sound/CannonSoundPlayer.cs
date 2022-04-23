using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonSoundPlayer : SoundPlayer
{
    [SerializeField] private AudioClip _fireClip;
    [SerializeField] private AudioClip _explosionClip;
    public void PlayFireSound()
    {
        PlayClipWithPitch(_fireClip);
    }

    public void PlayExplosionSound()
    {
        PlayClip(_explosionClip);
    }
    
}
