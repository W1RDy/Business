using TMPro;
using UnityEngine;

public interface ILinksService
{     
    public DarknessAnimationController _darknessAnimationController { get; }
}

public interface IUILinksService
{
    public TimeIndicator _timeIndicator { get; }
    public CoinsIndicator _handsCoinsIndicator { get; }
    public CoinsIndicator _bankCoinsIndicator { get; }
    public CoinsChangeView _handsCoinsChangeView { get; }
    public CoinsChangeView _bankCoinsChangeView { get; }

    public ClicksBlocker _clicksBlocker { get; }


    public Window[] _windows { get; }

    public Notification _ordersNotification { get; }
    public Notification _deliveryOrdersNotification { get; }

    public ShowADSForCoins _coinsADSButton { get; }
    public ShowADSForContinueButton _continueButton { get; }

    public TextMeshProUGUI _ordersTutorialText { get; }
    public TextMeshProUGUI _deliveryTutorialText { get; }
    public TextMeshProUGUI _confirmDeliveryTutorialText { get; }
    public TextMeshProUGUI _sendTutorialText { get; }
    public TextMeshProUGUI _distributeCoinsTutorialText { get; }
}

public interface IEntitiesLinksService
{
    public CompositeOrder _compositeDeliveryOrder { get; }
    #region PoolsTransforms

    public RectTransform _orderPoolContainer { get; }
    public Transform _orderParent {  get; }

    public RectTransform _goalPoolContainer { get; }
    public Transform _goalParent { get; }

    public RectTransform _deliveryOrderPoolContainer { get; }
    public Transform _deliveryOrderParent { get; }

    public RectTransform _goodsPoolContainer { get; }
    public Transform _goodsParent { get; }

    #endregion
}
