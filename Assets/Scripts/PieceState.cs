
using System.Collections.Generic;

public class PieceState
{
    string pieceName; // Player1: 1, Player 2: 2, Mirrors: M, rest invalid
    int x;
    int y;
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
        x = -1;
        y = -1;
        orientation = "";
    }

    public PieceState(PieceState anotherPiece)
    {
        pieceName = anotherPiece.getPieceName();
        x = anotherPiece.getPieceX();
        y = anotherPiece.getPieceY();
        orientation = anotherPiece.getPieceOrientation();
    }

    public PieceState(string pieceName, int x, int y, string orientation)
    {
        this.pieceName = pieceName;
        this.x = x;
        this.y = y;
        this.orientation = orientation;
    }

    public string getPieceName() { return pieceName; }
    public int getPieceX() { return x; }
    public int getPieceY() { return y; }
    public string getPieceOrientation() { return orientation; }

    public void setPieceName(string pieceName) { this.pieceName = pieceName; }
    public void setPieceRow(int x) { this.x = x; }
    public void setPiecey(int y) { this.y = y; }
    public void setPieceOrientation(string orientation) { this.orientation = orientation; }

    public void setNullState()
    {
        // TODO: Need to rethink about default orientation when no piece is there for now it is -1
        pieceName = "";
        x = -1;
        y = -1;
        orientation = "";
    }

    public void updatePieceState(string pieceName, int x, int y, string orientation)
    {
        this.pieceName = pieceName;
        this.x = x;
        this.y = y;
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
            if ((a.x == b.x) && (a.y == b.y) && a.getPieceOrientation().Equals(b.getPieceOrientation()))
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
            if ((a.x == this.x) && (a.y == this.y) && a.getPieceOrientation().Equals(this.getPieceOrientation()))
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
                if (rotation < 0)
                    rotation = -rotation;
                finalOrientation = NumtoOrientationMirror[rotation];
            }
            else
            {
                rotation = rotation % 4;
                if (rotation < 0)
                    rotation = -rotation;
                finalOrientation = NumtoOrientationPlayer[rotation];
            }

            setPieceOrientation(finalOrientation);
            return true;
        }
        else if (action.Contains("Move"))
        {
            int x1 = x, y1 = y;
            if (action.Contains("Up"))
                y1++;
            else if (action.Contains("Down"))
                y1--;
            else if (action.Contains("Left"))
                x1--;
            else if (action.Contains("Right"))
                x1++;

            if (x1 >= 4 || x1 < 0 || y1 >= 4 || y1 < 0)
                return false;
            else if (Board[x1, y1].getPieceState() != null)
                return false;

            else
            {
                Board[x, y].setPieceState(null);

                x = x1;
                y = y1;
                Board[x, y].setPieceState(this);
                
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
        { "right", 1},
        { "down", 2},
        { "left", 3},
        { "+1", 0},
        { "-1", 1},
        };

    Dictionary<int, string> NumtoOrientationPlayer = new Dictionary<int, string>()
    {
        { 0,"up" },
        { 1, "right"},
        { 2, "down"},
        { 3, "left"},

    };
    Dictionary<int, string> NumtoOrientationMirror = new Dictionary<int, string>()
    {
        { 0, "+1" },
        { 1, "-1"},

    };


}
