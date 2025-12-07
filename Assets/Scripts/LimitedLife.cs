using NaughtyAttributes;
using UnityEngine;

public class LimitedLife : MonoBehaviour
{
    [SerializeField] private bool _turnLimit;
    [SerializeField, ShowIf("_turnLimit")] private int _turns;
    [SerializeField, HideIf("_turnLimit")] private float _seconds;

    private Timer _timer;
    private int _turnsPassed;

    private void Start()
    {
        if (!_turnLimit) 
        {
            _timer = new Timer(_seconds, Timer.TimerReset.Manual);
            _timer.OnTimerDone += Die;
        }
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
        Destroy(gameObject);
    }
}
