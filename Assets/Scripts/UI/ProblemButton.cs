using System;
using TMPro;
using UnityEngine;

public class ProblemButton : CustomButton, IButtonWithNewButton
{
    [SerializeField] private TextMeshProUGUI _buttonText;

    [SerializeField] private CustomButton _buttonForChange;
    [SerializeField] private CustomButton _secondButtonForChange;
    [SerializeField] private ChangeCondition[] _changeConditions;

    public ChangeCondition[] ChangeConditions => _changeConditions;
    public CustomButton ButtonForChange {get; private set;}

    private GamesConditionChecker _conditionsChecker;
    private ButtonChangeController _changeController;

    private ProblemConfig _problem;
    private ClicksBlocker _clicksBlocker;

    protected override void Init()
    {
        base.Init();
        _conditionsChecker = ServiceLocator.Instance.Get<GamesConditionChecker>();
        _changeController = ServiceLocator.Instance.Get<ButtonChangeController>();
        _clicksBlocker = ServiceLocator.Instance.Get<ClicksBlocker>();

        ButtonForChange = _buttonForChange;
        _changeController.ChangeButtonToNewButton(this);
        _changeController.AddChangeButton(this);

        SetText();
    }

    public void SetProblem(ProblemConfig problem)
    {
        _problem = problem;
        if (_changeController != null) _changeController.ChangeButtonToNewButton(this);
    }

    protected override void ClickCallback()
    {
        base.ClickCallback();
        _clicksBlocker.UnblockClicks();
        _problem.Apply();
        _buttonService.CloseWindow(WindowType.ProblemWindow);
    }

    private void SetText()
    {
        _buttonText.text = "Ok";
    }

    public bool CheckButtonChangeCondition()
    {
        if (_secondButtonForChange != null && !_conditionsChecker.IsEnoughCoins(_problem.CoinsRequirements))
        {
            ButtonForChange = _secondButtonForChange;
            return true;
        }

        //if (ButtonForChange != _buttonForChange) ButtonForChange = _buttonForChange;
        return !_conditionsChecker.IsEnoughCoinsInHands(_problem.CoinsRequirements);
    }

    private void OnDestroy()
    {
        if ( _changeController != null )
        {
            _changeController.RemoveChangeButton(this);
        }
    }
}
