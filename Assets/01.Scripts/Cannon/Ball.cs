using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    [SerializeField] private float _expRadius = 2f;
    public Action OnCompleteExplosion;
    public LayerMask whatIsEnemy;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>(); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        bool isDebri = CheckDamage();

        OnCompleteExplosion?.Invoke();
        GameManager.instance.GenerateExplosionParticle(transform.position, isDebri);
        Destroy(gameObject);
    }

    private bool CheckDamage()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, _expRadius, whatIsEnemy);

        bool isDebri = false;
        foreach(Collider2D col in cols)
        {
            IDamageable damageable = col.GetComponent<IDamageable>();
            if(damageable != null)
            {
                Vector2 dir = col.transform.position - transform.position;
                float power = ((_expRadius + 1) - dir.magnitude) * 200f;
                damageable.OnDamage(1, gameObject, dir.normalized, power);
                isDebri = true;
            }
        }

        return isDebri; //1개이상 터졌다면
    }

    public void Fire(Vector2 dir, float power)
    {
        _rigidbody.AddForce(dir * power);
    }
}
