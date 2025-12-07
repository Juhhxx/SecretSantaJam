using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchDataPanel : MonoBehaviour
{
    [SerializeField] private GameVariableSettings _gameEvents;
    [SerializeField] private Image _fillTurnSkipImage;
    [SerializeField] private Image _staminaBar;

    private Actions _actorActions;
    private void Awake()
    {
        // if we dont have game events just disable the whole system
        if (_gameEvents == null)
            gameObject.SetActive(false); 
    }

    private void OnEnable()
    {
        _gameEvents.currentSkipHoldTime += UpdateSkipTime;
        _gameEvents.turnActorChange += ChangeLabel;
        _gameEvents.movementUsageUpdate += MovementUpdated;
    }

    private void OnDisable()
    {
        _gameEvents.currentSkipHoldTime -= UpdateSkipTime;
        _gameEvents.turnActorChange -= ChangeLabel;
        _gameEvents.movementUsageUpdate -= MovementUpdated;

    }

    private void MovementUpdated(float accumulatedMovement)
    {
        if (_actorActions == null)
        {
            _staminaBar.fillAmount = 1f;
            return;
        }
        float maxMovement = _actorActions.MaxMovementPoints;
        float value =  (maxMovement - accumulatedMovement) / maxMovement;
        _staminaBar.fillAmount = value;
    }

    private void ChangeLabel(Actor a)
    {
        if (a == null) return;
        _actorActions = a.GetComponent<Actions>();
    }
    
    private void UpdateSkipTime(float time)
    {
        _fillTurnSkipImage.fillAmount = time / _gameEvents.TurnSkipHoldTime;
    }
}