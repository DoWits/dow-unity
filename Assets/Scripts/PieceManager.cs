using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



//TODO : Check if we need to seperate the BasePieces into PlayerPiece and MirrorPiece

public class PieceManager : MonoBehaviour
{
    //List of Mirrors and Players

    AIScript helper = new AIScript();
    public bool currentTurn = true;
    private EdgeCollider2D edges;
    private string mGameMode = "PvA";
    private List<BasePiece> mMirrors = null;
    private Board mBoard;
    private int mTotalMirrors = 4;
    private BasePiece
        mPlayer1 = null,
        mPlayer2 = null;

    public GameObject mShootButtonPrefab;
    public GameObject mPiecePrefab;
    public GameObject mPieceButtonPrefab;
    public GameObject mPieceHolderPrefab;
    public List<BasePiece> mAllPieces;
    private string[] mMirrorOrientation = new string[4]
    {
        "-1","+1","+1","-1"
    };

    private int[,] mMirrorPositions = new int[4,2] 
    {
        {2,1},
        {2,2},
        {1,1},
        {1,2}
    };

    private Dictionary<string, Type> mPieceLibrary = new Dictionary<string, Type>()
    {
        {"up", typeof(PlayerPiece) },
        {"down", typeof(PlayerPiece) },
        {"left", typeof(PlayerPiece) },
        {"right", typeof(PlayerPiece) },
        {"+1", typeof(MirrorPiece) },
        {"-1", typeof(MirrorPiece) }
    };



    GameState mainGameState;


    public void Setup(Board board, List<BasePiece> allPieces, string GameMode)
    {
        currentTurn = true;

        mBoard = board;

        edges =  this.gameObject.AddComponent<EdgeCollider2D>() as EdgeCollider2D;

        edges.points = new Vector2[]
        {
            new Vector2(-215f,-215f),
            new Vector2(-215f, 215f),
            new Vector2( 215f, 215f),
            new Vector2( 215f,-215f),
            new Vector2(-215f,-215f),
        };


        mPlayer1 = CreatePlayer("up", Color.white, new Color32(80, 124, 159, 255), board, "P1");
        mPlayer2 = CreatePlayer("down", Color.black, new Color32(210, 95, 64, 255), board, "P2");
        mMirrors = CreateMirrors(Color.gray, new Color32(119, 136, 153, 255), board);


        PlacePlayer(0, 0, mPlayer1, board);
        PlacePlayer(3, 3, mPlayer2, board);
        PlaceMirrors(mMirrors, board, mMirrorPositions);

        allPieces.Add(mPlayer1);
        allPieces.Add(mPlayer2);
        allPieces.AddRange(mMirrors);

        mAllPieces = allPieces;

        mainGameState = new GameState();
        //Debug.Log(helper.HeuristicSum(mainGameState, mainGameState.currentTurn));

    }


