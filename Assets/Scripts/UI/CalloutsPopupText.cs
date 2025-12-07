using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class CalloutsPopupText : MonoBehaviour
{
    [SerializeField] private GameVariableSettings _gameEvents;
    [SerializeField] private Callouts _callouts;
    [SerializeField] private TextMeshPro _textInstance;

    private void Awake()
    {
        if (_textInstance == null)
        {
            gameObject.SetActive(false);
            return;
        }

        _textInstance.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _gameEvents.turnActorChange += DoCallout;
    }

    private void OnDisable()
    {
        _gameEvents.turnActorChange -= DoCallout;
    }

    [Button()]
    private void DebugCallout()
    {
        DoCallout(null);
    }

    private LTSeq _seq;

    public void DoCallout(Actor a)
    {
        if (LeanTween.isTweening(_textInstance.rectTransform))
        {
            LeanTween.cancel(_textInstance.gameObject);
        }

        if (_seq != null)
            LeanTween.cancel(_seq.id);

        string callout = _callouts.GetCallout();
        _textInstance.text = callout;
        _textInstance.alpha = 0f;
        _textInstance.ForceMeshUpdate();
        _textInstance.gameObject.SetActive(true);

        _seq = LeanTween.sequence();
        _seq.append(1f);

        Vector3 pos = a == null ? Vector3.zero : a.transform.position + Vector3.up * 2f;
        _textInstance.transform.position = pos;
        _textInstance.transform.SetParent(a.transform);

        LeanTween.alphaVertex(_textInstance.gameObject, 1f, 0.4f).setDelay(1f).setEaseInCirc();
        _seq.append(_textInstance.transform.LeanMoveY(
            _textInstance.transform.position.y + 0.2f, 1.1f).setEaseOutBack());
        _seq.append(LeanTween.alphaVertex(_textInstance.gameObject, 0f, 0.67f).setEaseOutCubic());
        _seq.append(() =>
        {
            _textInstance.alpha = 0f;
            _textInstance.ForceMeshUpdate();
            _textInstance.gameObject.SetActive(false);
            _textInstance.transform.SetParent(transform);
        });
    }
}