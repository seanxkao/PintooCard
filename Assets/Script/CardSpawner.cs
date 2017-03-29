using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardSpawner : MonoBehaviour {
	
	[System.Serializable]
	public class CardSprite{
		public Sprite[] spadeSprite;
		public Sprite[] diamondSprite;
		public Sprite[] heartSprite;
		public Sprite[] clubSprite;
	}
	[SerializeField] protected Card cardP;
	[SerializeField] protected CardSprite[] Cardskin;

	public Card spawnCard(CardSuit suit, int rank){
		Card card = (Card)Instantiate(cardP);
		card.Card_init (suit, rank);

		int skin = Manager.manager ().getSkin ();
		Sprite sprite;

		switch(skin){
		case 0: //Original
			if (suit == CardSuit.SPADE) {
				sprite = Cardskin[skin].spadeSprite [rank-1];
			} else if (suit == CardSuit.DIAMOND) {
				sprite = Cardskin[skin].diamondSprite [rank-1];
			} else if (suit == CardSuit.HEART) {
				sprite = Cardskin[skin].heartSprite [rank-1];
			} else if (suit == CardSuit.CLUB) {
				sprite = Cardskin[skin].clubSprite[rank-1];
			} else {
				sprite = null;
			}
			break;
		case 1: //V_butterfly
            //no difference between suits
			sprite = Cardskin [skin].spadeSprite [rank - 1];
			break;
        case 2: //Glass
            //no difference between suits
            sprite = Cardskin[skin].spadeSprite[rank - 1];
            break;
        default :
			sprite = null;
			break;
		}
		card.GetComponent<Image> ().sprite = sprite;
		return card;
	}
}
