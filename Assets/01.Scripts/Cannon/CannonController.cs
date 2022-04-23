using System;
using UnityEngine;
using UnityEngine.Events;

public class CannonController : MonoBehaviour
{
    public enum CannonState : short
    {
        Idle = 0,
        Moving = 1,
        Charging = 2,
        Fire = 3
    }
    [SerializeField] private float _rotateSpeed = 5f;
    [SerializeField] private float _maxfirePower = 800f;
    [SerializeField] private float _charginSpeed = 300f; //√ ¥Á 300
    [SerializeField] private Ball _ballPrefab;

    public UnityEvent<float> OnAngleChange = null;
    public UnityEvent<float> OnPowerChange = null;
    public UnityEvent OnFire = null;
    public UnityEvent OnExplosion = null;

    private Transform _barrelTrm;
    private Transform _firePosTrm;

    private float _currentFirePower = 0f;

    private float _currentRotate = 0f;

    
    [SerializeField] private CannonState _state = CannonState.Idle;

    private void Awake()
    {
        _barrelTrm = transform.Find("Barrel");
        _firePosTrm = _barrelTrm.Find("FirePos");
    }
    void Update()
    {
        HandleMove();
        HandleFire();
    }

    private void HandleFire()
    {
        if (Input.GetButtonDown("Jump") && (int)_state  < 2)
        {
            _state = CannonState.Charging;
        }
        if(Input.GetButton("Jump") && _state == CannonState.Charging)
        {
            _currentFirePower += _charginSpeed * Time.deltaTime;
            _currentFirePower = Mathf.Clamp(_currentFirePower, 0f, _maxfirePower);
            OnPowerChange?.Invoke(_currentFirePower / _maxfirePower);
        }
        if(Input.GetButtonUp("Jump") && _state == CannonState.Charging)
        {
            FireCannon();
        }
    }

    private void FireCannon()
    {
        _state = CannonState.Fire;

        Ball ball = Instantiate(_ballPrefab, _firePosTrm.position, Quaternion.identity) as Ball;
        ball.Fire(_firePosTrm.right, _currentFirePower);
        ball.OnCompleteExplosion += () =>
        {
            _state = CannonState.Idle;
            _currentFirePower = 0;
            OnExplosion?.Invoke();
            CameraManager.instance.ShakeCam(4, 0.6f);
        };

        OnFire?.Invoke();
        CameraManager.instance.ShakeCam(2 * _currentFirePower / 400, 0.6f);
    }

    private void HandleMove()
    {
        if(_state == CannonState.Idle || _state == CannonState.Moving)
        {
            float y = Input.GetAxisRaw("Vertical");
            _currentRotate += y * Time.deltaTime * _rotateSpeed;
            _currentRotate = Mathf.Clamp(_currentRotate, 0f, 90f);

            _barrelTrm.transform.rotation = Quaternion.Euler(0, 0, _currentRotate);
            
            if( Mathf.Abs(y) > 0f)
            {
                _state = CannonState.Moving;
                OnAngleChange?.Invoke(_currentRotate);
            }
            else
            {
                _state = CannonState.Idle;
            }
        }
    }

}
