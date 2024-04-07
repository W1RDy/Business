using OpenCover.Framework.Model;
using System.Collections;
using TMPro;
using UnityEngine;

public class ResultsWindow : Window
{
    #region Texts

    [Header("Texts")]

    [SerializeField] private TextMeshProUGUI _titleText;

    [SerializeField] private TextMeshProUGUI _costsText1;
    [SerializeField] private TextMeshProUGUI _costsText2;

    [SerializeField] private TextMeshProUGUI _incomeText1;
    [SerializeField] private TextMeshProUGUI _incomeText2;

    [SerializeField] private TextMeshProUGUI _summaryTimeText;
    [SerializeField] private TextMeshProUGUI _summaryCoinsText;

    #endregion

    private ResultsViewController _resultsViewController;

    [Header("Colors")]

    [SerializeField] private Color _positiveColor;
    [SerializeField] private Color _negativeColor;

    public void SetResults(IResults results)
    {
        if (_resultsViewController == null)
            _resultsViewController = new ResultsViewController(_titleText, _costsText1, _costsText2, _incomeText1, _incomeText2, _summaryTimeText, _summaryCoinsText, _positiveColor, _negativeColor);

        _resultsViewController.SetResults(results);
    }
}
