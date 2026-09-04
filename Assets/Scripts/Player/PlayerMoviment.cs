using System.Collections.Generic;
using UnityEngine;
using VictorGame.StateMachine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMoviment : MonoBehaviour, IDamageable
{
    public enum PlayerStates
    {
        Idle,
        Move,
        Run,
        Jump
    }

    public Animator animator;
    public CharacterController characterController;
    public float speed = 1f;
    public float turnSpeed = 10f;
    public float gravity = -9.8f;
    public float jumpHeight = 2f;

    [Header("Camera Setup")]
    public Transform _cameraTransform;

    [Header("Touch Setup")]
    public VirtualJoystick virtualJoystick;

    [Header("Run Setup")]
    public KeyCode KeyRun = KeyCode.LeftShift;
    public float speedRun = 1.5f;

    [Header("Jump Setup")]
    public KeyCode KeyJump = KeyCode.Space;

    [Header("Flash")]
    public List<FlashColor> flashColors;

    public StateMachine<PlayerStates> stateMachine;

    public float InputVertical { get; private set; }
    public float InputHorizontal { get; private set; }
    public float VerticalSpeed { get; private set; }
    public bool IsGrounded => characterController.isGrounded;

    public bool IsRunning { get; private set; }
    public bool JumpPressed { get; private set; }

    private bool isRunningButtonUI;
    private bool jumpRequestedUI;

    private Vector3 moveDir;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        if (characterController == null)
            characterController = GetComponent<CharacterController>();
        if (_cameraTransform == null && Camera.main != null)
            _cameraTransform = Camera.main.transform;
        if (animator == null)
            Debug.LogError($"[{name}] Animator não atribuído nem encontrado via GetComponent!");

        stateMachine = new StateMachine<PlayerStates>();
        stateMachine.Init();
        stateMachine.RegisterStates(PlayerStates.Idle, new PlayerStateIdle());
        stateMachine.RegisterStates(PlayerStates.Move, new PlayerStateMove());
        stateMachine.RegisterStates(PlayerStates.Run, new PlayerStateRun());
        stateMachine.RegisterStates(PlayerStates.Jump, new PlayerStateJump());
        stateMachine.SwitchState(PlayerStates.Idle, this);
    }

    private void Update()
    {
        ReadInput();
        ApplyGravity();
        stateMachine.Update();

        JumpPressed = false;
    }

    private void ReadInput()
    {
        float keyboardH = Input.GetAxis("Horizontal");
        float keyboardV = Input.GetAxis("Vertical");

        Vector2 joystickInput = Vector2.zero;
        if (virtualJoystick != null)
            joystickInput = virtualJoystick.Direction;

        InputHorizontal = Mathf.Clamp(keyboardH + joystickInput.x, -1f, 1f);
        InputVertical = Mathf.Clamp(keyboardV + joystickInput.y, -1f, 1f);

        if (_cameraTransform != null)
        {
            Vector3 camForward = _cameraTransform.forward;
            Vector3 camRight = _cameraTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            moveDir = camForward * InputVertical + camRight * InputHorizontal;
        }
        else
        {
            moveDir = transform.forward * InputVertical + transform.right * InputHorizontal;
        }

        bool keyboardRun = Input.GetKey(KeyRun);
        IsRunning = keyboardRun || isRunningButtonUI;

        bool keyboardJump = Input.GetKeyDown(KeyJump);
        if (keyboardJump || jumpRequestedUI)
        {
            JumpPressed = true;
            jumpRequestedUI = false;
        }
    }

    private void ApplyGravity()
    {
        if (IsGrounded)
        {
            if (VerticalSpeed < 0)
                VerticalSpeed = -1f;
        }
        else
        {
            VerticalSpeed += gravity * Time.deltaTime;
        }
    }

    // ---- Ações que os estados usam ----
    public void Move(float speedMultiplier)
    {
        Vector3 speedVector = moveDir * speed * speedMultiplier;
        speedVector.y = VerticalSpeed;
        characterController.Move(speedVector * Time.deltaTime);

        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }

    public void Jump()
    {
        VerticalSpeed = jumpHeight;
    }

    // ---- Métodos públicos para conectar nos botões da UI (touch) ----

    public void JumpButton()
    {
        jumpRequestedUI = true;
    }

    public void StartRun()
    {
        isRunningButtonUI = true;
    }

    public void StopRun()
    {
        isRunningButtonUI = false;
    }

    #region LIFE
    public void Damage(float damage)
    {
        flashColors.ForEach(i => i.Flash());
    }

    public void Damage(float damage, Vector3 dir)
    {
        Damage(damage);
        // Aplicar knockback ou outras reações aqui, se necessário
    }

    public void OnDamage(float damage)
    {
        // Implementar reação ao dano, como reduzir vida, tocar animação, etc.
    }
    #endregion
}

