using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Analytics;
using System.Net;
using System.Linq;
using UnityEngine.SocialPlatforms;

public class LobbyManager : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;

    [SerializeField]
    GameObject MainMenu;
    [SerializeField]
    GameObject ClientMenu;
    [SerializeField]
    GameObject ServerMenu;

    public TMP_InputField inputFieldClientIP;
    public TMP_InputField inputFieldClientPORT;
    public TMP_InputField inputFieldServerIP;
    public TMP_InputField inputFieldServerPORT;
    public TMP_Text showIP;

    public string clientAdressIP;
    public int clientAdressPort;
    public int serverAdressPort;

    void Start()
    {
        inputFieldClientIP.onValueChanged.AddListener(OnInputValueChangedForIP);
        inputFieldClientPORT.onValueChanged.AddListener(OnInputValueChangedForPORT);
        GetLocalIPv4();
        inputFieldServerIP.interactable = false;
    }

    public void GetLocalIPv4()
    {
        string LocalIP = Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();

        showIP.text = LocalIP;
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

        inputFieldClientIP.text = filteredText;
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

        inputFieldClientPORT.text = filteredText;
    }

    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SwitchSceneHoster(string sceneName)
    {
        string serverPort = inputFieldServerPORT.text;
        int.TryParse(serverPort, out serverAdressPort);

        SceneManager.LoadScene(sceneName);
        gameManager.IsHoster = true;
        gameManager.IsClient = false;
    }

    public void SwitchSceneClient(string sceneName)
    {
        clientAdressIP = inputFieldClientIP.text;
        string clientPort = inputFieldClientPORT.text;
        int.TryParse(clientPort, out clientAdressPort);

        SceneManager.LoadScene(sceneName);
        gameManager.IsHoster = false;
        gameManager.IsClient = true;
    }

    public void ServerScreen()
    {
        MainMenu.SetActive(false);
        ServerMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
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
        ServerMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void Quit()
    {
        Application.Quit();
    }    
}
