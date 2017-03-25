using UnityEngine;
using System.Collections;

[System.Serializable]
public class Row{
	public bool[] cell;
}

[System.Serializable]
public class MapInfo {
	[SerializeField]	protected int mRow;
	[SerializeField]	protected int mCol;
	[SerializeField]	protected Row[] mEnable;

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

	public Row[] enable{
		get{ 
			return mEnable;
		}
		set{ 
			mEnable = value;
		}
	}

	public int size(){
		int count = 0;
		for(int i=0;i<row;i++){
			for(int j=0;j<col;j++){
				if (mEnable [i].cell [j]) {
					count++;
				}
			}
		}
		return count;
	}
}
