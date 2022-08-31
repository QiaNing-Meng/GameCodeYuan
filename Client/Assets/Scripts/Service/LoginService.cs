using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
using TouchSocket.Sockets;
using System;

public class LoginService : TS_Singleton<LoginService>
{
    public LoginService()
    {
        TS_MessageDistribute<TcpClient>.Instance.Subscribe<LoginResponse>(OnLogin);
    }
    public void Init()
    {

    }

    public void SendLoginReq(TS_Message message)
    {
        TS_MessageSend.Send(GameStart.Instance._MainService.tcpClient, message);
    }
    private void OnLogin(TcpClient sender, LoginResponse message)
    {
        Debug.Log("LoginService:::" + message.reslut);

        LoginManager.Instance.OnLogin(sender, message);
    }
}
