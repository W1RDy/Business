using System;
using UnityEngine;

namespace CoinsCounter
{
    public abstract class CoinsCounter : IService, ICoinsCounter
    {
        protected int _coins;

        public int Coins => _coins;

        protected CoinsChangeView _changeView;
        private CoinsIndicator _coinsIndicator;

        public event Action CoinsChanged;

        public CoinsCounter(CoinsIndicator coinsIndicator, CoinsChangeView coinsChangeView)
        {
            _coinsIndicator = coinsIndicator;
            _changeView = coinsChangeView;
        }

        public virtual void AddCoins(int value)
        {
            _coins += value;
            UpdateIndicator();
            _changeView.ActivateChangeView(value);
            InvokeCoinsEvent();
        }

        public virtual void RemoveCoins(int value)
        {
            if (_coins >= value)
            {
                _coins = Mathf.Clamp(_coins - value, 0, _coins);
                _changeView.ActivateChangeView(-value);
                UpdateIndicator();
                InvokeCoinsEvent();
            }
        }

        public virtual void ChangeCoins(int value)
        {
            if (_coins > -1)
            {
                var difference = value - _coins;
                _coins = value;

                _changeView.ActivateChangeView(difference);
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
    }

    public class HandsCoinsCounter : CoinsCounter
    {
        private AudioPlayer _audioPlayer;

        public HandsCoinsCounter(CoinsIndicator coinsIndicator, CoinsChangeView changeView) : base(coinsIndicator, changeView) 
        {
            _coins = 50;
            UpdateIndicator();
            _audioPlayer = ServiceLocator.Instance.Get<AudioPlayer>();
        }

        public override void AddCoins(int value)
        {
            base.AddCoins(value);
            _audioPlayer.PlaySound("EarnCoins");
        }

        public override void RemoveCoins(int value)
        {
            base.RemoveCoins(value);
            _audioPlayer.PlaySound("WasteCoins");
        }

        public override void ChangeCoins(int value)
        {
            base.ChangeCoins(value);
            _audioPlayer.PlaySound("EarnCoins");
        }
    }

    public class BankCoinsCounter : CoinsCounter
    {
        public BankCoinsCounter(CoinsIndicator coinsIndicator, CoinsChangeView changeView) : base(coinsIndicator, changeView)
        {
            _coins = 1000;
            UpdateIndicator();
        }

        public void DoubleCoins()
        {
            _coins *= 2;
            UpdateIndicator();
            InvokeCoinsEvent();
        }
    }
}

