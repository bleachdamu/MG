using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : GridElement, IClickable , ICardComparer
{
    [SerializeField]
    private SpriteRenderer cardFrontRenderer;
    [SerializeField]
    private Vector3 defaultRotation;
    [SerializeField]
    private AnimationCurve rotationCurve;
    [SerializeField]
    private float flipDuration = 0.5f;
    private bool canFlip = true;
    private bool frontFace = true;
    private Coroutine flipCoroutine = null;

    //Card flipped bool will indicate wether its front face or not
    public Action<Card> CardFrontFacing;

    private int cardId;

    public int cardID
    {
        get { return cardId; }
    }

    /// <summary>
    /// Set the pokemon data.
    /// </summary>
    public void SetPokemonData(PokemonData pokemonData)
    {
        canFlip = true;
        cardId = pokemonData.pokemonId;
        cardFrontRenderer.sprite = pokemonData.pokemonSprite;
    }

    /// <summary>
    /// Handle the click event. 
    /// </summary>
    public void OnClick()
    {
        FlipCard();
    }

    /// <summary>
    /// Handle the pointer enter event.
    /// </summary>
    public void OnPointerEnter()
    {
    }

    /// <summary>
    /// Handle the pointer exit event.
    /// </summary>
    public void OnPointerExit()
    {
    }

    /// <summary>
    /// Flip the card.
    /// </summary>
    private void FlipCard()
    {
        if (flipCoroutine == null && canFlip)
        {
            flipCoroutine = StartCoroutine(Flip(!frontFace, flipDuration));
        }
    }

    /// <summary>
    /// Coroutine to flip the card.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Flip(bool frontFace, float totalDuration)
    {
        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.Euler(0, frontFace ? 0 : 180f, 0);
        float timeTaken = 0f;

        while (timeTaken < totalDuration)
        {
            transform.rotation = Quaternion.Slerp(startRot, endRot, rotationCurve.Evaluate(timeTaken / totalDuration));
            timeTaken += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRot;
        this.frontFace = frontFace;
        flipCoroutine = null;
        if (frontFace == false)
        {
            CardFrontFacing?.Invoke(this);
        }
    }

    /// <summary>
    /// Compare the card with the given card ID.
    /// </summary>
    /// <returns></returns>
    public bool CompareCard(int cardID)
    {
        bool isSame = this.cardId == cardID;
        if(isSame == false)
        {
            FlipCard();
        }
        else
        {
            canFlip = false;
        }
        return isSame;
    }
}
