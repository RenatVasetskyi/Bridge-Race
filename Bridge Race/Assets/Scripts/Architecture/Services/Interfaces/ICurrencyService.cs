using System;

namespace Architecture.Services.Interfaces
{
    public interface ICurrencyService 
    {
        event Action OnCoinsCountChanged; 
        int Coins { get; }
        void Buy(int price);
        void Earn(int amount);
        void Set(int amount);
        void Load();
    }
}
