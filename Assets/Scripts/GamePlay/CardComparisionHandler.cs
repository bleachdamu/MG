using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for card comparision.
/// </summary>
public interface ICardComparer
{
    public int cardID { get; }
    public bool CompareCard(int cardId);
}

public class CardComparisionHandler : MonoBehaviour
{
    private List<Card> cards;
    private Queue<Card> SelectedCards = new Queue<Card>();

    /// <summary>
    /// Initialize the CardComparisionHandler with a list of cards.
    /// </summary>
    public void Initialize(List<Card> cards)
    {
        this.cards = cards;
        for(int i=0;i<cards.Count;i++)
        {
            cards[i].CardFrontFacing += OnCardSelected;
        }
    }

    /// <summary>
    /// Handle the card selection event.
    /// </summary>
    public void OnCardSelected(Card card)
    {
        SelectedCards.Enqueue(card);
        CompareCards();
    }

    /// <summary>
    /// Compare the selected cards.
    /// </summary>
    public void CompareCards()
    {
        if (SelectedCards.Count == 2)
        {
            Card card1 = SelectedCards.Dequeue();
            Card card2 = SelectedCards.Dequeue();
            if(card1.CompareCard(card2.cardID))
            {
                Debug.Log("Cards are same");
            }else
            {
                Debug.Log("Cards are not same");
            }
            card2.CompareCard(card1.cardID);
            CompareCards();
        }
    }
}
