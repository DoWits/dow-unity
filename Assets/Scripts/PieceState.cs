using System.Collections.Generic;

public class PieceState
{
    string pieceName; // Player1: 1, Player 2: 2, Mirrors: M, rest invalid
    int row;
    int col;
    string orientation = "";
    //bool locked = false;
    /*
     * For mirrors: 0 is \ and +1 is /
     * For players: 0-Up, 1-Right, 2-Down, 3-Left
     */

    public PieceState()
    {
        // TODO: Need to rethink about default orientation when no piece is there for now it is -1
        pieceName = "";
        row = -1;
        col = -1;
        orientation = "";
    }

    public PieceState(PieceState anotherPiece)
    {
        pieceName = anotherPiece.getPieceName();
        row = anotherPiece.getPieceRow();
        col = anotherPiece.getPieceCol();
        orientation = anotherPiece.getPieceOrientation();
    }

    public PieceState(string pieceName, int row, int col, string orientation)
    {
        this.pieceName = pieceName;
        this.row = row;
        this.col = col;
        this.orientation = orientation;
    }

    public string getPieceName() { return pieceName; }
    public int getPieceRow() { return row; }
    public int getPieceCol() { return col; }
    public string getPieceOrientation() { return orientation; }

    public void setPieceName(string pieceName) { this.pieceName = pieceName; }
    public void setPieceRow(int row) { this.row = row; }
    public void setPieceCol(int col) { this.col = col; }
    public void setPieceOrientation(string orientation) { this.orientation = orientation; }

    public void setNullState()
    {
        // TODO: Need to rethink about default orientation when no piece is there for now it is -1
        pieceName = "";
        row = -1;
        col = -1;
        orientation = "";
    }

    public void updatePieceState(string pieceName, int row, int col, string orientation)
    {
        this.pieceName = pieceName;
        this.row = row;
        this.col = col;
        this.orientation = orientation;
    }

    public bool pieceExists()
    {
        if (pieceName.Contains("P") || pieceName.Contains("M")) return true;
        return false;
    }

    public bool ComparePieces(PieceState a, PieceState b)
    {
        if (a == null || b==null)
        {
            if (a == null && b == null)
                return true;
            else
                return false;

        }
        if(a.pieceName.Equals(b.pieceName))
        {
            if ((a.row == b.row) && (a.col == b.col) && a.getPieceOrientation().Equals(b.getPieceOrientation()))
                return true;
            else
                return false;
        }
        return false;
    }
    public bool ComparePieces(PieceState a)
    {
        if (a == null)
            return false;

        if (a.pieceName.Equals(pieceName))
        {
            if ((a.row == this.row) && (a.col == this.col) && a.getPieceOrientation().Equals(this.getPieceOrientation()))
                return true;
            else
                return false;
        }
        return false;
    }

    public bool ApplyAction(string action, CellState [,] Board)
    {
        

        if (action.Contains("Rotate"))
        {
            string initialOrientation = getPieceOrientation();
            string finalOrientation = "";

            int rotation = OrientationToNum[initialOrientation];

            if (action.Contains("Left"))
                rotation--;
            else
                rotation++;

            if(initialOrientation.Contains("1"))
            {
                rotation = rotation % 2;
                finalOrientation = NumtoOrientationMirror[rotation];
            }
            else
            {
                rotation = rotation % 4;
                finalOrientation = NumtoOrientationPlayer[rotation];
            }

            setPieceOrientation(finalOrientation);
            return true;
        }
        else if (action.Contains("Move"))
        {
            int x = row, y = col;
            if (action.Contains("Up"))
                y++;
            else if (action.Contains("Down"))
                y--;
            else if (action.Contains("Left"))
                x--;
            else if (action.Contains("Right"))
                x++;

            if (x > 4 || x < 0 || y > 4 || y < 0)
                return false;
            else if (Board[x, y].getPieceState() != null)
                return false;

            else
            {
                Board[row, col].setPieceState(null);

                row = x;
                col = y;
                Board[row, col].setPieceState(this);
                
                return true;
            }
        }

        else if (action.Contains("Shoot"))
        {
            //Shooting is only possible by a player
            if (getPieceName().Contains("P"))
                return true;
            else return false;
        }
        else return false;

    }


    Dictionary<string, int> OrientationToNum = new Dictionary<string, int>() {
        { "up", 0},
        { "down", 1},
        { "left", 2},
        { "right", 3},
        { "+1", 0},
        { "-1", 1},
        };

    Dictionary<int, string> NumtoOrientationPlayer = new Dictionary<int, string>()
    {
        { 0,"up" },
        { 1, "down"},
        { 2, "left"},
        { 3, "right"},

    };
    Dictionary<int, string> NumtoOrientationMirror = new Dictionary<int, string>()
    {
        { 0, "+1" },
        { 1, "-1"},

    };


}
