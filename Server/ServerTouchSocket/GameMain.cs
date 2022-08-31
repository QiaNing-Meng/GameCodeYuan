using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol;
using TouchSocket.Sockets;

namespace ServerTouchSocket
{
    public class GameMain:TS_Singleton<GameMain>
    {
        public Dictionary<int, int> players = new Dictionary<int, int>();

        public TcpService service;

        public GameMain()
        {
            //TS_MessageDistribute<SocketClient>.Instance.Subscribe<LoginRequest>();


        }

        public void Init()
        {
            players.Add(123, 123);
            players.Add(456, 456);
            players.Add(789, 789);

        }

        public void GetCliens() {
            var clients = service.GetClients();
        }
    }
}
