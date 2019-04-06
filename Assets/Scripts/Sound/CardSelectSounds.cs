using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class CardSelectSounds : Singleton<CardSelectSounds>
{
    [Serializable]
    public struct CardSound { public AudioClip Start, Loop; }

    [Serializable]
    public class AdditionalBag : BagRandomizer<CardSound> {}

    public List<CardSound> InitialSounds;
    public List<CardSound> AdditionalSounds;

    public List<AudioSource> StartPlayers, LoopPlayers;

    bool additional = false;
    int soundCount, initialSound;

    void Start ()
    {
        SingletonSetInstance(this, true);

        AdditionalSounds.ShuffleInPlace();
    }

    public void PlayNewSound ()
    {
        CardSound pair = additional ? AdditionalSounds[soundCount - 1] : InitialSounds[initialSound];

        AudioClip startSound = pair.Start;
        AudioClip loopSound = pair.Loop;

        StartPlayers[soundCount].PlayOneShot(startSound);

        LoopPlayers[soundCount].clip = loopSound;
        LoopPlayers[soundCount].PlayScheduled(AudioSettings.dspTime + startSound.length);

        soundCount++;
        if (!additional) additional = true;
    }

    public void StopAllSounds ()
    {
        // TODO: play stop effect

        for (int i = 0; i < soundCount; i++)
        {
            StartPlayers[i].Stop();
            LoopPlayers[i].Stop();
        }

        soundCount = 0;
        additional = false;
        initialSound = 1 - initialSound;
        AdditionalSounds.ShuffleInPlace();
    }
}
