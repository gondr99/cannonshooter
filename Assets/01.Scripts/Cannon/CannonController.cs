using System;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
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
    [SerializeField] private float _charginSpeed = 300f; //초당 300
    [SerializeField] private Ball _ballPrefab;

    public UnityEvent<float> OnAngleChange = null;
    public UnityEvent<float> OnPowerChange = null;
    public UnityEvent<Transform> OnFire = null;
    public UnityEvent OnExplosion = null;

    private Transform _barrelTrm;
    private Transform _firePosTrm;

    private float _currentFirePower = 0f;

    private float _currentRotate = 0f;
    private bool _isUITurn = false; //UI에서 클릭시 바로 발사되는걸 막아주는 변수

    
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
        if (Input.GetButtonDown("Jump") && (int)_state  < 2 && _isUITurn == false)
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
            ReadyToFire();
        }

        if (_isUITurn)
            _isUITurn = false;
    }

    private void ReadyToFire()
    {
        _state = CannonState.Fire;

        CameraManager.instance.SetCannonCamActive();
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(1f);
        seq.AppendCallback(() =>
        {
            FireCannon();
        });
    }

    private void FireCannon()
    {

        Ball ball = Instantiate(_ballPrefab, _firePosTrm.position, Quaternion.identity) as Ball;
        ball.Fire(_firePosTrm.right, _currentFirePower);
        ball.OnCompleteExplosion += () =>
        {
            UIManager.instance.ShowTextMsg("-계속 진행하려면 Space Bar-", ()=> {
                CameraManager.instance.SetRigCamActive();
                _state = CannonState.Idle;
                _isUITurn = true;
            });
            _currentFirePower = 0;
            OnExplosion?.Invoke();
            CameraManager.instance.ShakeCam(4, 0.6f);

            // 0.1초 대기후 0.2의 스케일로 내려갔다가, 그대로 0.5초 유지하고 다시 1의 스케일로 돌아온다.
            TimeController.instance.ModifyTimeScale(0.2f, 0.1f, () =>
            {
                TimeController.instance.ModifyTimeScale(1f, 0.5f);
            });
        };

        OnFire?.Invoke(ball.transform);
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
