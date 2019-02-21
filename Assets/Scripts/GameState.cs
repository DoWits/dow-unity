using System.Collections.Generic;

public class GameState
{
    public CellState[,] allBoardCells;
    public PieceState lastMirrorMoved;
    public bool currentTurn; // Player 1(True) | Player 2 (False)
    public List<PieceState> allPieces;

    public GameState()
    {

        allBoardCells = new CellState[4, 4];
        allPieces = new List<PieceState>();

        CellState tempCell;
        PieceState tempPiece;

        // Player 1
        tempPiece = new PieceState('1', 0, 0, 0);
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

    }

    public bool getCurrentTurn() { return currentTurn; }
    public void changeCurrentTurn() { this.currentTurn = !this.currentTurn; }

    public PieceState getLastMirrorMoved() { return lastMirrorMoved; }
    public void setLastMirrorMoved(PieceState piece) { this.lastMirrorMoved = piece; }

    public void updateAllBoardCells(CellState[,] allBoardCells)
    {
        this.allBoardCells = allBoardCells;
    }

    public CellState[,] getAllBoardCells() { return allBoardCells; }
    
}