    private List<BasePiece> CreateMirrors(Color teamColor, Color32 spriteColor, Board board)
    {
        List<BasePiece> allMirrors = new List<BasePiece>();


        //create new Object
        for (int i = 0; i < mTotalMirrors; i++)
        {

            string orientation = mMirrorOrientation[i];
            //Create the holder object and set its parent as the PieceManager
            GameObject pieceHolderObject = Instantiate(mPieceHolderPrefab);
            pieceHolderObject.transform.SetParent(transform);


            //Create the new Piece and set its parent as the piece holder
            GameObject newPieceObject = Instantiate(mPiecePrefab);
            newPieceObject.transform.SetParent(pieceHolderObject.transform);

            GameObject pieceButtonObject = Instantiate(mPieceButtonPrefab);
            pieceButtonObject.transform.SetParent(pieceHolderObject.transform);
                       

            //Set Scale and Position - For the Holder of object We need to manually do all of the settings from the script because I can't get it done from the GUI
            pieceHolderObject.transform.localPosition = new Vector3(0, 0, 0);
            pieceHolderObject.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            pieceHolderObject.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
            pieceHolderObject.GetComponent<RectTransform>().pivot = new Vector2(0.0f, 0.0f);
            pieceHolderObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            pieceHolderObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            pieceHolderObject.transform.localScale = new Vector3(1, 1, 1);
            pieceHolderObject.transform.localRotation = Quaternion.identity;

            pieceButtonObject.transform.localScale = new Vector3(1, 1, 1);
            pieceButtonObject.transform.localRotation = Quaternion.identity;

            newPieceObject.transform.localScale = new Vector3(1, 1, 1);
            newPieceObject.transform.localRotation = Quaternion.identity;

            
            string key = orientation;
            Type pieceType = mPieceLibrary[key];

            BasePiece newMirror = (BasePiece)newPieceObject.AddComponent(pieceType);
            PlayerButtons playerButtons = (PlayerButtons)pieceButtonObject.AddComponent(typeof(PlayerButtons));
            //setup the Mirror
            playerButtons.Setup(pieceButtonObject);

            newMirror.gameObject.name = "M" + (i + 1);
            newMirror.Setup(orientation, teamColor, spriteColor, this, pieceButtonObject,playerButtons);

           


            allMirrors.Add(newMirror);
        }
        

        return allMirrors;
    }

    private BasePiece CreatePlayer(string orientation, Color teamColor, Color32 spriteColor, Board board, string tag)
    {
       



        //Create the holder object and set its parent as the PieceManager
        GameObject pieceHolderObject = Instantiate(mPieceHolderPrefab);
        pieceHolderObject.transform.SetParent(transform);

        GameObject shootButton = Instantiate(mShootButtonPrefab);
        shootButton.transform.SetParent(pieceHolderObject.transform);


        //Create the new Piece and set its parent as the piece holder
        GameObject newPieceObject = Instantiate(mPiecePrefab);
        newPieceObject.transform.SetParent(pieceHolderObject.transform);

        GameObject pieceButtonObject = Instantiate(mPieceButtonPrefab);
        pieceButtonObject.transform.SetParent(pieceHolderObject.transform);

        

        //Set Scale and Position - For the Holder of object We need to manually do all of the settings from the script because I can't get it done from the GUI
        pieceHolderObject.transform.localPosition = new Vector3(0, 0, 0);
        pieceHolderObject.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        pieceHolderObject.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
        pieceHolderObject.GetComponent<RectTransform>().pivot = new Vector2(0.0f, 0.0f);
        pieceHolderObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        pieceHolderObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        pieceHolderObject.transform.localScale = new Vector3(1, 1, 1);
        pieceHolderObject.transform.localRotation = Quaternion.identity;

        pieceButtonObject.transform.localScale = new Vector3(1, 1, 1);
        pieceButtonObject.transform.localRotation = Quaternion.identity;

        newPieceObject.transform.localScale = new Vector3(1, 1, 1);
        newPieceObject.transform.localRotation = Quaternion.identity;

        string key = orientation;
        Type pieceType = mPieceLibrary[key];

        //setup the buttons and the player
        PlayerButtons playerButtons = (PlayerButtons)pieceButtonObject.AddComponent(typeof(PlayerButtons));
        BasePiece newPlayer = (BasePiece)newPieceObject.AddComponent(pieceType);
        newPlayer.gameObject.name = tag;


        playerButtons.Setup(pieceButtonObject);
        newPlayer.ShootButtonObject = shootButton;
        newPlayer.Setup(orientation, teamColor, spriteColor, this, pieceButtonObject, playerButtons);

        
        return newPlayer;
    }

    private void PlacePlayer(int row, int column, BasePiece player, Board board)
    {
        player.Place(board.mAllCells[row, column]);
    }

    private void PlaceMirrors(List<BasePiece> mirrors, Board board, int[,] mirrorPositions)
    {
        for (int i = 0; i < 4; i++)
        {
            mirrors[i].Place(board.mAllCells[mirrorPositions[i,0], mirrorPositions[i,1]]);
        }
    }

   

