using System;
using UnityEngine;

namespace CoinsCounter
{
    public abstract class CoinsCounter : IService, ICoinsCounter
    {
        protected int _coins;

        public int Coins => _coins;

        private CoinsIndicator _coinsIndicator;

        public CoinsCounter(CoinsIndicator coinsIndicator)
        {
            _coinsIndicator = coinsIndicator;
        }

        public virtual void AddCoins(int value)
        {
            _coins += value;
            UpdateIndicator();
        }

        public virtual void RemoveCoins(int value)
        {
            if (_coins >= value)
            {
                _coins = Mathf.Clamp(_coins - value, 0, _coins);
                UpdateIndicator();
            }
        }

        public virtual void ChangeCoins(int value)
        {
            if (_coins > -1)
            {
                _coins = value;
                UpdateIndicator();
            }
        }

        protected void UpdateIndicator()
        {
            _coinsIndicator.SetCoins(_coins);
        }
    }

    public class HandsCoinsCounter : CoinsCounter
    {
        public event Action CoinsChanged;

        public HandsCoinsCounter(CoinsIndicator coinsIndicator) : base(coinsIndicator) 
        {
            _coins = 50;
            UpdateIndicator();
        }

        public override void AddCoins(int value)
        {
            base.AddCoins(value);
            CoinsChanged?.Invoke();
        }

        public override void RemoveCoins(int value)
        {
            base.RemoveCoins(value);
            CoinsChanged?.Invoke();
        }

        public override void ChangeCoins(int value)
        {
            base.ChangeCoins(value);
            CoinsChanged?.Invoke();
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
        }
    }
}

