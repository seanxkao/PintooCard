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

public class Hands : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler  {
	[SerializeField]	protected Board board;
	[SerializeField]	protected Deck deckP;
	[SerializeField]	protected CardSuit[] cardSuits;
	[SerializeField]	protected HandsState state;
	//[SerializaField]	protected RectTransform scrollbar;

	protected RectTransform rectTransform;
	protected Vector2 touchPos;
	protected int threshold;
	protected RectTransform scroll;


	void Start () {
		threshold = 30;
		rectTransform = GetComponent<RectTransform> ();
		scroll = (RectTransform)transform.GetChild (0);
		//spawn decks and cards
		MapInfo mapInfo = Manager.manager().getMapInfo();
		int suitSize = Mathf.CeilToInt(mapInfo.size()/9f);
		cardSuits = new CardSuit[suitSize];
		for (int i = 0; i < suitSize; i++) {
			cardSuits[i] = (CardSuit)(i%4);
		}
		foreach (CardSuit suit in cardSuits) {
			for (int i = 1; i <= 9; i++) {
				Deck deck = (Deck)Instantiate (deckP, scroll);

				Card card = GetComponent<CardSpawner> ().spawnCard (suit, i);
				card.transform.SetParent (deck.transform);
			}
		}

		Vector2 deckSize = scroll.sizeDelta;
		deckSize.Set(Screen.width / 3 * cardSuits.Length * 9, deckSize.y);
		scroll.sizeDelta = deckSize;

		Vector2 handSize = rectTransform.sizeDelta;
		handSize.Set(handSize.x, (Screen.width / 2 + 10));
		rectTransform.sizeDelta = handSize;

		//Vector2 scrollbarPos = scrollbar.localPosition;

	}

	public virtual HandsState getState(){
		return state;
	}


	public void OnBeginDrag(PointerEventData eventData)
	{
		touchPos = eventData.position;

	}

	public void OnDrag(PointerEventData eventData)
	{
		switch (state) {
		case HandsState.STAY:
			Vector2 move = touchPos - eventData.position;
			if (move.magnitude > threshold) {
				if (Mathf.Abs (Vector2.Dot (move, Vector2.right)) > 0.71*move.magnitude) {
					//horizontal
					state = HandsState.SCROLLING;

				} else {
					//horizontal
					GetComponent<ScrollRect> ().horizontal = false;
					state = HandsState.DRAGGING;
				}
			}
			break;
		case HandsState.DRAGGING:
			break;
		case HandsState.SCROLLING:
			break;
		default:
			break;
		}

	}

	public void OnEndDrag(PointerEventData eventData)
	{
		GetComponent<ScrollRect> ().horizontal = true;
		state = HandsState.STAY;
	}

}
