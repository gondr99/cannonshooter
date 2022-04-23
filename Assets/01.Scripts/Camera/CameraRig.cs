using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private PolygonCollider2D _confiner;

    private Vector3 _boundMax;
    private Vector3 _boundMin;  
    private void Start()
    {
        _boundMax = _confiner.bounds.max;
        _boundMin = _confiner.bounds.min;
    }

    private void Update()
    {
        HandleMove();
    }

    private void HandleMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x + _moveSpeed * x * Time.deltaTime, _boundMin.x, _boundMax.x);
        transform.position = pos;
    }

}
