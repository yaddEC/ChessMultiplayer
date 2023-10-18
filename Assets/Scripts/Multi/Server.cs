using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Server : MonoBehaviour
{
    BinaryFormatter bFormatter = new BinaryFormatter();
    IPAddress serverAddress;
    Socket serverSocket;
    Socket clientSocket;
    public string serverHost = "10.2.103.121";
    public int serverPort = 8000;

    private void Start()
    {
        serverAddress = IPAddress.Parse(serverHost);
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
            Debug.Log("send fish!");
        }
        catch (Exception e)
        {
        }
    }
}