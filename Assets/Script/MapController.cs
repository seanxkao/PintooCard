using UnityEngine;
using System.Collections;

public class MapController : MonoBehaviour {
	[SerializeField] protected GameObject frame;

	protected Vector2 screenSize;
	protected Vector2 mapSize;

	protected Vector2 startMapPos;
	protected Vector2[] startTouchPos;
	protected Vector2 currentMapPos;

	protected float startScale;

	protected float scale;
	protected float scaleMax = 2f;
	protected float scaleMin = 0.5f;

	void Start () {
		screenSize = Camera.main.ScreenToWorldPoint(new Vector2 (Screen.width, Screen.height));

		currentMapPos = Vector2.zero;
		scale = 1f;

		startTouchPos = new Vector2[2];
	}
	
	// Update is called once per frame
	void Update () {

		Vector2[] currentTouchPos = new Vector2[2];
		if (Input.touchCount <= 2) {
			for(int i=0;i<Input.touchCount;i++){
				currentTouchPos [i] = Camera.main.ScreenToWorldPoint(Input.GetTouch (i).position);
				if (Input.GetTouch (i).phase == TouchPhase.Began) {
					startTouchPos [i] = currentTouchPos [i];
					startMapPos = Camera.main.ScreenToWorldPoint(transform.position);
					startScale = scale;
				}
			}
		}

		if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved) {
			//single finger moving
			//moving around
			if (!float.Equals (Input.GetTouch (0).deltaTime, 0f)) {
				currentMapPos = currentMapPos + screenToWorldVector (Input.GetTouch (0).deltaPosition * Time.deltaTime / Input.GetTouch (0).deltaTime);
			}
			if (scale >= 1f) {
				currentMapPos.x = Mathf.Clamp (currentMapPos.x, -screenSize.x * (scale - 1f), screenSize.x * (scale - 1f));
				currentMapPos.y = Mathf.Clamp (currentMapPos.y, -screenSize.y * (scale - 1f), screenSize.y * (scale - 1f));
			} else {
				currentMapPos.x = Mathf.Clamp (currentMapPos.x, -screenSize.x * (1f - scale), screenSize.x * (1f - scale));
				currentMapPos.y = Mathf.Clamp (currentMapPos.y, -screenSize.y * (1f - scale), screenSize.y * (1f - scale));
			}

			transform.position = Camera.main.WorldToScreenPoint(currentMapPos);

		} else if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved) {
			//double touch
			//zooming

			scale = startScale + (Vector2.Distance(currentTouchPos [0], currentTouchPos [1]) - Vector2.Distance(startTouchPos [0], startTouchPos [1])) * 0.5f;
			scale = Mathf.Clamp (scale, scaleMin, scaleMax);
			transform.localScale = new Vector2(scale, scale);

			Vector2 moveA;
			Vector2 moveB;
			if (!float.Equals (Input.GetTouch (0).deltaTime, 0f)) {
				moveA = screenToWorldVector (Input.GetTouch (0).deltaPosition);
			} else {
				moveA = Vector2.zero;
			}
			if (!float.Equals (Input.GetTouch (1).deltaTime, 0f)) {
				moveB = screenToWorldVector (Input.GetTouch (1).deltaPosition);
			} else {
				moveB =  Vector2.zero;
			}

			currentMapPos = currentMapPos + moveA + moveB;
			if (scale >= 1f) {
				currentMapPos.x = Mathf.Clamp (currentMapPos.x, -screenSize.x * (scale - 1f), screenSize.x * (scale - 1f));
				currentMapPos.y = Mathf.Clamp (currentMapPos.y, -screenSize.y * (scale - 1f), screenSize.y * (scale - 1f));
			} else {
				currentMapPos.x = Mathf.Clamp (currentMapPos.x, -screenSize.x * (1f - scale), screenSize.x * (1f - scale));
				currentMapPos.y = Mathf.Clamp (currentMapPos.y, -screenSize.y * (1f - scale), screenSize.y * (1f - scale));
			}

			transform.position = Camera.main.WorldToScreenPoint(currentMapPos);
			/*
			scale = startScale + (Vector2.Distance(currentTouchPos [0], currentTouchPos [1]) - Vector2.Distance(startTouchPos [0], startTouchPos [1])) * 0.5f;
			scale = Mathf.Clamp (scale, scaleMin, scaleMax);
			transform.localScale = new Vector2(scale, scale);

			if (scale >= 1f) {
				currentMapPos.x = Mathf.Clamp (startMapPos.x + (currentTouchPos [0].x + currentTouchPos [1].x)/2 - (startTouchPos [0].x + startTouchPos [1].x)/2, -screenSize.x * (scale - 1), screenSize.x * (scale - 1));
				currentMapPos.y = Mathf.Clamp (startMapPos.y + (currentTouchPos [0].y + currentTouchPos [1].y)/2 - (startTouchPos [0].y + startTouchPos [1].y)/2, -screenSize.y * (scale - 1), screenSize.y * (scale - 1));
			} else {
				currentMapPos = Vector2.zero;
			}
			transform.position = Camera.main.WorldToScreenPoint(currentMapPos);
			*/
		}
	

	}
	protected Vector2 screenToWorldVector(Vector2 delta){
		
		return (Vector2)(Camera.main.ScreenToWorldPoint(new Vector2(delta.x, delta.y)) - Camera.main.ScreenToWorldPoint(Vector2.zero));
	}
}
