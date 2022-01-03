// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Socket.TcpClient
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DoNet.Utility.Socket
{
  internal class TcpClient
  {
    private static void Main(string[] args)
    {
      byte[] numArray = new byte[1024];
      IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9110);
      System.Net.Sockets.Socket socket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      try
      {
        socket.Connect((EndPoint) remoteEP);
        Console.WriteLine("connect success...");
      }
      catch
      {
        Console.WriteLine("connect failure...");
        return;
      }
      try
      {
        int count = socket.Receive(numArray);
        Console.WriteLine("server : " + Encoding.UTF8.GetString(numArray, 0, count));
        while (true)
        {
          string s;
          do
          {
            s = Console.ReadLine();
          }
          while (s == null);
          if (!(s == "end"))
            socket.Send(Encoding.UTF8.GetBytes(s));
          else
            break;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
      finally
      {
        socket.Shutdown(SocketShutdown.Both);
        socket.Close();
      }
      Console.WriteLine("disconnect...");
      Console.ReadKey();
    }
  }
}
