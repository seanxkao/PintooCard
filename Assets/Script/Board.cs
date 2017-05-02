using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Board : MonoBehaviour {
	[SerializeField]	protected Mode mode;
	[SerializeField]	protected int rule;
	[SerializeField]	protected MapInfo mapInfo;
	[SerializeField]	protected Deck deckP;
	[SerializeField]	protected Messagebox messagebox;
	[SerializeField]	protected Text titleText;
	[SerializeField]	protected Text ruleText;
	[SerializeField]	protected Button finishButton;

	protected GridLayoutGroup grid;
	protected Deck[,] deck;
	public static bool win;

	protected virtual void Start () {
        //initing

		mode = Manager.manager ().getMode();
        mapInfo = Manager.manager().getMapInfo();
		setRule(Manager.manager ().getRule());
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
					deck [i, j].setDeckType (mapInfo.type [i].cell [j]);
			}
		}
		GetComponent<RectTransform> ().sizeDelta = new Vector2 (mapInfo.row * (grid.cellSize.x + grid.spacing.x), mapInfo.col * (grid.cellSize.y + grid.spacing.y));
		switch (mode) {
		case Mode.PUZZLE:
			titleText.text = "拼圖模式";
			break;
		case Mode.FREE:
			titleText.text = "自由模式";
			finishButton.gameObject.SetActive (true);
			break;
		}
	}

	protected virtual void Update()
	{
		if (win) {
			return;
		}
		if (Manager.manager ().getMode () == Mode.PUZZLE) {
			checkBoard ();
			checkWin ();
		} else {
			checkBoardFree ();
		}
	}
	public virtual void checkRule(){
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
	
	}

	public virtual void checkBoard(){
		win = true;
		//check if puzzle is finished.
		//if not, turn all decks that violate the rule into red.
		for (int i = 0; i < mapInfo.row; i++) {
			for (int j = 0; j < mapInfo.col; j++) {
				if (deck [i, j] != null && deck[i, j].getEnable()) {
					deck [i, j].setWrong (false);
					Card card = deck [i, j].card ();
					DeckType type = deck [i, j].getDeckType ();
					if (type == DeckType.NORMAL && card == null) {
						win = false;
					}
					else if (type == DeckType.TEMP && card != null) {
						win = false;
					}
				}
			}
		}
		checkRule ();
	}

	public virtual void checkBoardFree(){
		for (int i = 0; i < mapInfo.row; i++) {
			for (int j = 0; j < mapInfo.col; j++) {
				if (deck [i, j] != null && deck[i, j].getEnable()) {
					deck [i, j].setWrong (false);
				}
			}
		}
		checkRule ();
	}

	public virtual void checkWin(){
		//if play has won
		if (win) {
			for (int i = 0; i < mapInfo.row; i++) {
				for (int j = 0; j < mapInfo.col; j++) {
					if (deck [i, j].isMatchable()) {
						deck [i, j].card ().enabled = false;
					}
				}
			}
			string s = "成功!\n完成時間\n" + Timer.text;
			messagebox.showMessage (s);
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



	public virtual void finish(){
		win = true;
		checkBoardFree ();
		string s;
		if (!win) {
			s = "有卡牌違反規則!\n";
			messagebox.showMessage (s);
			return;
		}

		for (int i = 0; i < mapInfo.row; i++) {
			for (int j = 0; j < mapInfo.col; j++) {
				if (deck [i, j].isMatchable()) {
					deck [i, j].card ().enabled = false;
				}
			}
		}
		s = "結束!\n完成時間\n" + Timer.text;
		messagebox.showMessage (s);
	}

	//this may be used one day...
	/*
	public Vector3 getRealSize(){
		return RectTransformUtility.CalculateRelativeRectTransformBounds(GetComponent<RectTransform>().root, GetComponent<RectTransform>()).size;
	}
	*/
}
