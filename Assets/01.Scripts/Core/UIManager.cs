using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;
    public TextMeshProUGUI _textMsg;

    private Action _nextAction = null;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Multiple UIManager is running");
        }

        instance = this;
    }

    private void Update()
    {
        if(_nextAction != null && Input.GetButtonDown("Jump"))
        {
            _nextAction();
            _textMsg.DOKill();
            _textMsg.SetText("");
            _nextAction = null;
        }
    }

    public void ShowTextMsg(string text, Action nextAction = null )
    {
        _nextAction = nextAction;
        _textMsg.SetText(text);
        _textMsg.color = Color.white;
        _textMsg.DOFade(0.2f, 1f).SetLoops(-1, LoopType.Yoyo);
    }
}
