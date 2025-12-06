using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] private float _damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthSystem hs = collision.gameObject.GetComponent<HealthSystem>();

        if (hs != null)
        {
            hs.TakeDamage(_damage);
        }
    }
}
