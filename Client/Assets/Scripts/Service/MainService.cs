using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TouchSocket.Core.ByteManager;
using TouchSocket.Core.Config;
using TouchSocket.Core.Serialization;
using TouchSocket.Sockets;
using UnityEngine;
using Protocol;



public class MainService : TS_Singleton<MainService>
{
    public TcpClient tcpClient;

    public void ServiceInit() {
        LoginService.Instance.Init();

    }

    public void Init()
    {
        ServiceInit();

        tcpClient = new TcpClient();
        tcpClient.Connected += (client, e) => { Debug.Log("成功连接服务器"); };
        tcpClient.Disconnected += (client, e) => { Debug.Log("已从服务器断开"); };


        tcpClient.Received += (client, byteBlock, requestInfo) =>
        {
            TS_MessageDistribute<TcpClient>.Instance.Distribute(client, byteBlock, requestInfo);
        };

        //声明配置
        TouchSocketConfig config = new TouchSocketConfig();
        config.SetRemoteIPHost(new IPHost("127.0.0.1:7789"))
            .UsePlugin()
            .SetBufferLength(1024 * 10)
            .SetDataHandlingAdapter(() => { return new FixedHeaderPackageAdapter(); });

        //载入配置
        tcpClient.Setup(config);
        tcpClient.Connect();

    }

    public void Update()
    {
        TS_MessageDistribute<TcpClient>.Instance.Queue_Msg();
    }

    public void OnDestroy()
    {
        if (tcpClient != null)
        {
            tcpClient.Dispose();
        }
        
    }

}
