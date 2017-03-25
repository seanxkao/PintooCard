using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
	[SerializeField]	protected Card c;
	[SerializeField]	protected bool wrong;
	protected Color normalColor;
	protected Color wrongColor;
	protected Color noColor;
	[SerializeField]	protected bool enable;

	public virtual void Start(){
		//adjust size with respect to screen size
		normalColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
		wrongColor = new Color(1f, 0f, 0f, 0.5f);
		noColor = new Color (0f, 0f, 0f, 0f);
		GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width/3, Screen.width/2);
		setWrong (false);
	}

	protected virtual void Update(){
		GetComponent<Image> ().color = enable? (wrong ? wrongColor : normalColor):noColor;
	}

	public void setEnable(bool enable){
		this.enable = enable;
	}

	public bool getEnable(){
		return enable;
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
		return enable && c != null;
	}

}
