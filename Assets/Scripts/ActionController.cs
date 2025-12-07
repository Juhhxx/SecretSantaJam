using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionController : MonoBehaviour
{
    [Serializable]
    public struct ActionKey
    {
        public KeyCode Key;
        public AbilityInfo Ability;
    }

    [SerializeField] private List<ActionKey> _abilityKeys;

    private Actions _actions;

    private void Awake()
    {
        _actions = GetComponent<Actions>();
    }

    private void CheckActions()
    {
        foreach (ActionKey ak in _abilityKeys)
        {
            if (Input.GetKeyDown(ak.Key))
            {
                GameObject obj = ak.Ability.SpawnAbility(gameObject);
                
                var component = obj.AddComponent<DestroyMessage>();
                component.onDestroy += UseCombatPoint;
                return;
            }
        }
    }

    private void UseCombatPoint()
    {
        _actions.TakeCombatPoints(1);
    }

    private void Update()
    {
        if (_actions.Actor.IsTurn && _actions.CombatPointsUsed < _actions.MaxCombatPoints)
        {
            CheckActions();
        }
    }
}
