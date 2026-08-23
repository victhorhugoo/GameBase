using UnityEngine;
using UnityEngine.EventSystems;

// Anexe este script ao objeto "JoystickBackground"
public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform background;   // o próprio fundo do joystick
    public RectTransform handle;       // a alça que se move

    [Range(0.5f, 2f)]
    public float handleRange = 1f;     // limite de movimento da alça

    private Vector2 inputVector = Vector2.zero;

    // Valores públicos que outros scripts (ex: movimento do personagem) vão ler
    public float Horizontal => inputVector.x;
    public float Vertical => inputVector.y;
    public Vector2 Direction => inputVector;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background,
            eventData.position,
            eventData.pressEventCamera,
            out position
        );

        // Normaliza a posição em relação ao tamanho do fundo
        position.x = (position.x / background.sizeDelta.x);
        position.y = (position.y / background.sizeDelta.y);

        inputVector = new Vector2(position.x * 2, position.y * 2);
        inputVector = (inputVector.magnitude > handleRange)
            ? inputVector.normalized * handleRange
            : inputVector;

        handle.anchoredPosition = new Vector2(
            inputVector.x * (background.sizeDelta.x / 2) * handleRange,
            inputVector.y * (background.sizeDelta.y / 2) * handleRange
        );
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Move o joystick para onde o dedo tocou (opcional - joystick dinâmico)
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Volta a alça para o centro ao soltar
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }
}


/*
public class Joystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Referências de UI")]
    public RectTransform background; // o círculo de fundo do joystick
    public RectTransform handle;     // o "botão" que o dedo arrasta

    [Header("Configurações")]
    [Tooltip("Raio máximo que o handle pode se mover, em pixels")]
    public float handleRange = 100f;

    public float Horizontal => inputVector.x;
    public float Vertical => inputVector.y;

    private Vector2 inputVector = Vector2.zero;
    private Canvas canvas;
    private Camera uiCamera;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();

        // Se o Canvas for "Screen Space - Camera" ou "World Space", precisa da câmera de UI
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            uiCamera = canvas.worldCamera;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background, eventData.position, uiCamera, out position);

        // Normaliza a posição em relação ao raio do fundo
        position = Vector2.ClampMagnitude(position, handleRange);
        inputVector = position / handleRange;

        handle.anchoredPosition = position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }
}
*/