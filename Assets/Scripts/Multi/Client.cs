using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;
using UnityEngine;

public class Client : MonoBehaviour
{
    [SerializeField]
    LobbyManager lobbyManager;
    BinaryFormatter bFormatter = new BinaryFormatter();         
    IPAddress serverAddress;
    string ipServer;
    int serverPort;

    private void Start()
    {
        ipServer = lobbyManager.clientAdressIP;
        serverPort = lobbyManager.clientAdressPort;
        Debug.Log(ipServer);
        serverAddress = IPAddress.Parse(ipServer);
        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        clientSocket.Connect(new IPEndPoint(serverAddress, serverPort));
        Debug.Log("Connected to server!");

        Message serverMSG = new();
        serverMSG.Content = "AAAAAAAAAAAH YAAAAAAAAAANNNNNNN";
        byte[] buffer = new byte[1000];
        Stream stream = new MemoryStream(buffer);
        bFormatter.Serialize(stream, serverMSG);
        clientSocket.Send(buffer, buffer.Length, 0);

        byte[] buffer2 = new byte[1000];    
        int serverBytes = clientSocket.Receive(buffer2);
        MemoryStream ms = new MemoryStream(buffer2);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Binder = new PreMergeToMergedDeserializationBinder();
        Message message = (Message)formatter.Deserialize(ms);

        Debug.Log(message.Content);
    }

}