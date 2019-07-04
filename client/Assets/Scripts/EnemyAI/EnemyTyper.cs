using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CTShared;

public class EnemyTyper : MonoBehaviour
{
    public UIKeyboard UIKeyboard;

    public TextAsset SerializedRecording;

    Agent agent;
    KeyboardInputRecord recording;

    bool acceptingInput;

    float startTime;
    int nextIndex;

    void Start ()
    {
        recording = KeyboardInputRecord.Deserialize(SerializedRecording);

        ManagerContainer.Manager.OnTypePhaseStart += startPhase;
        ManagerContainer.Manager.OnTypePhaseEnd += endPhase;
    }

    void Update ()
    {
        if (!acceptingInput) return;

        var next = recording.Inputs[nextIndex];
        
        if (Time.time - startTime < next.Time) return;

        agent.PressKey(next.Key, next.Uppercase);

        nextIndex++;

        if (nextIndex >= recording.Inputs.Count)
        {
            acceptingInput = false;
        }
    }

    public void Initialize (Agent agent)
    {
        this.agent = agent;
    }

    void startPhase ()
    {
        acceptingInput = true;
        startTime = Time.time;
        nextIndex = 0;
    }

    void endPhase ()
    {
        acceptingInput = false;
    }
}
