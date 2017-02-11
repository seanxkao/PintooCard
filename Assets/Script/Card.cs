using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public enum EdgeColor { RED, YELLOW, GREEN, BLUE }
public enum EdgePos { LEFT, RIGHT } //{counterclockwise, clockwise}

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private class edgeblock
    {
        //(up, right, down, left) = index(0, 1, 2, 3)
        public EdgeColor[] color = new EdgeColor[4]; // red, yellow, green, blue
        public EdgePos[] pos = new EdgePos[4]; // trend to {counterclockwise, clockwise}
    }

    public Transform originalParent;
    [SerializeField]	private int rank; //1~13
    [SerializeField]	private string color; //spade, heart, diamond, club
    private edgeblock edge;
    //private int up, right, down, left;



    public void Card_init(int rank)
    {
        //type 1 red, yellow, green, blue
        //type 2 green, blue, red, yellow
        this.rank = rank;
        //init color
        this.edge.color[0] = EdgeColor.RED;
        this.edge.color[1] = EdgeColor.YELLOW;
        this.edge.color[2] = EdgeColor.GREEN;
        this.edge.color[3] = EdgeColor.BLUE;
        //init pos
        switch (rank)
        {
            case 1:
                this.edge.pos[0] = EdgePos.RIGHT;
                this.edge.pos[1] = EdgePos.LEFT;
                this.edge.pos[2] = EdgePos.LEFT;
                this.edge.pos[3] = EdgePos.RIGHT;
                break;
            case 2:
                this.edge.pos[0] = EdgePos.LEFT;
                this.edge.pos[1] = EdgePos.LEFT;
                this.edge.pos[2] = EdgePos.RIGHT;
                this.edge.pos[3] = EdgePos.LEFT;
                break;
            case 3:
                this.edge.pos[0] = EdgePos.RIGHT;
                this.edge.pos[1] = EdgePos.RIGHT;
                this.edge.pos[2] = EdgePos.LEFT;
                this.edge.pos[3] = EdgePos.LEFT;
                break;
            case 4:
                this.edge.pos[0] = EdgePos.LEFT;
                this.edge.pos[1] = EdgePos.RIGHT;
                this.edge.pos[2] = EdgePos.LEFT;
                this.edge.pos[3] = EdgePos.LEFT;
                break;
            case 5:
                this.edge.pos[0] = EdgePos.LEFT;
                this.edge.pos[1] = EdgePos.LEFT;
                this.edge.pos[2] = EdgePos.LEFT;
                this.edge.pos[3] = EdgePos.LEFT;
                break;
            case 6:
                this.edge.pos[0] = EdgePos.LEFT;
                this.edge.pos[1] = EdgePos.LEFT;
                this.edge.pos[2] = EdgePos.LEFT;
                this.edge.pos[3] = EdgePos.RIGHT;
                break;
            case 7:
                this.edge.pos[0] = EdgePos.LEFT;
                this.edge.pos[1] = EdgePos.LEFT;
                this.edge.pos[2] = EdgePos.RIGHT;
                this.edge.pos[3] = EdgePos.RIGHT;
                break;
            case 8:
                this.edge.pos[0] = EdgePos.RIGHT;
                this.edge.pos[1] = EdgePos.LEFT;
                this.edge.pos[2] = EdgePos.LEFT;
                this.edge.pos[3] = EdgePos.LEFT;
                break;
            case 9:
                this.edge.pos[0] = EdgePos.LEFT;
                this.edge.pos[1] = EdgePos.RIGHT;
                this.edge.pos[2] = EdgePos.RIGHT;
                this.edge.pos[3] = EdgePos.LEFT;
                break;
            default:
                break;

        }
    }

    protected virtual void Start()
    {
        //auto scale with screen size
        GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width /3 - 5, Screen.width / 2 - 5);
        edge = new edgeblock();
        Card_init(rank);
    }

    //for debug
    public static int match(Card a, int i1, Card b, int i2)
    {
        //(up, right, down, left) = index(0, 1, 2, 3)
        if (a.edge.color[i1] == b.edge.color[i2])
        {
            if (a.edge.pos[i1] != b.edge.pos[i2])
			{
                //same color same side
                return 0;
            }
            else
			{
                //same color diff side
                return 1;
            }
        }
        else
        {
            if (a.edge.pos[i1] != b.edge.pos[i2])
            {
                //diff color same side
                return 2;
            }
            else
			{
                //diff color diff side
                return 3;
            }
        }
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");

        //save old deck.
        originalParent = this.transform.parent;
        originalParent.GetComponent<Deck>().setCard(null);
        transform.SetParent(transform.root);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
		this.transform.position = eventData.position;

		float scale = 1f;
		if(MapController.main().contains(eventData.position)){
			scale = MapController.main ().getScale ();
		}
		this.transform.localScale = Vector2.Lerp(transform.localScale, Vector2.one*scale, 0.5f);
	}

    public void OnEndDrag(PointerEventData eventData)
	{
		Debug.Log("OnEndDrag");
		GameObject deckObject = eventData.pointerCurrentRaycast.gameObject;
		if (deckObject != null) {
			Deck deck = deckObject.GetComponent<Deck> ();
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
		}

        //oddbug later i'll fix
        this.transform.SetParent(originalParent);
        transform.localPosition = Vector2.zero;
		transform.localScale = Vector2.one;

        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }


    public void rotate_half() // 90 degree counterclockwise
    {
        EdgeColor temp_color = this.edge.color[0];
        EdgePos temp_pos = this.edge.pos[0];
        for (int i = 0; i < 3; i++)
        {
            this.edge.color[i] = this.edge.color[i + 1];
            this.edge.pos[i] = this.edge.pos[i + 1];
        }
        this.edge.color[3] = temp_color;
        this.edge.pos[3] = temp_pos;
    }
    public void rotate() // 180 degree counterclockwise
    {
        rotate_half();
        rotate_half();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        rotate();

        this.transform.Rotate(new Vector3(0, 0, 180));
        Debug.Log("rotate");
    }
}
