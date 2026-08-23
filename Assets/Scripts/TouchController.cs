/*
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimentação")]
    public float speed = 5f;
    public float rotationSpeed = 10f;
    public float gravity = -9.81f;

    [Header("Joystick")]
    public Joystick joystick;

    [Header("Câmera")]
    public Transform cameraTransform; // arraste a câmera principal aqui no Inspector

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        HandleMovement();
        HandleGravity();
    }

    void HandleMovement()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Vector2 input = new Vector2(horizontal, vertical);
        if (input.sqrMagnitude < 0.0001f)
            return; // joystick parado, não faz nada

        // Direção baseada na orientação da câmera (ignorando inclinação vertical)
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 direction = camRight * horizontal + camForward * vertical;
        if (direction.magnitude > 1f)
            direction.Normalize();

        // Move o personagem
        controller.Move(direction * speed * Time.deltaTime);

        // Rotaciona o personagem suavemente para a direção do movimento
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void HandleGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
*/