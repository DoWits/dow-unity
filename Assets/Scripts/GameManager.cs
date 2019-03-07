﻿using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Board mBoard;

    public PieceManager mPieceManager;

    public List<BasePiece> mPieces = null;

    //Default GameMode is PvP
    string GameMode = "PvP";

    // Start is called before the first frame update
    void Start()
    {

        QualitySettings.vSyncCount = 1;


        Camera.main.orthographicSize = 400;
        //Get the GameMode

        //Create the Board
        mBoard.Create();

        //Create the Pieces with the GameMode
        mPieceManager.Setup(mBoard,mPieces,GameMode);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
