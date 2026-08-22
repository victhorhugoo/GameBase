using UnityEngine;
using VictorGame.StateMachine;

public class Player1 : MonoBehaviour
{
    public enum PlayerStates
    {
        Idle,
        Move,
        Run,
        Jump,
        Shoot
    }

    public Animator animator;
    public CharacterController characterController;

    public float speed = 1f;
    public float turnSpeed = 1f;
    public float gravity = -9.8f;
    public float jumpHeight = 2f;

    [Header("Run Setup")]
    public KeyCode KeyRun = KeyCode.LeftShift;
    public float speedRun = 1.5f;

    [Header("Shoot Setup")]
    [Header("Shoot Setup")]
    public KeyCode KeyShoot = KeyCode.Mouse0;
    public Transform firePoint;       
    public GameObject bulletPrefab;   
    public float bulletSpeed = 20f;

    public StateMachine<PlayerStates> stateMachine;

    // Dados que os estados leem a cada frame
    public float InputVertical { get; private set; }
    public float InputHorizontal { get; private set; }
    public float VerticalSpeed { get; private set; }
    public bool IsGrounded => characterController.isGrounded;

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

        if (animator == null)
            Debug.LogError($"[{name}] Animator não atribuído nem encontrado via GetComponent!");

        stateMachine = new StateMachine<PlayerStates>();
        stateMachine.Init();
        stateMachine.RegisterStates(PlayerStates.Idle, new PlayerStateIdle());
        stateMachine.RegisterStates(PlayerStates.Move, new PlayerStateMove());
        stateMachine.RegisterStates(PlayerStates.Run, new PlayerStateRun());
        stateMachine.RegisterStates(PlayerStates.Jump, new PlayerStateJump());
        stateMachine.RegisterStates(PlayerStates.Shoot, new PlayerStateShoot());

        stateMachine.SwitchState(Player1.PlayerStates.Idle, this);
        //stateMachine.SwitchState(PlayerStates.Idle, this);
    }

    private void Update()
    {
        ReadInput();
        ApplyRotationAndGravity();

        // A lógica de "o que fazer agora" fica dentro do estado atual (OnStateStay)
        stateMachine.Update();
    }

    private void ReadInput()
    {
        InputHorizontal = Input.GetAxis("Horizontal");
        InputVertical = Input.GetAxis("Vertical");
    }

    private void ApplyRotationAndGravity()
    {
        transform.Rotate(0, InputHorizontal * turnSpeed * Time.deltaTime, 0);

        if (IsGrounded)
        {
            if (VerticalSpeed < 0)
                VerticalSpeed = -1f; // mantém grudado no chão
        }
        else
        {
            VerticalSpeed -= gravity * Time.deltaTime;
        }
    }

    // ---- Ações que os estados usam ----

    public void Move(float speedMultiplier)
    {
        var speedVector = transform.forward * InputVertical * speed * speedMultiplier;
        speedVector.y = VerticalSpeed;
        characterController.Move(speedVector * Time.deltaTime);
    }

    public void Jump()
    {
        VerticalSpeed = jumpHeight;
    }

    public void Shoot()
    {
        
        Debug.Log("Shoot!");
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning($"[{name}] Shoot: firePoint ou bulletPrefab não atribuído no Inspector.");
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        if (bullet.TryGetComponent(out Rigidbody bulletRb))
        {
            bulletRb.velocity = firePoint.forward * bulletSpeed;
        }

        if (animator != null)
        {
            animator.SetTrigger("Shoot");
        }
    }
}
