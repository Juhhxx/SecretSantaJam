using System;
using NaughtyAttributes;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameVariableSettings _settings;

    public Transform Target;
    private bool onTarget = false;

    private void Awake()
    {
        if (_settings == null)
        {
            Debug.LogError("Settings needs to be set");
        }
    }

    private void OnEnable()
    {
        _settings.turnActorChange += FocusActor;
    }

    private void OnDisable()
    {
        _settings.turnActorChange -= FocusActor;
    }

    // Force camera position on target
    public void ForceToTarget(Actor a)
    {
        FocusActor(a);
        Vector3 position = Target.position;
        position.z = transform.position.z;
        transform.position = position;
        onTarget = true;
    }

    public void FocusActor(Actor a)
    {
        if (a == null)
        {
            return;
        }


        onTarget = false;
        Target = a.gameObject.transform;
        Vector2 targetPos = Target.position;
        if (LeanTween.isTweening(gameObject))
        {
            LeanTween.cancel(gameObject);
        }

        var seq = LeanTween.sequence();
        seq.append(0.4f);
        seq.append(
            transform.LeanMove(targetPos, _settings.CameraTurnFollowTime).setEaseInCubic().setEaseOutCubic()
                .setOnComplete(() => onTarget = true));
    }
}