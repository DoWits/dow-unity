using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Board mBoard;

    public PieceManager mPieceManager;

    public List<BasePiece> mPieces = null;

    //Default GameMode is PvP
    string GameMode = "PvA";
    int Level = 1;

    // Start is called before the first frame update
    void Start()
    {
        GameMode = GameModeSelect.GameMode;
        Level = GameModeSelect.Level;
        QualitySettings.vSyncCount = 1;


        //Camera.main.orthographicSize = 400;
        //Camera.main.aspect = (9/16);
        //Get the GameMode

        //Create the Board
        mBoard.Create();

        //Create the Pieces with the GameMode
        mPieceManager.Setup(mBoard,mPieces,GameMode, Level);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

    }
}
