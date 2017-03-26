using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MapController : MonoBehaviour {
	//the singleton instance of MapController
	static MapController m;
	//this section is about the transforming of the map
	[SerializeField] protected RectTransform board;
	protected RectTransform rectTransform;
	protected Vector2 startMapPos;			//position of map when dragging start
	protected Vector2[] startTouchPos;		//position of touches when dragging start
	protected Vector2 currentMapPos;		//position of map in the current dragging
	protected Vector2[] currentTouchPos;	//position of touches in the current dragging
	//this section is about the scaling of the map
	protected float scaleMax = 1f;			
	protected float scaleMin = 0.1f;
	protected float startScale;				//scale when dragging start
	protected float scale;
	protected bool isDragging;

	void Start () {
		//check if there is already an instance
		if (m != null) {
			Destroy (gameObject);
			return;
		}
		m = this;
		//initialize
		rectTransform = GetComponent<RectTransform> ();
		rectTransform.sizeDelta = new Vector2 (Screen.width-20, Screen.height/2);
		startTouchPos = new Vector2[2];
		currentTouchPos = new Vector2[2];
		currentMapPos = Vector2.zero;
		scale = 0.5f;
		isDragging = false;
		//initing map scale
		board.localScale = new Vector2 (scale, scale);
	}

	void Update () {
		//dragging
		if (Input.touchCount == 0) {
			isDragging = false;
		}
		else if (Input.touchCount <= 2) {
			for(int i=0;i<Input.touchCount;i++){
				currentTouchPos [i] = Camera.main.ScreenToWorldPoint(Input.GetTouch (i).position);
				if (Input.GetTouch (i).phase == TouchPhase.Began) {
					//start dragging
					startTouchPos [i] = currentTouchPos [i];
					startMapPos = Camera.main.ScreenToWorldPoint(board.position);
					startScale = scale;
					//use raycast to check whether player is dragging a Card
					PointerEventData eventdata = new PointerEventData (EventSystem.current);
					eventdata.position = Input.GetTouch (i).position;
					List<RaycastResult> results = new List<RaycastResult>();
					EventSystem.current.RaycastAll (eventdata, results);
					//if player is touching a Card with one finger, do not move the map
					//if player is touching with two fingers, move and zoom the map
					foreach (RaycastResult result in results) {
						if(result.gameObject == gameObject){
							isDragging = true;
						}
						else if(result.gameObject.GetComponent<Card>()){
							if(Input.touchCount<2 && result.gameObject.GetComponent<Card>().enabled)
								break;
						}
					}
				}
			}//end for
		}

		//handle dragging
		if (isDragging) {


			//single touch: moving around
			if (Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Moved) {
				board.localScale = new Vector2 (scale, scale);
				//check if deltaTime is not 0
				if (!float.Equals (Input.GetTouch (0).deltaTime, 0f)) {
					//calculate moving distance
					float moveSpeed = scale < 1f ? 1f : scale;
					currentMapPos += Input.GetTouch (0).deltaPosition * Time.deltaTime / Input.GetTouch (0).deltaTime * moveSpeed;
					//let the map stay in the screen
					Vector2 moveBound = (board.sizeDelta*scale - rectTransform.sizeDelta)/2;
					moveBound.x = Mathf.Abs (moveBound.x);
					moveBound.y = Mathf.Abs (moveBound.y);
					currentMapPos.x = Mathf.Clamp (currentMapPos.x, -moveBound.x, moveBound.x);
					currentMapPos.y = Mathf.Clamp (currentMapPos.y, -moveBound.y, moveBound.y);
					board.anchoredPosition = currentMapPos;
				}
			}
			//double touch: zooming 
			else if (Input.touchCount == 2 && (Input.GetTouch (0).phase == TouchPhase.Moved || Input.GetTouch (1).phase == TouchPhase.Moved)) {
				//calculate scaling rate
				scale = startScale + (Vector2.Distance (currentTouchPos [0], currentTouchPos [1]) - Vector2.Distance (startTouchPos [0], startTouchPos [1])) * 2f;
				scale = Mathf.Clamp (scale, scaleMin, scaleMax);
				board.localScale = new Vector2 (scale, scale);
				//calculate moving distance
				float moveSpeed = Mathf.Clamp(scale, 1f, Mathf.Infinity);
				Vector2 moveA;
				Vector2 moveB;
				if (!float.Equals (Input.GetTouch (0).deltaTime, 0f)) {
					moveA = Input.GetTouch (0).deltaPosition * Time.deltaTime / Input.GetTouch (0).deltaTime * moveSpeed;
				} else {
					moveA = Vector2.zero;
				}
				if (!float.Equals (Input.GetTouch (1).deltaTime, 0f)) {
					moveB = Input.GetTouch (1).deltaPosition * Time.deltaTime / Input.GetTouch (1).deltaTime * moveSpeed;
				} else {
					moveB = Vector2.zero;
				}
				currentMapPos += (moveA + moveB) / 2;
				//let the map stay in the screen
				Vector2 moveBound = (board.sizeDelta *scale-rectTransform.sizeDelta)/2;
				moveBound.x = Mathf.Abs (moveBound.x);
				moveBound.y = Mathf.Abs (moveBound.y);
				currentMapPos.x = Mathf.Clamp (currentMapPos.x, -moveBound.x, moveBound.x);
				currentMapPos.y = Mathf.Clamp (currentMapPos.y, -moveBound.y, moveBound.y);
				board.anchoredPosition = currentMapPos;
			}
		}
	}

	//accessing the instance
	public static MapController main(){
		return m;
	}

	public float getScale(){
		return scale;
	}

	//check whether a point is inside the map
	public bool contains(Vector2 pos){
		return RectTransformUtility.RectangleContainsScreenPoint (rectTransform, pos);
	}

	//this may be used one day...
	/*
	protected Vector2 screenToWorldVector(Vector2 delta){
		return (Vector2)(Camera.main.ScreenToWorldPoint(new Vector2(delta.x, delta.y)) - Camera.main.ScreenToWorldPoint(Vector2.zero));
	}
	*/
}
