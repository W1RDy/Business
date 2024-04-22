using I2.Loc;
using System;
using TMPro;
using UnityEngine;

public class OpenGameResultsButton : CustomButton
{
    [SerializeField] private TextMeshProUGUI _buttonText; 
    protected GameController _gameController;

    protected override void ClickCallback()
    {
        base.ClickCallback();
        OpenResults();
    }

    protected void OpenResults()
    {
        if (_gameController == null)
        {
            _gameController = ServiceLocator.Instance.Get<GameController>();
            SetText("Show results");
        }
        _gameController.FinishGame();
    }

    private void SetText(string key)
    {
        _buttonText.text = LocalizationManager.GetTranslation(key);
    }
}