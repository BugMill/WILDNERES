using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    public int maxAmmo = 6;
    
    [Header("References")]
    public WeaponAnimationController animationController;
    
    private int currentAmmo;
    private float nextFireTime;
    private bool isReloading = false;
    private float reloadTime;
    
    void Start()
    {
        currentAmmo = maxAmmo;
        
        if (animationController == null)
            animationController = GetComponent<WeaponAnimationController>();
        
        if (firePoint == null)
            firePoint = transform.Find("FirePoint");
        
        // Устанавливаем время перезарядки
        reloadTime = 2.0f; // значение по умолчанию
        
        // Пытаемся получить длительность анимации перезарядки
        if (animationController != null)
        {
            reloadTime = animationController.GetReloadAnimationLength();
        }
    }
    
    void Update()
    {
        if (isReloading) return;
        
        HandleInput();
    }
    
    private void HandleInput()
    {
        // Стрельба - ЛКМ (только по нажатию)
        if (Mouse.current.leftButton.wasPressedThisFrame && 
            Time.time >= nextFireTime && 
            currentAmmo > 0)
        {
            Shoot();
        }
        
        // Перезарядка - R
        if (Keyboard.current.rKey.wasPressedThisFrame && currentAmmo < maxAmmo)
        {
            StartReload();
        }
        
        // Автоперезарядка
        if (currentAmmo <= 0)
        {
            StartReload();
        }
    }
    
    private void Shoot()
    {
        currentAmmo--;
        nextFireTime = Time.time + fireRate;
        
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
        
        if (animationController != null)
        {
            animationController.PlayShootAnimation();
        }
    }
    
    private void StartReload()
    {
        isReloading = true;
        
        if (animationController != null)
        {
            animationController.PlayReloadAnimation();
        }
        
        Invoke("FinishReload", reloadTime);
    }
    
    private void FinishReload()
    {
        currentAmmo = maxAmmo;  
        isReloading = false;
    }
    
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), $"Ammo: {currentAmmo}/{maxAmmo}");
        GUI.Label(new Rect(10, 30, 200, 20), $"Reloading: {isReloading}");
        GUI.Label(new Rect(10, 50, 200, 20), $"Reload Time: {reloadTime:F2}s");
    }
}