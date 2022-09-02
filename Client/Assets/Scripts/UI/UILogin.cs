using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Protocol;

public class UILogin : UIBase
{
    public GameObject enter_registerPlane;
    bool planeState;

    public TMP_InputField account;
    public TMP_InputField password;

    public TextMeshProUGUI planeStateText;

    public override void Init(params object[] obj)
    {
        base.Init(obj);
    }

    void Start()
    {
        enter_registerPlane.SetActive(false);
    }

    #region OnClick
    public void OnClickEnterBtn()
    {
        enter_registerPlane.SetActive(true);
        planeStateText.text = "进入游戏";
        planeState = true;
        account.text = string.Empty;
        password.text = string.Empty;

    }

    public void OnClickRegisterBtn()
    {
        enter_registerPlane.SetActive(true);
        planeStateText.text = "注册";
        planeState = false;
        account.text = string.Empty;
        password.text = string.Empty;

    }


    public void OnClickColsePlaneBtn()
    {
        enter_registerPlane.SetActive(false);
    }

    public void OnClickYesBtn()
    {
        if (account.text == string.Empty)
        {
            GameStart.Instance._UIManager.Show<UIMessageBox>().Init("请输入账号");
            return;
        }
        if (password.text == string.Empty)
        {
            GameStart.Instance._UIManager.Show<UIMessageBox>().Init("请输入密码");
            return;
        }

        if (planeState)
        {
            GameStart.Instance._UIManager.Show<UIMessageBox>().Init("欢迎回来");
        }
        else
        {
            GameStart.Instance._UIManager.Show<UIMessageBox>().Init("注册未开启。。");
            return;
        }

        TS_Message message = new TS_Message();
        message.MessageRequest = new MessageRequest();
        message.MessageRequest.LoginRequest = new LoginRequest();
        message.MessageRequest.LoginRequest.account = int.Parse(account.text) ;
        message.MessageRequest.LoginRequest.password = int.Parse(password.text);

        LoginManager.Instance.SendLoginReq(message);


        this.Close();
    }



    public void OnClickSettingBtn()
    {
        var uiLogin = GameStart.Instance._UIManager.Show<UIMessageBox>();
        uiLogin.Init("设置系统暂未开发");
    }
    public void OnClickExiteBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
    }
    #endregion
}
