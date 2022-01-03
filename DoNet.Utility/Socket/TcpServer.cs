// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Socket.TcpServer
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DoNet.Utility.Socket
{
  internal class TcpServer
  {
    private static readonly byte[] buffer = new byte[1024];

    private static void Main(string[] args)
    {
      try
      {
        IPEndPoint localEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9110);
        System.Net.Sockets.Socket parameter = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        parameter.Bind((EndPoint) localEP);
        parameter.Listen(10);
        Console.WriteLine("start to listen...");
        new Thread(new ParameterizedThreadStart(TcpServer.ListenClientConnect)).Start((object) parameter);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
      Console.ReadKey();
    }

    private static void ListenClientConnect(object listenSocket)
    {
      System.Net.Sockets.Socket socket = (System.Net.Sockets.Socket) listenSocket;
      while (true)
      {
        System.Net.Sockets.Socket parameter = socket.Accept();
        parameter.Send(Encoding.UTF8.GetBytes("hey guy..."));
        new Thread(new ParameterizedThreadStart(TcpServer.ReceiveMessage)).Start((object) parameter);
      }
    }

    private static void ReceiveMessage(object transferSocket)
    {
      System.Net.Sockets.Socket socket = (System.Net.Sockets.Socket) transferSocket;
      while (true)
      {
        try
        {
          int count = socket.Receive(TcpServer.buffer);
          if (count == 0)
          {
            Console.WriteLine("client " + (object) socket.RemoteEndPoint + " : disconnect...");
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            break;
          }
          Console.WriteLine("client " + (object) socket.RemoteEndPoint + " : " + Encoding.UTF8.GetString(TcpServer.buffer, 0, count));
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
          socket.Shutdown(SocketShutdown.Both);
          socket.Close();
        }
      }
    }
  }
}
