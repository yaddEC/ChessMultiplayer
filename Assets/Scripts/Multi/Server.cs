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
    LobbyManager    lobbyManager;
    BinaryFormatter bFormatter = new BinaryFormatter();
    IPAddress       serverAddress;
    Socket          serverSocket;
    Socket          clientSocket;

    private bool isLaunched = false;
    private bool foundClient = false;

    public event Action OnClientDisconnected;
    public int serverPort;

    public void LaunchServer()
    {
        string LocalIP = Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
        serverPort = lobbyManager.serverAdressPort;
        serverAddress = IPAddress.Parse(LocalIP);
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Bind(new IPEndPoint(serverAddress, serverPort));
        serverSocket.Listen(5);
        serverSocket.Blocking = false;
        isLaunched = true;
        FetchNewClient();
    }

    private IEnumerator MonitorClientConnection()
    {
        while (clientSocket != null && clientSocket.Connected)
        {
            yield return new WaitForSeconds(1.0f); 

            if (!IsClientConnected(clientSocket))
            {
                HandleClientDisconnect();
                break;
            }
        }
    }

    private bool IsClientConnected(Socket socket)
    {
        try
        {
            return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
        }
        catch (Exception) 
        { 
            return false;
        }
    }

    void FetchNewClient() 
    {
        while(!foundClient)
        { 
            try
            {
                clientSocket = serverSocket.Accept();
                clientSocket.Blocking = false;
                foundClient = true;

                StartCoroutine(MonitorClientConnection());

            }
            catch (Exception) {}

            try
            {
                byte[] buffer = new byte[1000];
                Stream stream = new MemoryStream(buffer);
                bFormatter.Serialize(stream, ChessGameManager.Instance.playerTeam);
                clientSocket.Send(buffer, buffer.Length, 0);
            }
            catch (Exception) {}
        }
    }

    private void HandleClientDisconnect()
    {
        if (OnClientDisconnected != null)
        {
            OnClientDisconnected();
        }
    }

    public void SendMove(ChessGameManager.Move move)
    {
        try
        {
            byte[] buffer = new byte[1000];
            Stream stream = new MemoryStream(buffer);
            bFormatter.Serialize(stream, move);
            clientSocket.Send(buffer, buffer.Length, 0);
        }
        catch (Exception) {}
    }

    public IEnumerator ReceivedMove()
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
            }
        }
        catch (Exception) {}

        yield return new WaitForSeconds(0.1f);
    }

    public void StopServer()
    {
        if (isLaunched)
        {
            isLaunched = false;

            if (clientSocket != null)
            {
                clientSocket.Close();
                clientSocket = null;
            }

            if (serverSocket != null)
            {
                serverSocket.Close();
                serverSocket = null;
            }
        }
    }

    private void OnDestroy()
    {
        StopServer();
    }
}