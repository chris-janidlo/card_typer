using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CTShared;
using TMPro;

public class TypingDisplay : MonoBehaviour
{
	public TagPair BadLetterTag;
	public TextMeshProUGUI ProgressDisplay;
	public Image Line;

	List<Card> play;
	Agent agent;

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

		endPhase();
	}

	public void Initialize (Agent agent)
	{
		this.agent = agent;

		agent.OnPlaySet += p => play = p;

		agent.OnKeyPressed += (k, s) =>
		{
			if (agent.Keyboard[k].Type == KeyStateType.Active)
			{
				type(k, s);
			}
		};
	}

	void startPhase ()
	{
		progress = "";
		ProgressDisplay.enabled = true;
		Line.enabled = true;
	}

	void endPhase ()
	{
		ProgressDisplay.enabled = false;
		Line.enabled = false;
	}

	void type (KeyboardKey key, bool shift)
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
				addLetter(key.ToChar(shift));
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
