using System;
using NaughtyAttributes;
using UnityEngine;

public class LimitedLife : MonoBehaviour
{
    [SerializeField] private bool _turnLimit;
    [SerializeField, ShowIf("_turnLimit")] private int _turns;
    [SerializeField, HideIf("_turnLimit")] private float _seconds;

    private Actor _actor;
    private Timer _timer;
    private int _turnsPassed;

    private void Awake()
    {
        _actor = GetComponent<Actor>();
    }

    private void Start()
    {
        if (!_turnLimit)
        {
            _timer = new Timer(_seconds, Timer.TimerReset.Manual);
            _timer.OnTimerDone += Die;
        }
    }

    private void OnEnable()
    {
        _actor.OnEndTurn += CountTurn;
    }

    private void OnDisable()
    {
        _actor.OnEndTurn -= CountTurn;
    }

    private void Update()
    {
        if (!_turnLimit)
        {
            _timer.CountTimer();
        }
    }

    public void CountTurn()
    {
        _turnsPassed++;

        if (_turnsPassed > _turns) Die();
    }

    private void Die()
    {
        Debug.Log("Die - " + gameObject.name);

        LeanTween.cancel(gameObject);
        var seq = LeanTween.sequence();

        seq.append(transform.LeanScale(Vector3.zero, 0.6f).setEasePunch().setEaseOutCubic());
        Destroy(gameObject);
    }
}