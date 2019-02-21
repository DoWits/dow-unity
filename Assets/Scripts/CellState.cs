public class CellState
{
    PieceState currentPiece;
    
    public CellState()
    {
        currentPiece = null;
    }

    public CellState(PieceState currentPiece)
    {
        this.currentPiece = currentPiece;
    }

    public CellState(CellState cs)
    {
        currentPiece = cs.getPiece();
    }

    // Getters and Setters
    public PieceState getPiece() { return currentPiece; }
    public void setPiece(PieceState piece)
    {
        currentPiece = piece;
    }
}