    void Update()
    {

    }

   

    public BasePiece GetPlayerIfShot(int x, int y, string direction)
    {
        Cell currentCell = mBoard.mAllCells[x, y];

        Dictionary<string, Vector2Int> getDirection = new Dictionary<string, Vector2Int>()
        {
            { "up", new Vector2Int(0,1)},
            { "down", new Vector2Int(0,-1)},
            { "left", new Vector2Int(-1,0) },
            { "right", new Vector2Int(1,0) }
        };

        Vector2Int directionIncrement = getDirection[direction];

        int X = 0,Y=0;
        BasePiece encounteredPiece = null ;
        //keep going till there you hit the board or a player
        do
        {
            encounteredPiece = null;
            X = currentCell.GetCellPosition().x + directionIncrement.x;
            Y = currentCell.GetCellPosition().y + directionIncrement.y;
            if (X >= 0 && Y >= 0 && X < 4 && Y < 4)
            {
                currentCell = mBoard.mAllCells[X, Y];
                encounteredPiece = mBoard.mAllCells[X, Y].mCurrentPiece;
            }

            if (encounteredPiece != null)
            {

                if (encounteredPiece.getOrientation().Equals("+1") || encounteredPiece.getOrientation().Equals("-1"))
                {
                    int flipSign = 1;
                    if (encounteredPiece.getOrientation().Equals("-1"))
                        flipSign = -1;

                    //update the new direction increment
                    int tempX = (int)directionIncrement.x;
                    int tempY = (int)directionIncrement.y;
                    directionIncrement = new Vector2Int(flipSign * tempY, flipSign * tempX);


                }
                else
                    break;
            }


        } while ( X >= 0 && Y >= 0 && X < 4 && Y < 4);

        return encounteredPiece;
    }


   



