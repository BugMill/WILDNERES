using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayer;
    
    [Header("Physics Settings")]
    public float playerMass = 70f; // Масса игрока в кг
    public float gravity = 9.81f; // Сила гравитации
    
    private PlayerInputActions playerInputActions;
    private Vector2 movementInput;
    private CharacterController characterController;
    private bool isGrounded;
    private float verticalVelocity; // Вертикальная скорость для гравитации

    private void Awake()
    {
        // Настройка CharacterController
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            characterController = gameObject.AddComponent<CharacterController>();
            characterController.height = 2f;
            characterController.radius = 0.5f;
            characterController.center = new Vector3(0f, 1f, 0f);
        }
        
        playerInputActions = new PlayerInputActions();
        
        Debug.Log($"Масса игрока установлена: {playerMass} кг");
    }

    private void OnEnable()
    {
        playerInputActions?.Player.Enable();
    }

    private void OnDisable()
    {
        playerInputActions?.Player.Disable();
    }

    private void Update()
    {
        // Получаем ввод движения
        movementInput = playerInputActions.Player.Move.ReadValue<Vector2>();
        
        // Проверяем землю под ногами
        CheckGrounded();
        
        // Обрабатываем гравитацию
        HandleGravity();
        
        // Обрабатываем движение
        HandleMovement();
    }

    private void HandleGravity()
    {
        if (isGrounded)
        {
            // Если на земле, сбрасываем вертикальную скорость (но немного прижимаем к земле)
            verticalVelocity = -0.5f;
        }
        else
        {
            // Если в воздухе, применяем гравитацию
            verticalVelocity -= gravity * Time.deltaTime;
            
            // Ограничиваем максимальную скорость падения
            verticalVelocity = Mathf.Clamp(verticalVelocity, -20f, 10f);
        }
        
        // Применяем вертикальное движение
        Vector3 verticalMovement = new Vector3(0, verticalVelocity, 0) * Time.deltaTime;
        characterController.Move(verticalMovement);
    }

    private void HandleMovement()
    {
        if (movementInput.magnitude > 0.1f)
        {
            // Движение относительно направления игрока (уже повернут камерой)
            Vector3 moveDirection = transform.forward * movementInput.y + transform.right * movementInput.x;
            Vector3 movement = moveDirection * moveSpeed * Time.deltaTime;
            
            // Применяем горизонтальное движение
            characterController.Move(movement);
        }
    }

    private void CheckGrounded()
    {
        // Проверяем, стоит ли игрок на земле
        isGrounded = characterController.isGrounded;
        
        // Дополнительная проверка лучом для точности
        RaycastHit hit;
        Vector3 rayStart = transform.position;
        bool raycastGrounded = Physics.Raycast(rayStart, Vector3.down, out hit, 
            characterController.height / 2 + groundCheckDistance, groundLayer);
        
        // Если CharacterController говорит что мы на земле, но луч нет - доверяем CharacterController
        // И наоборот
        if (isGrounded != raycastGrounded)
        {
            isGrounded = isGrounded; // Приоритет CharacterController
        }
        
        // Визуализация в редакторе
        Debug.DrawRay(rayStart, Vector3.down * (characterController.height / 2 + groundCheckDistance), 
                     isGrounded ? Color.green : Color.red);
    }

    // Метод для получения массы игрока
    public float GetPlayerMass()
    {
        return playerMass;
    }

    // Метод для изменения массы (если нужно)
    public void SetPlayerMass(float newMass)
    {
        if (newMass > 0)
        {
            playerMass = newMass;
            Debug.Log($"Масса игрока изменена: {playerMass} кг");
        }
    }

    // Для отладки
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 20), $"Move Input: {movementInput}");
        GUI.Label(new Rect(10, 30, 300, 20), $"Grounded: {isGrounded}");
        GUI.Label(new Rect(10, 50, 300, 20), $"Vertical Velocity: {verticalVelocity:F2}");
        GUI.Label(new Rect(10, 70, 300, 20), $"Mass: {playerMass} kg");
        GUI.Label(new Rect(10, 90, 300, 20), $"Position: {transform.position}");
    }
}   