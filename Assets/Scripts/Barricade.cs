using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Barricade : MonoBehaviour
{
    [SerializeField] private GameObject _barricadePrefab;
    [SerializeField] private float _distanceFromPlayer;
    [SerializeField] private Transform _previewPivot;
    Vector3 _mousePos;
    Vector2 _wallDir;
    private void Start()
    {
        DoWallPrepare();
    }
    private void DoWallPrepare()
    {
        StopAllCoroutines();
        StartCoroutine(WallBuildCR());
        _previewPivot.localPosition = new Vector3(_distanceFromPlayer, 0 , 0);
    }
    private void Update()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        _mousePos.z = 0f;

        _wallDir = Vector3.Normalize(_mousePos - transform.position);

        Vector3 angles = transform.rotation.eulerAngles;

        angles.z = Mathf.Atan2(_wallDir.y, _wallDir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(angles);
    }
    public void BuildWall(Vector3 directionAimed)
    {
        Vector3 angles = Vector3.zero;

        angles.z = Mathf.Atan2(directionAimed.y, directionAimed.x) * Mathf.Rad2Deg; 

        Instantiate(_barricadePrefab, transform.position + (directionAimed * _distanceFromPlayer), Quaternion.Euler(angles));
    }
    private IEnumerator WallBuildCR()
    {
        yield return new WaitUntil(() => Mouse.current.leftButton.wasPressedThisFrame);
        BuildWall(_wallDir);
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }
}
