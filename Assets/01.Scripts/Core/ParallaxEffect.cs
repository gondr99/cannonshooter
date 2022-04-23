using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private float _parallaxRatio;
    private Camera _mainCam;
    private float length, startPos;
    private void Start()
    {
        _mainCam = CameraManager.instance.MainCam;
        startPos = transform.position.x; //���� ��ġ

        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        length = sr.sprite.bounds.size.x; //�ʺ�
    }

    private void FixedUpdate()
    {
        float dist = _mainCam.transform.position.x * _parallaxRatio;

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        float temp = _mainCam.transform.position.x * (1 - _parallaxRatio);
        if (temp > startPos + length) startPos += length; //�ϳ� ���ؼ� �̵�
        else if(temp < startPos - length) startPos -= length;
    }
}
