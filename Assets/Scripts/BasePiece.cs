using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

//TODO : Check if we need to seperate the BasePieces into PlayerPiece and MirrorPiece

public abstract class BasePiece : EventTrigger
{

    public Color mColor = Color.clear;
    protected Cell mCurrentCell = null;
    protected RectTransform mRectTransform = null;
    protected PieceManager mPieceManager;
    protected string mOrientation = "";
    protected Button[] mButtons;
    protected Vector3Int mMovement = new Vector3Int(1, 1, 0);
    //This variable is for Locking the Piece's movement 
    protected bool mLockMovement = false;
    protected bool mMouseSelected = false;

    GameObject mPieceButtons = null;
    public virtual void Setup(string orientation, Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager, GameObject pieceButtons)
    {


        mPieceButtons = pieceButtons;
        mButtons = mPieceButtons.GetComponentsInChildren<Button>();
        string s = base.name;

        foreach (Button b in mButtons)
            b.onClick.AddListener(() => newPieceManager.MovePiece(this, b.name));

        ClearButtons();

        mPieceManager = newPieceManager;
       
        mColor = newTeamColor;
        GetComponent<Image>().color = newSpriteColor;
        mRectTransform = GetComponent<RectTransform>();
        mOrientation = orientation;

    }

    public bool getLockMovement()
    { return mLockMovement; }

    public void setLockMovement(bool locker)
    { mLockMovement = locker; }

    public Cell getCurrentCell()
    { return mCurrentCell; }

    public void setCurrentCell(Cell newCell)
    { mCurrentCell = newCell; }

    public GameObject getPieceButtons()
    { return mPieceButtons; }

    public void Place(Cell newCell)
    {
        //Place them in the given Cell

        mCurrentCell = newCell;
       // mOriginalCell = newCell;
        mCurrentCell.mCurrentPiece = this;

        transform.position = newCell.transform.position;
        mPieceButtons.transform.position = newCell.transform.position;


        mPieceButtons.SetActive(true);
        gameObject.SetActive(true);
    }

    

    //May not need to keep this as virtual since all pieces have similar movement - (Overridable functions base class' function needs to be virtual)
    //TODO still need to check the validity of each call - Done
    //TODO remember that you cannot walk over another sprite - Done
    public void ShowButtons()
    {
        //Hide every other piece's buttons
        List<BasePiece> allPieces = mPieceManager.mAllPieces;
        foreach(BasePiece piece in allPieces)
        {
            piece.ClearButtons();
        }



        //If in case the Piece's movement is locked, then exit this function
        if (getLockMovement())
            return;


        //Current Position
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;

        int targetX = 0, targetY = 0;
        //Check all adjacent sides
        for (int x = -1; x <= 1; x++)
        {
            targetX = currentX + x;
            //Skip if the destinaiton is outside the board
            if (targetX < 0 || targetX > 3)
                continue;

            for (int y = -1; y <= 1; y++)
            {
                targetY = currentY + y;

                //Instead of skipping (0,0) as your destination, have the ShowButton function show rotations for it.
                //Skip the destination being out of the board
                if (targetY < 0 || targetY > 3)
                    continue;
                // Skip if the destination already has a piece. 
                if (mCurrentCell.mBoard.mAllCells[targetX, targetY].mCurrentPiece != null)
                    continue;

                //TODO clean up the following

                if (x == -1 && y == 0)
                {
                    foreach (Button button in mButtons)
                    {
                        if (button.name.Equals("Move Left"))
                            button.gameObject.SetActive(true);
                    }
                }
                else if (x == 1 && y == 0)
                {
                    foreach (Button button in mButtons)
                    {
                        if (button.name.Equals("Move Right"))
                            button.gameObject.SetActive(true);
                    }
                }
                else if (x == 0 && y == 1)
                {
                    foreach (Button button in mButtons)
                    {
                        if (button.name.Equals("Move Up"))
                            button.gameObject.SetActive(true);
                    }
                }
                else if (x == 0 && y == -1)
                {
                    foreach (Button button in mButtons)
                    {
                        if (button.name.Equals("Move Down"))
                            button.gameObject.SetActive(true);
                    }
                }
               









            }
        }

        //put in rotate left and rotate right irrespective

        foreach (Button button in mButtons)
        {
            if(button.name.Contains("Rotate"))
                button.gameObject.SetActive(true);
        }
        mMouseSelected = true;


    }

    //Remove the Highlighted cells and empty its list
    public void ClearButtons()
    {
        /*
        foreach (Cell cell in mHighlightedCells)
            cell.mOutlineImage.enabled = false;

        mHighlightedCells.Clear();
        */

        foreach (Button child in mButtons)
        {
            child.gameObject.SetActive(false);
            // child.GetComponent<Renderer>().enabled = true;
        }


        mMouseSelected = false;
    }

    
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        if (!mMouseSelected)
        {
            
            ShowButtons();
        }

        else
        {
            //get the next selected click location
            Vector3 newPosition = (Vector3)eventData.delta;

            //If the new click location is in one of the highlighted cells, move the sprite there

            /*
            if (newPosition.)

            //If the new selection is not a highlighted cell, deselect everything
            else
            {
            }
            */
            ClearButtons();

        }




    }

    



}


