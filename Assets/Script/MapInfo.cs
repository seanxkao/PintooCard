using UnityEngine;
using System.Collections;

[System.Serializable]
public class Row{
	public DeckType[] cell;
}

[System.Serializable]
public class MapInfo {
	[SerializeField]	protected bool mHasGrid;
	[SerializeField]	protected int mRow;
	[SerializeField]	protected int mCol;
	[SerializeField]	protected Row[] mType;

	public MapInfo(int row, int col, bool hasGrid){
		mHasGrid = hasGrid;
		mRow = row;
		mCol = col;
		mType = new Row[row];
		for (int i = 0; i < row; i++) {
			mType[i] = new Row();
			mType[i].cell = new DeckType[col];
			for (int j = 0; j < col; j++) {
				mType[i].cell[j] = DeckType.NORMAL;
			}
		}
	}

	public bool hasGrid{
		get{ 
			return mHasGrid;
		}
		set{ 
			mHasGrid = value;
		}
	}
	public int row{
		get{ 
			return mRow;
		}
		set{ 
			mRow = value;
		}
	}

	public int col{
		get{ 
			return mCol;
		}
		set{ 
			mCol = value;
		}
	}

	public Row[] type{
		get{ 
			return mType;
		}
		set{ 
			mType = value;
		}
	}

	public int size(){
		int count = 0;
		for(int i=0;i<row;i++){
			for(int j=0;j<col;j++){
				if (mType [i].cell [j] == DeckType.NORMAL) {
					count++;
				}
			}
		}
		return count;
	}
}
