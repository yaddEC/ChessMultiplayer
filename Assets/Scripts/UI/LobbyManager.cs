using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Analytics;

public class LobbyManager : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;

    [SerializeField]
    GameObject MainMenu;
    [SerializeField]
    GameObject ClientMenu;

    public TMP_InputField inputFieldIP;
    public TMP_InputField inputFieldPORT;
    public Button sendButton;

    void Start()
    {
        inputFieldIP.onValueChanged.AddListener(OnInputValueChangedForIP);
        inputFieldPORT.onValueChanged.AddListener(OnInputValueChangedForPORT);
    }

    void OnInputValueChangedForIP(string newText)
    {
        string filteredText = string.Empty;

        foreach (char character in newText)
        {
            if (char.IsDigit(character) || character == '.')
            {
                filteredText += character;
            }
        }

        // Mise à jour du texte filtré dans le champ de texte TMP.
        inputFieldIP.text = filteredText;
    }

    void OnInputValueChangedForPORT(string newText)
    {
        string filteredText = string.Empty;

        foreach (char character in newText)
        {
            if (char.IsDigit(character))
            {
                filteredText += character;
            }
        }

        // Mise à jour du texte filtré dans le champ de texte TMP.
        inputFieldPORT.text = filteredText;
    }

    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SwitchSceneHoster(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        gameManager.IsHoster = true;
        gameManager.IsClient = false;
    }

    public void SwitchSceneClient(string sceneName)
    {
        string adressIP = inputFieldIP.text;
        string port = inputFieldPORT.text;
        int result;
        int.TryParse(port, out result);

        if (gameManager.server.serverHost == adressIP && gameManager.server.serverPort == result)
        {
            SceneManager.LoadScene(sceneName);
            gameManager.IsHoster = false;
            gameManager.IsClient = true;
        }
        
    }

    public void ClientScreen()
    {
        MainMenu.SetActive(false);
        ClientMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void Back()
    {
        MainMenu.SetActive(true);
        ClientMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void Quit()
    {
        Application.Quit();
    }    
}