/*using UnityEngine;
using VictorGame.StateMachine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMoviment : MonoBehaviour
{
    public enum PlayerStates
    {
        Idle,
        Move,
        Run,
        Jump
        
    }

    public Animator animator;
    public CharacterController characterController;
    public float speed = 1f;
    public float turnSpeed = 10f;
    public float gravity = -9.8f;
    public float jumpHeight = 2f;

    [Header("Camera Setup")]
    public Transform _cameraTransform;

    [Header("Touch Setup")]
    public VirtualJoystick virtualJoystick;

    [Header("Run Setup")]
    public KeyCode KeyRun = KeyCode.LeftShift;
    public float speedRun = 1.5f;

    [Header("Jump Setup")]
    public KeyCode KeyJump = KeyCode.Space;

    [Header("Gun Setup")]
    public GunBase gun; // NOVO: arraste o objeto da arma aqui
    public KeyCode KeyShoot = KeyCode.Mouse0;

    public StateMachine<PlayerStates> stateMachine;

    public float InputVertical { get; private set; }
    public float InputHorizontal { get; private set; }
    public float VerticalSpeed { get; private set; }
    public bool IsGrounded => characterController.isGrounded;

    public bool IsRunning { get; private set; }
    public bool JumpPressed { get; private set; }

    private bool isRunningButtonUI;
    private bool jumpRequestedUI;

    private Vector3 moveDir;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        if (characterController == null)
            characterController = GetComponent<CharacterController>();
        if (_cameraTransform == null && Camera.main != null)
            _cameraTransform = Camera.main.transform;
        if (animator == null)
            Debug.LogError($"[{name}] Animator não atribuído nem encontrado via GetComponent!");
        if (gun == null)
            Debug.LogWarning($"[{name}] Gun não atribuído no Inspector — o tiro não vai funcionar.");

        stateMachine = new StateMachine<PlayerStates>();
        stateMachine.Init();
        stateMachine.RegisterStates(PlayerStates.Idle, new PlayerStateIdle());
        stateMachine.RegisterStates(PlayerStates.Move, new PlayerStateMove());
        stateMachine.RegisterStates(PlayerStates.Run, new PlayerStateRun());
        stateMachine.RegisterStates(PlayerStates.Jump, new PlayerStateJump());
        // REMOVIDO: RegisterStates(PlayerStates.Shoot, ...)
        stateMachine.SwitchState(PlayerStates.Idle, this);
    }

    private void Update()
    {
        ReadInput();
        ApplyGravity();
        stateMachine.Update();

        JumpPressed = false;
    }

    private void ReadInput()
    {
        float keyboardH = Input.GetAxis("Horizontal");
        float keyboardV = Input.GetAxis("Vertical");

        Vector2 joystickInput = Vector2.zero;
        if (virtualJoystick != null)
            joystickInput = virtualJoystick.Direction;

        InputHorizontal = Mathf.Clamp(keyboardH + joystickInput.x, -1f, 1f);
        InputVertical = Mathf.Clamp(keyboardV + joystickInput.y, -1f, 1f);

        if (_cameraTransform != null)
        {
            Vector3 camForward = _cameraTransform.forward;
            Vector3 camRight = _cameraTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            moveDir = camForward * InputVertical + camRight * InputHorizontal;
        }
        else
        {
            moveDir = transform.forward * InputVertical + transform.right * InputHorizontal;
        }

        bool keyboardRun = Input.GetKey(KeyRun);
        IsRunning = keyboardRun || isRunningButtonUI;

        bool keyboardJump = Input.GetKeyDown(KeyJump);
        if (keyboardJump || jumpRequestedUI)
        {
            JumpPressed = true;
            jumpRequestedUI = false;
        }

        // NOVO: tiro via GunBase (segurar para atirar continuamente, soltar para parar)
        if (gun != null)
        {
            if (Input.GetKeyDown(KeyShoot))
                gun.StartShoot();

            if (Input.GetKeyUp(KeyShoot))
                gun.StopShoot();
        }
    }

    private void ApplyGravity()
    {
        if (IsGrounded)
        {
            if (VerticalSpeed < 0)
                VerticalSpeed = -1f;
        }
        else
        {
            VerticalSpeed += gravity * Time.deltaTime;
        }
    }

    // ---- Ações que os estados usam ----
    public void Move(float speedMultiplier)
    {
        Vector3 speedVector = moveDir * speed * speedMultiplier;
        speedVector.y = VerticalSpeed;
        characterController.Move(speedVector * Time.deltaTime);

        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }

    public void Jump()
    {
        VerticalSpeed = jumpHeight;
    }

    // REMOVIDO: método Shoot() antigo (agora vive dentro de GunBase)

    // ---- Métodos públicos para conectar nos botões da UI (touch) ----

    public void JumpButton()
    {
        jumpRequestedUI = true;
    }

    public void StartRun()
    {
        isRunningButtonUI = true;
    }

    public void StopRun()
    {
        isRunningButtonUI = false;
    }

    // NOVO: substituem o antigo ShootButton()
    public void StartShootButton()
    {
        if (gun != null)
            gun.StartShoot();
    }

    public void StopShootButton()
    {
        if (gun != null)
            gun.StopShoot();
    }
}
*/

