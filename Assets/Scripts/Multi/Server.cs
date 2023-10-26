using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections.Generic;
using static ChessGameManager;
using UnityEngine.UIElements;

public class Server : MonoBehaviour
{
    [SerializeField]
    LobbyManager lobbyManager;
    BinaryFormatter bFormatter = new BinaryFormatter();
    IPAddress serverAddress;
    Socket serverSocket;
    Socket clientSocket;
    public int serverPort;

    private void Start()
    {
        string LocalIP = Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
        serverPort = lobbyManager.serverAdressPort;
        serverAddress = IPAddress.Parse(LocalIP);
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Bind(new IPEndPoint(serverAddress, serverPort));
        serverSocket.Listen(5);
        Debug.Log("Server is listening for connections");
        serverSocket.Blocking = false;
    }

    private void Update()   
    {
        try
        {
            clientSocket = serverSocket.Accept();
            clientSocket.Blocking = false;
            Debug.Log("Client connected!");

        }catch(SocketException e)
        {}

        if(clientSocket == null)
        {
            return;
        }

        try
        {
            Message serverMSG = new();
            serverMSG.Content = "This is a fish >><>";
            byte[] buffer = new byte[1000];
            Stream stream = new MemoryStream(buffer);
            bFormatter.Serialize(stream, serverMSG);
            clientSocket.Send(buffer, buffer.Length, 0);
        }
        catch (Exception e)
        {
        }
    }
}