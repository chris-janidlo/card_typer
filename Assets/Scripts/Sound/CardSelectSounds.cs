using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class CardSelectSounds : Singleton<CardSelectSounds>
{
    [Serializable]
    public struct CardSound { public AudioClip Start, Loop; }

    public List<CardSound> InitialSounds;
    public List<CardSound> AdditionalSounds;

    public List<AudioSource> StartPlayers, LoopPlayers;
    public AudioSource StopPlayer; // this is the only one where you should set the clip in the source directly

    public float FadeTime;

    bool additional = false;
    int soundCount, initialSound;

    Dictionary<AudioSource, IEnumerator> faders;

    void Awake ()
    {
        SingletonSetInstance(this, true);

        AdditionalSounds.ShuffleInPlace();

        faders = new Dictionary<AudioSource, IEnumerator>();
    }

    public void PlayNewSound ()
    {
        CardSound pair = additional ? AdditionalSounds[soundCount - 1] : InitialSounds[initialSound];

        AudioSource startPlayer = StartPlayers[soundCount];
        AudioSource loopPlayer = LoopPlayers[soundCount];

        stopFade(startPlayer);
        startPlayer.PlayOneShot(pair.Start);

        stopFade(loopPlayer);
        loopPlayer.clip = pair.Loop;
        loopPlayer.PlayScheduled(AudioSettings.dspTime + pair.Start.length);

        soundCount++;
        if (!additional) additional = true;
    }

    public void StopAllSounds ()
    {
        if (soundCount == 0) return;

        StopPlayer.Play();

        for (int i = 0; i < soundCount; i++)
        {
            fade(StartPlayers[i]);
            fade(LoopPlayers[i]);
        }

        soundCount = 0;
        additional = false;
        initialSound = 1 - initialSound;
        AdditionalSounds.ShuffleInPlace();
    }

    void fade (AudioSource source)
    {
        if (faders.ContainsKey(source)) return;

        faders[source] = fader(source);
        StartCoroutine(faders[source]);
    }

    void stopFade (AudioSource source)
    {
        if (!faders.ContainsKey(source)) return;

        StopCoroutine(faders[source]);
        faders[source] = null;
        source.volume = 1;
    }

    IEnumerator fader (AudioSource source)
    {
        float initialVolume = source.volume;

        while (source.volume > 0)
        {
            source.volume -= initialVolume / FadeTime * Time.deltaTime;
            yield return null;
        }

        source.Stop();
        source.volume = initialVolume;

        faders.Remove(source);
    }
}
