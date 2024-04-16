using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using UnityEngine.UI;

public class Goal : ObjectForInitialization, IPoolElement<Goal>
{
    private int _id;
    private int _cost;
    private int _time;

    private int _remainTime;

    #region View

    [SerializeField] private TextMeshProUGUI _titleText;

    [SerializeField] private Image _timeProgressImage;
    [SerializeField] private TextMeshProUGUI _remainingTimeText;
    [SerializeField] private TextMeshProUGUI _remainingQualityText;

    [SerializeField] private TextMeshProUGUI _rewardText;
    [SerializeField] private SendOrderButton _sendButton;

    private GoalView _view;

    #endregion

    public bool IsFree => !gameObject.activeInHierarchy;
    public Goal Element => this;

    public override void Init()
    {
        base.Init();
        Release();
        _view = new GoalView(_titleText, _timeProgressImage, _remainingTimeText, _rewardText, _remainingQualityText);
    }

    public void InitVariant(Order order)
    {
        _id = order.ID;
        _cost = order.Cost;
        _time = order.Time;

        _remainTime = order.Time;

        _view.SetView(_id, _cost, _remainTime, _time, order.NeededGoods);
        _sendButton.SetOrder(order);
    }

    public void SetRemainTime(int remainTime)
    {
        _remainTime = remainTime;

        _view.SetTime(remainTime, _time);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Release()
    {
        gameObject.SetActive(false);
    }
}
