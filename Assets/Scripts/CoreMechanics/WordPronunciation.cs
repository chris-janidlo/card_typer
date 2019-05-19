using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordPronunciation
{
	public List<Pronunciation> Pronunciations;

	// returns true if any two pronunciations rhyme
	public bool RhymesWith (WordPronunciation other)
	{
		// TODO: more efficient?
		foreach (var p1 in Pronunciations)
		{
			foreach (var p2 in other.Pronunciations)
			{
				if (p1.RhymesWith(p2))
				{
					return true;
				}
			}
		}
		return false;
	}
}

public class Pronunciation
{
	public List<Syllable> Syllables;

	// returns true if, from the last stressed syllable onward, every syllable rhymes
	public bool RhymesWith (Pronunciation other)
	{
		List<Syllable> endThis = stressEnd(), endOther = other.stressEnd();

		return endThis.Except(endOther).Count() == 0 && 
		       endOther.Except(endThis).Count() == 0;
	}

	List<Syllable> stressEnd ()
	{
		return Syllables.GetRange(lastStressedLocation(), Syllables.Count);
	}

	int lastStressedLocation ()
	{
		int primaryIndex = 0, secondaryIndex = 0;
		
		for (int i = 0; i < Syllables.Count; i++)
		{
			switch (Syllables[i].Stress)
			{
				case Stress.Primary:
					primaryIndex = i;
					break;
				
				case Stress.Secondary:
					secondaryIndex = i;
					break;
			}
		}

		if (primaryIndex == 0)
		{
			return secondaryIndex;
		}
		else
		{
			return primaryIndex;
		}
	}
}

public class Syllable
{
	public Stress Stress;
	public Phoneme Phoneme;
}

public enum Stress
{
	Unstressed, Primary, Secondary
}

// based on CMU pronouncing dictionary http://www.speech.cs.cmu.edu/cgi-bin/cmudict
public enum Phoneme
{ 
	AA, AE, AH, AO, AW, AY, B, CH, D, DH, EH, ER, EY, F, G, HH, IH, IY, JH, K, L, M, N, NG, OW, OY, P, R, S, SH, T, TH, UH, UW, V, W, Y, Z, ZH
}
