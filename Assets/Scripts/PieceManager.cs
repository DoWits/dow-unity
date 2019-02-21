using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



//TODO : Check if we need to seperate the BasePieces into PlayerPiece and MirrorPiece

public class PieceManager : MonoBehaviour
{
    //List of Mirrors and Players


    private EdgeCollider2D edges;
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

    private Dictionary<string, char> gameStatePieceConventions = new Dictionary<string, char>()
    {
        {"P1", '1' },
        {"P2", '2' },
        {"M1", 'M' },
        {"M2", 'M' },
        {"M3", 'M' },
        {"M4", 'M' },
    };

    private Dictionary<string, int> gameStateOrientationConventions = new Dictionary<string, int>()
    {
        {"up", 0 },
        {"right", 1 },
        {"down", 2 },
        {"left", 3 },
        {"-1", -1 },
        {"+1", 1 },
    };

    GameState gameState;


    public void Setup(Board board, List<BasePiece> allPieces)
    {


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

        gameState = new GameState();

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

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateGameState(BasePiece updatedPiece, Cell previousCell, Cell currentCell, string action)
    {

        // Debug.Log("The updatedPiece name: " + updatedPiece.gameObject.name);
        // Debug.Log("Action peformed is " + action);
        // TODO: action string is never used, can be removed later
        Vector2Int prevCellPos, currCellPos;

        prevCellPos = previousCell.GetCellPosition();
        int prevCol = prevCellPos.x;
        int prevRow = prevCellPos.y;

        currCellPos = currentCell.GetCellPosition();
        int currentCol = currCellPos.x;
        int currentRow = currCellPos.y;

        int orientation = gameStateOrientationConventions[updatedPiece.getOrientation()];
        char pieceName = gameStatePieceConventions[updatedPiece.gameObject.name];

        // updating if last mirror moved or set to null
        if(pieceName.Equals('M'))
        {
            gameState.setLastMirrorMoved(new PieceState('M', currentRow, currentCol, orientation));
        } else
        {
            gameState.setLastMirrorMoved(null);
        }

        // updating cells
        if (prevCellPos.Equals(currCellPos))
        {
            // update orienation only even if it just shoots
            gameState.getCellState(prevRow, prevCol).getPieceState().setPieceOrientation(orientation);
        } else
        {
            /* update the piece position
             * 1. previous cell to null
             * 2. current cell will now have the piece with the same orientation
             */

            // previous cell
            gameState.getCellState(prevRow, prevCol).getPieceState().setNullState();

            // current cell
            
            CellState cellState = gameState.getCellState(currentRow, currentCol);
            PieceState pieceState = cellState.getPieceState();
            pieceState.updatePieceState(pieceName, currentRow, currentCol, orientation);

        }

        gameState.changeCurrentTurn();
        // check for goal state
    }


}