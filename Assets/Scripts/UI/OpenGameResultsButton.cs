using I2.Loc;
using System;
using TMPro;
using UnityEngine;

public class OpenGameResultsButton : CustomButton
{
    [SerializeField] private TextMeshProUGUI _buttonText; 
    protected GameController _gameController;

    public override void Init()
    {
        base.Init();
        _gameController = ServiceLocator.Instance.Get<GameController>();
        SetText("Show results");
    }

    protected override void ClickCallback()
    {
        base.ClickCallback();
        OpenResults();
    }

    protected void OpenResults()
    {
        _gameController.FinishGame();
    }

    private void SetText(string key)
    {
        _buttonText.text = LocalizationManager.GetTranslation(key);
    }
}