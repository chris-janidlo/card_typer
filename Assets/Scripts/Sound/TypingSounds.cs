using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class TypingSounds : Singleton<TypingSounds>
{
    public SoundCycler LetterSounds, WordSounds, DudSounds;

    void Awake () => SingletonSetInstance(this, true);

    public void PlayLetter () => LetterSounds.Play();
    public void PlayWord () => WordSounds.Play();
    public void PlayDud () => DudSounds.Play();
}

[System.Serializable]
public class SoundCycler
{
    [System.Serializable]
    public class SoundBag : BagRandomizer<AudioClip> {}

    public SoundBag Sounds;
    public List<AudioSource> Players;

    int index;
    
    public void Play ()
    {
        Players[index].PlayOneShot(Sounds.GetNext());
        index = (index + 1) % Players.Count;
    }
}
