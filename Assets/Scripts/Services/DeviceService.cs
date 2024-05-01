using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class DeviceService : MonoBehaviour, IService
{
    [SerializeField] private GameObject _desktopLinksServiceObj;
    [SerializeField] private GameObject _mobileLinksServiceObj;

    public ILinksService LinksService { get; private set; }
    public IUILinksService UILinksService { get; private set; }
    public IEntitiesLinksService EntitiesLinksService { get; private set;}

    public IInitializable[] _initializables;

    public bool IsDesktop { get; private set; }

    private void Awake()
    {
        var deviceKey = YandexGame.EnvironmentData.deviceType;
#if UNITY_EDITOR

        deviceKey = "desktop";

#endif

        if (deviceKey == "desktop" || deviceKey == "tv")
        {
            _desktopLinksServiceObj.gameObject.SetActive(true);

            LinksService = _desktopLinksServiceObj.GetComponent<ILinksService>();
            UILinksService = _desktopLinksServiceObj.GetComponent<IUILinksService>();
            EntitiesLinksService = _desktopLinksServiceObj.GetComponent<IEntitiesLinksService>();

            IsDesktop = true;
        }
        else
        {
            _mobileLinksServiceObj.gameObject.SetActive(true);

            LinksService = _mobileLinksServiceObj.GetComponent<ILinksService>();
            UILinksService = _mobileLinksServiceObj.GetComponent<IUILinksService>();
            EntitiesLinksService = _mobileLinksServiceObj.GetComponent<IEntitiesLinksService>();

            IsDesktop = false;
        }
    }
}
