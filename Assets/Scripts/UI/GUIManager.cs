using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{

    #region Singleton
    static GUIManager instance = null;
    public static GUIManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GUIManager>();
            return instance;
        }
    }
    #endregion

    Transform whiteToMoveTr = null;
    Transform blackToMoveTr = null;
    Text      whiteScoreText = null;
    Text      blackScoreText = null;

    void Awake()
    {
        whiteToMoveTr = transform.Find("PlayerTurnText");
        blackToMoveTr = transform.Find("OpponentTurnText");

        whiteToMoveTr.gameObject.SetActive(false);
        blackToMoveTr.gameObject.SetActive(false);

        whiteScoreText = transform.Find("WhiteScoreText").GetComponent<Text>();
        blackScoreText = transform.Find("BlackScoreText").GetComponent<Text>();

        ChessGameManager.Instance.OnPlayerTurn += DisplayTurn;
        ChessGameManager.Instance.OnScoreUpdated += UpdateScore;
    }
	
    void DisplayTurn(bool isWhiteMove)
    {
        whiteToMoveTr.gameObject.SetActive(isWhiteMove);
        blackToMoveTr.gameObject.SetActive(!isWhiteMove);
    }

    void UpdateScore(uint whiteScore, uint blackScore)
    {
        whiteScoreText.text = string.Format("White : {0}", whiteScore);
        blackScoreText.text = string.Format("Black : {0}", blackScore);
    }
}
