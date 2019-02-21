using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;


public abstract class BasePiece : EventTrigger
{
    public GameObject ShootButtonObject;
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
    protected PlayerButtons mPlayerButtons = null;


    GameObject mPieceButtons = null;
    public virtual void Setup(string orientation, Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager, GameObject pieceButtons, PlayerButtons playerButtons)
    {

        mPlayerButtons = playerButtons;
        mPieceButtons = pieceButtons;
        mButtons = mPieceButtons.GetComponentsInChildren<Button>();
        string s = base.name;

        foreach (Button b in mButtons)
            b.onClick.AddListener(() => MovePiece(this, b.name));

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
        IEnumerator coroutine = MovePieceRoutine(currentPiece, buttonName);
        StartCoroutine(coroutine);
    }


    public virtual IEnumerator ShootingAnimation()
    {


        yield return 0; 
    }


    public virtual IEnumerator MovePieceRoutine(BasePiece currentPiece, string buttonName)
    {
        BasePiece 
            p1 = null, 
            p2 = null;

        Cell currentCell = currentPiece.mCurrentCell;
        Cell targetCell = currentCell;
        //Set all peices as Locked
        foreach (BasePiece piece in mPieceManager.mAllPieces)
        {


            if (piece.mOrientation.Equals("left") || piece.mOrientation.Equals("up") || piece.mOrientation.Equals("down") || piece.mOrientation.Equals("right"))
            {
                //It's not this player's turn
                if (piece.mLockMovement)
                    p2 = piece;
                //It is this player's turn
                else
                    p1 = piece;

            }
            piece.mLockMovement = true;
            piece.ClearButtons();
            
        }
        //disable the shoot buttons so that p2 can't press it during the animation
        p1.DisableShoot();

        string playerName = p1.gameObject.name;
        string pieceName = currentPiece.gameObject.name;

        Debug.Log(playerName + " : " + pieceName + " " + buttonName);

        int x, y;
        x = currentCell.mBoardPosition.x;
        y = currentCell.mBoardPosition.y;

        //Move the required piece
        if (buttonName.Equals("Rotate Left"))
        {
            Quaternion initialRotation = currentPiece.transform.rotation;
            float targetRotation = 90f;
            
            while(Quaternion.Angle(initialRotation, currentPiece.transform.rotation) < 90)
                yield return RotateAnimation(currentPiece, targetRotation);

            //Change the orientation of the piece

            if (currentPiece.mOrientation.Equals("up"))
                currentPiece.mOrientation = "left";
            else if (currentPiece.mOrientation.Equals("left"))
                currentPiece.mOrientation = "down";
            else if (currentPiece.mOrientation.Equals("down"))
                currentPiece.mOrientation = "right";
            else if (currentPiece.mOrientation.Equals("right"))
                currentPiece.mOrientation = "up";
            else if (currentPiece.mOrientation.Equals("+1"))
                currentPiece.mOrientation = "-1";
            else
                currentPiece.mOrientation = "+1";



        }
        else if (buttonName.Equals("Rotate Right"))
        {

            Quaternion initialRotation = currentPiece.transform.rotation;
            float targetRotation = -90f;

            while (Quaternion.Angle(initialRotation, currentPiece.transform.rotation) < 90)
                yield return RotateAnimation(currentPiece, targetRotation);

            //Change the orientation of the piece

            if (currentPiece.mOrientation.Equals("left"))
                currentPiece.mOrientation = "up";
            else if (currentPiece.mOrientation.Equals("down"))
                currentPiece.mOrientation = "left";
            else if (currentPiece.mOrientation.Equals("right"))
                currentPiece.mOrientation = "down";
            else if(currentPiece.mOrientation.Equals("up"))
                currentPiece.mOrientation = "right";
            else if (currentPiece.mOrientation.Equals("+1"))
                currentPiece.mOrientation = "-1";
            else
                currentPiece.mOrientation = "+1";
        }
        else if (buttonName.Equals("Move Up"))
        {
            //get the current cell of the piece
            //set the current cell to the new cell
            //set the position of the piece as the new Cell

            y = y + 1;
            targetCell = currentCell.mBoard.mAllCells[x, y];
        }
        else if (buttonName.Equals("Move Down"))
        {
            y = y - 1;
            targetCell = currentCell.mBoard.mAllCells[x, y];


        }
        else if (buttonName.Equals("Move Left"))
        {
            x = x - 1;
            targetCell = currentCell.mBoard.mAllCells[x, y];


        }
        else if (buttonName.Equals("Move Right"))
        {
            x = x + 1;
            targetCell = currentCell.mBoard.mAllCells[x, y];

        }

        currentPiece.mCurrentCell = targetCell;
        Vector3 distanceToTravel = targetCell.transform.position - currentPiece.transform.position;
        while (currentPiece.transform.position != targetCell.transform.position)
        {
            
            yield return MoveAnimation(currentPiece, distanceToTravel);
        }


        currentPiece.mPieceButtons.transform.position = targetCell.transform.position;
        currentCell.mCurrentPiece = null;
        targetCell.mCurrentPiece = currentPiece;


        yield return p1.ShootingAnimation();

        /*
         * After the shootanimation, call a piecemanagaer function that makes checks if anyone was shot
         */

        BasePiece shotPlayer = mPieceManager.GetPlayerIfShot(
            p1.mCurrentCell.GetCellPosition().x, 
            p1.mCurrentCell.GetCellPosition().y,
            p1.mOrientation);

        if (shotPlayer == p1)
        {
            Debug.Log(p2.name + " Wins");
            mPieceManager.ResetGame();
        }
        else if (shotPlayer == p2)
        {
            Debug.Log(p1.name + " Wins");
            mPieceManager.ResetGame();
        }
        foreach (BasePiece piece in mPieceManager.mAllPieces)
        {
            piece.mLockMovement = (false);
            piece.ClearButtons();
        }

        if (currentPiece != p1 && currentPiece != p2)
            currentPiece.mLockMovement = true;

          p2.mLockMovement = (true);
          p1.mLockMovement = (false);



        //Change turns for both players
        p1.ChangeTurn();
        p2.ChangeTurn();


    }

    public IEnumerator RotateAnimation(BasePiece mPiece, float targetRotation)
    {
        mPiece.transform.rotation *= Quaternion.Euler(0, 0, targetRotation / 10);
        
        yield return 0;
    }

    public IEnumerator MoveAnimation(BasePiece currentPiece, Vector3 distanceToTravel)
    {

        currentPiece.transform.position = currentPiece.transform.position + (distanceToTravel / 10);//, ref vel, smoothTime);

        yield return 0;
    }
    

   
}