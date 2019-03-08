using System.Collections.Generic;

public class GameState
{
    public CellState[,] allBoardCells;
    public PieceState lastMirrorMoved;
    public bool currentTurn; // Player 1(True) | Player 2(False)
    public List<PieceState> allPieces;
    public int numOfMoves;
    public List<GameState> availableGameStates;
    public PieceState winningPiece;
    public bool winningState;

    public GameState()
    {
        allBoardCells = new CellState[4, 4];
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                // TODO: Need to rethink about default orientation when no piece is there for now it is -1
                allBoardCells[row, col] = new CellState(null);
            }
        }

        allPieces = new List<PieceState>();

        CellState tempCell;
        PieceState tempPiece;

        // Player 1
        tempPiece = new PieceState("P1", 0, 0, "up");
        allPieces.Add(tempPiece);
        allBoardCells[0, 0].setPieceState(tempPiece);

        //Player 2
        tempPiece = new PieceState("P2", 3, 3, "down");
        allPieces.Add(tempPiece);
        allBoardCells[3, 3].setPieceState(tempPiece);

        // Mirror 1
        tempPiece = new PieceState("M1", 2, 1, "-1");
        allPieces.Add(tempPiece);
        allBoardCells[2, 1].setPieceState(tempPiece);

        // Mirror 2
        tempPiece = new PieceState("M2", 2, 2, "+1");
        allPieces.Add(tempPiece);
        allBoardCells[2, 2].setPieceState(tempPiece);

        // Mirror 3
        tempPiece = new PieceState("M3", 1, 1, "+1");
        allPieces.Add(tempPiece);
        allBoardCells[1, 1].setPieceState(tempPiece);

        // Mirror 4
        tempPiece = new PieceState("M4", 1, 2, "-1");
        allPieces.Add(tempPiece);
        allBoardCells[1, 2].setPieceState(tempPiece);

        currentTurn = true;
        lastMirrorMoved = null;
        numOfMoves = 0;
        winningPiece = null;//TODO : is this really needed??
        winningState = false;
    }

    public GameState getGameState() { return this; }

    public GameState(GameState oldGameState)
    {
        allPieces = new List<PieceState>(oldGameState.allPieces);
        allBoardCells = new CellState[4, 4];
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                // TODO: Need to rethink about default orientation when no piece is there for now it is -1
                allBoardCells[row, col] = new CellState(new PieceState("", row, col, ""));
            }
        }

        foreach (PieceState piece in allPieces)
        {
            int row = piece.getPieceRow();
            int col = piece.getPieceCol();
            allBoardCells[row, col].setPieceState(piece);
        }

        currentTurn = oldGameState.currentTurn;
        lastMirrorMoved = oldGameState.lastMirrorMoved;
        numOfMoves = oldGameState.numOfMoves;
        winningPiece = oldGameState.winningPiece;
        winningState = oldGameState.winningState;
    }


    public bool isCurrentTurn() { return currentTurn; }
    public void changeCurrentTurn() { this.currentTurn = !this.currentTurn; }

    public PieceState getLastMirrorMoved() { return lastMirrorMoved; }
    public void setLastMirrorMoved(PieceState piece) { this.lastMirrorMoved = piece; }

    public void updateAllBoardCells(CellState[,] allBoardCells)
    {
        this.allBoardCells = allBoardCells;
    }

    public CellState[,] getAllBoardCells() { return allBoardCells; }

    public CellState getCellState(int r, int c) { return allBoardCells[r, c]; }

    public void incrementNumOfMoves() { this.numOfMoves = this.numOfMoves + 1; }

    // methods for winning state
    public void gameWon(PieceState piece)
    {
        winningState = true;
        winningPiece = piece;
    }

    public bool isWon() { return winningState; }

    public bool ReachedWinningState()
    {

        return true;
    }

    public List<GameState> GetAvailableChildStates(GameState currentGameState)
    {
        List<GameState> availableMoveStates = new List<GameState>();

        if (currentGameState.isWon())
            return null;

        foreach (PieceState piece in currentGameState.allPieces)
        {

            //The P1 and P2 are skipped if it was thier turns in the previous gamestate, i.e , currentGameState
            if (piece.ComparePieces(currentGameState.lastMirrorMoved) ||
                (piece.Equals("P1") && currentGameState.isCurrentTurn()) || (piece.Equals("P2") && !currentGameState.isCurrentTurn()))
                continue;

            //rotations are always possible except for last mirror moved and opponentPlayer
            foreach(string action in ActionList)
            {
                GameState gameStateOnAction = new GameState(currentGameState);
                //the extra function calling here is neccessary so as to make sure that only the newly created gamestate is isoolated and affected 
                bool actionDone = gameStateOnAction.GetGameStateOnAction(action,  allBoardCells[piece.getPieceRow(), piece.getPieceCol()].getPieceState() );

                if(actionDone)
                {
                    availableMoveStates.Add(gameStateOnAction);
                }

            }
            //incomplete



        }


        return availableMoveStates;
    }
    

    //incomplete check TODO
    public bool GetGameStateOnAction(string action, PieceState pieceToBeChanged)
    {
        //no action can be taken if the game is won, or the piece to be moved is locked or u are trying to change the enemy piece
        if (isWon() || pieceToBeChanged.ComparePieces(lastMirrorMoved)
            || (pieceToBeChanged.Equals("P1") && isCurrentTurn()) || (pieceToBeChanged.Equals("P2") && !isCurrentTurn()))
            return false;


        foreach (PieceState piece in allPieces)
        {
            if (!piece.ComparePieces(piece, pieceToBeChanged))
                continue;

            bool actionDone = pieceToBeChanged.ApplyAction(action, allBoardCells);

            //If it's not possible for the action to be completed
            if (!actionDone)
                return false;

            if (pieceToBeChanged.getPieceName().Contains("M"))
                setLastMirrorMoved(pieceToBeChanged);
            else
                setLastMirrorMoved(null);

            numOfMoves++;
            changeCurrentTurn();
            // TODO check if this is a winning state
            // TODO if yes, then set winning piece


            return true;

        }

        return false;
    }

    public bool HasWon()
    {


    // check this link: https://gist.github.com/abiduzz420/856580f0311c3e3fb16e66df3b66b2c8


        return false;
    }


    List<string> ActionList = new List<string>()
    {
        "Shoot",
        "Rotate Left",
        "Rotate Right",
        "Move Up",
        "Move Down",
        "Move Left",
        "Move Right",
    };


    public override string ToString()
    {
        string gameState = "";

        if (currentTurn)
            gameState += "P1's turn \n";
        else
            gameState += "P2's turn \n";

        foreach (PieceState piece in allPieces)
        {
            gameState += piece.getPieceName() +" ";
            gameState += piece.getPieceOrientation() + " ";
            gameState += "(" + piece.getPieceRow() + "," + piece.getPieceCol() + ") \n";
           
        }

       
        if (lastMirrorMoved != null)
            gameState += "Last mirror = " + lastMirrorMoved.getPieceName() + "\n\n";
        else
            gameState += "Last mirror = None\n\n";

        return gameState;
    }






}
