using UnityEngine;
using System.Collections.Generic;

public class ChessAI : MonoBehaviour
{
    #region Singleton
    static ChessAI instance = null;
    public static ChessAI Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<ChessAI>();
            return instance;
        }
    }
    #endregion

    #region AI

    public ChessGameManager.Move ComputeMove()
    {
        ChessGameManager.Move move;
        move.from = 0;
        move.to = 1;

        ChessGameManager.EChessTeam aiTeam = (ChessGameManager.Instance.playerTeam == ChessGameManager.EChessTeam.White) ? ChessGameManager.EChessTeam.Black : ChessGameManager.EChessTeam.White;
        List <ChessGameManager.Move> moves = new List<ChessGameManager.Move>(); ;
        ChessGameManager.Instance.GetBoardState().GetValidMoves(aiTeam, moves);

        if (moves.Count > 0)
            move = moves[Random.Range(0, moves.Count - 1)];

        return move;
    }

    #endregion
}
