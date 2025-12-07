using System;
using NaughtyAttributes;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int _maxHP;

    [ProgressBar("_maxHP", EColor.Red)]
    [SerializeField, ReadOnly] private float _currentHP;
    public float CurrentHP
    {
        get => _currentHP;

        set
        {
            if (value <= 0)
            {
                OnDeath?.Invoke();
                _currentHP = 0;
            }
            else if (value >= _maxHP)
            {
                _currentHP = _maxHP;
            }
            else _currentHP = value;
        }
    }

    public event Action OnDeath;
    public event Action OnHit;

    private void Start()
    {
        _currentHP = _maxHP;

        OnDeath += () => Debug.Log($"I DIED {gameObject.name}");
    }

    public void TakeDamage(float amount)
    {
        CurrentHP -= amount;
        OnHit?.Invoke();
    } 
    public void Heal(float amount) => CurrentHP += amount;
    
}
