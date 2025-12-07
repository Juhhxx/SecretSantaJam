using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MatchManager : MonoBehaviour
{
    [SerializeField] private GameVariableSettings _gameSettings;
    [SerializeField] private bool _startOnAwake = true;
    [SerializeField] private Camera _portraitCamera;
    private float _skipTurnTimer;

    private List<Actor> _pcs = new List<Actor>(); // playable chars
    private List<Actor> _teamA = new List<Actor>();
    private List<Actor> _teamB = new List<Actor>();

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
        GameObject[] pcs = GameObject.FindGameObjectsWithTag("Player");

        foreach (var elf in pcs)
        {
            Actor actor = elf.GetComponent<Actor>();
            if (actor)
            {
                PlayerMovement pm = actor.GetComponent<PlayerMovement>();
                pm.SetTeam(actor.teamID);
                
                actor.initiative = Mathf.CeilToInt(Random.value * 20);
                _pcs.Add(actor);
                
                switch (actor.teamID)
                {
                    case 0:
                        _teamA.Add(actor);
                        break;
                    case 1:
                        _teamB.Add(actor);
                        break;
                }
            }
        }

        TurnManager.instance.OrderByInitiative();

        _pcs = _pcs.OrderBy(x => x.initiative).ToList();
        
        yield return null;
        GenerateElfPortraits();

        yield return new WaitForSeconds(0.6f);
        
        _gameSettings.RaiseGameStart();
        
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

        if (inputTeam <= 2)
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

    public void GenerateElfPortraits()
    {
        if (_portraitCamera == null) return;
        
        Camera camera = GameObject.Instantiate(_portraitCamera);
        Rect rect = new Rect(0, 0, 32, 32);
        
        // Create elf portraits
        foreach (var actor in _pcs)
        {
            
            RenderTexture rt = RenderTexture.GetTemporary(32, 32, 24);
            camera.targetTexture = rt;
            
            Vector3 pos = actor.transform.position + Vector3.up * 0.5f;
            camera.transform.position = new Vector3(pos.x, pos.y, -1f);
            camera.Render();

            RenderTexture.active = rt;
            Texture2D portrait = new Texture2D(32, 32, TextureFormat.RGBA32, false);
            portrait.filterMode = FilterMode.Point;
            
            portrait.ReadPixels(rect, 0, 0);
            portrait.Apply();

            _gameSettings.RaisePortraitGenerated(actor, portrait);
            
            camera.targetTexture = null;
            RenderTexture.active = null;

            rt = null;
            Destroy(rt);
        }
        
        Destroy(camera.gameObject);
    }
}