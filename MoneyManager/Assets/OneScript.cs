using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

[SerializeField]
public class OneScript : MonoBehaviour
{
    public InputField _addMoneyInput;
    public InputField _limitMoneySettingInput;
    public Text _outputText;
    public Text _totalText;
    public Text _todayText;
    public Text _dayText;

    public Text _startDay;
    public Text _startDayInSetting;

    public CalendarManager _calendarManager;
    public UsageHistoryManager _usageHistoryManager;

    DateTime _dtFirstDate;
    [SerializeField]int _day;
    int _totalMoney;
    Color _safeColor;
    Color _overColor;

    int _limitMoney;

    void Start()
    {
        InitData(); // 데이터 갱신

        //if (_settingPanel == null)
        //{
        //    Debug.Log("settingPanel is null");
        //}

        if (_addMoneyInput == null)
        {
            Debug.Log("input is null");
        }

        if (_outputText == null)
        {
            Debug.Log("outputText is null");
        }

        if (_totalText == null)
        {
            Debug.Log("totalText is null");
        }

        if (_dayText == null)
        {
            Debug.Log("dayText is null");
        }


        OutPutUpdate();
    }


    public void ClickPlus()
    {
        int plusMoney = 0;
        if (int.TryParse(_addMoneyInput.text, out plusMoney))
        {
            if (plusMoney > 0)
            {
                _totalMoney += plusMoney;

                string today = DateTime.Now.ToString("yyyy.MM.dd HH:mm");

                _usageHistoryManager.AddContent(string.Format("{0}\n: {1}", today, plusMoney));
                _usageHistoryManager.GetComponent<ScrollRect>().verticalScrollbar.value = 0;
            }
            else
            {
                Debug.Log("Minos Number");
            }
        }
        else
        {
            Debug.Log("input text is not Number");
        }

        OutPutUpdate();
    }

    public void ClickReset()
    {
        PlayerPrefs.DeleteAll();
        _usageHistoryManager.ResetUHManager();
        
        LoadDate();
        OutPutUpdate();
    }

    //public void ClickOpenSettingPanel()
    //{
    //    _settingPanel.SetActive(true);
    //}

    //public void ClickCloseSettingPanel()
    //{
    //    _settingPanel.SetActive(false);
    //}
    public void ClickClosePanel(GameObject go)
    {
        go.SetActive(false);
    }

    public void ClickOpenPanel(GameObject go)
    {
        go.SetActive(true);
    }

    public void ClickExit()
    {
        Application.Quit();
    }

    public void ClickEditLimitMoney()
    {
        if (int.TryParse(_limitMoneySettingInput.text, out int num))
        {
            _limitMoney = num;
        }
        else
        {
            Debug.Log("That is not Number");
        }

        OutPutUpdate();
    }

    public void ClickNextMonth()
    {
        int year;
        int month;

        if (_calendarManager.CurDisplayMonth == 12)
        {
            year = _calendarManager.CurDisplayYear + 1;
            month = 1;
        }
        else
        {
            year = _calendarManager.CurDisplayYear;
            month = _calendarManager.CurDisplayMonth + 1;
        }

        _calendarManager.MakeCalendar(year, month);
    }

    public void ClickPreMonth()
    {
        int year;
        int month;

        if (_calendarManager.CurDisplayMonth == 1)
        {
            year = _calendarManager.CurDisplayYear - 1;
            month = 12;
        }
        else
        {
            year = _calendarManager.CurDisplayYear;
            month = _calendarManager.CurDisplayMonth - 1;
        }

        _calendarManager.MakeCalendar(year, month);
    }

    public void ChangeStartDay(int day)
    {
        int year = _calendarManager.CurDisplayYear;
        int month = _calendarManager.CurDisplayMonth;

        DateTime dateTime = new DateTime(year, month, day);

        _dtFirstDate = dateTime;

        _calendarManager.StartDay = _dtFirstDate;

        TimeSpan timeSpan = DateTime.Now - _dtFirstDate;

        _day = timeSpan.Days + 1;

        OutPutUpdate();
    }

    public void IFSetTheNumberWithComma(Text targetText)
    {
        if (int.TryParse(targetText.text, out int num))
        {
            string temp = string.Format("{0:#,###}", num);

            targetText.text = temp;
        }
    }

    private void InitData()
    {
        _safeColor = new Color(93 / 255f, 214 / 255f, 92 / 255f);
        _overColor = Color.red;
        _calendarManager.Init();
        _usageHistoryManager.Init();

        LoadDate();
    }
    private void OutPutUpdate()
    {
        string forText = "";
        int num = 0;

        if (_totalMoney > (_limitMoney * _day)) // 만원 초과
        {
            forText = string.Format("초과\n");
            num = (_totalMoney - (_limitMoney * _day));
            _outputText.color = _overColor;
        }
        else // 만원 이하
        {
            forText = string.Format("여유\n");
            num = ((_limitMoney * _day) - _totalMoney);
            _outputText.color = _safeColor;
        }

        forText += string.Format("{0:#,###}", num) + " 원";
        _outputText.text = forText;

        _totalText.text = "누적사용 : " + string.Format("{0:#,#}",_totalMoney) + " 원";
        _dayText.text = _day + " 일";

        _startDay.text = "시작일 " + _dtFirstDate.ToString("yyyy.MM.dd");
        _startDayInSetting.text = _dtFirstDate.ToString("yyyy.MM.dd");

        _todayText.text = DateTime.Now.ToString("yyyy.MM.dd");


        _addMoneyInput.text = "";

        Text limitInputFieldPlaceHolder = _limitMoneySettingInput.placeholder.GetComponent<Text>();
        limitInputFieldPlaceHolder.text = string.Format("{0}", _limitMoney);

        _limitMoneySettingInput.text = "";

        var year = _calendarManager.CurDisplayYear;
        var month = _calendarManager.CurDisplayMonth;

        _calendarManager.MakeCalendar(year, month);

        SaveData();
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("LimitMoney", _limitMoney);
        PlayerPrefs.SetInt("TotalMoney", _totalMoney);
        PlayerPrefs.SetString("StartDay", _dtFirstDate.ToString());
        _usageHistoryManager.SaveData();

        PlayerPrefs.Save();
    }

    private void LoadDate()
    {
        _usageHistoryManager.LoadData();

        if (PlayerPrefs.HasKey("TotalMoney"))
        {
            _totalMoney = PlayerPrefs.GetInt("TotalMoney");
        }
        else
        {
            _totalMoney = 0;
        }

        if (PlayerPrefs.HasKey("LimitMoney")) // 제한금
        {
            _limitMoney = PlayerPrefs.GetInt("LimitMoney");
        }
        else
        {
            _limitMoney = 10000;
        }

        if (PlayerPrefs.HasKey("StartDay"))
        {
            if (DateTime.TryParse(PlayerPrefs.GetString("StartDay"), out DateTime firstTime))
            {
                TimeSpan timeSpan = DateTime.Now - firstTime;

                _day = timeSpan.Days + 1;
                _dtFirstDate = firstTime;
            }
            else
            {
                Debug.Log("시작일에 문제 발생");
            }
        }
        else
        {

            _dtFirstDate = DateTime.Now.Date;
            _day = 1;
        }

        _calendarManager.StartDay = _dtFirstDate;
    }
}
