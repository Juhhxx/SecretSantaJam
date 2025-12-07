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
                ak.Ability.SpawnAbility(gameObject);
                _actions.TakeCombatPoints(1);
                return;
            }
        }
    }

    private void Update()
    {
        if (_actions.Actor.IsTurn)
        {
            CheckActions();
        }
    }
}
