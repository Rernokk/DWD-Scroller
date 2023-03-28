using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardScroller : MonoBehaviour
{
    [SerializeField]
    private ScrollRect scrollRect;

    [SerializeField]
    private GameObject cardPrefab;

    [SerializeField]
    private List<CardModel> cardsInHand = new List<CardModel>();

    [SerializeField]
    private float delta;

    private List<Card> currentCards = new List<Card>();
    private float currentOffset = 0;
    private float cardWidth = 100;
    private float baseOffset = 0;

    private void Start()
    {
        RectTransform contentTransform = scrollRect.content;
        baseOffset = scrollRect.content.localPosition.x;
        delta = scrollRect.content.localPosition.x - baseOffset;

        // Sorting hand first by suit then by value descending.
        cardsInHand = cardsInHand.OrderBy(ctx => ctx.CardSuit).ThenBy(ctx => -ctx.CardValue).ToList();

        // If we have less than 7 cards to render, we need to spawn additional cards to fill out the loop.
        while (cardsInHand.Count < 7)
        {
            // Duplicating the cards "in-hand" after sorting will achieve this.
            cardsInHand.AddRange(cardsInHand);
        }

        // Generate the cards that have been configured.
        for (int i = 0; i < cardsInHand.Count; i++)
        {
            // Spawn game object instance and retrieve Card component for reference.
            Card cardInstance = Instantiate(cardPrefab, contentTransform).GetComponent<Card>();

            // Retrieve card artwork from Resources.
            cardInstance.RenderCardFace(cardsInHand[i]);

            // Rename game object for clarity.
            cardInstance.name = $"{cardsInHand[i].CardValue} of {cardsInHand[i].CardSuit}";

            // Retrieve card width for other calculations.
            cardWidth = (cardInstance.transform as RectTransform).rect.width;

            // Position card in the rotation.
            cardInstance.transform.localPosition = new Vector3((cardWidth * .5f) + (cardWidth * i), -80, 0);

            // Add the card to a list in case we need a reference to it for other work.
            currentCards.Add(cardInstance);
        }
    }

    public void OnVectorChanged(Vector2 ctx)
    {
        // Retrieve offset from parent for content object since the ScrollRect behavior is to displace the Content object.
        delta = scrollRect.content.localPosition.x - baseOffset;

        // Tracking currentOffset identifies where we last updated a card position. If we've moved farther than one card width in either direction, we should update again.
        if (Mathf.Abs(delta - currentOffset) > cardWidth)
        {
            // Retrieve direction of scrolling.
            float isPositive = Mathf.Sign(delta - currentOffset);
            if (isPositive > 0)
            {
                // Scrolling to the left, so retrieve the farthest card on the right and reposition it to the "front" of the hand.
                Transform migratedCard = scrollRect.content.transform.GetChild(scrollRect.content.transform.childCount - 1);
                migratedCard.SetAsFirstSibling();
                migratedCard.localPosition = new Vector3(migratedCard.localPosition.x - (cardWidth * currentCards.Count), migratedCard.localPosition.y, migratedCard.localPosition.z);
            }
            else
            {
                // Scrolling to the right, retrieving the farthest card on the left and reposition to the "back" of the hand.
                Transform migratedCard = scrollRect.content.transform.GetChild(0);
                migratedCard.SetAsLastSibling();
                migratedCard.localPosition = new Vector3(migratedCard.localPosition.x + (cardWidth * currentCards.Count), migratedCard.localPosition.y, migratedCard.localPosition.z);
            }
            
            // Update currentOffset for the next trigger.
            currentOffset += isPositive * cardWidth;
        }
    }
}
