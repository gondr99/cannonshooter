using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriterRenderer;
    private PolygonCollider2D _polygonCollider2D;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriterRenderer = GetComponent<SpriteRenderer>();
        _polygonCollider2D = GetComponent<PolygonCollider2D>(); 
    }

    public void SetSprite(Sprite sprite)
    {
        _spriterRenderer.sprite = sprite;
        
        if(_polygonCollider2D != null)
        {
            Destroy(_polygonCollider2D);
        }
        _polygonCollider2D = gameObject.AddComponent<PolygonCollider2D>();
        
        //_polygonCollider2D.pathCount = sprite.GetPhysicsShapeCount();

        //List<Vector2> path = new List<Vector2>();
        //for(int i = 0; i < _polygonCollider2D.pathCount; i++)
        //{
        //    path.Clear();
        //    sprite.GetPhysicsShape(i, path);
        //    _polygonCollider2D.SetPath(i, path.ToArray());
        //}
    }

    public void AddForceToDebri(Vector3 force)
    {
        _rigidbody.AddForce(force);
    }
}
