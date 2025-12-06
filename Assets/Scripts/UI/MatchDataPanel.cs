using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchDataPanel : MonoBehaviour
{
    [SerializeField] private GameVariableSettings _gameEvents;
    [SerializeField] private Image _fillTurnSkipImage;
    [SerializeField] private TextMeshProUGUI _actorLabel;
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
    }

    private void OnDisable()
    {
        _gameEvents.currentSkipHoldTime -= UpdateSkipTime;
        _gameEvents.turnActorChange -= ChangeLabel;

    }

    private void ChangeLabel(Actor a)
    {
        _actorLabel.text = a.name + $" (team ID {a.teamID})";
    }
    private void UpdateSkipTime(float time)
    {
        _fillTurnSkipImage.fillAmount = time / _gameEvents.TurnSkipHoldTime;
    }
}