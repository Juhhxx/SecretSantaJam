using System;
using System.Collections.Generic;
using CameraShake;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class ElfVisuals : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _hitPS;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioPlayer _hitSound;
    [SerializeField] private float hitRotationAmplitude = 10;
    [SerializeField] private Color hitColor = Color.red;

    private SpriteRenderer[] _sprites;
    private PlayerMovement _movement;
    private HealthSystem _health;
    private Actor _actor;
    private static readonly int _movementHash = Animator.StringToHash("Movement");

    private void Awake()
    {
        _actor = GetComponentInParent<Actor>();
        _health = GetComponentInParent<HealthSystem>();
        _movement = GetComponentInParent<PlayerMovement>();
        _sprites = GetComponentsInChildren<SpriteRenderer>(true);
    }

    private void Start()
    {
        
        RandomizeElf();
    }

    [Button()]
    public void RandomizeElf()
    {
        _animator.SetFloat("Team", _actor ? _actor.teamID : 0f);
        _animator.SetFloat("Face", Random.Range(0f, 10f));
        _animator.SetFloat("Ears", Random.Range(0f, 5f));
    }

    private void OnEnable()
    {
        _health.OnHit += GetHit;
    }

    private void OnDisable()
    {
        _health.OnHit -= GetHit;
    }

    private void Update()
    {
        if (_movement != null)
        {
            _animator.SetFloat(_movementHash, _movement.CanMove ? _movement.Velocity.magnitude : 0f);
        }
    }

    [Button()]
    public void Attack()
    {
        _animator.SetTrigger("Attack");
    }

    [Button()]
    public void GetHit()
    {
        if (LeanTween.isTweening(gameObject))
        {
            LeanTween.cancel(gameObject);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            
            foreach (var sprite in _sprites)
            {
                sprite.color = Color.white;
            }
        }
        
        CameraShaker.Presets.ShortShake2D();
        
        _hitSound?.Play(_audioSource);
        _hitPS?.Play();
        
        transform.LeanScale(Vector3.one * Random.Range(0.8f, 1.2f), 0.8f).setEasePunch();
        transform.LeanRotateZ(Random.Range(-hitRotationAmplitude, hitRotationAmplitude), 0.6f).setEasePunch();

        foreach (var sprite in _sprites)
        {
            LeanTween.color(sprite.gameObject, hitColor, 0.45f).setEasePunch();
        }
    }
}