using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour, IPoolElement<Goal>
{
    private int _id;
    private int _cost;
    private int _time;

    private int _remainTime;

    #region View

    [SerializeField] private TextMeshProUGUI _titleText;

    [SerializeField] private Image _timeProgressImage;
    [SerializeField] private TextMeshProUGUI _remainingTimeText;

    [SerializeField] private TextMeshProUGUI _rewardText;

    private GoalView _view;



    #endregion

    public bool IsFree => !gameObject.activeInHierarchy;
    public Goal Element => this;

    public void InitInstance()
    {
        Release();
        _view = new GoalView(_titleText, _timeProgressImage, _remainingTimeText, _rewardText);
    }

    public void InitVariant(int id, int cost, int time)
    {
        _id = id;
        _cost = cost;
        _time = time;

        _remainTime = time;

        _view.SetView(id, cost, time, time);
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
