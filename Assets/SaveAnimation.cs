using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveAnimation : ObjectForInitialization, ISubscribable
{
    [SerializeField] private Image _image; 
    private DataSaver _saver;

    private SubscribeController _subscribeController;

    private Sequence _sequence;

    public override void Init()
    {
        _saver = ServiceLocator.Instance.Get<DataSaver>();
        _subscribeController = ServiceLocator.Instance.Get<SubscribeController>();
        _image.transform.localRotation = Quaternion.identity;

        Subscribe();
    }

    private void ActivateAnimation()
    {
        _image.gameObject.SetActive(true);
        _sequence = DOTween.Sequence();

        _sequence
            .Append(_image.transform.DOLocalRotate(new Vector3(0,0,180), 0.2f))
            .Append(_image.transform.DOLocalRotate(new Vector3(0,0,360), 0.2f));
        _sequence.SetLoops(-1);
        _sequence.Play();
    }

    private void KillAnimation()
    {
        _image.gameObject.SetActive(false);
        _sequence.Kill();
        _image.transform.localRotation = Quaternion.identity;
    }

    public void Subscribe()
    {
        _subscribeController.AddSubscribable(this);

        _saver.OnStartSaving += ActivateAnimation;
        _saver.OnEndSaving += KillAnimation;
    }

    public void Unsubscribe()
    {
        _saver.OnStartSaving -= ActivateAnimation;
        _saver.OnEndSaving -= KillAnimation;
    }
}
