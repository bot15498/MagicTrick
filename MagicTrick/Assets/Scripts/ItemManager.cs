using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [Header("List of all the cards you can possibly get")]
    [SerializeField]
    private List<PlayableCard> AllCards = new List<PlayableCard>();
    [Header("List of all the Props")]
    [SerializeField]
    private List<PlayableProp> AllProps = new List<PlayableProp>();
    private System.Random rand;

    void Awake()
    {
        rand = new System.Random();
    }

    void Update()
    {

    }

    public PlayableCard GetRandomCard()
    {
        return AllCards[rand.Next(0, AllCards.Count)];
    }

    public PlayableCard GetRandomCard(CardType targetType)
    {
        List<PlayableCard> tempCards = new List<PlayableCard>();
        tempCards.AddRange(AllCards);
        tempCards = tempCards.Where(x => x.Type.Contains(targetType)).ToList();
        return tempCards[rand.Next(0, tempCards.Count)];
    }

    public PlayableProp GetRandomProp(List<PlayableProp> currentInventory)
    {
        List<PlayableProp> tempProps = new List<PlayableProp>();
        tempProps.AddRange(AllProps);
        tempProps = tempProps.Where(x => !currentInventory.Contains(x)).ToList();
        return tempProps[rand.Next(0, tempProps.Count)];
    }
}
