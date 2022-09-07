using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameStart : MonoSingleton<GameStart>
{
    public Transform MainUITransf;

    #region 临时
    public PlayerQin playerQin;
    public Transform enemy;
    #endregion

    #region Service
    public MainService _MainService;
    #endregion

    #region Manager
    public UIManager _UIManager;
    public SceneManager _SceneManager;
    public ResourcesManager _ResourcesManager;
    public DataManager _DataManager;
    public SkillManager _SkillManager;

    #endregion

    #region Cinemachine
    public Camera camera;
    public CinemachineVirtualCamera _virtualCamera;
    public CinemachineFreeLook _freeLook;
    #endregion


    #region 对象池
    public Transform BezierBulletEnqueueGo ;
    public BezierBullet bezierBulletPrefab;
    public Queue<BezierBullet> bullets = new Queue<BezierBullet>();
    public Transform BezierBulletFireGo ;


    public Transform FightingTextEnqueueGo;
    public FightingText FightingTextPrefab;
    public Queue<FightingText> FightingTexts = new Queue<FightingText>();
    #endregion
    protected override void OnStart()
    {
        base.OnStart();
        camera = Camera.main;

        //this._virtualCamera.gameObject.SetActive(true);
        //this._freeLook.gameObject.SetActive(false);
        this._freeLook.gameObject.SetActive(true);

        _MainService = MainService.Instance;


        _ResourcesManager = ResourcesManager.Instance;
        _DataManager = DataManager.Instance;
        _DataManager.Load();
        _UIManager = UIManager.Instance;
        _SceneManager = SceneManager.Instance;
        _SkillManager = SkillManager.Instance;



        _UIManager.Show<UILogin>();

        for (int i = 0; i < 20; i++)
        {
            FightingText fightingTxt = _ResourcesManager.ResourcesLoadInstantiate<FightingText>(
                    FightingTextPrefab, FightingTextEnqueueGo);
            fightingTxt.gameObject.SetActive(false);
            FightingTexts.Enqueue(fightingTxt);
        }

        Init();
    }

    // Update is called once per frame
    void Update()
    {
        _MainService.Update();

    }


    public void Init() {
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


    #region 虚拟按键输入检测
    public void TouchMoveInput(UnityEngine.Vector2 vector2)
    {
        playerQin.TouchMoveInput(vector2);
    }

    public void TouchJump(bool isJumping) {
        playerQin.TouchJump(isJumping);
    }


    public void TouchSpeedUp(bool isSpeedUp)
    {
        playerQin.TouchSpeedUp(isSpeedUp);

    }

    public void TouchEnterExitAttackMode() {
        playerQin.TouchEnterExitAttackMode();


    }

    public void TouchSkill1Attack()
    {
        playerQin.TouchSkill1Attack();

    }
    #endregion


    #region 战斗飘字
    public FightingText GetFightingText()
    {
        FightingText results;

        if (FightingTexts.Count > 0)
        {
            results = FightingTexts.Dequeue();
        }
        else
        {
            FightingText txt = _ResourcesManager.ResourcesLoadInstantiate<FightingText>(
                    FightingTextPrefab, FightingTextEnqueueGo);
            results = txt;
        }

        results.gameObject.SetActive(true);

        return results;
    }

    public void FightingTextEnqueue(FightingText txt)
    {
        txt.gameObject.SetActive(false);

        txt.gameObject.transform.position = Vector3.zero;

        FightingTexts.Enqueue(txt);
    }
    #endregion

    #region 技能相关
    public BezierBullet GetBezierBullet()
    {
        BezierBullet results;

        if (bullets.Count > 0)
        {
            results = bullets.Dequeue();
        }
        else
        {
            BezierBullet bullet = _ResourcesManager.ResourcesLoadInstantiate<BezierBullet>(
                bezierBulletPrefab, BezierBulletEnqueueGo);
            results = bullet;
        }


        //results.gameObject.transform.position = BezierBulletFireGo.position;
        results.gameObject.SetActive(true);


        return results;
    }

    public void BulletEnqueue(BezierBullet bullet)
    {
        bullet.gameObject.SetActive(false);

        bullet.gameObject.transform.position = Vector3.zero;

        bullets.Enqueue(bullet);
    }
    #endregion
}
