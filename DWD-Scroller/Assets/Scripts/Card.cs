using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class Card : MonoBehaviour
{
    private Image imageRenderer;
    private CardModel cardData;

    public Suit CardSuit
    {
        get
        {
            return cardData.CardSuit;
        }
    }

    public int CardValue
    {
        get
        {
            return cardData.CardValue;
        }
    }

    private void Start()
    {
        imageRenderer = GetComponent<Image>();
    }

    public void RenderCardFace(CardModel cardModel)
    {
        if (imageRenderer == null)
        {
            imageRenderer = GetComponent<Image>();
        }
        cardData = cardModel;
        imageRenderer.sprite = Resources.Load<Sprite>($"Cards\\{cardData.CardValue}_of_{cardData.CardSuit.ToString().ToLower()}");
    }
}
