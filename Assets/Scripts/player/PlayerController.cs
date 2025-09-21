    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5.0f;
    public float jumpForce = 8.0f;
    
    [Header("Weight Settings")]
    public float mass = 3.0f; // Увеличьте до 5-10 для большего веса
    public float drag = 2.0f; // Сопротивление движению
    public float angularDrag = 0.5f;
    
    private CharacterController controller;
    private Rigidbody rb;
    private Vector2 movementInput;
    private Vector3 moveDirection;
    private bool usePhysics = true; // Переключение между физикой и CharacterController
    
    private void Start()
    {
        // Пытаемся получить CharacterController
        controller = GetComponent<CharacterController>();
        
        // Добавляем или получаем Rigidbody для физики
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();
        
        // Настраиваем Rigidbody для "веса"
        rb.mass = mass;
        rb.linearDamping = drag;
        rb.angularDamping = angularDrag;
        rb.freezeRotation = true; // Важно! Предотвращает падение
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Решаем, использовать ли физику или CharacterController
        usePhysics = (controller == null);
    }

    private void Update()
    {
        // Получаем ввод
        movementInput = new Vector2(
            Keyboard.current.dKey.isPressed ? 1 : Keyboard.current.aKey.isPressed ? -1 : 0,
            Keyboard.current.wKey.isPressed ? 1 : Keyboard.current.sKey.isPressed ? -1 : 0
        );
        
        // Вычисляем направление движения
        moveDirection = transform.forward * movementInput.y + transform.right * movementInput.x;
        
        if (usePhysics)
        {
            HandlePhysicsMovement();
        }
        else
        {
            HandleCharacterControllerMovement();
        }
    }
    
    private void HandlePhysicsMovement()
    {
        // Двигаем через физику
        rb.AddForce(moveDirection * moveSpeed, ForceMode.Force);
        
        // Ограничиваем максимальную скорость
        if (rb.linearVelocity.magnitude > moveSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
        }
    }
    
    private void HandleCharacterControllerMovement()
    {
        // Двигаем через CharacterController
        Vector3 motion = moveDirection * moveSpeed * Time.deltaTime;
        
        // Применяем гравитацию
        if (!controller.isGrounded)
        {
            motion.y -= 9.81f * Time.deltaTime;
        }
        
        controller.Move(motion);
    }
    
    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }
    
    // Для прыжка (опционально)
    private void HandleJump()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (usePhysics && Physics.Raycast(transform.position, Vector3.down, 1.1f))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }
}