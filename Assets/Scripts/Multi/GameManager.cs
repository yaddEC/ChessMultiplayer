using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool IsHoster;
    public bool IsClient;

    [SerializeField]
    public Server server;

    [SerializeField]
    public Client client;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if(IsHoster)
        {
            server.enabled = true;
            DontDestroyOnLoad(server);
            IsHoster = false;
        }
        else if (IsClient)
        {
            client.enabled = true;
            DontDestroyOnLoad(client);
            IsClient = false;
        }
    }


}
