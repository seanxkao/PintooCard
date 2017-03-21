using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MapController : MonoBehaviour {
	static MapController m;

	[SerializeField] protected RectTransform board;

	protected RectTransform rectTransform;

	protected Vector2 realSize;
	protected Vector2 frameRealSize;
	protected Vector2 mapSize;

	protected Vector2 startMapPos;			//position of map when dragging start
	protected Vector2[] startTouchPos;		//position of touches when dragging start
	protected Vector2 currentMapPos;		//position of map in the current dragging
	protected Vector2[] currentTouchPos;	//position of touches in the current dragging

	protected float startScale;
	protected bool isDragging;
	protected float scale;
	protected float scaleMax = 2f;
	protected float scaleMin = 0.5f;

	public static MapController main(){
		return m;
	}

	void Start () {
		m = this;

		rectTransform = GetComponent<RectTransform> ();
		rectTransform.sizeDelta = new Vector2 (Screen.width-20, Screen.height/2);
		startTouchPos = new Vector2[2];
		currentTouchPos = new Vector2[2];
		//frameRealSize = Camera.main.ScreenToWorldPoint(board.GetComponent<Board>().getRealSize());
		frameRealSize = board.sizeDelta;
		//realSize = Camera.main.ScreenToWorldPoint(rectTransform.sizeDelta);
		realSize = rectTransform.sizeDelta;
		currentMapPos = Vector2.zero;
		scale = 1f;
		isDragging = false;

	}

	public float getScale(){
		return scale;
	}

	public bool contains(Vector2 pos){
		return RectTransformUtility.RectangleContainsScreenPoint (rectTransform, pos);
	}

	// Update is called once per frame
	void Update () {


		if (Input.touchCount <= 2) {
			for(int i=0;i<Input.touchCount;i++){
				currentTouchPos [i] = Camera.main.ScreenToWorldPoint(Input.GetTouch (i).position);
				if (Input.GetTouch (i).phase == TouchPhase.Began) {
					startTouchPos [i] = currentTouchPos [i];
					startMapPos = Camera.main.ScreenToWorldPoint(board.position);

					startScale = scale;

					PointerEventData eventdata = new PointerEventData (EventSystem.current);
					eventdata.position = Input.GetTouch (i).position;
					List<RaycastResult> results = new List<RaycastResult>();
					EventSystem.current.RaycastAll (eventdata, results);
					foreach (RaycastResult result in results) {
						if(result.gameObject == gameObject){
							isDragging = true;
						}
						else if(result.gameObject.GetComponent<Card>()){
							if(result.gameObject.GetComponent<Card>().enabled)
								break;
						}
					}

				}
			}
		}
		if (Input.touchCount == 0) {
			isDragging = false;
		}
		if (isDragging) {
			if (Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Moved) {
				//single finger moving
				//moving around

				//check deltaTime is not 0
				board.localScale = new Vector2 (scale, scale);
				if (!float.Equals (Input.GetTouch (0).deltaTime, 0f)) {
					//move speed 
					float moveSpeed = Mathf.Clamp(scale, 1f, Mathf.Infinity);
					currentMapPos += Input.GetTouch (0).deltaPosition * Time.deltaTime / Input.GetTouch (0).deltaTime * moveSpeed;

					//Vector2 moveBound = new Vector2 (Mathf.Abs(realSize.x), Mathf.Abs(realSize.y));
					Vector2 moveBound = (board.sizeDelta *scale-rectTransform.sizeDelta)/2;
					moveBound.x = Mathf.Abs (moveBound.x);
					moveBound.y = Mathf.Abs (moveBound.y);
					currentMapPos.x = Mathf.Clamp (currentMapPos.x, -moveBound.x, moveBound.x);
					currentMapPos.y = Mathf.Clamp (currentMapPos.y, -moveBound.y, moveBound.y);

					board.anchoredPosition = currentMapPos;
				}


			} else if (Input.touchCount == 2 && (Input.GetTouch (0).phase == TouchPhase.Moved || Input.GetTouch (1).phase == TouchPhase.Moved)) {
				//double touch
				//zooming

				scale = startScale + (Vector2.Distance (currentTouchPos [0], currentTouchPos [1]) - Vector2.Distance (startTouchPos [0], startTouchPos [1])) * 2f;
				scale = Mathf.Clamp (scale, scaleMin, scaleMax);

				board.localScale = new Vector2 (scale, scale);


				//move speed 
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

				//let the map stays inside the screen
				//Vector2 moveBound = new Vector2 (Mathf.Abs(frameSize.x * (scale - 1f)), Mathf.Abs(frameSize.y * (scale - 1f)));

				Vector2 moveBound = new Vector2 (Mathf.Abs(realSize.x), Mathf.Abs(realSize.y));

				currentMapPos.x = Mathf.Clamp (currentMapPos.x, -moveBound.x, moveBound.x);
				currentMapPos.y = Mathf.Clamp (currentMapPos.y, -moveBound.y, moveBound.y);
				board.localPosition = currentMapPos;
			}
		}
	

	}
	protected Vector2 screenToWorldVector(Vector2 delta){
		return (Vector2)(Camera.main.ScreenToWorldPoint(new Vector2(delta.x, delta.y)) - Camera.main.ScreenToWorldPoint(Vector2.zero));
	}
}
