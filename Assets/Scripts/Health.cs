using System;
using UnityEngine;

// Sistema de vida compartido por jugador y enemigos.
public class Health : MonoBehaviour
{
    public float max = 100f;
    public float Current { get; private set; }
    public bool IsDead => Current <= 0f;

    public event Action<float> OnDamaged;
    public event Action OnDeath;

    void Awake() { Current = max; }

    public void Init(float maxHealth)
    {
        max = maxHealth;
        Current = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (IsDead) return;
        Current = Mathf.Max(0f, Current - amount);
        OnDamaged?.Invoke(amount);
        if (IsDead) OnDeath?.Invoke();
    }
}
