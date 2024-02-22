using System;
using System.Collections.Generic;
using Architecture.Services.Interfaces;
using Audio;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Daily
{
    public class DailyBonusSystem : MonoBehaviour
    {
        private const string LastDaySaveId = "LastDay";
        private const string IsDailyBonusGotSaveId = "IsDailyBonusGot";
        private const string DaysInARowSaveId = "DaysInARow";

        private readonly List<int> _bonuses = new() { 50, 100, 150, 200, 250, 500, 1000 };

        [SerializeField] private Button _button;

        [SerializeField] private DailyBonusWindow _dailyBonusWindow;

        private ISaveService _saveService;
        private IAudioService _audioService;
        private ICurrencyService _currencyService;

        private int _lastDay;
        private int _daysInARow;

        private bool _isBonusGot;

        [Inject]
        public void Construct(ISaveService saveService, IAudioService audioService, 
            ICurrencyService currencyService)
        {
            _saveService = saveService;
            _audioService = audioService;
            _currencyService = currencyService;
        }

        private void OnEnable()
        {
            Show();
            
            _button.onClick.AddListener(Open);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Open);
        }

        private void Show()
        {
            Load();

            if (CanActivate())
                Activate();
            else
                Deactivate();
        }
        
        private void Open()
        {
            _isBonusGot = true;
            
            Deactivate();
            
            _audioService.PlaySfx(SfxType.UIClick);

            if (IsYesterdayWasLastDayOfTakingBonus() & IsDaysInARowCountLessOrTheSameThanBonusCount())
                AddDaysInARow();
            else if(DateTime.Now.DayOfYear - 1 != _lastDay)
                ResetDaysInARow();
            
            _currencyService.Earn(_bonuses[_daysInARow]);
            
            _lastDay = DateTime.Now.DayOfYear;
            
            _audioService.PlaySfx(SfxType.GetCoin);

            ShowDailyBonusWindow();

            Save();
        }
        
        private bool CanActivate()
        {
            return _lastDay != DateTime.Now.DayOfYear ||
                   (_lastDay == DateTime.Now.DayOfYear && _isBonusGot == false);
        }
        
        private void Activate()
        {
            _button.gameObject.SetActive(true);
            
            _audioService.PlaySfx(SfxType.DailyBonusActivated);
        }
        
        private void Deactivate()
        {
            _button.gameObject.SetActive(false);
        }

        private bool IsDaysInARowCountLessOrTheSameThanBonusCount()
        {
            return _daysInARow < _bonuses.Count - 1;
        }

        private bool IsYesterdayWasLastDayOfTakingBonus()
        {
            return DateTime.Now.DayOfYear - 1 == _lastDay;
        }
        
        private void AddDaysInARow()
        {
            _daysInARow++;
        }

        private void ShowDailyBonusWindow()
        {
            _dailyBonusWindow.Initialize(_bonuses[_daysInARow]);
            _dailyBonusWindow.gameObject.SetActive(true);
        }
        
        private void ResetDaysInARow()
        {
            _daysInARow = 0;
        }
        
        private void Save()
        {
            _saveService.SaveInt(LastDaySaveId, _lastDay);
            _saveService.SaveInt(DaysInARowSaveId, _daysInARow);
            _saveService.SaveBool(IsDailyBonusGotSaveId, _isBonusGot);
        }

        private void Load()
        {
            _lastDay = PlayerPrefs.HasKey(LastDaySaveId) ? _saveService.LoadInt(LastDaySaveId) : DateTime.Now.DayOfYear;

            if (PlayerPrefs.HasKey(IsDailyBonusGotSaveId))
                _isBonusGot = _lastDay == DateTime.Now.DayOfYear && _saveService.LoadBool(IsDailyBonusGotSaveId);
            else
                _isBonusGot = false;

            if (PlayerPrefs.HasKey(DaysInARowSaveId))
                _daysInARow = _saveService.LoadInt(DaysInARowSaveId);
        }
    }
}
