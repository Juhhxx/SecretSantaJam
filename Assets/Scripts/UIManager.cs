using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] private GameVariableSettings _gameSettings;
    
    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }
    
    
}