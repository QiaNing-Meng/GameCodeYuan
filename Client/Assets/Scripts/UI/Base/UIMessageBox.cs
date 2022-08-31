using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UIMessageBox : UIBase
{
    public TextMeshProUGUI message;
    public TextMeshProUGUI title;
    public Button buttonYes;
    public Button buttonNo;

    public TextMeshProUGUI buttonYesTitle;
    public TextMeshProUGUI buttonNoTitle;

    public UnityAction OnYes;
    public UnityAction OnNo;


    void Start()
    {
        //this.transform.localPosition =new Vector3(0, 700, 0);
    }

    public void Init(string message, string title="",string btnOK = "", string btnCancel = "")
    {
        this.message.text = message;
        this.title.text = title;

        if (!string.IsNullOrEmpty(btnOK)) this.buttonYesTitle.text = btnOK;
        if (!string.IsNullOrEmpty(btnCancel)) this.buttonNoTitle.text = btnCancel;

        this.buttonYes.onClick.AddListener(OnClickYes);
        this.buttonNo.onClick.AddListener(OnClickNo);

    }

    void OnClickYes()
    {
        if (this.OnYes != null)
            this.OnYes();
        else
            this.Close();
    }

    void OnClickNo()
    {
        if (this.OnNo != null)
            this.OnNo();
        else
            this.Close();
    }
}
