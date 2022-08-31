using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
using TouchSocket.Sockets;
using System;

public class LoginManager : TS_Singleton<LoginManager>
{
    public void SendLoginReq(TS_Message message)
    {
        LoginService.Instance.SendLoginReq(message);
    }

    internal void OnLogin(TcpClient sender, LoginResponse message)
    {
        Debug.Log("LoginManager:::" + message.reslut);

        if (message.reslut)
        {

            GameStart.Instance._virtualCamera.gameObject.SetActive(false);
            GameStart.Instance._freeLook.gameObject.SetActive(true);
            //GameStart.Instance._SceneManager.LoadScene("MainCity");

        }
        else
        {
            GameStart.Instance._UIManager.Show<UIMessageBox>().Init("’À∫≈√‹¬Î≤ª’˝»∑");

            GameStart.Instance._UIManager.Show<UILogin>();
        }

        
    }
}
