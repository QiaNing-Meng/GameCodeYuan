using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol;
using ServerTouchSocket.Manager;
using TouchSocket.Sockets;

namespace ServerTouchSocket.Service
{
    public class LoginService : TS_Singleton<LoginService>
    {
        public LoginService()
        {
            TS_MessageDistribute<SocketClient>.Instance.Subscribe<LoginRequest>(LoginReq);
        }

        public void Init()
        {

        }

        public void LoginReq(SocketClient sender, LoginRequest message)
        {
            Console.WriteLine("LoginService:::" + message.account+"::"+message.password);

            LoginManager.Instance.Login(sender, message);

        }

        public void SendLoginRes(SocketClient sender, TS_Message message)
        {
            TS_MessageSend.Send(sender, message);
        }

    }
}
