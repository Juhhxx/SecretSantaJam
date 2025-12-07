using System.Collections;
using NaughtyAttributes;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shotgun : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletPivot;

    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _bulletSpreadAngle;

    private Vector3 _mousePos;
    private Vector2 _shotDir;
    private bool _useController = false;

    private void Start()
    {
        DoShot();

        Actor actor = GetComponentInParent<Actor>();

        _useController = actor.teamID == 1;
    }

    private void Update()
    {
        if (!_useController)
        {
            _mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            _mousePos.z = 0f;

            _shotDir = Vector3.Normalize(_mousePos - transform.position);
        }
        else
        {
            _shotDir.x = Input.GetAxis("AimX");
            _shotDir.y = Input.GetAxis("AimY");

            _shotDir = _shotDir.normalized;
        }

        Debug.Log(_shotDir);

        Vector3 angles = transform.rotation.eulerAngles;

        angles.z = Mathf.Atan2(_shotDir.y, _shotDir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(angles);

        MirrorHand();
    }

    private void MirrorHand()
    {
        if (_bulletPivot.position.x >= transform.position.x)
        {
            _bulletPivot.GetComponentInParent<SpriteRenderer>().flipX = false;
        }
        else
        {
            _bulletPivot.GetComponentInParent<SpriteRenderer>().flipX = true;
        }
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    public void DoShot()
    {
        StopAllCoroutines();
        StartCoroutine(ShootCR());
    }

    private IEnumerator ShootCR()
    {
        if (_useController)
            yield return new WaitUntil(() => Input.GetButtonDown("Fire1"));
        else
            yield return new WaitUntil(() => Input.GetButtonDown("Fire2"));

        ShotSpread(_bulletPivot.position, _shotDir);

        yield return new WaitForEndOfFrame();

        Destroy(gameObject);
    }

    private void ShotSpread(Vector2 position, Vector2 directionAimed)
    {
        Shoot(position, directionAimed);

        Vector2 divergence1 = Rotate(directionAimed, _bulletSpreadAngle);
        Vector2 divergence2 = Rotate(directionAimed, -_bulletSpreadAngle);
    
        Shoot(position, divergence1);
        Shoot(position, divergence2);
    }

    private void Shoot(Vector2 position, Vector2 directionAimed)
    {
        Debug.Log($"SHOT DIR : {directionAimed}");

        GameObject instantiatedBullet = Instantiate(_bulletPrefab, position, Quaternion.identity);
        Rigidbody2D bulletRB = instantiatedBullet.GetComponent<Rigidbody2D>();

        bulletRB.linearVelocity = directionAimed * _bulletSpeed;
    }

    private Vector2 Rotate(Vector2 v, float degrees)
    {
        // Convert Desired Rotation to Radians
        float rad = degrees * Mathf.Deg2Rad;

        // Get Cos and Sin Values of the Desired Angle
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        // Create New Rotation Vector Using the 2D Rotation Matrix
        return new Vector2 ( 
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos
        );
    }
}
