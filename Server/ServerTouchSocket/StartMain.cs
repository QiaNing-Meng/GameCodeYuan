using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core.ByteManager;
using TouchSocket.Core.Config;
using TouchSocket.Core.Dependency;
using TouchSocket.Core.Log;
using TouchSocket.Core.Serialization;
using TouchSocket.Sockets;
using Protocol;
using ServerTouchSocket.Service;
using ServerTouchSocket.Manager;
using System.Threading;

namespace ServerTouchSocket
{
    public class StartMain:TS_Singleton<StartMain>
    {
        public static TcpService service;


        public static void Main(string[] args)
        {
            LoginService.Instance.Init();


            DataManager.Instance.Load();
            LoginManager.Instance.Init();


            GameMain.Instance.Init();


            service = new TcpService();

            service.Connecting += (client, e) => { Console.WriteLine($"有客户端正在连接"); };
            service.Connected += (client, e) => { Console.WriteLine($"有客户端连接"); };
            service.Disconnected += (client, e) => { Console.WriteLine($"有客户端断开连接"); };
            service.Received += (client, byteBlock, requestInfo) =>
            {
                TS_MessageDistribute<SocketClient>.Instance.Distribute(client, byteBlock, requestInfo);
            };

            service.Setup(new TouchSocketConfig()//载入配置     
                .SetListenIPHosts(new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) })//同时监听两个地址
                .SetMaxCount(10000)
                .SetThreadCount(10)
                .SetDataHandlingAdapter(() => { return new FixedHeaderPackageAdapter(); })//解决粘包
                .ConfigurePlugins(a =>
                {
                  
                })
                .ConfigureContainer(a =>
                {
                    a.SetSingletonLogger<ConsoleLogger>();//添加一个日志注入
                }))
                .Start();//启动


            service.Logger.Message("服务器已启动");



            Thread thread = new Thread(new ThreadStart(Update));
            thread.Start();
            Console.ReadLine();
        }


        public static void Update()
        {
            while (true)
            {
                TS_MessageDistribute<SocketClient>.Instance.Queue_Msg();
            }

        }

    }
}
