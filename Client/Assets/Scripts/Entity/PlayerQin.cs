using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public enum PlayerPosture { 
    Crouch, //�¶�
    Stand,  //վ��
    Jumping, //�Ϳ�
    Landing,  //��½
    Falling     //����
}

public enum LocomotionState
{
    Idle,
    Walk,
    Run

}

public enum ArmState { 

    Normal,
    Aim
}

public class PlayerQin : MonoBehaviour
{
    Transform tr;
    Animator animator;
    CharacterController controller;
    Transform camTransform;

    Vector3 playerMovement=Vector3.zero;
    public CinemachineVirtualCamera Cinemachine;
    CinemachineFramingTransposer framingTransposer;

    float crouchSpeed = 1.5f;//�¶��ٶ�
    float walkSpeed = 2.5f;//�����ٶ�
    float runSpeed = 5.5f;//�����ٶ�

    Vector2 moveInput;//���߷�������
    Vector2 aimInput;//��׼��������

    bool isRuning;//�Ƿ���
    bool isCrouch;//�Ƿ��¶�
    bool isAiming;//�Ƿ���׼
    bool isJumping;//�Ƿ���Ծ
    bool currentAimState;//��ǰ���״̬(����׼ ���� ����״̬)
    

    #region �����̬��״̬��װ�����
    [HideInInspector]
    public PlayerPosture playerPosture = PlayerPosture.Stand;
    [HideInInspector]
    public LocomotionState locomotionState = LocomotionState.Idle;
    [HideInInspector]
    public ArmState armState = ArmState.Normal;

    #endregion


    #region ״̬����ϣֵ
    float crouchThreshold = 0f;
    float standThreshold = 1f;
    float midairThreshold = 2.1f;
    float landingThreshold = 1f;

    int postureHash;
    int moveSpeedHash;
    int turnSpeedHash;
    int verticalVelecityHash;
    int feetTweenHash;
    #endregion

    #region ��Ծ���
    public float gravity = -9.8f;//����
    public float verticalVelecity;//��ֱ������ٶ�


    public float maxHeight = 1.5f;//��Ծ�����߶�
    float fallMultiplier = 1.5f;//�½��ı���


    static readonly int CACHE_SIZE = 3;
    Vector3[] velCache = new Vector3[CACHE_SIZE];
    int currentChacheIndex = 0;
    Vector3 averageVel = Vector3.zero;


    bool isGrounded;
    float groundCheckOffset = 0.5f;

    float feetTween;

    float jumpCD = 0.5f;
    bool isLanding;//�Ƿ�cd����
    #endregion

    #region ����
    bool couldFall;//�Ƿ���Ե���

    float fallHeigt = 0.5f;//����߶ȣ����볬�����ֵ���ܵ���

    #endregion
    void Start()
    {
        framingTransposer = Cinemachine.GetCinemachineComponent<CinemachineFramingTransposer>();
        camTransform = Camera.main.transform;
        tr = transform;
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        postureHash = Animator.StringToHash("�����̬");
        moveSpeedHash = Animator.StringToHash("�ƶ��ٶ�");
        turnSpeedHash = Animator.StringToHash("ת���ٶ�");
        verticalVelecityHash = Animator.StringToHash("��ֱ�ٶ�");
        feetTweenHash = Animator.StringToHash("���ҽ�");



    }

    void Update()
    {
        CheckGround();
        SwitchPlayerStates();
        CaculateGravity();
        Jump();

        
        CaculateInputDirection();
        SetupAnimator();

        //ChangePlayerState();
        //PlayerMove();
        //PlayerRotate();
    }

