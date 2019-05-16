using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;


public abstract class BasePiece : EventTrigger
{
    public GameObject ShootButtonObject;
    public Color mColor = Color.clear;
   
    protected RectTransform mRectTransform = null;
    protected PieceManager mPieceManager;
   
    protected Button[] mButtons;
    protected Vector3Int mMovement = new Vector3Int(1, 1, 0);
    //This variable is for Locking the Piece's movement 
    protected bool mMouseSelected = false;
    //TODO : change the below back to protected during clean-up 
    public Cell mCurrentCell = null;
    public string mOrientation = "";
    public bool mLockMovement = false;
    public bool doneShooting = true;
    public PlayerButtons mPlayerButtons = null;
    public GameObject mPieceButtons = null;




    public bool IsDoneShooting()
    {
        return doneShooting;
    }

    public bool IsAllDoneShooting()
    {
        List<BasePiece> AllPieces = mPieceManager.mAllPieces;
        foreach(BasePiece piece in AllPieces)
        {
            if (!piece.IsDoneShooting())
                return false;
        }
        return true;
    }


    public void ResetShooting()
    {
        doneShooting = true;
    }




    
    public virtual void Setup(string orientation, Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager, GameObject pieceButtons, PlayerButtons playerButtons)
    {

        mPlayerButtons = playerButtons;
        mPieceButtons = pieceButtons;
        mButtons = mPieceButtons.GetComponentsInChildren<Button>();
        string s = base.name;

        foreach (Button b in mButtons)
            b.onClick.AddListener( () => MovePiece(this, b.name));

        ClearButtons();

        mPieceManager = newPieceManager;
       
        mColor = newTeamColor;
        GetComponent<Image>().color = newSpriteColor;
        mRectTransform = GetComponent<RectTransform>();
        mOrientation = orientation;

    }

    public string getOrientation()
    { return mOrientation; }

  

    public void Place(Cell newCell)
    {
        //Place them in the given Cell

        mCurrentCell = newCell;
        mCurrentCell.mCurrentPiece = this;

        transform.position = newCell.transform.position;
        mPieceButtons.transform.position = newCell.transform.position;


        mPieceButtons.SetActive(true);
        gameObject.SetActive(true);
    }

    

    public void ShowButtons()
    {
        //Hide every other piece's buttons
        List<BasePiece> allPieces = mPieceManager.mAllPieces;
        foreach(BasePiece piece in allPieces)
        {
            piece.ClearButtons();
        }



        //If in case the Piece's movement is locked, then exit this function
        if (mLockMovement)
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
                    mPlayerButtons.Show("Move Left");
                }
                else if (x == 1 && y == 0)
                {
                    mPlayerButtons.Show("Move Right");
                }
                else if (x == 0 && y == 1)
                {
                    mPlayerButtons.Show("Move Up");                   
                }
                else if (x == 0 && y == -1)
                {
                    mPlayerButtons.Show("Move Down");
                }
               
            }
        }

        //put in rotate left and rotate right irrespective of the piece

        mPlayerButtons.Show("Rotate Left");
        mPlayerButtons.Show("Rotate Right");
        mMouseSelected = true;


    }

    //Remove the Highlighted cells and empty its list
    public void ClearButtons()
    {
        mPlayerButtons.HideAll();

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
            ClearButtons();
        }

    }

    

    public virtual void ChangeTurn()
    { }
    public virtual void DisableShoot() { }
    public virtual void EnableShoot() { }

    public virtual void MovePiece(BasePiece currentPiece, string buttonName)
    {
        IEnumerator coroutine = mPieceManager.MovePieceCoroutine(currentPiece, buttonName);
        StartCoroutine(coroutine);


    }


    public virtual IEnumerator ShootingAnimation()
    {

        yield return false;
      //  yield return null; 
    }
    public virtual IEnumerator ShootingAnimation(Vector3 direction)
    {

        yield return false;
        //  yield return null; 
    }

    

   
    

   
}