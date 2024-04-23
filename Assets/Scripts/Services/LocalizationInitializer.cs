using I2.Loc;
using YG;

public class LocalizationInitializer
{
    public LocalizationInitializer()
    {
        InitializeLocalization();
    }

    private void InitializeLocalization()
    {
        YandexGame.InitLang();
        var languageKey = YandexGame.lang;
        LocalizationManager.CurrentLanguageCode = languageKey;
    }
}