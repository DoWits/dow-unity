using UnityEngine;

public class CellState
{
    PieceState pieceState;

    public CellState()
    {
        pieceState = new PieceState();
    }

    public CellState(PieceState pieceState)
    {
        this.pieceState = pieceState;
    }

    public PieceState getPieceState() { return pieceState; }
    public void setPieceState(PieceState pieceState) { this.pieceState = pieceState; }

    public void PrintCellState()
    {
        Debug.Log("Piece Name: " + pieceState.getPieceName());
        Debug.Log("Piece Row: " + pieceState.getPieceRow());
        Debug.Log("Piece Col: " + pieceState.getPieceCol());
        Debug.Log("Piece Orientation: " + pieceState.getPieceOrientation());
    }
}
