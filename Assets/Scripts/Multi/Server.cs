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
using System.Runtime.Serialization;
using System.Collections;

public class Server : MonoBehaviour
{
    [SerializeField]
    LobbyManager lobbyManager;
    bool isLaunched = false;
    bool foundClient = false;
    bool moveReceived = false;
    bool isRecieving = false;
    BinaryFormatter bFormatter = new BinaryFormatter();
    IPAddress serverAddress;
    Socket serverSocket;
    Socket clientSocket;
    public int serverPort;

    private void Start()
    {

    }

    public void LaunchServer()
    {
        string LocalIP = Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
        serverPort = lobbyManager.serverAdressPort;
        serverAddress = IPAddress.Parse(LocalIP);
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Bind(new IPEndPoint(serverAddress, serverPort));
        serverSocket.Listen(5);
        Debug.Log("Server is listening for connections");
        serverSocket.Blocking = false;
        isLaunched = true;
        StartCoroutine(FetchNewClient());
    }



    IEnumerator FetchNewClient() 
    {
        while(!foundClient)
        { 
        try
        {
            clientSocket = serverSocket.Accept();
            clientSocket.Blocking = false;
            Debug.Log("Client connected!");
            foundClient = true;

        }
        catch (SocketException e)
        {
            Debug.Log(e);
        }

        try
        {
            //sending the team of the host
            byte[] buffer = new byte[1000];
            Stream stream = new MemoryStream(buffer);
            bFormatter.Serialize(stream, ChessGameManager.Instance.playerTeam);
            clientSocket.Send(buffer, buffer.Length, 0);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        yield return new WaitForSeconds(0.1f);
        }
    }

    public void SendMove(ChessGameManager.Move move)
    {
        try
        {
            moveReceived = false;
            isRecieving = false;
            byte[] buffer = new byte[1000];
            Stream stream = new MemoryStream(buffer);
            bFormatter.Serialize(stream, move);
            clientSocket.Send(buffer, buffer.Length, 0);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public IEnumerator ReceivedMove()
    {
        if (isRecieving) yield return null;
        else
        {
            isRecieving = true;
            while (!moveReceived)
         {
            try
            {
                byte[] buffer = new byte[1000];
                int bytesReceived = clientSocket.Receive(buffer);
                if (bytesReceived > 0)
                {
                    MemoryStream ms = new MemoryStream(buffer);
                    bFormatter.Binder = new PreMergeToMergedDeserializationBinder();
                    ChessGameManager.Move move = (ChessGameManager.Move)bFormatter.Deserialize(ms);

                    ChessGameManager.Instance.PlayTurn(move);
                    ChessGameManager.Instance.UpdatePieces();
                        moveReceived = true;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            yield return null;
         }
        }
    }
}