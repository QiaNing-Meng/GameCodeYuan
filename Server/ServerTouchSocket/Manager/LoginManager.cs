using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol;
using ServerTouchSocket.Service;
using TouchSocket.Sockets;

namespace ServerTouchSocket.Manager
{
    public class LoginManager : TS_Singleton<LoginManager>
    {
        public void Init()
        {

        }

        public void Login(SocketClient sender, LoginRequest message)
        {
            TS_Message messageClass = new TS_Message();
            messageClass.MessageResponse = new MessageResponse();
            messageClass.MessageResponse.LoginResponse = new LoginResponse();
            messageClass.MessageResponse.LoginResponse.reslut = false;

            if (GameMain.Instance.players.TryGetValue(message.account,out int val))
            {
                if (val == message.password)
                {
                    messageClass.MessageResponse.LoginResponse.reslut = true;
                }
            }


            Console.WriteLine("LoginManager:::" + messageClass.MessageResponse.LoginResponse.reslut);
            
            LoginService.Instance.SendLoginRes(sender, messageClass);
        }

    }
}
