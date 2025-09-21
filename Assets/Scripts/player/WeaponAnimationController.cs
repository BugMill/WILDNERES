using UnityEngine;

public class WeaponAnimationController : MonoBehaviour
{
    [Header("Animator")]
    public Animator weaponAnimator;
    
    [Header("Animation States")]
    public string idleState = "Idle";
    public string shootState = "Shoot";
    public string reloadState = "Reload";
    
    private float shootAnimationLength;
    private float reloadAnimationLength;
    
    void Start()
    {
        if (weaponAnimator == null)
            weaponAnimator = GetComponent<Animator>();
        
        // Получаем длительность анимаций
        shootAnimationLength = GetAnimationLength(shootState);
        reloadAnimationLength = GetAnimationLength(reloadState);
        
        Debug.Log($"Shoot anim length: {shootAnimationLength}, Reload anim length: {reloadAnimationLength}");
    }
    
    private float GetAnimationLength(string stateName)
    {
        if (weaponAnimator == null) return 0f;
        
        RuntimeAnimatorController ac = weaponAnimator.runtimeAnimatorController;
        foreach (AnimationClip clip in ac.animationClips)
        {
            if (clip.name.Contains(stateName))
                return clip.length;
        }
        return 0.1f; // fallback
    }
    
    public void PlayShootAnimation()
    {
        if (weaponAnimator != null)
        {
            weaponAnimator.Play(shootState, -1, 0f);
        }
    }
    
    public void PlayReloadAnimation()
    {
        if (weaponAnimator != null)
        {
            weaponAnimator.Play(reloadState, -1, 0f);
        }
    }
    
    public float GetShootAnimationLength() => shootAnimationLength;
    public float GetReloadAnimationLength() => reloadAnimationLength;
}