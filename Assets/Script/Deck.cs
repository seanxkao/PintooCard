using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
	[SerializeField]
	protected Card c;
	[SerializeField]
	protected bool wrong;

	public virtual void Start(){
		//adjust size with respect to screen size
		GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width/9-10, Screen.width/6-10);
		setWrong (false);
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
		GetComponent<Image> ().color = wrong ? Color.red : Color.gray;
	}

}
