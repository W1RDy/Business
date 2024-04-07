using System;
using UnityEngine;

namespace CoinsCounter
{
    public abstract class CoinsCounter : IService, ICoinsCounter
    {
        protected int _coins;

        public int Coins => _coins;

        private CoinsIndicator _coinsIndicator;

        public event Action CoinsChanged;

        public CoinsCounter(CoinsIndicator coinsIndicator)
        {
            _coinsIndicator = coinsIndicator;
        }

        public virtual void AddCoins(int value)
        {
            _coins += value;
            UpdateIndicator();
            InvokeCoinsEvent();
        }

        public virtual void RemoveCoins(int value)
        {
            if (_coins >= value)
            {
                _coins = Mathf.Clamp(_coins - value, 0, _coins);
                UpdateIndicator();
                InvokeCoinsEvent();
            }
        }

        public virtual void ChangeCoins(int value)
        {
            if (_coins > -1)
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
    }

    public class HandsCoinsCounter : CoinsCounter
    {
        public HandsCoinsCounter(CoinsIndicator coinsIndicator) : base(coinsIndicator) 
        {
            _coins = 50;
            UpdateIndicator();
        }
    }

    public class BankCoinsCounter : CoinsCounter
    {
        public BankCoinsCounter(CoinsIndicator coinsIndicator) : base(coinsIndicator)
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

