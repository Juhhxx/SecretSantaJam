using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    [SerializeField] private GameVariableSettings _gameSettings;
    [SerializeField] private bool _startOnAwake = true;
    private float _skipTurnTimer;

    private void OnEnable()
    {
        _gameSettings.turnActorChange += TurnChange;
    }

    private void OnDisable()
    {
        _gameSettings.turnActorChange -= TurnChange;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.6f);

        if (_startOnAwake)
        {
            TurnManager.instance.StartNextTurn();
        }
    }

    private void TurnChange(Actor newActor)
    {
        _actor = newActor;
        _breakSkipInput = true;
    }

    private Actor _actor;
    private bool _breakSkipInput = false;

    private void Update()
    {
        if (_actor == null) return;

        int inputTeam = _actor.teamID + 1; // 0, 1

        if (inputTeam <= 1)
        {
            bool skipTurn = Input.GetButton("Jump" + inputTeam);
            if (!skipTurn)
            {
                _breakSkipInput = false;
            }

            skipTurn = skipTurn && !_breakSkipInput;

            if (skipTurn)
            {
                _skipTurnTimer += Time.deltaTime;

                _gameSettings.RaiseHoldTime(_skipTurnTimer);

                if (_skipTurnTimer > _gameSettings.TurnSkipHoldTime)
                {
                    TurnManager.instance.StartNextTurn();
                    ResetSkipTimer();
                }
            }
            else if (_skipTurnTimer > 0f)
            {
                ResetSkipTimer();
            }
        }
    }

    private void ResetSkipTimer()
    {
        _skipTurnTimer = 0f;
        _gameSettings.RaiseHoldTime(_skipTurnTimer);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Rect r = new Rect(0, 0, 100, 20);
        GUI.Label(r, "Hold Jump button to skip turn");
    }
#endif
}