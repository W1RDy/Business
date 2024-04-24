using System;
using UnityEngine;

namespace CoinsCounter
{
    public abstract class CoinsCounter : ResetableClassForInit, IService, ICoinsCounter, ISubscribable
    {
        protected int _coins;

        public int Coins => _coins;

        protected DifficultyController _difficultyController;

        protected CoinsChangeView _changeView;
        private CoinsIndicator _coinsIndicator;

        public event Action CoinsChanged;

        protected SubscribeController _subscribeController;
        protected DataSaver _dataSaver;

        protected Action SaveDelegate;

        public CoinsCounter(CoinsIndicator coinsIndicator, CoinsChangeView coinsChangeView) : base()
        {
            _coinsIndicator = coinsIndicator;
            _changeView = coinsChangeView;
        }

        public override void Init()
        {
            base.Init();
            _difficultyController = ServiceLocator.Instance.Get<DifficultyController>();
            _dataSaver = ServiceLocator.Instance.Get<DataSaver>();

            _subscribeController = ServiceLocator.Instance.Get<SubscribeController>();
            Subscribe();
        }

        public virtual void AddCoins(int value)
        {
            _coins += value;
            if (value != 0)
            {
                UpdateIndicator();
                _changeView.ActivateChangeView(value);
                InvokeCoinsEvent();
            }
        }

        public virtual void RemoveCoins(int value)
        {
            if (_coins >= value)
            {
                _coins = Mathf.Clamp(_coins - value, 0, _coins);
                if (value != 0)
                {
                    _changeView.ActivateChangeView(-value);
                    UpdateIndicator();
                    InvokeCoinsEvent();
                }
            }
        }

        public virtual void ChangeCoins(int value)
        {
            if (_coins > -1)
            {
                var difference = value - _coins;
                _coins = value;

                if (difference != 0)
                {
                    _changeView.ActivateChangeView(difference);
                    UpdateIndicator();
                    InvokeCoinsEvent();
                }
            }
        }

        public virtual void SetCoins(int value)
        {
            if (value > -1)
            {
                _coins = value;
                UpdateIndicator();
                InvokeCoinsEvent();
            }
        }

        protected void UpdateIndicator()
        {
            _coinsIndicator.SetCoins(_coins);
        }

        protected void InvokeCoinsEvent()
        {
            CoinsChanged?.Invoke();
        }

        public virtual void Subscribe()
        {
            _subscribeController.AddSubscribable(this);
        }

        public void Unsubscribe()
        {
            _dataSaver.OnStartSaving -= SaveDelegate;
        }
    }

    public class HandsCoinsCounter : CoinsCounter
    {
        private AudioPlayer _audioPlayer;

        public HandsCoinsCounter(CoinsIndicator coinsIndicator, CoinsChangeView changeView) : base(coinsIndicator, changeView) 
        {

        }

        public override void Init()
        {
            base.Init();
            _coins = _difficultyController.StartCoinsInHands;
            UpdateIndicator();
            _audioPlayer = ServiceLocator.Instance.Get<AudioPlayer>();
        }

        public override void AddCoins(int value)
        {
            base.AddCoins(value);
            if (value != 0) _audioPlayer.PlaySound("EarnCoins");
        }

        public override void RemoveCoins(int value)
        {
            base.RemoveCoins(value);
            if (value != 0) _audioPlayer.PlaySound("WasteCoins");
        }

        public override void ChangeCoins(int value)
        {
            var difference = _coins - value;
            base.ChangeCoins(value);
            if (difference != 0) _audioPlayer.PlaySound("EarnCoins");
        }



        public override void Subscribe()
        {
            base.Subscribe();
            SaveDelegate = () =>
            {
                _dataSaver.SaveHandCoins(Coins);
            };
            _dataSaver.OnStartSaving += SaveDelegate;
        }

        public override void Reset()
        {
            _coins = _difficultyController.StartCoinsInHands;
            UpdateIndicator();
            InvokeCoinsEvent();
        }
    }

    public class BankCoinsCounter : CoinsCounter
    {
        public BankCoinsCounter(CoinsIndicator coinsIndicator, CoinsChangeView changeView) : base(coinsIndicator, changeView)
        {
            
        }

        public override void Init()
        {
            base.Init();
            _coins = _difficultyController.StartCoinsInBank;
            UpdateIndicator();
        }

        public void DoubleCoins()
        {
            var prevCoins = _coins;
            _coins = (int)Mathf.Ceil(_coins * 1.5f);

            if (prevCoins != _coins) _changeView.ActivateChangeView(_coins - prevCoins);
            UpdateIndicator();
            InvokeCoinsEvent();
        }

        public override void Subscribe()
        {
            base.Subscribe();
            SaveDelegate = () => _dataSaver.SaveBankCoins(Coins);
            _dataSaver.OnStartSaving += SaveDelegate;
        }

        public override void Reset()
        {
            _coins = _difficultyController.StartCoinsInBank;
            UpdateIndicator();
            InvokeCoinsEvent();
        }
    }
}

