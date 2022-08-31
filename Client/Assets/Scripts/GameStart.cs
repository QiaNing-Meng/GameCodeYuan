using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class GameStart : MonoSingleton<GameStart>
{
    public Transform MainUITransf;

    #region Service
    public MainService _MainService;
    #endregion

    #region Manager
    public UIManager _UIManager;
    public SceneManager _SceneManager;
    public ResourcesManager _ResourcesManager;
    public DataManager _DataManager;


    #endregion

    #region Cinemachine
    public CinemachineVirtualCamera _virtualCamera;
    public CinemachineFreeLook _freeLook;
    #endregion


    protected override void OnStart()
    {
        base.OnStart();
        this._virtualCamera.gameObject.SetActive(true);
        this._freeLook.gameObject.SetActive(false);

        _MainService = MainService.Instance;


        _DataManager = DataManager.Instance;
        _UIManager = UIManager.Instance;
        _SceneManager = SceneManager.Instance;
        _ResourcesManager = ResourcesManager.Instance;



        _UIManager.Show<UILogin>();



        Init();
    }

    // Update is called once per frame
    void Update()
    {
        _MainService.Update();

    }


    public void Init() {
        _DataManager.Load();

        _MainService.Init();

        //Cursor.lockState = CursorLockMode.Locked;
    }


    private void OnDestroy()
    {
        _MainService.OnDestroy();


    }

    #region startCoroutine
    public Coroutine GS_StartCoroutine(IEnumerator enumerator)
    {
        return StartCoroutine(enumerator);
    }
    public Coroutine GS_StartCoroutine(string methodName)
    {
        return StartCoroutine(methodName);
    }
    public Coroutine GS_StartCoroutine(string methodName, object value)
    {
        return StartCoroutine(methodName, value);
    }

    public void GS_StopCoroutine(Coroutine coroutine) {
        StopCoroutine(coroutine);
    }
    public void GS_StopCoroutine(string methodName)
    {
        StopCoroutine(methodName);
    }
    public void GS_StopCoroutine(IEnumerator enumerator)
    {
        StopCoroutine(enumerator);
    }

    #endregion


}
