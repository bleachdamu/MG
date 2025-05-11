using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

/// <summary>
/// Interface for card comparision.
/// </summary>
public interface ICardComparer
{
    public int CardID { get; }
    public bool CompareCard(ICardComparer cardId);
}

public class CardComparisionHandler : MonoBehaviour
{
    private List<Card> cards;
    private Queue<Card> SelectedCards = new Queue<Card>();
    private Action<int> OnCardMatching;
    private int combo = 0;
    private SaveData saveData;

    /// <summary>
    /// Initialize the CardComparisionHandler with a list of cards.
    /// </summary>
    public void Initialize(ref List<Card> cards,Action<int> OnCardMatching,SaveData saveData)
    {
        this.saveData = saveData;
        this.OnCardMatching = OnCardMatching;
        this.cards = cards;
        for(int i=0;i<cards.Count;i++)
        {
            cards[i].CardFrontFacing.AddListener(OnCardSelected);
        }
        LoadComparedData();
    }

    /// <summary>
    /// Load the previously matched cards from save data.
    /// </summary>
    private void LoadComparedData()
    {
        for (int i = 0; i < saveData.matchedPositions.Count; i++)
        {
            cards.Find(x => x.gridPosition == saveData.matchedPositions[i]).CardMatched();
        }
    }

    /// <summary>
    /// Handle the card selection event.
    /// </summary>
    public void OnCardSelected(Card card)
    {
        if (SelectedCards.Count > 0 )
        {
            if (card != SelectedCards.Peek())
            {
                SelectedCards.Enqueue(card);
            }
        }else
        {
            SelectedCards.Enqueue(card);
        }
        CompareCards();
    }

    /// <summary>
    /// Compare the selected cards.
    /// </summary>
    public void CompareCards()
    {
        if (SelectedCards.Count > 0 && SelectedCards.Count % 2 == 0)
        {
            Card card1 = SelectedCards.Dequeue();
            Card card2 = SelectedCards.Dequeue();
            card1.CompareCard(card2);
            if (card2.CompareCard(card1))
            {
                saveData.matchedPositions.Add(card1.gridPosition);
                saveData.matchedPositions.Add(card2.gridPosition);
                combo++;
                OnCardMatching(combo);
                AudioHandler.Instance.PlayOneShot(2);
                Debug.Log("Cards are same");
            }else
            {
                combo = 0;
                AudioHandler.Instance.PlayOneShot(3);
                Debug.Log("Cards are not same");
            }
            CompareCards();
        }
    }

    /// <summary>
    /// Reset the card comparision handler.
    /// </summary>
    public void Reset()
    {
        SelectedCards.Clear();
    }
}
