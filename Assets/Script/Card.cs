using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;


public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	public Transform originalParent;
	private Vector2 originalPosition;
	[SerializeField]
	private int rank; //1~13
	[SerializeField]
    private string color; //spade, heart, diamond, club
    private int up, right, down, left;
	protected bool freeze;

    private class edgeblock
	{
        //(up, right, down, left) = index(0, 1, 2, 3)
        public string[] color = new string[4]; // red, yellow, green, blue
        public string[] pos = new string[4]; // right, left for(0, 2) ; up, down for(1, 3)
    }
    private edgeblock edge;

    //public Card(int rank)
	protected virtual void Start()
	{
        //type 1 red, yellow, green, blue
        //type 2 green, blue, red, yellow

		freeze = false;
		//auto scale with screen size
		GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width/9-20, Screen.width/6-20);


		edge = new edgeblock ();

		this.edge.color[0] = "red";
		this.edge.color[1] = "yellow";
		this.edge.color[2] = "green";
		this.edge.color[3] = "blue";
		/*
        //init color
        switch (rank)
		{


            case 1: case 3: case 4: case 5: case 8:
                this.edge.color[0] = "red";
                this.edge.color[1] = "yellow";
                this.edge.color[2] = "green";
                this.edge.color[3] = "blue";
                break;
            case 2: case 6: case 7: case 9:
                this.edge.color[0] = "green";
                this.edge.color[1] = "blue";
                this.edge.color[2] = "red";
                this.edge.color[3] = "yellow";
                break;
            default:
                break;
		}
		*/

		//for debug......
		switch (rank)
		{
		case 1: 
			this.edge.pos[0] = "R";
			this.edge.pos[1] = "L";
			this.edge.pos[2] = "L";
			this.edge.pos[3] = "R";
			break;
		case 2: 
			this.edge.pos[0] = "L";
			this.edge.pos[1] = "L";
			this.edge.pos[2] = "R";
			this.edge.pos[3] = "L";
			break;
		case 3: 
			this.edge.pos[0] = "R";
			this.edge.pos[1] = "R";
			this.edge.pos[2] = "L";
			this.edge.pos[3] = "L";
			break;
		case 4: 
			this.edge.pos[0] = "L";
			this.edge.pos[1] = "R";
			this.edge.pos[2] = "L";
			this.edge.pos[3] = "L";
			break;
		case 5:
			this.edge.pos[0] = "L";
			this.edge.pos[1] = "L";
			this.edge.pos[2] = "L";
			this.edge.pos[3] = "L";
			break;
		case 6: 
			this.edge.pos[0] = "L";
			this.edge.pos[1] = "L";
			this.edge.pos[2] = "L";
			this.edge.pos[3] = "R";
			break;
		case 7: 
			this.edge.pos[0] = "L";
			this.edge.pos[1] = "L";
			this.edge.pos[2] = "R";
			this.edge.pos[3] = "R";
			break;
		case 8: 
			this.edge.pos[0] = "R";
			this.edge.pos[1] = "L";
			this.edge.pos[2] = "L";
			this.edge.pos[3] = "L";
			break;
		case 9: 
			this.edge.pos[0] = "L";
			this.edge.pos[1] = "R";
			this.edge.pos[2] = "R";
			this.edge.pos[3] = "L";
			break;
		default:

			break;

		}
        //init pos

		/*
        switch (rank)
        {
            case 1: case 9:
                this.edge.pos[0] = "right";
                this.edge.pos[1] = "up";
                this.edge.pos[2] = "right";
                this.edge.pos[3] = "up";
                break;
            case 2: case 8:
                this.edge.pos[0] = "right";
                this.edge.pos[1] = "up";
                this.edge.pos[2] = "right";
                this.edge.pos[3] = "down";
                break;
            case 3: case 7:
                this.edge.pos[0] = "right";
                this.edge.pos[1] = "down";
                this.edge.pos[2] = "right";
                this.edge.pos[3] = "down";
                break;
            case 4: case 6:
                this.edge.pos[0] = "left";
                this.edge.pos[1] = "down";
                this.edge.pos[2] = "right";
                this.edge.pos[3] = "down";
                break;
            case 5:
                this.edge.pos[0] = "left";
                this.edge.pos[1] = "up";
                this.edge.pos[2] = "right";
                this.edge.pos[3] = "down";
                break;
            default:

                break;

        }
        */
    }

	
	// Update is called once per frame
	void Update () {
	
	}


	//for debug
	public static int match(Card a, int i1, Card b, int i2){
		//(up, right, down, left) = index(0, 1, 2, 3)
		if(a.edge.color[i1] == b.edge.color[i2]){
			if(a.edge.pos[i1] != b.edge.pos[i2]){
				//same color same side
				return 0;
			}
			else{
				//same color diff side
				return 1;
			}
		}
		else{
			if(a.edge.pos[i1] != b.edge.pos[i2]){
				//diff color same side
				return 2;
			}
			else{
				//diff color diff side
				return 3;
			}
		}
	}


	public void OnBeginDrag(PointerEventData eventData)
	{
		//Debug.Log("OnBeginDrag");

		//save old deck.
		if (isFreeze())
			return;
		originalParent = this.transform.parent;
		originalPosition = this.transform.position;
		originalParent.GetComponent<Deck> ().setCard (null);
		transform.SetParent(transform.root);
		GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		//Debug.Log("OnDrag");
		if (isFreeze())
			return;
		this.transform.position = eventData.position;

	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (isFreeze())
			return;
		Deck deck = eventData.pointerCurrentRaycast.gameObject.GetComponent<Deck>();
		if (deck != null && deck.card () == null) {
			//deck exist, no other card on deck.
			//go to new deck.
			originalParent = deck.transform;
			deck.setCard (this);
		} else {
			//nothing here. or already another card on deck.
			//go back to old deck.
			originalParent.GetComponent<Deck> ().setCard (this);
		}
			
		//oddbug later i'll fix
		this.transform.SetParent(originalParent);
		transform.localPosition = Vector2.zero;
		//Debug.Log("OnEndDrag");

		GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

	public virtual bool isFreeze(){
		return freeze;
	}
	public virtual void setFreeze(bool freeze){
		this.freeze = freeze;
	}

}
