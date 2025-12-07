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
    [SerializeField] private bool _isActing;

    private void CheckActions()
    {
        foreach (ActionKey ak in _abilityKeys)
        {
            if (Input.GetKeyDown(ak.Key)) ak.Ability.SpawnAbility(gameObject);
        }
    }

    private void Update()
    {
        if (_isActing) CheckActions();
    }
}
