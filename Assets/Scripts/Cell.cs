using UnityEngine;
public class Cell : MonoBehaviour
{
    //TODO - Remove the variable mOutlineImage since we're using buttons instead
	//public Image mOutlineImage;

	//[HideInInspector]
	public Vector2Int mBoardPosition = Vector2Int.zero;

	//[HideInInspector]
	public Board mBoard = null;
    //[HideInInspector]
    public RectTransform mRectTransform = null;

    public BasePiece mCurrentPiece = null;



    // Start is called before the first frame update
    public void Setup(Vector2Int newBoardPosition, Board newBoard)
    {
		mBoardPosition = newBoardPosition;
		mBoard = newBoard;
		mRectTransform = GetComponent<RectTransform>();
    }

    public bool IsEmpty()
    {
        if (mCurrentPiece == null)
            return true;
        else
            return false;
    }

    public Vector2Int GetCellPosition()
    {
        return mBoardPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
