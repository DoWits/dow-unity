public class PieceState 
{
    public char pieceName; // player 1:1, player: 2, mirror: m, empty for nil piece
    public int orientation;
    public int row;
    public int col;

    /*
* Orientation for mirror piece
*      -1 means \
*      1 means /
*  For player piece
*      0=> Up | 1=> Right | 2=> Down | 3=>Left
*/


    public PieceState(char pieceName, int row, int col, int orientation)
    {
        this.pieceName = pieceName;
        this.row = row;
        this.col = col;
        this.orientation = orientation;
    }

    // Getters and Setters

    public void setPieceName(char name) { this.pieceName = name; }
    public void setPieceOrientation(char orientation) { this.orientation = orientation; }
    public void setPieceRow(int row) { this.row = row; }
    public void setPieceCol(int col) { this.col = col; }

    public char getPieceName() { return pieceName; }
    public int getPieceOrientation() { return orientation; }
    public int getPieceRow() { return row; }
    public int getPieceCol() { return col; }

}
