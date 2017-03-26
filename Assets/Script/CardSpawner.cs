using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardSpawner : MonoBehaviour {
	[SerializeField] protected Card cardP;
	[SerializeField] protected Sprite[] spadeSprite;
	[SerializeField] protected Sprite[] diamondSprite;
	[SerializeField] protected Sprite[] heartSprite;
	[SerializeField] protected Sprite[] clubSprite;

	public Card spawnCard(CardSuit suit, int rank){
		Card card = (Card)Instantiate(cardP);
		card.Card_init (suit, rank);
		Sprite sprite;
		if (suit == CardSuit.SPADE) {
			sprite = spadeSprite [rank-1];
		} else if (suit == CardSuit.DIAMOND) {
			sprite = diamondSprite [rank-1];
		} else if (suit == CardSuit.HEART) {
			sprite = heartSprite [rank-1];
		} else if (suit == CardSuit.CLUB) {
			sprite = clubSprite [rank-1];
		} else {
			sprite = null;
		}
		card.GetComponent<Image> ().sprite = sprite;
		return card;
	}
}
