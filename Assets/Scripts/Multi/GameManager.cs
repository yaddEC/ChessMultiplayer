using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool IsHoster;

    [SerializeField]
    public Server server;

    [SerializeField]
    public Client client;

    static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GameManager>();
            return instance;
        }
    }
    private void Start()
    {
        server.OnClientDisconnected += OnClientDisconnected;
        client.OnServerDisconnected += OnServerDisconnected;
    }

    private void OnClientDisconnected()
    {
        SceneManager.LoadScene("Disconnected");
        Destroy(this.gameObject);
    }

    private void OnServerDisconnected()
    {
        SceneManager.LoadScene("Disconnected");
        Destroy(this.gameObject);
    }

    private void Update()
    {
        if(IsHoster)
        {
            server.enabled = true;
            DontDestroyOnLoad(server);
            IsHoster = true;
        }
        else 
        {
            client.enabled = true;
            DontDestroyOnLoad(client);
            IsHoster = false;
        }
    }


}
