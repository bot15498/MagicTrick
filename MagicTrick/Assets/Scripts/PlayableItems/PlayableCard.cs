using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public enum CardType
{ 
    Showmanship,
    Deception,
    Criminal,
    Financial
}

public enum Stats
{
    Captivation,
    SleightOfHand,
    Payout,
    Liability,
}

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class PlayableCard : PlayableItem
{
    public string CardName;
    [TextArea]
    public string Description;
    public Sprite Image;
    public List<CardType> Type = new List<CardType>();
    public string AnimationName;

    public ActionContainer ApplyCard(ActionContainer container, int slot, GameManager gameManager)
    {
        // Preview what the card will add in score.
        foreach (ActAction act in Actions)
        {
            container = act.AddAction(container, slot, gameManager);
        }

        return container;
    }
}
