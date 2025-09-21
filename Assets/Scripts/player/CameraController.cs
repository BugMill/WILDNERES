using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100.0f;
    public Transform playerBody;
    
    private float xRotation = 0f;
    private Vector2 mouseInput;
    
    void Start()
    {
        // Автоматически находим игрока если не назначен вручную
        if (playerBody == null)
        {
            // Ищем родительский объект (если камера - дочерний объект игрока)
            playerBody = transform.parent;
            
            // Если нет родителя, ищем по тегу
            if (playerBody == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    playerBody = player.transform;
                }
            }
            
            // Если всё ещё не нашли, выводим предупреждение
            if (playerBody == null)
            {
                Debug.LogWarning("PlayerBody not assigned and cannot be found automatically!");
            }
        }
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update()
    {
        // Если playerBody не назначен, не выполняем обновление
        if (playerBody == null) return;
        
        GetMouseInput();
        
        float mouseX = mouseInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseInput.y * mouseSensitivity * Time.deltaTime;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
    
    private void GetMouseInput()
    {
        #if ENABLE_INPUT_SYSTEM
        // Новая система ввода
        var mouse = Mouse.current;
        if (mouse != null)
        {
            mouseInput = mouse.delta.ReadValue();
        }
        #else
        // Старая система ввода
        mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        #endif
    }
    
    // Для новой системы ввода
    public void OnLook(InputValue value)
    {
        mouseInput = value.Get<Vector2>();
    }
}