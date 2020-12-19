using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalendarManager : MonoBehaviour
{
    public Transform _content;
    public GameObject _prefabsDay;
    public Text _txtCurDisplayYear;
    public Text _txtCurDisplayMonth;

    List<Day> _days;
    int _curDisplayMonth;
    int _curDisplayYear;

    public int CurDisplayMonth
    { 
        get { return _curDisplayMonth; }
        set { _curDisplayMonth = value; }
    }
    public int CurDisplayYear
    {
        get { return _curDisplayYear; }
        set { _curDisplayYear = value; }
    }


    public void Init()
    {
        _days = new List<Day>();
        _days.Clear();

        for (int i = 0; i < 42; i++)
        {
            GameObject temp = Instantiate(_prefabsDay, _content);

            Day inTemp = temp.GetComponent<Day>();

            _days.Add(inTemp);
        }

        _curDisplayYear = DateTime.Now.Year;
        _curDisplayMonth = DateTime.Now.Month;
    }

    public void MakeCalendar(int year, int month)
    {
        _curDisplayYear = year;
        _curDisplayMonth = month;

        _txtCurDisplayMonth.text = string.Format("{0} 월", month);
        _txtCurDisplayYear.text = string.Format("{0} 년", year);

        DateTime firstDayOfMonth = new DateTime(year, month, 1);
        var lastDayofMonth = DateTime.DaysInMonth(year, month);
        
        var firstDayOfWeek = firstDayOfMonth.DayOfWeek;
        bool isStart = false;
        var day = 1;

        for (int i = 0; i < _days.Count; i++)
        {
            _days[i].Init();

            if (isStart || (int)firstDayOfWeek == i)
            {
                if (day <= lastDayofMonth)
                {
                    isStart = true;

                    _days[i].Days = day;
                    _days[i].IsDay = true;

                    day++;
                }
            }

            _days[i].dayUpdate();
        }
    }

}
