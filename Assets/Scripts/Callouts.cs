using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Callouts")]
public class Callouts : ScriptableObject
{
    [SerializeField, TextArea] private List<string> callouts;

    private int lastCallout = -1;

    public string GetCallout()
    {
        List<string> choices = new List<string>(callouts);
        if (lastCallout >= 0)
        {
            choices.RemoveAt(lastCallout);
        }

        int id = Random.Range(0, choices.Count);
        lastCallout = callouts.IndexOf(choices[id]);
        return choices[id];
    }
}