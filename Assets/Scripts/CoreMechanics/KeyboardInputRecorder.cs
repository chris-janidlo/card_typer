using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
public class KeyboardInputRecorder : MonoBehaviour
{
    KeyboardInputRecord recording;
    float recordingStartTime;
    bool isRecording;

    [SerializeField]
    string totalInput;

    void OnGUI ()
    {
        if (!isRecording) return;

        Event e = Event.current;

        if (!e.isKey || e.type != EventType.KeyDown) return;

        KeyCode key = e.keyCode;

        if (key.IsAcceptableInput(true))
        {
            recording.AddInput(key, e.shift, Time.time - recordingStartTime);
        }

        if (key.IsAcceptableInput(false))
        {
            totalInput += key.ToChar(e.shift);
        }

        if (key == KeyCode.Backspace)
        {
            totalInput = totalInput.Substring(0, totalInput.Length - 1);      
        }
    }
    
    public void SetRecordingState (bool state)
    {
        if (state)
        {
            startRecording();
        }
        else
        {
            endRecording();
        }
    }

    void startRecording ()
    {
        if (isRecording)
        {
            Debug.Log("Already recording");
            return;
        }

        isRecording = true;
        recordingStartTime = Time.time;
        recording = new KeyboardInputRecord();
        totalInput = "";
        
        Debug.Log("Started recording");
    }

    void endRecording ()
    {
        if (!isRecording)
        {
            Debug.Log("Not recording");
            return;
        }
        
        isRecording = false;
        GUIUtility.systemCopyBuffer = JsonUtility.ToJson(recording);
        
        Debug.Log("Stopped recording. The recording has been copied to your clipboard");
    }
}
#endif

[Serializable]
public class KeyboardInputRecord
{
    [Serializable]
    public class MomentaryInput
    {
        public KeyCode Key;
        public bool Uppercase;
        public float Time;
    }

    public List<MomentaryInput> Inputs;

    public static KeyboardInputRecord Deserialize (TextAsset source)
    {
        return JsonUtility.FromJson<KeyboardInputRecord>(source.text);
    }

    public KeyboardInputRecord ()
    {
        Inputs = new List<MomentaryInput>();
    }

    public void AddInput (KeyCode key, bool uppercase, float time)
    {
        var next = new MomentaryInput { Key = key, Uppercase = uppercase, Time = time };
        Inputs.Add(next);
    }
}
