using System;
using Architecture.Services.Interfaces;
using UnityEngine;

namespace Architecture.Services
{
    public class CurrencyService : ICurrencyService
    {
        private const string SaveCoinsCountId = "CoinsCount";

        private const int StartValue = 0;

        private readonly ISaveService _saveService;
        
        public event Action OnCoinsCountChanged;
    
        public int Coins { get; private set; } 

        public CurrencyService(ISaveService saveService)
        {
            _saveService = saveService;
        }

        public void Load()
        {
            Coins = PlayerPrefs.HasKey(SaveCoinsCountId) ?
                _saveService.LoadInt(SaveCoinsCountId) : StartValue;
        }

        public void Buy(int price)
        {
            Coins -= price;
            
            Save();
            
            OnCoinsCountChanged?.Invoke();
        }

        public void Earn(int amount)
        { 
            Coins += amount; 
            
            Save();
            
            OnCoinsCountChanged?.Invoke();
        }

        public void Set(int amount)
        {
            Coins = amount;
            
            Save();
            
            OnCoinsCountChanged?.Invoke();
        }
        
        private void Save()
        {
            _saveService.SaveInt(SaveCoinsCountId, Coins);
        }
    }
}