/*
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public VirtualJoystick virtualJoystick;
    public Transform _cameraTransform;
    public float moveSpeed = 5f;
    public float playerRotationSpeed = 10f;
    public float playerGravity = -9.81f;
    public float jumpHeight = 1.5f;
    public float speedRun = 8f;
    public KeyCode runKey = KeyCode.Space; // NOVO: tecla configurável no Inspector

    private CharacterController characterController;
    private float verticalVelocity = 0f;
    private bool jumpRequested;
    private bool isRunningButton; // controlado pelo botão da UI

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        if (_cameraTransform == null && Camera.main != null)
            _cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        Vector2 input = virtualJoystick.Direction;

        Vector3 camForward = _cameraTransform.forward;
        Vector3 camRight = _cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * input.y + camRight * input.x;

        // Reset da velocidade vertical quando no chão
        if (characterController.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        // Pulo
        if (jumpRequested && characterController.isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * playerGravity);
        }
        jumpRequested = false;

        // Aplica gravidade
        verticalVelocity += playerGravity * Time.deltaTime;

        // NOVO: corrida acionada pelo botão OU pela tecla (útil pra testar no Editor)
        bool isRunningKeyboard = Input.GetKey(runKey);
        bool isRunning = isRunningButton || isRunningKeyboard;

        bool isWalking = input.magnitude > 0.01f;
        float currentSpeed = (isWalking && isRunning) ? speedRun : moveSpeed;

        Vector3 finalMove = moveDir * currentSpeed;
        finalMove.y = verticalVelocity;
        characterController.Move(finalMove * Time.deltaTime);

        // Rotaciona o personagem para a direção do movimento
        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, playerRotationSpeed * Time.deltaTime);
        }

        bool isMoving = moveDir.sqrMagnitude > 0.01f;
        animator.SetBool("Run", isMoving);
    }

    // Chamado pelo OnClick() do botão Jump na UI
    public void Jump()
    {
        jumpRequested = true;
    }

    // Chamados pelo Event Trigger do botão de correr (Pointer Down / Pointer Up)
    public void StartRun()
    {
        isRunningButton = true;
    }

    public void StopRun()
    {
        isRunningButton = false;
    }
}
*/