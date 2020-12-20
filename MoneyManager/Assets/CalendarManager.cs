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

    DateTime _dtStartDay;

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

    public DateTime StartDay
    {
        set { _dtStartDay = value; }
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
            _days[i].Init(); // DayObject 초기화

            if (isStart || (int)firstDayOfWeek == i) // 매월 1일에 해당하는 요일부터 시작
            {
                if (day <= lastDayofMonth) // 마지막 날까지
                {
                    isStart = true;

                    _days[i].Days = day;
                    _days[i].IsDay = true;


                    // 시작일과 오늘 사이 체크
                    DateTime dtDay = new DateTime(_curDisplayYear, _curDisplayMonth, day);

                    if (_dtStartDay.DayOfYear <= dtDay.DayOfYear &&
                        dtDay.DayOfYear <= DateTime.Today.DayOfYear)
                    {
                        _days[i].IsInclude = true;
                    }// 시작일과 오늘 사이 체크


                    day++;
                }
            }

            _days[i].dayUpdate(); // DayObject 데이터 갱신
        }
    }

}
