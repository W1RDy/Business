using TMPro;
using UnityEngine;

public class DesktopLinksUIService : MonoBehaviour, IUILinksService
{
    [SerializeField] private TimeIndicator _timeIndicator;
    [SerializeField] private CoinsIndicator _handsCoinsIndicator;
    [SerializeField] private CoinsIndicator _bankCoinsIndicator;
    [SerializeField] private CoinsChangeView _handsCoinsChangeView;
    [SerializeField] private CoinsChangeView _bankCoinsChangeView;

    [SerializeField] private ClicksBlocker _clicksBlocker;

    [SerializeField] private Window[] _windows;

    [SerializeField] private Notification _ordersNotification;
    [SerializeField] private Notification _deliveryOrdersNotification;

    [SerializeField] private ShowADSForCoins _coinsADSButton;
    [SerializeField] private ShowADSForContinueButton _continueButton;

    [SerializeField] private TextMeshProUGUI _ordersTutorialText;
    [SerializeField] private TextMeshProUGUI _deliveryTutorialText;
    [SerializeField] private TextMeshProUGUI _confirmDeliveryTutorialText;
    [SerializeField] private TextMeshProUGUI _sendTutorialText;
    [SerializeField] private TextMeshProUGUI _distributeCoinsTutorialText;

    TimeIndicator IUILinksService._timeIndicator => _timeIndicator;

    CoinsIndicator IUILinksService._handsCoinsIndicator => _handsCoinsIndicator;

    CoinsIndicator IUILinksService._bankCoinsIndicator => _bankCoinsIndicator;

    CoinsChangeView IUILinksService._handsCoinsChangeView => _handsCoinsChangeView;

    CoinsChangeView IUILinksService._bankCoinsChangeView => _bankCoinsChangeView;

    ClicksBlocker IUILinksService._clicksBlocker => _clicksBlocker;

    Window[] IUILinksService._windows => _windows;

    Notification IUILinksService._ordersNotification => _ordersNotification;

    Notification IUILinksService._deliveryOrdersNotification => _deliveryOrdersNotification;

    ShowADSForCoins IUILinksService._coinsADSButton => _coinsADSButton;

    ShowADSForContinueButton IUILinksService._continueButton => _continueButton;

    TextMeshProUGUI IUILinksService._ordersTutorialText => _ordersTutorialText;

    TextMeshProUGUI IUILinksService._deliveryTutorialText => _deliveryTutorialText;

    TextMeshProUGUI IUILinksService._confirmDeliveryTutorialText => _confirmDeliveryTutorialText;

    TextMeshProUGUI IUILinksService._sendTutorialText => _sendTutorialText;

    TextMeshProUGUI IUILinksService._distributeCoinsTutorialText => _distributeCoinsTutorialText;
}
