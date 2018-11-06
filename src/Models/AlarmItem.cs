﻿using AlarmClock.Managers;
using AlarmClock.Misc;
using AlarmClock.Properties;
using AlarmClock.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace AlarmClock.Models
{
    public class AlarmItem : NotifyPropertyChanged
    {
        #region attributes
        private static readonly Regex Regex = new Regex("[^0-9.-]+");
        private const byte MaxMinutes = (byte)(TimeSpan.TicksPerMinute / TimeSpan.TicksPerSecond) - 1;
        private const byte MaxHours = (byte)(TimeSpan.TicksPerDay / TimeSpan.TicksPerHour) - 1;

        private readonly ObservableCollection<AlarmItem> _owner;
        private readonly ClockRepository _clocks;

        private int _hour;
        private int _minute;
        private Clock _clock;
        private ICommand _clickUpHour;
        private ICommand _clickDownHour;
        private ICommand _clickUpMinute;
        private ICommand _clickDownMinute;
        private ICommand _addAlarm;
        private ICommand _deleteAlarm;
        private ICommand _ringAlarm;

        private bool _isActive;
        private bool _isVisible = true;
        private bool _isStopped = false;
        #endregion

        #region properties
        public List<AlarmItem> UserAlarms => _owner
            .Where(item => !item.IsBaseAlarm && item.IsVisible)
            .ToList();

        public string Hour
        {
            get => $"{_hour:00}";
            set
            {
                if (!IsValidTime(value, MaxHours))
                    return;

                _hour = int.Parse(value);

                OnPropertyChanged(nameof(Hour));
                OnPropertyChanged(nameof(IsAllowedTime));
            }
        }

        public string Minute
        {
            get => $"{_minute:00}";
            set
            {
                if (!IsValidTime(value, MaxMinutes))
                    return;

                _minute = int.Parse(value);

                OnPropertyChanged(nameof(Minute));
                OnPropertyChanged(nameof(IsAllowedTime));
            }
        }

        public bool IsBaseAlarm => _clock == null;
        public bool IsAddEnabled => IsBaseAlarm;
        public bool IsSaveEnabled => !IsBaseAlarm;
        public bool IsCancelEnabled => !IsBaseAlarm;
        public bool IsRingEnabled => !IsBaseAlarm;
        public bool IsDeleteEnabled => !IsBaseAlarm;

        public bool IsAllowedTime =>
            !UserAlarms
                   .Where(item => item != this)
                   .Select(GetTimeValue)
                   .Contains(GetTimeValue(this));

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (value && !UserAlarms.Any(item => item.IsActive) || !value)
                {
                    _isActive = value;

                    OnPropertyChanged(nameof(IsEnabled));
                    OnPropertyChanged(nameof(IsActive));
                }
            }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;

                OnPropertyChanged(nameof(IsVisible));
            }
        }

        public bool IsStopped
        {
            get => _isStopped;
            set
            {
                _isStopped = value;

                IsActive = !IsActive;
            }
        }
        public bool IsEnabled => IsBaseAlarm || !IsActive;

        public Clock Clock => _clock;
        #endregion

        #region commands
        public ICommand ClickUpHour =>
            _clickUpHour ??
           (_clickUpHour = new RelayCommand(
                delegate { ChangeAlarm(ref _hour, nameof(Hour), 1, MaxHours); }
            ));

        public ICommand ClickDownHour =>
            _clickDownHour ??
           (_clickDownHour = new RelayCommand(
                delegate { ChangeAlarm(ref _hour, nameof(Hour), -1, MaxHours); }
           ));

        public ICommand ClickUpMinute =>
            _clickUpMinute ??
           (_clickUpMinute = new RelayCommand(
               delegate { ChangeAlarm(ref _minute, nameof(Minute), 1, MaxMinutes); }
           ));

        public ICommand ClickDownMinute =>
            _clickDownMinute ??
           (_clickDownMinute = new RelayCommand(
                delegate { ChangeAlarm(ref _minute, nameof(Minute), -1, MaxMinutes); }
           ));

        public ICommand AddAlarm => _addAlarm ?? (_addAlarm = new RelayCommand(AddAlarmExecute));

        public ICommand DeleteAlarm => _deleteAlarm ?? (_deleteAlarm = new RelayCommand(DeleteAlarmExecute));

        public ICommand RingAlarm => _ringAlarm ?? (_ringAlarm = new RelayCommand(
            delegate
            {
                if (IsActive)
                    _isStopped = true;
                IsActive = !IsActive;
            }));
        #endregion

        public AlarmItem(ObservableCollection<AlarmItem> owner, ClockRepository clocks, int hour, int minute)
        {
            _owner = owner;
            _clocks = clocks;
            _hour = hour;
            _minute = minute;
        }

        private int GetTimeValue(AlarmItem ai) => ai._hour * (MaxMinutes + 1) + ai._minute;
        private int GetTimeValue(DateTime dt) => dt.Hour * (MaxMinutes + 1) + dt.Minute;
        public bool Equals(DateTime dt) => GetTimeValue(dt) == GetTimeValue(this);

        private void DeleteAlarmExecute(object obj)
        {
            var userClocks = _clocks.ForUser(StationManager.CurrentUser.Id);

            for (int i = 1, j = 0; i < _owner.Count; i++)
            {
                if (_owner[i].IsVisible && _owner[i] != this)
                {
                    _owner[i].Clock.LastTriggered = j == 0 ? new DateTime() : userClocks[j - 1].NextTrigger;

                    _owner[i].Clock.NextTrigger = j + 1 == userClocks.Count ? new DateTime() : userClocks[j + 1].LastTriggered;

//                    (in process for filter clocks in the same order as alarms)_clocks[i - 1] = _owner[i].Clock;
                    j++;
                }
            }
            _clocks.Delete(Clock.Id);
            _owner.Remove(this);

            Update();
        }

        private void ChangeAlarm(ref int v, string obj, int offset, byte highBound)
        {
            var newValue = v + offset;

            v = newValue == -1 ? highBound : (newValue == highBound + 1 ? 0 : newValue);

            _isStopped = false;

            OnPropertyChanged(obj);
            OnPropertyChanged(nameof(IsAllowedTime));

            Update();
        }

        public void Update()
        {
            var now = DateTime.Now;

            //TODO change time in first alarm 
            //_hour = now.Hour;
            //_minute = now.Minute;
            _owner[0].OnPropertyChanged(nameof(IsAllowedTime));
            //OnPropertyChanged(nameof(Hour));
            //OnPropertyChanged(nameof(Minute));
        } 

        private void AddAlarmExecute(object obj)
        {
            try
            {
                var alarm = new AlarmItem(_owner, _clocks, _hour, _minute)
                {
                    _clock = _clocks.Add(new Clock(StationManager.CurrentUser))
                };
                var index = _owner
                    .ToList()
                    .FindIndex(item => GetTimeValue(item) > GetTimeValue(alarm));

                _owner.Insert(index == -1 ? _owner.Count() : index, alarm);
                //Rearrange();

                OnPropertyChanged(nameof(IsAllowedTime));
            }
            catch (Exception)
            {
                MessageBox.Show(Resources.CantParseTimeError);
            }
        }

        public void Rearrange()//delete
        {
            var sortedList = UserAlarms.OrderBy(GetTimeValue).ToList();

            for (int i = 1, j = 0; i < _owner.Count; i++)
            {
                if (_owner[i].IsVisible)
                {
                    _owner[i] = sortedList[j];

                    if (j > 0)
                        _owner[i].Clock.LastTriggered = GetNewClockTime(sortedList[j - 1]._hour, sortedList[j - 1]._minute);

                    if (j < sortedList.Count - 1)
                        _owner[i].Clock.NextTrigger = GetNewClockTime(sortedList[j + 1]._hour, sortedList[j + 1]._minute);

                    //(the same as above)_clocks[i - 1] = _owner[i].Clock;
                    j++;
                }
            }
        }

        private static DateTime GetNewClockTime(int hour, int minute)
        {
            var tmpDate = DateTime.Now.AddDays(1);

            return new DateTime(tmpDate.Year, tmpDate.Month, tmpDate.Day, hour, minute, tmpDate.Second);
        }

        private bool IsValidTime(string text, int param) =>
            !Regex.IsMatch(text) && text.Length == 2 &&
                int.Parse(text) >= 0 && int.Parse(text) <= param;
    }
}
