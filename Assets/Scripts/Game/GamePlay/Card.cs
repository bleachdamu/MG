using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Card : GridElement, IClickable, ICardComparer
{
    [SerializeField]
    private SpriteRenderer cardFrontRenderer;
    [SerializeField]
    private AnimationCurve rotationCurve;
    [SerializeField]
    private float flipDuration = 0.5f;


    private bool canFlip = true;
    private bool frontFace = false;
    private Coroutine flipCoroutine = null;
    private PokemonData pokemonData;


    public Action<Card> CardFrontFacing;

    public PokemonData PokemonData
    {
        get { return pokemonData; }
    }

    public int CardID => pokemonData.pokemonId;

    /// <summary>
    /// Set the pokemon data.
    /// </summary>
    public void SetPokemonData(PokemonData pokemonData,float visibleTime)
    {
        canFlip = true;
        frontFace = true;
        this.pokemonData = pokemonData;
        cardFrontRenderer.sprite = pokemonData.pokemonSprite;

        //This is to make sure card is visible for a certain time.
        //Visible time is 0 when we are recovering data.
        frontFace = visibleTime > 0 ? false : true;
        Debug.Log("Visible time: " + visibleTime);
        if (!frontFace)
        {
            canFlip = false;
            transform.rotation = Quaternion.Euler(0, 180f, 0);
            StartCoroutine(TimerCoroutine(visibleTime, () =>
            {
                canFlip = true;
                FlipCard();
            }));
        }
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
        if (flipCoroutine == null && canFlip && gameObject.activeInHierarchy)
        {
            AudioHandler.Instance.PlayOneShot(1);
            flipCoroutine = StartCoroutine(Flip(!frontFace, flipDuration));
        }
    }

    /// <summary>
    /// Flip the card to fixed face.
    /// </summary>
    private void FlipCard(bool frontFace)
    {
        if (flipCoroutine == null && canFlip && gameObject.activeInHierarchy)
        {
            AudioHandler.Instance.PlayOneShot(1);
            flipCoroutine = StartCoroutine(Flip(frontFace, flipDuration));
        }
    }

    public void CardMatched()
    {
        transform.rotation = Quaternion.Euler(0, 0f, 0);
        //Directly calling ReleaseToPool.
        OnReleaseToPool();
    }

    public void Reset()
    {
        transform.rotation = Quaternion.Euler(0, 0f, 0);
        canFlip = true;
        frontFace = true;
        StopAllCoroutines();
        flipCoroutine = null;
    }

    /// <summary>
    /// Coroutine to handle the timer.
    /// </summary>
    /// <returns></returns>
    private IEnumerator TimerCoroutine(float time,Action OnComplete)
    {
        float timeTaken = 0f;
        while (timeTaken < time)
        {
            timeTaken += Time.deltaTime;
            yield return null;
        }
        OnComplete();
    }

    /// <summary>
    /// Coroutine to flip the card.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Flip(bool frontFace, float totalDuration)
    {
        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.Euler(0, frontFace ? 0f : 180f , 0);
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
        if (endRot.y > 0)
        {
            CardFrontFacing?.Invoke(this);
        }
    }

    /// <summary>
    /// Compare the card with the given card ID.
    /// </summary>
    /// <returns></returns>
    public bool CompareCard(ICardComparer card)
    {
        bool isSame = CardID == card.CardID;
        if(isSame == false)
        {
            FlipCard(true);
        }
        else
        {
            CardMatched();
        }
        return isSame;
    }

    public override void OnGetFromPool()
    {
        base.OnGetFromPool();
    }

    public override void OnReleaseToPool()
    {
        Reset();
        transform.localScale = Vector3.one;
        base.OnReleaseToPool();
    }
}
