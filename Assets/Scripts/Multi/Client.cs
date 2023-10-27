using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using System.Collections;

public class Client : MonoBehaviour
{
    [SerializeField]
    LobbyManager lobbyManager;
    BinaryFormatter bFormatter = new BinaryFormatter();         
    IPAddress serverAddress;
    bool moveReceived = false;
    bool isRecieving = false;
    Socket clientSocket;
    string ipServer;
    int serverPort;

    private void Start()
    {


    }

    public void LaunchClient()
    {
        ipServer = lobbyManager.clientAdressIP;
        serverPort = lobbyManager.clientAdressPort;
        Debug.Log(ipServer);
        serverAddress = IPAddress.Parse(ipServer);
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        clientSocket.Connect(new IPEndPoint(serverAddress, serverPort));
        Debug.Log("Connected to server!");
    }

    public void SetTeam()
    {
        //setting the client team based on the host team
        byte[] buffer2 = new byte[1000];
        int serverBytes = clientSocket.Receive(buffer2);
        MemoryStream ms = new MemoryStream(buffer2);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Binder = new PreMergeToMergedDeserializationBinder();
        ChessGameManager.Instance.playerTeam = ((ChessGameManager.EChessTeam)formatter.Deserialize(ms) == ChessGameManager.EChessTeam.White) ? ChessGameManager.EChessTeam.Black : ChessGameManager.EChessTeam.White;
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

        }
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
                catch (SocketException s)
                {
                    Debug.Log(s);
                }
                catch (Exception e)
                {
                   ;
                    Debug.Log(e);
                }
                yield return new WaitForSeconds(0.1f);
      
        
    }



}