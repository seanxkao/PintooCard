using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Board : MonoBehaviour {
	[SerializeField]
	protected int row;
	[SerializeField]
	protected int col;
	[SerializeField]
	protected Deck deckP;
	protected Deck[,] deck;
	protected const int deckWidth = 40;
	protected const int deckHeight = 60;
	[SerializeField]
	protected Messagebox messagebox;
	[SerializeField]
	protected Text ruleText;

	[SerializeField]
	protected int rule;
	protected bool win;

	protected virtual void Start () {
		//fixed col
		win = false;
		GetComponent<GridLayoutGroup>().constraintCount = col;
		GetComponent<GridLayoutGroup> ().cellSize = new Vector2 (Screen.width / 9-10, Screen.width / 6 -10);
		GetComponent<RectTransform> ().sizeDelta = new Vector2 (Screen.width / 3+90, Screen.width / 2+90);
		deck= new Deck[row, col];
		for (int i = 0; i < row; i++) {
			for (int j = 0; j < col; j++) {
				deck[i, j] = (Deck)Instantiate (deckP);
				deck [i, j].transform.SetParent (transform, false);
			}
		}
		setRule (Manager.manager().getRule());
	}

	public virtual void setRule(int rule){
		Debug.Log ("set rule");
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

	public virtual void check(){
		if (win)
			return;
		win = true;
		//check if puzzle is finished.
		//if not, turn all decks that violate the rule into red.
		for (int i = 0; i < row; i++) {
			for (int j = 0; j < col; j++) {
				if (deck [i, j] != null) {
					deck [i, j].setWrong (false);
					if (deck [i, j].card () == null) {
						win = false;
					}
				}
			}
		}
		//check horizontal
		for (int i = 0; i < row; i++) {
			for (int j = 0; j < col-1; j++) {
				if (deck [i, j] != null && deck [i, j].card () != null && deck [i, j+1] != null && deck [i, j+1].card () != null) {
					if (Card.match (deck [i, j].card (), 1, deck [i, j+1].card (), 3) != rule) {
						deck [i, j+1].setWrong (true);
						deck [i, j].setWrong (true);
						win = false;
					}
				}
			}
		}
		//check vertical
		for (int i = 0; i < row-1; i++) {
			for (int j = 0; j < col; j++) {				
				if (deck [i, j] != null && deck [i, j].card () != null && deck [i+1, j] != null && deck[i+1, j].card()!=null) {
					if (Card.match (deck [i, j].card (), 2, deck [i+1, j].card (), 0) != rule) {
						deck [i+1, j].setWrong (true);
						deck [i, j].setWrong (true);
						win = false;
					}
				}
			}
		}
		//if win
		if (win) {
			for (int i = 0; i < row; i++) {
				for (int j = 0; j < col; j++) {
					deck [i, j].card ().enabled = false;
				}
			}
			messagebox.showMessage ("成功！");
		}
	}


	protected virtual void Update(){
		check ();
	}
}
