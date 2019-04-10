using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Cards1492;
using crass;

public class Enemy : Agent
{
    class spellPlan
    {
        public List<Card> Spells;
        public List<float> SuccessChances;
        public List<Card> OtherInDraw;

        public spellPlan (List<Card> spells, List<float> successChances, List<Card> otherInDraw)
        {
            Spells = spells;
            SuccessChances = successChances;
            OtherInDraw = otherInDraw;
        }

        public List<Card> GetCasting ()
        {
            List<Card> casting = new List<Card>();
            for (int i = 0; i < Spells.Count; i++)
            {
                if (RandomExtra.Chance(SuccessChances[i]))
                {
                    casting.Add(Spells[i]);
                }
                else
                {
                    break;
                }
            }
            return casting;
        }

        // randomly shuffles Spells and OtherInDraw together to look like a real draw
        public string GetDrawString ()
        {
            var draw = Spells.Concat(OtherInDraw).ToList();
            draw.ShuffleInPlace();

            string result = "";
            for (int i = 0; i < draw.Count - 1; i++)
            {
                result += draw[i].Word + ", ";
            }
            result += " and " + draw[draw.Count - 1].Word;

            return result;
        }
    }

    BagRandomizer<spellPlan> plans;
    spellPlan currentPlan;

    protected override void Awake ()
    {
        base.Awake();

        plans = new BagRandomizer<spellPlan>();
        plans.AvoidRepeats = true;

        plans.Items = new List<spellPlan> {
            new spellPlan(
                new List<Card> {
                    new Lock { Word = "locked" },
                    new TwoFaced { Word = "two-faced" },
                    new Sword { Word = "sword" }
                },
                new List<float> {
                    .9f,
                    .8f,
                    1
                },
                new List<Card> {
                    new Flaming { Word = "flaming" },
                    new Hound { Word = "Hounded" }
                }
            ),
            new spellPlan(
                new List<Card> {
                    new Zealot { Word = "zealot" },
                    new Grim { Word = "Grim" }
                },
                new List<float> {
                    .7f,
                    .9f,
                    .6f
                },
                new List<Card> {
                    new Lock { Word = "locked" },
                    new Heart { Word = "heart" },
                    new Prince { Word = "Prince" }
                }
            ),
            new spellPlan(
                new List<Card> {
                    new Priest { Word = "priest" },
                    new Heart { Word = "heart" },
                    new Zealot { Word = "zealot" },
                    new Devise { Word = "devised" }
                },
                new List<float> {
                    1,
                    .5f,
                    1,
                    1
                },
                new List<Card> {
                    new Hound { Word = "Hounded" }
                }
            ),
            new spellPlan(
                new List<Card> {
                    new Ancient { Word = "ancient" },
                    new Grim { Word = "Grim" },
                    new Abhor { Word = "abhorred" }
                },
                new List<float> {
                    .7f,
                    1,
                    .9f
                },
                new List<Card> {
                    new Sunset { Word = "sunset" },
                    new Unveil { Word = "unveild'st" }
                }
            )
        };
    }

    protected override void Start ()
    {
        base.Start();
        Death += a => SceneManager.LoadScene("Victory");
    }

    public string GetDraw ()
    {
        currentPlan = plans.GetNext();
        return currentPlan.GetDrawString();
    }

    public List<Card> GetHand ()
    {
        return currentPlan.GetCasting();
    }
}
