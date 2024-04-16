﻿using System;
using UnityEngine;

public class ConfirmHandler : ClassForInitialization
{
    private SuggestionGenerator _suggestionGenerator;

    private ButtonService _buttonService;

    private Action _rememberedAction;
    private Suggestion _suggestion;

    public ConfirmHandler() : base() { }

    public override void Init()
    {
        _buttonService = ServiceLocator.Instance.Get<ButtonService>();
        _suggestionGenerator = ServiceLocator.Instance.Get<SuggestionGenerator>();
    }

    public void ConfirmAction(Action action, ConfirmType confirmType, int skipTime, int wasteCoins)
    {
        RememberActionForSuggestion(action);
        OpenSuggestionWindow(confirmType, skipTime, wasteCoins);
    }

    public void ConfirmAction(Action action, ConfirmType confirmType, int skipTime)
    {
        if (confirmType != ConfirmType.SkipTime) throw new System.ArgumentException("Should add wasteCoins parameter");
        Debug.Log("ConstructPC");
        ConfirmAction(action, confirmType, skipTime, 0);
    }

    public void ConfirmAction(Action action, ProblemConfig problem)
    {
        RememberActionForSuggestion(action);
        OpenSuggestionWindowByProblem(problem);
    }

    private void RememberActionForSuggestion(Action action)
    {
        _rememberedAction = action;
    }

    private void OpenSuggestionWindow(ConfirmType confirmType, int skipTime, int wasteCoins)
    {
        _suggestion = _suggestionGenerator.GenerateSuggestion(confirmType, skipTime, wasteCoins);

        _suggestion.Applied += ConfirmSuggestion;
        _suggestion.Skipped += CancelSuggestion;

        if (confirmType == ConfirmType.DistributeCoins)
            _buttonService.OpenWindow(WindowType.DistributeSuggestionWindow);
        else
            _buttonService.OpenWindow(WindowType.SuggestionWindow);
    }

    private void OpenSuggestionWindowByProblem(ProblemConfig problem)
    {
        _suggestion = _suggestionGenerator.GenerateSuggestionByProblem(problem);

        _suggestion.Applied += ConfirmSuggestion;
        _suggestion.Skipped += CancelSuggestion;

        _buttonService.OpenWindow(WindowType.DistributeSuggestionWindow);
    }

    private void ConfirmSuggestion()
    {
        _rememberedAction?.Invoke();
        CancelSuggestion();
    }

    private void CancelSuggestion()
    {
        if (_rememberedAction != null)
        {
            _rememberedAction = null;

            if (_suggestion != null)
            {
                _suggestion.Applied -= ConfirmSuggestion;
                _suggestion.Skipped -= CancelSuggestion;
                _suggestion = null;
            }
        }
    }
}

public enum ConfirmType
{
    SkipTime,
    SkipTimeAndWasteCoins,
    DistributeCoins,
    SolveProblemWithDistributeCoins
}