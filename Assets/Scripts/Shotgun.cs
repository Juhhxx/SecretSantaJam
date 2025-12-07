using NaughtyAttributes;
using Unity.Collections;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private PlayerMovement _player;

    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _bulletSpreadAngle;

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    public void ShotTest() => ShotSpread(_player.CurrentPosition, _player.CurrentDirection);

    public void ShotSpread(Vector2 position, Vector2 directionAimed)
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

        GameObject instantiatedBullet = Instantiate(_bulletPrefab, position + (directionAimed * 1), Quaternion.identity);
        Rigidbody2D bulletRB = instantiatedBullet.GetComponent<Rigidbody2D>();
        bulletRB.linearVelocity = directionAimed * _bulletSpeed;
    }
    private Vector2 Rotate(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2 ( 
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos
        );
    }
}
