using CoinsCounter;
using System;
using UnityEngine;

public class ServiceLocatorUILoader : MonoBehaviour
{
    [SerializeField] private DeviceService _deviceService;
    private IUILinksService _linksService;

    [SerializeField] private IconComponentsRandomizer _iconComponentsRandomizer;

    [SerializeField] private AudioDataConfigs _audioDataConfigs;
    [SerializeField] private AudioPlayer _audioPlayerPrefab;
    private AudioPlayer _audioPlayer;

    public void BindUI()
    {
        _linksService = _deviceService.UILinksService;

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
        ServiceLocator.Instance.Register(_linksService._clicksBlocker);
    }

    private void BindNotificationServices()
    {
        var notificationService = new NotificationService(new Notification[] { _linksService._ordersNotification, _linksService._deliveryOrdersNotification });

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
        var windowService = new WindowService(_linksService._windows);
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
        var bankCoinsCounter = new BankCoinsCounter(_linksService._bankCoinsIndicator, _linksService._bankCoinsChangeView);
        var handCoinsCounter = new HandsCoinsCounter(_linksService._handsCoinsIndicator, _linksService._handsCoinsChangeView);

        ServiceLocator.Instance.Register(bankCoinsCounter);
        ServiceLocator.Instance.Register(handCoinsCounter);
    }

    private void BindTimeController()
    {
        var timeController = new TimeController(_linksService._timeIndicator);
        ServiceLocator.Instance.Register(timeController);
    }
}