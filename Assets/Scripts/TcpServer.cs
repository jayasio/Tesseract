using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

[Serializable]
public class TcpServer
{
    #region tcp
    TcpListener listener;
    Thread thread;
    TcpClient client;
    string IP;
    int port;
    #endregion

    #region tcp state
    bool connected = false;
    public bool isConnected() => connected;
    public Queue<string> stream = new Queue<string>();
    #endregion

    #region tcp methods
    public void ServerStart(string IP, int port)
    {
        this.IP = IP;
        this.port = port;

        thread = new Thread(new ThreadStart(ServerListen));
        thread.IsBackground = true;
        thread.Start();

        connected = true;
    }

    public void ServerStop()
    {
        if (connected)
        {
            listener.Stop();
            thread.Abort();
            Debug.Log("Disconnected");
        }

        listener = null;
        thread = null;
        client = null;

        connected = false;
    }

    void ServerListen()
    {
        try
        {
            listener = new TcpListener(IPAddress.Parse(IP), port);
            listener.Start();

            Debug.Log("Server on IP: " + IP + " : : Listening for client on port: " + port);

            Byte[] bytes = new Byte[1024];

            while (true)
            {
                using (client = listener.AcceptTcpClient())
                {
                    Debug.Log("Connected to: " + client);
                    using (NetworkStream stream = client.GetStream())
                    {
                        int length;
                        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            var data = new byte[length];
                            Array.Copy(bytes, 0, data, 0, length);

                            string message = Encoding.ASCII.GetString(data);
                            Decode(message);
                        }
                    }
                }
            }
        }
        catch (SocketException SocketException)
        { Debug.Log("Socket exception " + SocketException.ToString()); }
    }
    #endregion

    void Decode(string message)
    {
        string[] separator = { "&&##@@" };
        string[] list = message.Split(separator, 20, StringSplitOptions.RemoveEmptyEntries);

        foreach (string s in list)
        {
            if (s[0] == '{' && s[s.Length - 1] == '}') stream.Enqueue(s);
            Debug.Log(s);
        }
    }
}

