using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public enum PlayerPosture { 
    Crouch, //�¶�
    Stand,  //վ��
    Midair  //�Ϳ�

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

public class Player : MonoBehaviour
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
    float midairThreshold = 2f;

    int postureHash;
    int moveSpeedHash;
    int turnSpeedHash;

    #endregion

    #region ��Ծ���
    public float gravity = -9.8f;//����
    public float verticalVelecity;//��ֱ������ٶ�

    public float jumpVelecity = 5f;//��Ծ���ϵ��ٶ�
    public int verticalVelecityHash;

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

        //verticalVelecityHash = Animator.StringToHash("��ֱ�ٶ�");

        
    }

    void Update()
    {
        CaculateGravity();
        Jump();

        CaculateInputDirection();
        SwitchPlayerStates();
        SetupAnimator();

        //ChangePlayerState();
        //PlayerMove();
        //PlayerRotate();
    }

    void Jump() {
        if (controller.isGrounded && isJumping)
        {
            verticalVelecity = jumpVelecity;
        }
    
    }

    void CaculateGravity() {

        if (controller.isGrounded)
        {
            verticalVelecity = 0;
            return;
        }
        else
        {
            verticalVelecity += gravity * Time.deltaTime;
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
        if (isCrouch)
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

        if (armState==ArmState.Normal)
        {
            float rad = Mathf.Atan2(playerMovement.x,playerMovement.z);
            animator.SetFloat(turnSpeedHash,rad,0.1f,Time.deltaTime);
            tr.Rotate(0,rad * 100 * Time.deltaTime,0);
        }

    
    }


    private void OnAnimatorMove()
    {
        Vector3 movement = animator.deltaPosition;
        movement.y = verticalVelecity * Time.deltaTime;
        controller.Move(movement);


        //controller.SimpleMove(animator.velocity);
        tr.Rotate(animator.deltaRotation.eulerAngles);
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
