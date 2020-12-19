using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Day : MonoBehaviour
{
    [SerializeField] int _days;
    [SerializeField] bool _isDay = false;

    public int Days
    {
        set { _days = value; }
        get { return _days; }
    }

    public bool IsDay
    { 
        set { _isDay = value; }
        get { return _isDay; }
    }

    public Text _text;
    public Image _edge;
    public Image _backGround;

    public void Init()
    {
        _isDay = false;
        _text.text = "";
        _edge.color = new Color(_edge.color.r, _edge.color.g, _edge.color.b, 1f);
        _backGround.color = Color.white;
    }

    public void dayUpdate()
    {
        if (_isDay)
        {
            _text.text = string.Format("{0}", _days);

            if (_days == DateTime.Now.Day)
            {
                _backGround.color = Color.green;
            }
        }
        else
        {
            _edge.color = new Color(_edge.color.r, _edge.color.g, _edge.color.b, 0.3f);
            _backGround.color = new Color(_backGround.color.r, _backGround.color.g, _backGround.color.b, 0.0f);
        }
    }
}
