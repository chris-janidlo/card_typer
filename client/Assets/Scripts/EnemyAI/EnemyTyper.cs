using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTyper : LocalTyper
{
    public TextAsset SerializedRecording;

    KeyboardInputRecord recording;

    float startTime;
    int nextIndex;

    void Start ()
    {
        recording = KeyboardInputRecord.Deserialize(SerializedRecording);
    }

    public override void StartPhase ()
    {
        base.StartPhase();
        startTime = Time.time;
        nextIndex = 0;
    }

    void Update ()
    {
        if (!AcceptingInput) return;

        var next = recording.Inputs[nextIndex];
        
        if (Time.time - startTime < next.Time) return;

        typeKey(next.Key, next.Uppercase);
        nextIndex++;

        if (nextIndex >= recording.Inputs.Count)
        {
            AcceptingInput = false;
        }
    }
}
