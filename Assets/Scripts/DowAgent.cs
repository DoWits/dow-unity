using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEngine;

public class DowAgent : Agent
{
    GameState RLGame;
    // Start is called before the first frame update
    void Start()
    {
        RLGame = new GameState();
    }

    public override void AgentOnDone()
    {
        Debug.Log("Reward for game = " + GetCumulativeReward());
        AgentReset();
    }

    public override void AgentReset()
    {
        ResetReward();
        RLGame = new GameState();
    }

    // Update is called once per frame

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        bool playerBool = RLGame.GetCurrentTurn();
        int player = RLGame.GetCurrentTurn() ? 0 : 1;
        PieceState playerPiece = RLGame.allPieces[player];
        PieceState M1 = RLGame.allPieces[2];
        PieceState M2 = RLGame.allPieces[3];
        PieceState M3 = RLGame.allPieces[4];
        PieceState M4 = RLGame.allPieces[5];


        bool NextWin = false;
        List<GameState> next = RLGame.GetAvailableChildStates(RLGame);

        foreach (GameState nextState in next)
        {
            if (nextState.winningPiece != null && nextState.winningPlayer == RLGame.GetCurrentTurn())
            {
                NextWin = true;
                break;
            }
        }


        int action = (int)vectorAction[0];

        if (action < 6)
        {
            string actionStr = ActionList[action];
            RLGame.GetGameStateOnAction(actionStr, playerPiece);
        }
        else if (action < 11)
        {
            string actionStr = ActionList[action];
            RLGame.GetGameStateOnAction(actionStr, M1);

        }
        else if (action < 16)
        {
            string actionStr = ActionList[action];
            RLGame.GetGameStateOnAction(actionStr, M2);


        }
        else if (action < 21)
        {
            string actionStr = ActionList[action];
            RLGame.GetGameStateOnAction(actionStr, M3);

        }
        else if (action < 26)
        {
            string actionStr = ActionList[action];
            RLGame.GetGameStateOnAction(actionStr, M4);

        }
        else if (action == 26)
        {
            string actionStr = "Shoot";
            RLGame.GetGameStateOnAction(actionStr, playerPiece);

        }
        else
        {
            Debug.Log("Action out of bounds " + action);
        }

        if(NextWin && RLGame.winningPiece != null)
        {
            Debug.Log("Didn't take the Winning Move");
            AddReward(-0.3f);
        }

        if(RLGame.winningPiece != null && RLGame.winningPlayer == playerBool )
        {
            Debug.Log("Won by shooting AI");
            AddReward(1f);
            Done();

        }
        else if(RLGame.winningPiece != null && RLGame.winningPlayer != playerBool)
        {
            Debug.Log("Lost by shooting Self");
            AddReward(-1f);
            Done();

        }

        AIScript AI = new AIScript();// Ai makes their move
        RLGame = AI.ComputerMove(RLGame, "MvA", 1);

        if (RLGame.winningPiece != null && RLGame.winningPlayer == playerBool)
        {
            Debug.Log("Won by AI shooting itself");
            AddReward(1f);
            Done();

        }
        else if (RLGame.winningPiece != null && RLGame.winningPlayer != playerBool)
        {
            Debug.Log("Lost by shooting you");
            AddReward(-1f);
            Done();


        }
        NextWin = false;
        next = RLGame.GetAvailableChildStates(RLGame);

