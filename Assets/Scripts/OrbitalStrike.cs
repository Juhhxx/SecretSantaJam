using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class OrbitalStrike : MonoBehaviour
{
    [SerializeField] private GameObject _nukeSitePrefab;
    private Vector2 mousePosition;
    private Vector2 targetedLocation;
    private bool hasSelectedTarget = false;
    private void Update()
    {
        TrackMouse();
    }
    private void TrackMouse()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Debug.Log(mousePosition);
        if (Mouse.current.leftButton.wasPressedThisFrame && !hasSelectedTarget)
        {
            targetedLocation = mousePosition;
            hasSelectedTarget = true;   
            StartCoroutine(LaunchSequence());
            Debug.Log("hasClicked");
        }
    }
    private IEnumerator LaunchSequence()
    {
        GameObject instantiatedNuke = Instantiate(_nukeSitePrefab, targetedLocation, quaternion.identity);
        CircleCollider2D nukeCollider = instantiatedNuke.GetComponent<CircleCollider2D>();
        yield return new WaitForSecondsRealtime(3);
        SpriteRenderer nukeSiteSR = instantiatedNuke.GetComponent<SpriteRenderer>();
        nukeCollider.enabled = true;
        SetAlpha(nukeSiteSR, 1);
    }
    void SetAlpha(SpriteRenderer sr, float alpha)
    {
        Color c = sr.color;
        c.a = alpha;
        sr.color = c;
    }
}