    public  IEnumerator MovePieceCoroutine(BasePiece currentPiece, string buttonName)
    {
        BasePiece
            p1 = null,
            p2 = null;

        Cell currentCell = currentPiece.mCurrentCell;
        Cell targetCell = currentCell;
        //Set all peices as Locked
        foreach (BasePiece piece in mAllPieces)
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

        //Debug.Log(playerName + " : " + pieceName + " " + buttonName);

        int x, y;
        x = currentCell.mBoardPosition.x;
        y = currentCell.mBoardPosition.y;




        //Update the GameState here

        // UpdateGameState(currentPiece, currentCell, targetCell, buttonName);
        bool noError = mainGameState.GetGameStateOnAction(buttonName, mainGameState.allBoardCells[x, y].getPieceState());

        //Testing


       // Debug.Log("Move Possible = "+ noError + "\n" + mainGameState);


        //Move the required piece

        if (buttonName.Contains("Rotate"))
        {
            string orientation = currentPiece.mOrientation;
            Quaternion initialRotation = currentPiece.transform.rotation;
            float targetRotation = 0;
            int numOrientation = OrientationToNum[orientation];

            if (buttonName.Equals("Rotate Left"))
            {
                targetRotation = 90f;

                while (Quaternion.Angle(initialRotation, currentPiece.transform.rotation) < 90)
                    yield return RotateAnimation(currentPiece, targetRotation);

                numOrientation--;

            }
            else if (buttonName.Equals("Rotate Right"))
            {
                 targetRotation = -90f;

                while (Quaternion.Angle(initialRotation, currentPiece.transform.rotation) < 90)
                    yield return RotateAnimation(currentPiece, targetRotation);

                numOrientation++;
            }

            if (currentPiece.mOrientation.Contains("1"))
            {
                numOrientation = numOrientation % 2;
                if (numOrientation < 0)
                    numOrientation = 2 + numOrientation;
                currentPiece.mOrientation = NumtoOrientationMirror[numOrientation];

            }
            else
            {
                numOrientation = numOrientation % 4;
                if (numOrientation < 0)
                    numOrientation = 4 + numOrientation;
                currentPiece.mOrientation = NumtoOrientationPlayer[numOrientation];
            }
        }
        else if (buttonName.Contains("Move")) { 
            if (buttonName.Equals("Move Up"))           
                y = y + 1;           
            else if (buttonName.Equals("Move Down"))           
                y = y - 1;           
            else if (buttonName.Equals("Move Left"))                
                x = x - 1;            
            else if (buttonName.Equals("Move Right"))            
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

       

        p1.doneShooting = false;

        while (p1.IsDoneShooting() == false)
            yield return p1.ShootingAnimation();

        p1.ResetShooting();
      


        BasePiece shotPlayer = GetPlayerIfShot(
            p1.mCurrentCell.GetCellPosition().x,
            p1.mCurrentCell.GetCellPosition().y,
            p1.mOrientation);

        if (shotPlayer == p1)
        {
            Debug.Log(p2.name + " Wins");
          //  UpdateWinState(p2, p2.mCurrentCell.GetCellPosition().y, p2.mCurrentCell.GetCellPosition().x);
            ResetGame();
        }
        else if (shotPlayer == p2)
        {
            Debug.Log(p1.name + " Wins");
          //  UpdateWinState(p1, p1.mCurrentCell.GetCellPosition().y, p1.mCurrentCell.GetCellPosition().x);
            ResetGame();
        }
        else { /* do nothing*/ }

        foreach (BasePiece piece in mAllPieces)
        {
            piece.mLockMovement = (false);
            piece.ClearButtons();
            piece.mPlayerButtons.RotateButtons();
        }

        if (currentPiece != p1 && currentPiece != p2)
            currentPiece.mLockMovement = true;

        p2.mLockMovement = (true);
        p1.mLockMovement = (false);



        //Change turns for both players
        p1.ChangeTurn();
        p2.ChangeTurn();
       // Debug.Log(helper.HeuristicSum(mainGameState, mainGameState.currentTurn));
        //TODO remove the part below this

        currentTurn = !currentTurn;

        //while (currentTurn)
        //    yield return 0;
        /*
        if (!currentTurn)
        {
            AIMove(mainGameState, mGameMode);

        }
        */
    }


    public void AIMove(GameState gameState, string gameMode)
    {
        GameState AI = null;

        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        //float start = Time.time;
        sw.Start();

        AI = helper.ComputerMove(gameState, gameMode);

        sw.Stop();
        Debug.Log("Computer Move took - " + (sw.Elapsed));


        //start = Time.time;
        sw.Restart();
        Tuple<BasePiece, string> change = mainGameState.GameStateChangeToAction(gameState, AI, mAllPieces);
        /*
        if (change.Item1 == null)
            Debug.Log("\n" + change.Item2);
        else
            Debug.Log("\n" + change.Item1.name + "  " + change.Item2 + " " +mainGameState.numOfMoves );
            */
        foreach (BasePiece pieceToMove in mAllPieces)
        {
            if (pieceToMove.name.Equals(change.Item1.name))
            {
                pieceToMove.MovePiece(pieceToMove, change.Item2);

                break;
            }

        }
        


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


    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public string ComputerMove()
    {
        string action = "";


        return action;

    }

   

   

    Dictionary<string, int> OrientationToNum = new Dictionary<string, int>() {
        { "up", 0},
        { "right", 1},
        { "down", 2},
        { "left", 3},
        { "+1", 0},
        { "-1", 1},
        };

    Dictionary<int, string> NumtoOrientationPlayer = new Dictionary<int, string>()
    {
        { 0,"up" },
        { 1, "right"},
        { 2, "down"},
        { 3, "left"},

    };
    Dictionary<int, string> NumtoOrientationMirror = new Dictionary<int, string>()
    {
        { 0, "+1" },
        { 1, "-1"},

    };



}