        foreach (GameState nextState in next)
        {
            if (nextState.winningPiece != null && nextState.winningPlayer == RLGame.GetCurrentTurn())
            {
                NextWin = true;
                break;
            }
        }
        if (NextWin)
        {
            Debug.Log("Can win next round");
            AddReward(0.3f);
        }

    }


    public override void CollectObservations()
    {
        int player = RLGame.GetCurrentTurn() ? 0 : 1;
        int opponent = RLGame.GetCurrentTurn() ? 1 : 0;
        PieceState playerPiece = RLGame.allPieces[player];
        PieceState opponentPiece = RLGame.allPieces[opponent];
        PieceState M1 = RLGame.allPieces[2];
        PieceState M2 = RLGame.allPieces[3];
        PieceState M3 = RLGame.allPieces[4];
        PieceState M4 = RLGame.allPieces[5];

        List<GameState> next = RLGame.GetAvailableChildStates(RLGame);
        bool NextWin = false;
        
        foreach(GameState nextState in next)
        {
            if (nextState.winningPiece != null && nextState.winningPlayer == RLGame.GetCurrentTurn())
            {
                NextWin = true;
                break;
            }
        }

        AddVectorObs(NextWin);

        float x = 0, y = 0, orientation = 0;
        x = (float)playerPiece.getPieceX() / 3f;
        y = (float)playerPiece.getPieceY() / 3f;
        orientation = ConvertOrientation(playerPiece.getPieceOrientation());
        AddVectorObs(x);
        AddVectorObs(y);
        AddVectorObs(orientation);

        x = (float)opponentPiece.getPieceX() / 3f;
        y = (float)opponentPiece.getPieceY() / 3f;
        orientation = ConvertOrientation(opponentPiece.getPieceOrientation());

        AddVectorObs(x);
        AddVectorObs(y);
        AddVectorObs(orientation);

        x = (float)M1.getPieceX() / 3f;
        y = (float)M1.getPieceY() / 3f;
        orientation = ConvertOrientation(M1.getPieceOrientation());

        AddVectorObs(x);
        AddVectorObs(y);
        AddVectorObs(orientation);

        x = (float)M2.getPieceX() / 3f;
        y = (float)M2.getPieceY() / 3f;
        orientation = ConvertOrientation(M2.getPieceOrientation());

        AddVectorObs(x);
        AddVectorObs(y);
        AddVectorObs(orientation);

        x = (float)M3.getPieceX() / 3f;
        y = (float)M3.getPieceY() / 3f;
        orientation = ConvertOrientation(M3.getPieceOrientation());

        AddVectorObs(x);
        AddVectorObs(y);
        AddVectorObs(orientation);

        x = (float)M4.getPieceX() / 3f;
        y = (float)M4.getPieceY() / 3f;
        orientation = ConvertOrientation(M4.getPieceOrientation());

        AddVectorObs(x);
        AddVectorObs(y);
        AddVectorObs(orientation);

        PieceState lastMirror = RLGame.getLastMirrorMoved();
        if(lastMirror != null)
        {

            x = (float)lastMirror.getPieceX() / 3f;
            y = (float)lastMirror.getPieceY() / 3f;
            orientation = ConvertOrientation(lastMirror.getPieceOrientation());

            AddVectorObs(true);
            AddVectorObs(x);
            AddVectorObs(y);
            AddVectorObs(orientation);
        }
        else
        {
            AddVectorObs(false);
            AddVectorObs(-1f);
            AddVectorObs(-1f);
            AddVectorObs(-1f);
        }
        int[] indices = GetMaskIndices(RLGame).ToArray();
        SetActionMask(0, indices);
    }

    public float ConvertOrientation(string orientation)
    {
        float res = -1;

        if (orientation.Equals("up"))
            res = 0;
        else if (orientation.Equals("right"))
            res = 1f / 5f;
        else if (orientation.Equals("down"))
            res = 2f / 5f;
        else if (orientation.Equals("left"))
            res = 3f / 5f;
        else if (orientation.Equals("+1"))
            res = 4f / 5f;
        else if (orientation.Equals("-1"))
            res = 5f / 5f;

        if (res == -1)
            Debug.Log("Error in getting the orientation");

        return res;

    }


    public List<int> GetMaskIndices(GameState gs)
    {
        List<int> indices = new List<int>();

        List<string> moves = new List<string>()
        { "Move Up", "Move Right", "Move Down", "Move Left" };


        GameState gameState = new GameState(gs);
        int player = RLGame.GetCurrentTurn() ? 0 : 1;
        PieceState playerPiece = RLGame.allPieces[player];
        PieceState M1 = RLGame.allPieces[2];
        PieceState M2 = RLGame.allPieces[3];
        PieceState M3 = RLGame.allPieces[4];
        PieceState M4 = RLGame.allPieces[5];

        int pieceNum = 0;
        
        foreach(string Move in moves)
        {
            gameState = new GameState(gs);
            PieceState piece = new PieceState(playerPiece);

            if (!piece.ApplyAction(Move, gameState.getAllBoardCells()))
                indices.Add(pieceNum);
            pieceNum++;
        }
        pieceNum = 6;
        gameState = new GameState(gs);

        if (gameState.getLastMirrorMoved() != M1 )
        {
            foreach (string Move in moves)
            {
                gameState = new GameState(gs);
                PieceState piece = new PieceState(M1);

                if (!piece.ApplyAction(Move, gameState.getAllBoardCells()))
                    indices.Add(pieceNum);
                pieceNum++;
            }

        }
        else
        {
            indices.Add(6);
            indices.Add(7);
            indices.Add(8);
            indices.Add(9);
            indices.Add(10);
        }
        pieceNum = 11;
        gameState = new GameState(gs);

        if (gameState.getLastMirrorMoved() != M2)
        {
            foreach (string Move in moves)
            {
                gameState = new GameState(gs);
                PieceState piece = new PieceState(M2);

                if (!piece.ApplyAction(Move, gameState.getAllBoardCells()))
                    indices.Add(pieceNum);
                pieceNum++;
            }

        }
        else
        {
            indices.Add(11);
            indices.Add(12);
            indices.Add(13);
            indices.Add(14);
            indices.Add(15);
        }
        pieceNum = 16;
        gameState = new GameState(gs);

        if (gameState.getLastMirrorMoved() != M3)
        {
            foreach (string Move in moves)
            {
                gameState = new GameState(gs);
                PieceState piece = new PieceState(M3);

                if (!piece.ApplyAction(Move, gameState.getAllBoardCells()))
                    indices.Add(pieceNum);
                pieceNum++;
            }

        }
        else
        {
            indices.Add(16);
            indices.Add(17);
            indices.Add(18);
            indices.Add(19);
            indices.Add(20);
        }

        pieceNum = 21;
        gameState = new GameState(gs);

        if (gameState.getLastMirrorMoved() != M4)
        {
            foreach (string Move in moves)
            {
                gameState = new GameState(gs);
                PieceState piece = new PieceState(M4);

                if (!piece.ApplyAction(Move, gameState.getAllBoardCells()))
                    indices.Add(pieceNum);
                pieceNum++;
            }

        }
        else
        {
            indices.Add(21);
            indices.Add(22);
            indices.Add(23);
            indices.Add(24);
            indices.Add(25);
        }






        return indices;
    }

    Dictionary<int, string> ActionList = new Dictionary<int, string>()
    {
        {0, "Move Up" },
        {1, "Move Right" },
        {2, "Move Down" },
        {3, "Move Left" },
        {4, "Rotate Left" },
        {5, "Rotate Right" },


    };
}
