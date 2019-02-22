public class PieceState
{
    char pieceName; // Player1: 1, Player 2: 2, Mirrors: M, rest invalid
    int row;
    int col;
    int orientation;
    /*
     * For mirrors: 0 is \ and +1 is /
     * For players: 0-Up, 1-Right, 2-Down, 3-Left
     */

    public PieceState()
    {
        // TODO: Need to rethink about default orientation when no piece is there for now it is -1
        pieceName = ' ';
        row = -1;
        col = -1;
        orientation = -1;
    }

    public PieceState(PieceState anotherPiece)
    {
        pieceName = anotherPiece.getPieceName();
        row = anotherPiece.getPieceRow();
        col = anotherPiece.getPieceCol();
        orientation = anotherPiece.getPieceOrientation();
    }

    public PieceState(char pieceName, int row, int col, int orientation)
    {
        this.pieceName = pieceName;
        this.row = row;
        this.col = col;
        this.orientation = orientation;
    }

    public char getPieceName() { return pieceName; }
    public int getPieceRow() { return row; }
    public int getPieceCol() { return col; }
    public int getPieceOrientation() { return orientation; }

    public void setPieceName(char pieceName) { this.pieceName = pieceName; }
    public void setPieceRow(int row) { this.row = row; }
    public void setPieceCol(int col) { this.col = col; }
    public void setPieceOrientation(int orientation) { this.orientation = orientation; }

    public void setNullState()
    {
        // TODO: Need to rethink about default orientation when no piece is there for now it is -1
        pieceName = ' ';
        row = -1;
        col = -1;
        orientation = -1;
    }

    public void updatePieceState(char pieceName, int row, int col, int orientation)
    {
        this.pieceName = pieceName;
        this.row = row;
        this.col = col;
        this.orientation = orientation;
    }

    public bool pieceExists()
    {
        if (pieceName == '1' || pieceName == '2' || pieceName == 'M') return true;
        return false;
    }
}
