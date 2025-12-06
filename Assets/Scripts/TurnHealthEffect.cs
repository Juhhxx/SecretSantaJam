using NaughtyAttributes;
using UnityEngine;

public class TurnHPEffect : MonoBehaviour
{
    [SerializeField] private float _amount;
    [SerializeField] private bool _takeLife;
    [SerializeField] private float _radius;
    [SerializeField] private bool _showRadius;

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void GiveDamage()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, _radius, transform.forward);

        foreach (RaycastHit2D hit in hits)
        {
            HealthSystem hs = hit.collider.gameObject.GetComponent<HealthSystem>();

            if (hs != null)
            {
                if (_takeLife)
                    hs.TakeDamage(_amount);
                else
                    hs.Heal(_amount);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_showRadius)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
        
}
