using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class WeaponController : MonoBehaviour
{
    public WeaponAnimationController weaponAnim; // —сылка на скрипт анимации оружи€
    public int maxAmmo = 30;
    public int currentAmmo;
    public float shootCooldown = 0.2f;

    private bool canShoot = true;

    private void Awake()
    {
        currentAmmo = maxAmmo;

        if (weaponAnim == null)
            weaponAnim = GetComponentInChildren<WeaponAnimationController>();
    }

    public void OnShoot()
    {
        if (canShoot && currentAmmo > 0)
        {
            Shoot();
        }
    }

    public void OnReload()
    {
        Reload();
    }

    private void Shoot()
    {
        currentAmmo--;
        weaponAnim.PlayShoot();
        StartCoroutine(ShootCooldown());
        // TODO: добавить создание пули/луча дл€ урона
    }

    private void Reload()
    {
        currentAmmo = maxAmmo;
        weaponAnim.PlayReload();
    }

    private System.Collections.IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
}
