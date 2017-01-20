using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Deck : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	protected Card c;
	[SerializeField]
	protected bool wrong;

	public virtual void Start(){
		setWrong (false);
		GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width/9-10, Screen.width/6-10);

	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnPointerEnter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("OnPointerexit");
    }
    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log(eventData.pointerDrag.name + " was dropped on " + this.name);
        //Debug.Log(this.transform.childCount);
        
		/*
		if (this.transform.childCount == 0) {
			c = eventData.pointerDrag.GetComponent<Card> ();
			if (c != null) {
				c.originalParent = this.transform;

				//if(transform.parent.GetComponent<Board>()!=null)
				//	transform.parent.GetComponent<Board> ().check ();
			}
		}
		*/
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
