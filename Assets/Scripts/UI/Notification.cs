using TMPro;
using UnityEngine;

public class Notification : ResetableObjForInit
{
    [SerializeField] private NotificationType _notificationType;

    [SerializeField] private GameObject _view;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private bool _withAudio;

    public NotificationType NotificationType => _notificationType;

    private int _count = 0;
    private bool _isActivated;

    private AudioPlayer _audioPlayer;

    public override void Init()
    {
        base.Init();
    }

    public void AddNotification()
    {
        if (!_isActivated) ActivateNotification();

        _count++;
        _text.text = _count.ToString();
        if (_withAudio) _audioPlayer.PlaySound("Notification");
    }

    private void ActivateNotification()
    {
        _isActivated = true;
        if (_audioPlayer == null) _audioPlayer = ServiceLocator.Instance.Get<AudioPlayer>();
        _view.SetActive(true);
    }

    public void RemoveNotification()
    {
        _count = Mathf.Clamp(--_count, 0, int.MaxValue);
        _text.text = _count.ToString();

        if (_count == 0) DeactivateNotification();
    }

    private void DeactivateNotification()
    {
        if (_isActivated)
        {
            _isActivated = false;
            _view.SetActive(false);
            _count = 0;
        }
    }

    private void RemoveAllNotifications()
    {
        _count = 0;
        _text.text = _count.ToString();

        DeactivateNotification();
    }

    public override void Reset()
    {
        RemoveAllNotifications();
    }
}

