/*
    # Game State

    - Game
      	- board state (all pieces state)
      	- player turn
      	- mirror moved in last turn

    - Player (x2)
      	- position
      	- orientation

    - Mirror (x4)
      	- position
      	- orientation
*/

using System.Collections.Generic;

public class GameState
{
    // Each Cell has a piece (it may be null if nothing is present)
    private CellState[,] allBoardCells;
    private PieceState lastMovedMirror;
    public List<PieceState> allPieces;
    private bool currentTurn; // player 1: true, player 2: false

    public GameState()
    {
        allPieces = new List<PieceState>();
        CellState tempCell;
        allBoardCells = new CellState[4, 4];

        // Player 1
        PieceState tempPiece = new PieceState('1', 0, 0, 0);
        allPieces.Add(tempPiece);
        tempCell = new CellState(tempPiece);
        allBoardCells[0, 0] = tempCell;

        //Player 2
        tempPiece = new PieceState('2', 3, 3, 2);
        allPieces.Add(tempPiece);
        tempCell = new CellState(tempPiece);
        allBoardCells[3, 3] = tempCell;

        // Mirror 1
        tempPiece = new PieceState('M', 1, 1, 1);
        allPieces.Add(tempPiece);
        tempCell = new CellState(tempPiece);
        allBoardCells[1, 1] = tempCell;

        // Mirror 2
        tempPiece = new PieceState('M', 1, 2, -1);
        allPieces.Add(tempPiece);
        tempCell = new CellState(tempPiece);
        allBoardCells[1, 2] = tempCell;

        // Mirror 3
        tempPiece = new PieceState('M', 2, 1, -1);
        allPieces.Add(tempPiece);
        tempCell = new CellState(tempPiece);
        allBoardCells[2, 1] = tempCell;

        // Mirror 4
        tempPiece = new PieceState('M', 2, 2, 1);
        allPieces.Add(tempPiece);
        tempCell = new CellState(tempPiece);
        allBoardCells[2, 2] = tempCell;

        this.lastMovedMirror = null;
        this.currentTurn = true;
    }
    // Getters and Setters

    // function for winning

    // function for all adjacent available moves

    

}


