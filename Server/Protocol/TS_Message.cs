using System;
using TouchSocket.Core.ByteManager;
using TouchSocket.Core.Config;
using TouchSocket.Core.Dependency;
using TouchSocket.Core.Log;
using TouchSocket.Core.Serialization;
using TouchSocket.Sockets;

namespace Protocol
{
    #region 发送方法
    public static class TS_MessageSend
    {
        public static void Send(SocketClient client, object obj)
        {
            byte[] data = SerializeConvert.FastBinarySerialize(obj);
            client.Send(data, 0, data.Length);
        }

        public static void Send(TcpClient client, object obj)
        {
            byte[] data = SerializeConvert.FastBinarySerialize(obj);
            client.Send(data, 0, data.Length);
        }
    }
    #endregion


    [Serializable]
    public class TS_Message
    {

        public MessageResponse MessageResponse { get; set; }
        public MessageRequest MessageRequest { get; set; }




        


        




    }

    #region response
    [Serializable]
    public class MessageResponse
    {
        public LoginResponse LoginResponse { get; set; }

    }
    [Serializable]
    public class LoginResponse
    {
        public bool reslut { get; set; }

    }
    #endregion



    #region request
    [Serializable]
    public class MessageRequest
    {
        public LoginRequest LoginRequest { get; set; }

    }
    [Serializable]
    public class LoginRequest
    {
        public int account { get; set; }
        public int password { get; set; }
    }
    #endregion

}


