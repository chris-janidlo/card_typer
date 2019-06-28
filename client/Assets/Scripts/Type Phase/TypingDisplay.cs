using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CTShared;
using TMPro;

public class TypingDisplay : MonoBehaviour
{
	public TagPair BadLetterTag;
	public TextMeshProUGUI ProgressDisplay;

	List<Card> play;

	string progress
	{
		get => ProgressDisplay.text;
		set => ProgressDisplay.text = value;
	}

	void Start ()
	{
		var mgr = ManagerContainer.Manager;

		mgr.OnTypePhaseStart += startPhase;
		mgr.OnTypePhaseEnd += endPhase;
	}

	public void SetPlay (List<Card> _play)
	{
		play = _play;
	}

	void startPhase ()
	{
		progress = "";
		ProgressDisplay.enabled = true;
	}

	void endPhase ()
	{
		ProgressDisplay.enabled = false;
	}

	public void Type (KeyboardKey key)
	{
		switch (key)
		{
			case KeyboardKey.Return:
			case KeyboardKey.Space:
				progress = "";
				break;
			
			case KeyboardKey.Backspace:
				deleteLetter();
				break;
			
			default:
				addLetter(key.ToChar());
				break;
		}
	}

	void deleteLetter ()
	{
		int delLen = 1;
		if (progress[progress.Length - 1] == '>')
		{
			// 'c' here is totally arbitrary - just getting the length of one wrapped character
			delLen = BadLetterTag.Wrap('c').Length;
		}

		progress = progress.Substring(0, progress.Length - delLen);
	}

	void addLetter (char letter)
	{
		var next = progress + letter;

		if (play.Any(c => c.Word.StartsWith(next)))
		{
			progress = next;
		}
		else
		{
			next = BadLetterTag.Wrap(letter);
			progress += next;
		}
	}
}
