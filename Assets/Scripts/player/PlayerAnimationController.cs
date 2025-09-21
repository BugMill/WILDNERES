using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private Vector3 lastPosition;
    private bool isRunning;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
    }

    private void Update()
    {
        // Считаем скорость движения по горизонтали
        Vector3 delta = transform.position - lastPosition;
        float velocity = new Vector2(delta.x, delta.z).magnitude / Time.deltaTime;
        animator.SetFloat("Speed", velocity);

        // Ускоряем анимацию, если игрок бежит
        animator.speed = isRunning ? 1.5f : 1.0f;

        lastPosition = transform.position;
    }

    // Метод для установки бега
    public void SetRunning(bool running)
    {
        isRunning = running;
    }
}
