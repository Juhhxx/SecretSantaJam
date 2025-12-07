using System;
using UnityEngine;

public class DestroyMessage : MonoBehaviour
{

    public event Action onDestroy;
    private void OnDestroy()
    {
        onDestroy?.Invoke();
    }
}