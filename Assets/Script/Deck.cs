using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum DeckType{
	NORMAL,
	TEMP,
	HIDDEN
}

public enum DeckFrame{
	NORMAL,
	TOPLEFT,
	TOPRIGHT,
	BOTLEFT,
	BOTRIGHT
}

public class Deck : MonoBehaviour
{
	[SerializeField]	protected Card c;
	[SerializeField]	protected bool wrong;
	[SerializeField]	protected DeckType type;
	[SerializeField]	protected Sprite[] frameSprite;
	protected Color normalColor;
	protected Color tempColor;
	protected Color wrongColor;
	protected Color noColor;

	public virtual void Start(){
		//adjust size with respect to screen size
		GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width/3, Screen.width/2);
		normalColor = new Color(1f, 1f, 1f, 1f);
		tempColor = new Color(0.8f, 0.8f, 1f, 1f);
		wrongColor = new Color(1f, 0f, 0f, 1f);
		noColor = new Color (0f, 0f, 0f, 0f);
		setWrong (false);
	}

	protected virtual void Update(){
		//if Deck is a hole, hide the Deck.
		//if Deck is not a hole and the Card is placed wrong, turn red.
		//otherwise, show the normal color
		Color color;
		switch(type){
		case DeckType.NORMAL:
			color = normalColor;
			break;
		case DeckType.TEMP:
			color = tempColor;
			break;
		case DeckType.HIDDEN:
			color = noColor;
			break;
		default:
			color = normalColor;
			break;
		}

		if (wrong && color != noColor)
			color = wrongColor;
		GetComponent<Image> ().color = color;
	}

	public void setDeckType(DeckType type){
		this.type= type;
	}

	public DeckType getDeckType(){
		return type;
	}

	public bool getEnable(){
		return type != DeckType.HIDDEN;
	}

	public void setSize(Vector2 size){
		GetComponent<RectTransform>().sizeDelta = size;
	}

	public void setCard(Card c){
		this.c = c;
	}

	public virtual Card card(){
		return c;
	}

	public virtual bool isWrong(){
		return wrong;
	}

	public virtual void setWrong(bool wrong){
		this.wrong = wrong;
	}

	public virtual bool isMatchable(){
		return getEnable() && c != null;
	}

	public virtual void setDeckFrame(DeckFrame deckFrame){
		GetComponent<Image> ().sprite = frameSprite [(int)deckFrame];
	}

}
