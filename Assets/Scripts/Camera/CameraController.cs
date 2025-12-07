using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameVariableSettings _settings;

    public Transform camera;
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

    private Coroutine cameraMove;
    private LTSeq lastSequence = null;

    public void FocusActor(Actor a)
    {
        if (a == null)
        {
            return;
        }

        CinemachineCamera cam = camera.GetComponent<CinemachineCamera>();

        onTarget = false;
        Target = a.gameObject.transform;
        Vector2 targetPos = Target.position;
        if (LeanTween.isTweening(gameObject))
        {
            LeanTween.cancel(gameObject);
        }

        if (cameraMove != null)
            StopCoroutine(cameraMove);
        cameraMove = StartCoroutine(SetNewTarget(cam, a.transform));
    }

    private IEnumerator SetNewTarget(CinemachineCamera cam, Transform target)
    {
        yield return new WaitForSeconds(0.4f);

        cam.Target = new CameraTarget()
        {
            TrackingTarget = target
        };

        yield return new WaitForSeconds(0.4f);
        onTarget = true;
        cameraMove = null;
    }
}