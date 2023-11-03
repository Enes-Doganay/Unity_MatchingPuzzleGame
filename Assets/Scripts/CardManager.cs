using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : Singleton<CardManager>
{
    [SerializeField] private CardData cardData;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Transform cardHolderTransform;
    [SerializeField] private int maxCardCount = 20;
    private List<Card> shuffledCards = new List<Card>();

    private bool isCardFlipping = false;
    private Card firstCard;
    private Card secondCard;

    private void Start()
    {
        PrepeareCards();
    }
    public void OnCardClicked(Card card)
    {
        if (isCardFlipping)
            return;

        if (card != null && !card.IsMatched)
        {
            FlipCard(card);

            if (firstCard == null)
            {
                firstCard = card;
            }
            else
            {
                secondCard = card;
                CheckMatch();
            }
        }
    }
    private void PrepeareCards()
    {
        ShuffleCards(SelectSprites());
        PlaceCards();
    }
    private int CalculateCardCount()
    {
        int level = PlayerPrefs.GetInt("Level");
        int cardsInLevel;

        if (level % 2 == 0)
        {
            level -= 1;
        }

        cardsInLevel = (level + 3) * 2;

        if (cardsInLevel > 12)
        {
            cardsInLevel = (level - 2) * 5;
            if (cardsInLevel > maxCardCount)
                cardsInLevel = maxCardCount;

            cardHolderTransform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(200, 200);
        }
        return cardsInLevel;
    }
    private List<Sprite> SelectSprites()
    {
        List<Sprite> selectedSprites = new List<Sprite>();
        int totalSprites = cardData.cardSprites.Length;
        int spritesToSelect = Mathf.Min(CalculateCardCount(), totalSprites);

        while (selectedSprites.Count < spritesToSelect)
        {
            int randomIndex = Random.Range(0, totalSprites);
            Sprite selectedSprite = cardData.cardSprites[randomIndex];

            if (!selectedSprites.Contains(selectedSprite))
            {
                selectedSprites.Add(selectedSprite);
            }
        }
        return selectedSprites;
    }

    private void ShuffleCards(List<Sprite> selectedSprites)
    {
        shuffledCards.Clear();
        for (int i = 0; i < selectedSprites.Count; i++)
        {
            Card card = Instantiate(cardPrefab, cardHolderTransform);
            card.SetCardFront(selectedSprites[i], 2f);
            shuffledCards.Add(card);
            //We also add the pairs of matching cards
            Card matchedCard = Instantiate(cardPrefab, cardHolderTransform);
            matchedCard.SetCardFront(selectedSprites[i], 2f);
            shuffledCards.Add(matchedCard);
        }

        // Fisher-Yates shuffle algorithm
        int n = shuffledCards.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            Card temp = shuffledCards[k];
            shuffledCards[k] = shuffledCards[n];
            shuffledCards[n] = temp;
        }
    }

    private void PlaceCards()
    {
        for (int i = 0; i < shuffledCards.Count; i++)
        {
            shuffledCards[i].transform.SetSiblingIndex(i);
        }
    }
    private void FlipCard(Card card)
    {
        card.FlipCard();
    }

    private void CheckMatch()
    {
        if (firstCard.cardFront == secondCard.cardFront) // If there is a match, leave the cards open
        {
            //particle
            firstCard.IsMatched = true;
            secondCard.IsMatched = true;
            firstCard = null;
            secondCard = null;
        }
        else
        {
            isCardFlipping = true;
            Invoke("ResetCards", 1f);
        }
        if (IsAllCardsMatched())
        {
            UIManager.Instance.SetVictoryPanel();
        }
    }
    private void ResetCards()
    {
        firstCard.FlipCard();
        secondCard.FlipCard();
        firstCard = null;
        secondCard = null;
        isCardFlipping = false;
    }

    private bool IsAllCardsMatched()
    {
        foreach (Card card in shuffledCards)
        {
            if (!card.IsMatched)
            {
                return false;
            }
        }
        return true;
    }
}
