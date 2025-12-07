using UnityEngine;
using UnityEngine.UI;

public class Portrait_UI : MonoBehaviour
{
    [SerializeField] private Image _portraitEdges;
    [SerializeField] private RawImage _image;

    [SerializeField] private Color[] _teamColors;
    public void SetPortrait(Actor a, Texture2D portrait)
    {
        _portraitEdges.color = _teamColors[a.teamID];
        _image.texture = portrait;
    }
}