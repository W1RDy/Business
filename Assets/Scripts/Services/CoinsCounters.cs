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

        public void AddCoins(int value)
        {
            _coins += value;
            UpdateIndicator();
        }

        public void RemoveCoins(int value)
        {
            if (_coins >= value)
            {
                _coins = Mathf.Clamp(_coins - value, 0, _coins);
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
        }
    }
}

