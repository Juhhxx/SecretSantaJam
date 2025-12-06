using Unity.Collections;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletSpread;
    private void Shoot(Vector2 directionAimed)
    {
        GameObject instantiatedBullet = Instantiate(_bulletPrefab, gameObject.transform.position, Quaternion.identity);
        Rigidbody2D bulletRB = instantiatedBullet.GetComponent<Rigidbody2D>();
        bulletRB.linearVelocity = directionAimed * bulletSpeed;
    }
    public void ShotSpread(Vector2 directionAimed)
    {
        Shoot(directionAimed);

        Vector2 divergence1 = Rotate(directionAimed, bulletSpread);
        Vector2 divergence2 = Rotate(directionAimed, -bulletSpread);
    
        Shoot(divergence1);
        Shoot(divergence2);
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
