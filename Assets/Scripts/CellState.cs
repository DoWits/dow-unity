public class CellState
{
    PieceState piece;

    public CellState()
    {
        piece = new PieceState();
    }

    public CellState(PieceState piece)
    {
        this.piece = piece;
    }

    public PieceState getPiece() { return piece; }

}
