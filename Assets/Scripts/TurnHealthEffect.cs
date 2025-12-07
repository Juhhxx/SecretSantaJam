using System;
using NaughtyAttributes;
using UnityEngine;

public class TurnHPEffect : Actor
{
    [SerializeField] private float _amount;
    [SerializeField] private bool _takeLife;
    [SerializeField] private float _radius;
    [SerializeField] private bool _showRadius;

    [SerializeField] private ParticleSystem ps;

    
    public override void StartTurn()
    {
        base.StartTurn();
        
        var seq = LeanTween.sequence();
        seq.append(0.6f);
        seq.append(() =>
        {
            transform.LeanScale(Vector3.one * 1.2f, 0.3f).setEasePunch();
            GiveDamage();
        });

        seq.append(1f);
        seq.append(() =>
        {
            FinishTurnActor();
        });
    }

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
        
        ps?.Play();
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
