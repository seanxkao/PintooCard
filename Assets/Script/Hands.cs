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
	[SerializeField]
	protected Card[] cards;
	protected Vector2 touchPos;
	protected int threshold;
	[SerializeField]
	protected HandsState state;

	void Start () {
		threshold = 30;
		cards = GetComponentsInChildren<Card> ();
		Array.Sort (cards, (Card a, Card b) => a.transform.position.x.CompareTo(b.transform.position.x));
		Vector2 deckSize = transform.GetChild (0).GetComponent<RectTransform> ().sizeDelta;
		deckSize.Set(Screen.width, deckSize.y);
		transform.GetChild (0).GetComponent<RectTransform> ().sizeDelta = deckSize;

		Vector2 handSize = GetComponent<RectTransform> ().sizeDelta;
		handSize.Set(handSize.x, (Screen.width / 6 + 10));
		GetComponent<RectTransform> ().sizeDelta = handSize;
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
