using System;
using System.Collections.Generic;
using UnityEngine;



//TODO : Check if we need to seperate the BasePieces into PlayerPiece and MirrorPiece

public class PieceManager : MonoBehaviour
{
    //List of Mirrors and Players
    private List<BasePiece> mMirrors = null;

    private int mTotalMirrors = 4;
    private BasePiece
        mPlayer1 = null,
        mPlayer2 = null;

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

    public void Setup(Board board, List<BasePiece> allPieces)
    {
        
        //TODO find approprite colors for first and second players. for now, it's black and white
        mPlayer1 = CreatePlayer("up", Color.white, new Color32(80, 124, 159, 255), board);
        mPlayer2 = CreatePlayer("down", Color.black, new Color32(210, 95, 64, 255), board);
        mMirrors = CreateMirrors(Color.gray, new Color32(119, 136, 153, 255), board);


        PlacePlayer(0, 0, mPlayer1, board);
        PlacePlayer(3, 3, mPlayer2, board);
        PlaceMirrors(mMirrors, board, mMirrorPositions);

        allPieces.Add(mPlayer1);
        allPieces.Add(mPlayer2);
        allPieces.AddRange(mMirrors);

        mAllPieces = allPieces;

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
            pieceHolderObject.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            pieceHolderObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            pieceHolderObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            pieceHolderObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            pieceHolderObject.transform.localScale = new Vector3(1, 1, 1);
            pieceHolderObject.transform.localRotation = Quaternion.identity;

            pieceButtonObject.transform.localScale = new Vector3(1, 1, 1);
            pieceButtonObject.transform.localRotation = Quaternion.identity;

            newPieceObject.transform.localScale = new Vector3(1, 1, 1);
            newPieceObject.transform.localRotation = Quaternion.identity;

            //Below TODO is done. just need to test it
            //TODO: Recheck the tutprial, they used some type thingie. Test with and without it. If it works fine without, then remove this
            string key = orientation;
            Type pieceType = mPieceLibrary[key];

            BasePiece newMirror = (BasePiece)newPieceObject.AddComponent(pieceType);
            //setup the Mirror
            newMirror.Setup(orientation, teamColor, spriteColor, this, pieceButtonObject);

           


            allMirrors.Add(newMirror);
        }
        

        return allMirrors;
    }

    private BasePiece CreatePlayer(string orientation, Color teamColor, Color32 spriteColor, Board board)
    {
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
        pieceHolderObject.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        pieceHolderObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        pieceHolderObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        pieceHolderObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        pieceHolderObject.transform.localScale = new Vector3(1, 1, 1);
        pieceHolderObject.transform.localRotation = Quaternion.identity;

        pieceButtonObject.transform.localScale = new Vector3(1, 1, 1);
        pieceButtonObject.transform.localRotation = Quaternion.identity;

        newPieceObject.transform.localScale = new Vector3(1, 1, 1);
        newPieceObject.transform.localRotation = Quaternion.identity;



        //Below TODO is done. just need to test it
        //TODO: Recheck the tutprial, they used some type thingie. Test with and without it. If it works fine without, then remove this
        string key = orientation;
        Type pieceType = mPieceLibrary[key];


        //setup the Player
        BasePiece newPlayer = (BasePiece)newPieceObject.AddComponent(pieceType);

        newPlayer.Setup(orientation, teamColor, spriteColor, this, pieceButtonObject);

        


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

    public void MovePiece(BasePiece currentPiece, string buttonName)
    {


       // Vector3 targetposition;
        Cell currentCell = currentPiece.getCurrentCell();
        Cell targetCell = currentCell;
        //Set all peices as Locked
        foreach (BasePiece piece in mAllPieces)
        {
            piece.setLockMovement(true);
            piece.ClearButtons();
        }
        int x, y;
        x = currentCell.mBoardPosition.x;
        y = currentCell.mBoardPosition.y;

        //Move the required piece
        Debug.Log(buttonName);
        if (buttonName.Equals("Rotate Left"))
        {
            
            currentPiece.transform.Rotate(new Vector3(0, 0, 90));

        }
        else if (buttonName.Equals("Rotate Right"))
        {
            currentPiece.transform.Rotate(new Vector3(0, 0, 270));
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

        currentPiece.setCurrentCell(targetCell);
        currentPiece.transform.position = targetCell.transform.position;
        currentPiece.getPieceButtons().transform.position = targetCell.transform.position;
        currentCell.mCurrentPiece = null;
        targetCell.mCurrentPiece = currentPiece;



        foreach (BasePiece piece in mAllPieces)
        {
            piece.setLockMovement(false);
            piece.ClearButtons();
        }

    }

    private void Update()
    {
        


    }

}