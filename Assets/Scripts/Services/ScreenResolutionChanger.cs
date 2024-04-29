using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class ScreenResolutionChanger : ObjectForInitialization
{
    [SerializeField] private CanvasScaler _canvas;

    public override void Init()
    {
        base.Init();
        var deviceType = YandexGame.EnvironmentData.deviceType;
        if (deviceType == "mobile" || deviceType == "tablet")
        {
            _canvas.referenceResolution = new Vector2(1600, 900);
        }
    }
}
