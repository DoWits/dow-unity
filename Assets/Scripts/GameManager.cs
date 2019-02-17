using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Board mBoard;

    public PieceManager mPieceManager;

    public List<BasePiece> mPieces = null;

    // Start is called before the first frame update
    void Start()
    {
        Camera.main.orthographicSize = 400;
        //Create the Board
        mBoard.Create();

        //Create the Pieces
        mPieceManager.Setup(mBoard,mPieces);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
