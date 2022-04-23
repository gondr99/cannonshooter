using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour, IDamageable
{
    [SerializeField] private int _hp;

    SpriteRenderer _spriteRenderer;

    public void OnDamage(int damage, GameObject damageDealer, Vector2 direction, float force)
    {
        BoxExplosion(direction, force);
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            
        }
    }

    private void BoxExplosion(Vector2 dir, float power)
    {
        float x = _spriteRenderer.bounds.size.x;
        float width = _spriteRenderer.sprite.texture.width;

        GameManager.instance.MakeDebris(width / x, _spriteRenderer.sprite, transform.position, dir, power);

        Destroy(gameObject);
    }
}