    void CheckGround() {
        if (Physics.SphereCast(tr.position + (Vector3.up * groundCheckOffset), 
                                controller.radius,
                                Vector3.down,
                                out RaycastHit raycastHit,
                                groundCheckOffset-controller.radius+2*controller.skinWidth))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded=false;
            couldFall = !Physics.Raycast(tr.position, Vector3.down, fallHeigt);
        }
    
    }

    void CaculateGravity()
    {

        if (playerPosture != PlayerPosture.Jumping && playerPosture != PlayerPosture.Falling)
        {
            if (!isGrounded)
            {
                verticalVelecity += gravity *fallMultiplier *Time.deltaTime;
            }
            else 
            {
                verticalVelecity = gravity * Time.deltaTime;
            }

            
            return;
        }
        else
        {
            if (verticalVelecity <= 0 || !isJumping)
            {
                verticalVelecity += gravity * fallMultiplier * Time.deltaTime;
            }
            else
            {
                verticalVelecity += gravity * Time.deltaTime;
            }


        }

    }

    void Jump() {
        if (playerPosture == PlayerPosture.Stand && isJumping)
        {
            verticalVelecity = Mathf.Sqrt(-2 * gravity * maxHeight);
            feetTween = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime,1) ;
            feetTween = feetTween < 0.5f ? 1 : -1;//1�ҽ���ǰ -1�����ǰ

            if (locomotionState == LocomotionState.Run)
            {
                feetTween *= 3;
            }
            else if (locomotionState == LocomotionState.Walk) {
                feetTween *= 2;
            }
            else
            {
                feetTween *= Random.Range(0.5f, 1f);
            }

        }
    
    }

    

    void CaculateInputDirection() {
        //�������
        Vector3 camForwardPro = new Vector3(camTransform.forward.x, 0, camTransform.forward.z).normalized;

        playerMovement = camForwardPro * moveInput.y + camTransform.right * moveInput.x;
        playerMovement = tr.InverseTransformVector(playerMovement);
    }
    void SwitchPlayerStates()
    {
        if (!isGrounded)
        {
            if (verticalVelecity > 0)
            {
                playerPosture = PlayerPosture.Jumping;
            }
            else if (playerPosture != PlayerPosture.Jumping) {
                if (couldFall)
                {
                    playerPosture = PlayerPosture.Falling;
                }

            
            }

            
        }
        else if (playerPosture == PlayerPosture.Jumping) { 
            StartCoroutine(CoolDownJump());
        }
        else if (isLanding)
        {
            playerPosture = PlayerPosture.Landing;
        }
        else if (isCrouch)
        {
            playerPosture = PlayerPosture.Crouch;
        }
        else
        {
            playerPosture = PlayerPosture.Stand;
        }


        if (moveInput.magnitude == 0)
        {
            locomotionState = LocomotionState.Idle;
        }
        else if (!isRuning)
        {
            locomotionState = LocomotionState.Walk;
        }
        else
        {
            locomotionState = LocomotionState.Run;
        }


        if (isAiming)
        {
            armState = ArmState.Aim;
        }
        else
        {
            armState = ArmState.Normal;
        }
    }

    void SetupAnimator() {
        if (playerPosture == PlayerPosture.Stand)
        {
            animator.SetFloat(postureHash,standThreshold,0.1f,Time.deltaTime);

            switch (locomotionState)
            {
                case LocomotionState.Idle:
                    animator.SetFloat(moveSpeedHash, 0, 0.1f, Time.deltaTime);
                    break;
                case LocomotionState.Walk:
                    animator.SetFloat(moveSpeedHash, playerMovement.magnitude * walkSpeed , 0.1f, Time.deltaTime);
                    break;
                case LocomotionState.Run:
                    animator.SetFloat(moveSpeedHash, playerMovement.magnitude * runSpeed, 0.1f, Time.deltaTime);
                    break;
            }
        }
        else if (playerPosture == PlayerPosture.Crouch)
        {
            animator.SetFloat(postureHash, crouchThreshold, 0.1f, Time.deltaTime);

            switch (locomotionState)
            {
                case LocomotionState.Idle:
                    animator.SetFloat(moveSpeedHash, 0, 0.1f, Time.deltaTime);
                    break;
                default:
                    animator.SetFloat(moveSpeedHash, playerMovement.magnitude * crouchSpeed, 0.1f, Time.deltaTime);
                    break;
            }
        }
        else if (playerPosture == PlayerPosture.Jumping)
        {
            animator.SetFloat(postureHash, midairThreshold);
            animator.SetFloat(verticalVelecityHash, verticalVelecity);
            animator.SetFloat(feetTweenHash, feetTween);
        }
        else if (playerPosture == PlayerPosture.Landing)
        {
            animator.SetFloat(postureHash, landingThreshold, 0.03f, Time.deltaTime);

            switch (locomotionState)
            {
                case LocomotionState.Idle:
                    animator.SetFloat(moveSpeedHash, 0, 0.1f, Time.deltaTime);
                    break;
                case LocomotionState.Walk:
                    animator.SetFloat(moveSpeedHash, playerMovement.magnitude * walkSpeed, 0.1f, Time.deltaTime);
                    break;
                case LocomotionState.Run:
                    animator.SetFloat(moveSpeedHash, playerMovement.magnitude * runSpeed, 0.1f, Time.deltaTime);
                    break;
            }
        }
        else if (playerPosture == PlayerPosture.Falling)
        {
            animator.SetFloat(postureHash, midairThreshold);
            animator.SetFloat(verticalVelecityHash, verticalVelecity);
        }

        if (armState==ArmState.Normal)
        {
            float rad = Mathf.Atan2(playerMovement.x,playerMovement.z);
            animator.SetFloat(turnSpeedHash,rad,0.1f,Time.deltaTime);
            tr.Rotate(0,rad * 100 * Time.deltaTime,0);
        }

    
    }

    Vector3 AverageVel(Vector3 newVel) {
        velCache[currentChacheIndex] = newVel;
        currentChacheIndex++;
        currentChacheIndex %= CACHE_SIZE;
        Vector3 average = Vector3.zero;
        foreach (Vector3  vel in velCache)
        {
            average += vel;
        }
        return average / CACHE_SIZE;
    }

    private void OnAnimatorMove()
    {
        if (playerPosture != PlayerPosture.Jumping && playerPosture != PlayerPosture.Falling)
        {
            Vector3 movement = animator.deltaPosition;
            movement.y = verticalVelecity * Time.deltaTime;
            controller.Move(movement);


            //controller.SimpleMove(animator.velocity);
            //tr.Rotate(animator.deltaRotation.eulerAngles);
            averageVel = AverageVel(animator.velocity);
        }
        else
        {
            averageVel.y = verticalVelecity;
            Vector3 movement = averageVel * Time.deltaTime;
            controller.Move(movement);

            //tr.Rotate(animator.deltaRotation.eulerAngles);
        }

        
    }

    IEnumerator CoolDownJump() {
        landingThreshold =  Mathf.Clamp(verticalVelecity,-10,0);
        landingThreshold /= 20;
        landingThreshold += 1;
        isLanding = true;
        playerPosture = PlayerPosture.Landing;
        yield return new WaitForSeconds(jumpCD);
        isLanding = false;  
    }



    #region �����˳�
    void ChangePlayerState()
    {
        if (currentAimState != isAiming)
        {
            currentAimState = isAiming;
            animator.SetBool("isAiming", isAiming);
        }

    }
    void PlayerMove()
    {
        float targetSpeed = isRuning ? runSpeed : walkSpeed;
        animator.SetFloat("Vertical Speed", moveInput.magnitude * targetSpeed, 0.1f, Time.deltaTime);
        //if (isAiming)
        //{
        //    animator.SetFloat("Vertical Speed", dir.z * targetSpeed, 0.1f, Time.deltaTime);
        //    animator.SetFloat("Horizontal Speed",dir.x * targetSpeed, 0.1f, Time.deltaTime);
        //}
        //else
        //{
        //    animator.SetFloat("Vertical Speed", moveInput.magnitude * targetSpeed ,0.1f,Time.deltaTime);

        //}

    }
    void PlayerRotate()
    {
        float rad = Mathf.Atan2(playerMovement.x, playerMovement.z);
        animator.SetFloat("Trun Speed", rad, 0.1f, Time.deltaTime);

        transform.Rotate(0, rad * 100 * Time.deltaTime, 0);

        //if (isAiming)
        //{
        //    Vector3 playerTargetPosition;
        //    Ray ray = Camera.main.ScreenPointToRay(aimInput);
        //    Plane plane = new Plane(Vector3.up,tr.position);


        //    float distance;
        //    if (plane.Raycast(ray, out distance)) 
        //    {
        //        playerTargetPosition = ray.GetPoint(distance);

        //        float targetDistance = (playerTargetPosition - tr.position).magnitude;
        //        targetDistance = Mathf.Clamp(targetDistance * targetDistance /2.25f, 0f,1.5f);
        //        framingTransposer.m_TrackedObjectOffset.z = targetDistance;

        //        tr.LookAt(playerTargetPosition);
        //    }

        //}
        //else
        //{
        //    float rad = Mathf.Atan2(dir.x,dir.z);
        //    animator.SetFloat("Trun Speed", rad, 0.1f, Time.deltaTime);

        //    transform.Rotate(0, rad * 120 * Time.deltaTime, 0);
        //}

    }
    #endregion

    
    #region �������
    public void GetMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void GetRunInput(InputAction.CallbackContext context)
    {
        isRuning = context.ReadValueAsButton();
    }
    public void GetCrouchInput(InputAction.CallbackContext context)
    {
        isCrouch = context.ReadValueAsButton();
    }
    public void GetAimInput(InputAction.CallbackContext context)
    {
        isAiming = context.ReadValueAsButton();
    }

    public void GetAimPosInput(InputAction.CallbackContext context)
    {
        aimInput = context.ReadValue<Vector2>();
    }
    public void GetJumpInput(InputAction.CallbackContext context)
    {
        isJumping = context.ReadValueAsButton();
    }
    #endregion
}
