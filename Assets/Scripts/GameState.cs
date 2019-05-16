using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public CellState[,] allBoardCells;
    public PieceState lastMirrorMoved;
    public bool currentTurn; // Player 1(True) | Player 2(False)
    public List<PieceState> allPieces;
    public int numOfMoves;
   // public List<GameState> availableGameStates;
    public PieceState winningPiece;
    public bool winningPlayer;
    public GameState parentGameState;
    public int value = 0 ;

    public GameState()
    {
        allBoardCells = new CellState[4, 4];
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                allBoardCells[row, col] = new CellState(null);
            }
        }

        allPieces = new List<PieceState>();

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
        winningPlayer = false;
        parentGameState = null;
    }

    public GameState getGameState() { return this; }

    public GameState(GameState oldGameState)
    {
        allPieces = new List<PieceState>();
        foreach(PieceState piece in oldGameState.allPieces)
        {
            allPieces.Add(new PieceState(piece));
        }
        allBoardCells = new CellState[4, 4];
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                allBoardCells[row, col] = new CellState(null);
            }
        }

        foreach (PieceState piece in allPieces)
        {
            int row = piece.getPieceX();
            int col = piece.getPieceY();
            allBoardCells[row, col].setPieceState(piece);
        }

        currentTurn = oldGameState.currentTurn;
        lastMirrorMoved = oldGameState.lastMirrorMoved;
        numOfMoves = oldGameState.numOfMoves;
        if (winningPiece != null)
        {
            winningPiece = new PieceState(oldGameState.winningPiece);
        }
        winningPlayer= oldGameState.winningPlayer;
        //parentGameState = oldGameState.parentGameState;
    }


    public bool GetCurrentTurn() { return currentTurn; }
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
        winningPlayer = true;
        winningPiece = piece;
    }

    //public bool isWon() { return winningPlayer; }

    public bool ReachedWinningState() // TODO remove this
    {

        return true;
    }

    public List<GameState> GetAvailableChildStates(GameState gameState ) //TODO need to test this function
    {
      //  System.Diagnostics.Stopwatch childStates = new System.Diagnostics.Stopwatch();
      //  childStates.Start();
        GameState currentGameState = new GameState(gameState);

        List<GameState> availableMoveStates = new List<GameState>();

        if (currentGameState.winningPiece != null)
            return null;

        foreach (PieceState piece in currentGameState.allPieces)
        {

            //The P1 and P2 are skipped if it was thier turns in the previous gamestate, i.e , currentGameState
            if (piece.ComparePieces(currentGameState.lastMirrorMoved) ||
                (piece.Equals("P1") && currentGameState.GetCurrentTurn()) || (piece.Equals("P2") && !currentGameState.GetCurrentTurn()))
                continue;

            //rotations are always possible except for last mirror moved and opponentPlayer
            foreach(string action in ActionList)
            {
                //Part for optimization.. not perfect but will do for now
                if (action.Equals("Rotate Right") && piece.getPieceName().Contains("M"))
                    continue;
                if(action.Contains("Move"))
                {
                    if(action.Contains("Up"))
                    {
                        if (piece.getPieceY() == 3 || currentGameState.allBoardCells[piece.getPieceX(), piece.getPieceY() + 1].getPieceState() != null)
                            continue;
                    }
                    else if (action.Contains("Down"))
                    {
                        if (piece.getPieceY() == 0 || currentGameState.allBoardCells[piece.getPieceX(), piece.getPieceY() - 1].getPieceState() != null)
                            continue;
                    }
                    else if(action.Contains("Left"))
                    {
                        if (piece.getPieceX() == 0 || currentGameState.allBoardCells[piece.getPieceX() - 1, piece.getPieceY()].getPieceState() != null)
                            continue;
                    }
                    else if (action.Contains("Right"))
                    {
                        if (piece.getPieceX() == 3 || currentGameState.allBoardCells[piece.getPieceX() + 1, piece.getPieceY()].getPieceState() != null)
                            continue;
                    }


                }
                //end of optimization attempt

                GameState gameStateOnAction = new GameState(currentGameState);
                

                //the extra function calling here is neccessary so as to make sure that only the newly created gamestate is isoolated and affected 
                bool actionDone = gameStateOnAction.GetGameStateOnAction(action, gameStateOnAction.allBoardCells[piece.getPieceX(), piece.getPieceY()].getPieceState() );

                if(actionDone)
                {
                    //gameStateOnAction.parentGameState = this;
                    availableMoveStates.Add(gameStateOnAction);
                }

            }
            

            // TODO : double check if this is done..

        }
       // childStates.Stop();
       // Debug.Log("Getting ChildStates time " + childStates.Elapsed);

        if (availableMoveStates.Count == 0)
            return null;


        return availableMoveStates;
    }
    



    //incomplete check TODO
    public bool GetGameStateOnAction(string action, PieceState pieceToBeChanged)
    {
        if (winningPiece != null || pieceToBeChanged.ComparePieces(lastMirrorMoved))
            return false;
        if (pieceToBeChanged.getPieceName().Equals("P1")  || pieceToBeChanged.getPieceName().Equals("P2"))
        {
            if ((pieceToBeChanged.getPieceName().Equals("P1") && !GetCurrentTurn()) || (pieceToBeChanged.getPieceName().Equals("P2") && GetCurrentTurn()))
                return false;
        }
        //no action can be taken if the game is won, or the piece to be moved is locked or u are trying to change the enemy piece
        


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
            winningPiece = HasWon();
            if (winningPiece != null) {

                if(winningPiece.getPieceName().Equals("P1"))
                    winningPlayer = true;
                else if (winningPiece.getPieceName().Equals("P2"))
                    winningPlayer = false;

                return true;
            }
            else
                changeCurrentTurn();
            

            return true;

        }

        return false;
    }



    public PieceState HasWon()
    {
        int x, y;
        int initialDirection;
        int directionX = 0, directionY = 0;
        PieceState encounteredPiece = null;
        PieceState p1 = null, p2 = null;

        foreach (PieceState piece in allPieces)
        {
            if (piece.getPieceName().Equals("P1"))
                p1 = piece;
            if (piece.getPieceName().Equals("P2"))
                p2 = piece;
        }

        if (currentTurn)
        {
            x = p1.getPieceX();
            y = p1.getPieceY();
            initialDirection = OrientationToNum[p1.getPieceOrientation()];

        }
        else
        {
            x = p2.getPieceX();
            y = p2.getPieceY();
            initialDirection = OrientationToNum[p2.getPieceOrientation()];
        }


        CellState currentCell = null;
        // Convert the initial direction to a x and y increments
        switch (initialDirection)
        {
            case 0: directionX = 0; directionY = 1; break;
            case 1: directionX = 1; directionY = 0; break;
            case 2: directionX = 0; directionY = -1; break;
            case 3: directionX = -1; directionY = 0; break;
        }

        int X = x, Y = y;
        do
        {
            encounteredPiece = null;
            X = X + directionX;
            Y = Y + directionY;
            if (X >= 0 && Y >= 0 && X < 4 && Y < 4)
            {
                currentCell = allBoardCells[X, Y];
                encounteredPiece = allBoardCells[X, Y].getPieceState();
            }

            if (encounteredPiece != null)
            {

                if (encounteredPiece.getPieceOrientation().Equals("+1") || encounteredPiece.getPieceOrientation().Equals("-1"))
                {
                    int flipSign = 1;
                    if (encounteredPiece.getPieceOrientation().Equals("-1"))
                        flipSign = -1;
                    int tempX = directionX, tempY = directionY;
                    //update the new direction increment

                    directionX = flipSign * tempY;
                    directionY = flipSign * tempX;

                }
                else
                    break;
            }


        } while (X >= 0 && Y >= 0 && X < 4 && Y < 4);

        if (encounteredPiece != null)
        {
            if (encounteredPiece.ComparePieces(p1)) return p2;
            else return p1;
        }
        return null;
    }



    //This function assumes that the parameters are parent and the child gamestates
    // TODO need to test this
    public System.Tuple<PieceState, string> CompareGameStates(GameState parent, GameState child)
    {
        List<PieceState>
            pieceStatesA = parent.allPieces,
            pieceStatesB = child.allPieces;
            
        PieceState 
            changedPieceA = null,
            changedPieceB = null,
            p1 = null,
            p2 = null;
        string action = null;

        foreach (PieceState pieceA in pieceStatesA)
        {
            if ( p1 != null && pieceA.getPieceName().Equals("P1"))
                p1 = pieceA;
            if (p2 != null && pieceA.getPieceName().Equals("P2"))
                p2 = pieceA;

            foreach (PieceState pieceB in pieceStatesB)
            {
                //if they both have the same name then 
                if (pieceA.getPieceName().Equals(pieceB.getPieceName()))
                {
                    //if the pieces are different in any way then return that piece
                    if (!pieceB.ComparePieces(pieceA))
                    {
                        changedPieceA =  pieceA;
                        changedPieceB = pieceB;
                        break;
                    }
                }


            }
            if (changedPieceA != null)
                break;

        }

        //get the action Performed

        // shooting 

        if(changedPieceA == null)
        {
            action = "Shoot";
            if (parent.currentTurn)
                changedPieceA = p1;
            else
                changedPieceA = p2;

        }
        else
        {
            //Rotating
            if (!changedPieceA.getPieceOrientation().Equals(changedPieceB.getPieceOrientation()))
            {
                if (changedPieceA.getPieceName().Contains("M"))
                { action = "Rotate Left"; }
                else
                {
                    int orientationA = OrientationToNum[changedPieceA.getPieceOrientation()];
                    int orientationB = OrientationToNum[changedPieceB.getPieceOrientation()];

                    if (orientationA-orientationB == 1 || (orientationA == 0 && orientationB == 3))
                    {
                        action = "Rotate Left";
                    }
                    else if (orientationB - orientationA == 1 || (orientationB == 0 && orientationA == 3))
                    {
                        action = "Rotate Right"; 
                    }
                    else
                    {
                       Debug.LogError("Error in comparing gamestates about orientations");

                    }
                }
                
            }
            //Moving... check if thier positions are not the same
            else if (changedPieceA.getPieceX() != changedPieceB.getPieceX() || changedPieceA.getPieceY() != changedPieceB.getPieceY())
            {
                if (changedPieceA.getPieceX() < changedPieceB.getPieceX())
                    action = "Move Right";
                else if (changedPieceA.getPieceX() > changedPieceB.getPieceX())
                    action = "Move Left";
                else if (changedPieceA.getPieceY() < changedPieceB.getPieceY())
                    action = "Move Up";
                else if (changedPieceA.getPieceY() > changedPieceB.getPieceY())
                    action = "Move Down";
                else
                    Debug.LogError("Error in comparing gamestates about moving");

            }
            else
            {
                Debug.LogError("Error in comparing gamestates about not rotating,moving or even shooting");
            }
        }

        return System.Tuple.Create(changedPieceA, action);
    }


    //TDOO need to test this
    public System.Tuple<BasePiece, string> GameStateChangeToAction(GameState initialGameState, GameState finalGameState, List<BasePiece> allPieces)
    {
        System.Tuple<PieceState, string> change = null;
        BasePiece pieceToChange = null;
        string buttonName = "";

        change = CompareGameStates(initialGameState, finalGameState);

        buttonName = change.Item2;
        //get player 1 and 2
        //BasePiece p1 = null, p2 = null; //mirror = null, player = null;

        foreach (BasePiece basePiece in allPieces)
        {

            if (change.Item1 != null && change.Item1.getPieceName().Equals(basePiece.gameObject.name))
                pieceToChange = basePiece;
            else if(change.Item1 == null)
            {
                if (initialGameState.currentTurn && basePiece.name.Equals("P1"))
                    pieceToChange = basePiece;
                else if (!initialGameState.currentTurn && basePiece.name.Equals("P2"))
                    pieceToChange = basePiece;
            }

        }
        
        return System.Tuple.Create(pieceToChange, buttonName);
    }





    List<string> ActionList = new List<string>()
    {
       "Rotate Right",
       "Rotate Left",
       "Move Up",
       "Move Down",
       "Move Left",
       "Move Right",
          
        // "Shoot",
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
            gameState += "(" + piece.getPieceX() + "," + piece.getPieceY() + ") \n";
           
        }

       
        if (lastMirrorMoved != null)
            gameState += "Last mirror = " + lastMirrorMoved.getPieceName() + "\n\n";
        else
            gameState += "Last mirror = None\n\n";

        return gameState;
    }




    Dictionary<string, int> OrientationToNum = new Dictionary<string, int>() {
        { "up", 0},
        { "right", 1},
        { "down", 2},
        { "left", 3},
        { "+1", 0},
        { "-1", 1},
        };

}
