using CoinsCounter;
using UnityEngine;

public class ServiceLocatorUILoader : MonoBehaviour
{
    [SerializeField] private TimeIndicator _timeIndicator;
    [SerializeField] private CoinsIndicator _handsCoinsIndicator;
    [SerializeField] private CoinsIndicator _bankCoinsIndicator;
    [SerializeField] private CoinsChangeView _handsCoinsChangeView;
    [SerializeField] private CoinsChangeView _bankCoinsChangeView;

    [SerializeField] private ClicksBlocker _clicksBlocker;
    [SerializeField] private IconComponentsRandomizer _iconComponentsRandomizer;

    [SerializeField] private AudioDataConfigs _audioDataConfigs;
    [SerializeField] private AudioPlayer _audioPlayerPrefab;
    private AudioPlayer _audioPlayer;

    [SerializeField] private Window[] _windows;

    [SerializeField] private Notification _ordersNotification;
    [SerializeField] private Notification _deliveryOrdersNotification;

    public void BindUI()
    {
        BindAudioServices();
        BindNotificationServices();

        BindClicksBlocker();
        BindWindowService();
        BindWindowActivator();

        BindIconComponentsRandomizer();

        BindCoinsCounters();
        BindTimeController();
        BindButtonService();
    }

    private void BindClicksBlocker()
    {
        ServiceLocator.Instance.Register(_clicksBlocker);
    }

    private void BindNotificationServices()
    {
        var notificationService = new NotificationService(new Notification[] { _ordersNotification, _deliveryOrdersNotification });

        var notificationController = new NotificationController(notificationService);
        ServiceLocator.Instance.Register(notificationController);
    }

    private void BindAudioServices()
    {
        var audioService = new AudioService(_audioDataConfigs);

        _audioPlayer = Instantiate(_audioPlayerPrefab);
        _audioPlayer.Init(audioService);

        ServiceLocator.Instance.Register(_audioPlayer);
    }

    private void BindIconComponentsRandomizer()
    {
        ServiceLocator.Instance.Register(_iconComponentsRandomizer);
    }

    private void BindWindowService()
    {
        var windowService = new WindowService(_windows);
        ServiceLocator.Instance.Register(windowService);
    }

    private void BindWindowActivator()
    {
        var windowActivator = new WindowActivator();
        ServiceLocator.Instance.Register(windowActivator);
    }

    private void BindButtonService()
    {
        var buttonService = new ButtonService();
        ServiceLocator.Instance.Register(buttonService);
    }

    private void BindCoinsCounters()
    {
        var bankCoinsCounter = new BankCoinsCounter(_bankCoinsIndicator, _bankCoinsChangeView);
        var handCoinsCounter = new HandsCoinsCounter(_handsCoinsIndicator, _handsCoinsChangeView);

        ServiceLocator.Instance.Register(bankCoinsCounter);
        ServiceLocator.Instance.Register(handCoinsCounter);
    }

    private void BindTimeController()
    {
        var timeController = new TimeController(_timeIndicator);
        ServiceLocator.Instance.Register(timeController);
    }
}