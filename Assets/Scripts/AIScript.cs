using System.Collections.Generic;
using UnityEngine;

public class AIScript
{
    int numOfStates = 0;

    // Start is called before the first frame update
    public List<GameState> AlphaBetaPrune(GameState gameState)
    {
        int alpha = -100000; // act like negative infinity
        int beta = 100000; // act like infinity
        int tempBeta;

        List<GameState> bestMoves;
        List<GameState> min;
        List<GameState> max;

        bestMoves = new List<GameState>();

        GameState temp = new GameState(gameState);
        if (temp.HasWon() != null)
        {
            temp.changeCurrentTurn();
            bestMoves.Add(temp);
            return bestMoves;
        }

        // Populate min ('non - AI' turn) level of alpha-beta prune tree
        min = gameState.GetAvailableChildStates(gameState);
        numOfStates += min.Count;

        //Do a quick check if the game can be won in the next move
        foreach(GameState gsMIN in min)
        {
            if (gsMIN.winningPiece != null && (gsMIN.GetCurrentTurn() == gsMIN.winningPlayer))
            {
                bestMoves.Clear();
                gsMIN.value = 1000000; // winning states are like infinities
                bestMoves.Add(gsMIN);
                return bestMoves;
            }
        }

        // Populate max branches one at a time (opponent turn)
        foreach (GameState gsMIN in min)
        {
            if (gsMIN.winningPiece != null && (gsMIN.GetCurrentTurn() == gsMIN.winningPlayer))
            {
                bestMoves.Clear();
                gsMIN.value = 1000000; // winning states are like infinities
                bestMoves.Add(gsMIN);
                return bestMoves;
            }
            else if (gsMIN.winningPiece != null)
                continue;

            max = gsMIN.GetAvailableChildStates(gsMIN);
            numOfStates += max.Count;

            beta = 100000; // acts like infinity
            // Calculate current max branch leaf values
            bool badMoveFlag = false;

            foreach (GameState gsMAX in max)
            {
                if (gsMAX.winningPiece != null && (gsMAX.GetCurrentTurn() == gsMAX.winningPlayer))
                {
                    badMoveFlag = true;
                }
                else if (gsMAX.winningPiece != null)
                    continue;


                tempBeta = HeuristicSum(gsMAX, gsMAX.GetCurrentTurn());

                if (beta > tempBeta)
                    beta = tempBeta;

                // Prune
                if (beta <= alpha)
                {
                    break;
                }
            }
            if (badMoveFlag)
                continue;

            gsMIN.value = beta;

            // Replace alpha if beta is better, and replace bestMoves List with game state
            if (beta > alpha)
            {
                alpha = beta;
               // bestMoves.Clear(); // TODO check if this line needs to be commented
                bestMoves.Add(gsMIN);
            }
            // If beta is an equally good choice, add game state to bestMoves
            else if (beta == alpha)
            {
                bestMoves.Add(gsMIN);
            }
        }

        return bestMoves;
    }

    public GameState ComputerMove(GameState gameState, string gameMode)
    {



        List<GameState> possibleMoves = AlphaBetaPrune(gameState);

        Debug.Log("Number of total children states = " + numOfStates+"\n Number of moves available = "+possibleMoves.Count);

        if(possibleMoves.Count == 0)
        {
            GameState temp = new GameState(gameState);
            temp.changeCurrentTurn();
            return temp;
        }

        List<GameState> moves = new List<GameState>() ;
        
        //Debug.Log("Total availabale GamesStates =  " + possibleMoves.Count + "\n");
        //Debug.Log("Available gameStates values = ");
        while(possibleMoves.Count > 0) // TODO see if this part works fine
        {
            int max = -10000000;
            GameState temp = null;
            foreach(GameState state in possibleMoves)
            {
                if(state.value > max)
                {
                    max = state.value;
                    temp = state;
                }
            }
            
            moves.Add(temp);
            possibleMoves.Remove(temp);

            //Debug.Log(max + " , ");
        }
        Debug.Log("Total no. of movees to choose from= " + moves.Count + "\nMinimum value of branches = " + moves[moves.Count - 1].value + "\nValue of branch chosen = " + moves[0].value);
        //Debug.Log("\n");
        //TODO insert logic for getting the easy/medium/hard choice
        for(int i = 0; i<moves.Count;i++)
        {
            if(i>0 && moves[i].value < moves[i-1].value)
                return moves[Random.Range(0, i - 1)];

        }

        return moves[Random.Range(0,moves.Count - 1)];
    }

    public int HeuristicSum(GameState gameState, bool player) // should it be a ratio?? or should it be a difference?
    {
        int resultSum = 0; //for now

        GameState currentGameState = new GameState(gameState);

        List<GameState> ourStates = currentGameState.GetAvailableChildStates(currentGameState);
        numOfStates += ourStates.Count;
        foreach (GameState our in ourStates)
        {
            if(our.winningPiece != null)
            {
                if ((player == our.winningPlayer))//TODO check if this is correct or the other way aound
                {
                    return 1500;
                }
            }
            else
            {
                List<GameState> thierStates = our.GetAvailableChildStates(our);
                numOfStates += thierStates.Count;
                foreach (GameState their in thierStates)
                {
                    if (their.winningPiece != null)
                    {
                        if ((player != their.winningPlayer))//TODO check if this is correct or the other way aound

                            resultSum -= 50;
                    }
                }
            }
        }



        return resultSum;
    }

  

}
