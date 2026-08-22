using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateTest : MonoBehaviour
{

    public enum CharacterState { Idle, Walking, Running, Jumping }

    public CharacterState currentState;
    private Rigidbody rb;
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 7f;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        currentState = CharacterState.Idle;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Método que lida com a execução do estado atual
        ExecuteCurrentState();

        // Método que lida com a transição de estados
        HandleStateTransitions();
    }

    // Método para executar a ação associada ao estado atual
    private void ExecuteCurrentState()
    {
        if (currentState == CharacterState.Idle)
        {
            Idle();
        }
        else if (currentState == CharacterState.Walking)
        {
            Walk();
        }
        else if (currentState == CharacterState.Running)
        {
            Run();
        }
        else if (currentState == CharacterState.Jumping)
        {
            Jump();
        }
    }

    // Métodos para cada estado
    private void Idle()
    {
        // Código para o estado Idle
        Debug.Log("Personagem está parado.");
    }

    private void Walk()
    {
        Debug.Log("Personagem está andando.");
        // Movimentação ao andar
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.Translate(move * walkSpeed * Time.deltaTime);
    }

    private void Run()
    {
        Debug.Log("Personagem está correndo.");
        // Movimentação ao correr
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.Translate(move * runSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            Debug.Log("Personagem está pulando.");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    // Método para lidar com as transições de estados
    private void HandleStateTransitions()
    {
        // Se o personagem estiver no ar, não permitir transições até ele tocar o chão
        if (currentState == CharacterState.Jumping && !isGrounded)
        {
            return; // Não processa outras transições enquanto está pulando
        }

        // Verificar se o jogador parou de se mover e voltar para o estado Idle
        if (currentState == CharacterState.Walking && Input.GetAxis("Vertical") == 0)
        {
            currentState = CharacterState.Idle;
        }
        // Verificar se o jogador começou a andar
        else if (currentState == CharacterState.Idle && Input.GetAxis("Vertical") != 0)
        {
            currentState = CharacterState.Walking;
        }
        // Verificar se o jogador começou a correr
        else if (currentState == CharacterState.Walking && Input.GetKey(KeyCode.LeftShift))
        {
            currentState = CharacterState.Running;
        }
        // Verificar se o jogador parou de correr e voltou a andar
        else if (currentState == CharacterState.Running && !Input.GetKey(KeyCode.LeftShift))
        {
            currentState = CharacterState.Walking;
        }
        // Verificar se o jogador tentou pular
        else if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            currentState = CharacterState.Jumping;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Detecta se o personagem tocou o chão
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            currentState = Input.GetAxis("Vertical") != 0 ? CharacterState.Walking : CharacterState.Idle;
        }
    }
}