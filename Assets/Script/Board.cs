using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Board : MonoBehaviour {
	[SerializeField]	protected int rule;
	[SerializeField]	protected MapInfo mapInfo;
	[SerializeField]	protected Deck deckP;
	[SerializeField]	protected Messagebox messagebox;
	[SerializeField]	protected Text ruleText;
	protected GridLayoutGroup grid;
	protected Deck[,] deck;
	protected bool win;

	protected virtual void Start () {
		//initing
		mapInfo = Manager.manager().getMapInfo();
		setRule (Manager.manager ().getRule());
		win = false;
		//setup table, spawn Decks
		grid = GetComponent<GridLayoutGroup>();
		grid.constraintCount = mapInfo.col;
		grid.cellSize = new Vector2 (Screen.width / 3, Screen.width / 2);
		deck = new Deck[mapInfo.row, mapInfo.col];
		for (int i = 0; i < mapInfo.row; i++) {
			for (int j = 0; j < mapInfo.col; j++) {
					deck [i, j] = (Deck)Instantiate (deckP);
					deck [i, j].transform.SetParent (transform, false);
					deck [i, j].setEnable (mapInfo.enable [i].cell [j]);
			}
		}
		GetComponent<RectTransform> ().sizeDelta = new Vector2 (mapInfo.row * (grid.cellSize.x + grid.spacing.x), mapInfo.col * (grid.cellSize.y + grid.spacing.y));
	}

	protected virtual void Update(){
		check ();
	}

	public virtual void check(){
		if (win) {
			return;
		}
		win = true;
		//check if puzzle is finished.
		//if not, turn all decks that violate the rule into red.
		for (int i = 0; i < mapInfo.row; i++) {
			for (int j = 0; j < mapInfo.col; j++) {
				if (deck [i, j] != null && deck[i, j].getEnable()) {
					deck [i, j].setWrong (false);
					if (deck [i, j].card () == null) {
						win = false;
					}
				}
			}
		}
		//check horizontal
		for (int i = 0; i < mapInfo.row; i++) {
			for (int j = 0; j < mapInfo.col-1; j++) {
				if (deck [i, j] != null && deck [i, j].isMatchable() && deck [i, j+1] != null && deck [i, j+1].isMatchable()) {
					if (Card.match (deck [i, j].card (), 1, deck [i, j+1].card (), 3) != rule) {
						deck [i, j+1].setWrong (true);
						deck [i, j].setWrong (true);
						win = false;
					}
				}
			}
		}
		//check vertical
		for (int i = 0; i < mapInfo.row-1; i++) {
			for (int j = 0; j < mapInfo.col; j++) {				
				if (deck [i, j] != null && deck [i, j].isMatchable() && deck [i+1, j] != null && deck[i+1, j].isMatchable()) {
					if (Card.match (deck [i, j].card (), 2, deck [i+1, j].card (), 0) != rule) {
						deck [i+1, j].setWrong (true);
						deck [i, j].setWrong (true);
						win = false;
					}
				}
			}
		}
		//if play has won
		if (win) {
			for (int i = 0; i < mapInfo.row; i++) {
				for (int j = 0; j < mapInfo.col; j++) {
					if (deck [i, j].isMatchable()) {
						deck [i, j].card ().enabled = false;
					}
				}
			}
			messagebox.showMessage ("成功！");
		}
	}
		
	public virtual void setRule(int rule){
		this.rule = rule;
		switch (rule) {
		case 0:
			ruleText.text = "規則：同色對齊";
			break;
		case 1:
			ruleText.text = "規則：同色交錯";
			break;
		case 2:
			ruleText.text = "規則：異色對齊";
			break;
		case 3:
			ruleText.text = "規則：異色交錯";
			break;
		}
	}

	public int getRow(){
		return mapInfo.row;
	}

	public int getCol(){
		return mapInfo.col;
	}


	//this may be used one day...
	/*
	public Vector3 getRealSize(){
		return RectTransformUtility.CalculateRelativeRectTransformBounds(GetComponent<RectTransform>().root, GetComponent<RectTransform>()).size;
	}
	*/
}
