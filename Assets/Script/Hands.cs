using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public enum HandsState{
	STAY,
	DRAGGING,
	SCROLLING
}

public class Hands : MonoBehaviour {
	[SerializeField]	protected Board board;
	[SerializeField]	protected Deck deckP;
	[SerializeField]	protected CardSuit[] cardSuits;
	[SerializeField]	protected HandsState state;

	protected RectTransform rectTransform;
	protected RectTransform scroll;


	void Start () {
		//initing
		rectTransform = GetComponent<RectTransform> ();
		scroll = (RectTransform)transform.GetChild (0);
		//get map information
		MapInfo mapInfo = Manager.manager().getMapInfo();
		int suitSize = Mathf.CeilToInt(mapInfo.size()/9f);	//a suit has 9 Cards
		cardSuits = new CardSuit[suitSize];
		for (int i = 0; i < suitSize; i++) {
			cardSuits[i] = (CardSuit)(i%4);		//add suit in the order of Club, Diamond, Heart and Spade
		}
		//spawn Decks and Cards
		foreach (CardSuit suit in cardSuits) {
			for (int i = 1; i <= 9; i++) {
				Deck deck = (Deck)Instantiate (deckP, scroll);
				Card card = GetComponent<CardSpawner> ().spawnCard (suit, i);
				card.transform.SetParent (deck.transform);
			}
		}
		//set scroll size
		Vector2 scrollSize = scroll.sizeDelta;
		scrollSize.x = Screen.width / 3 * cardSuits.Length * 9;
		scroll.sizeDelta = scrollSize;
		//set hand size
		Vector2 handSize = rectTransform.sizeDelta;
		handSize.y = Screen.width / 2 + 10;
		rectTransform.sizeDelta = handSize;
	}
}
