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
    private bool _useController;

    private void Start()
    {
        DoWallPrepare();

        Actor actor = GetComponentInParent<Actor>();

        _useController = actor.teamID == 1;
    }
    private void DoWallPrepare()
    {
        StopAllCoroutines();
        StartCoroutine(WallBuildCR());
        _previewPivot.localPosition = new Vector3(_distanceFromPlayer, 0 , 0);
    }

    private void Update()
    {
        if (!_useController)
        {
            _mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            _mousePos.z = 0f;

            _wallDir = Vector3.Normalize(_mousePos - transform.position);
        }
        else
        {
            _wallDir.x = Input.GetAxis("AimX");
            _wallDir.y = Input.GetAxis("AimY");

            _wallDir = _wallDir.normalized;
        }

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
        if (_useController)
            yield return new WaitUntil(() => Input.GetButtonDown("Fire1"));
        else
            yield return new WaitUntil(() => Input.GetButtonDown("Fire2"));

        BuildWall(_wallDir);
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }
}
