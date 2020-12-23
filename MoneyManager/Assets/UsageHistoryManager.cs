using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UsageHistoryManager : MonoBehaviour
{
    public GameObject _content;
    public Transform _contentParent;

    int _limitListSize;

    List<GameObject> _historyContents;

    public void Init()
    {
        _historyContents = new List<GameObject>();
        _historyContents.Clear();

        _limitListSize = 20;

        if (_content == null)
        {
            Debug.Log("Content is null");
        }

        if (_contentParent == null)
        {
            Debug.Log("Content Parent is null");
        }
    }

    public void AddContent(string str)
    {
        if (_historyContents.Count < _limitListSize)
        {
            GameObject gob = Instantiate(_content, _contentParent);
            _historyContents.Add(gob);
            gob.GetComponent<Text>().text = str;
        }
        else
        {
            shiftLeft(_historyContents, 1);
            Destroy(_historyContents[19]);

            _historyContents[_limitListSize - 1] = Instantiate(_content, _contentParent);
            _historyContents[_limitListSize - 1].GetComponent<Text>().text = str;
        }
    }

    public void ResetUHManager()
    {
        foreach(GameObject gob in _historyContents)
        {
            Destroy(gob);
        }

        _historyContents.Clear();
    }

    void reverse(List<GameObject> gobList, int start, int end)
    {
        GameObject temp;
        end = end - 1;

        while (start < end)
        {
            temp = gobList[start];
            gobList[start] = gobList[end];
            gobList[end] = temp;

            start++;
            end--;
        }
    }

    void shiftLeft(List<GameObject> gobList, int d)
    {
        int count = gobList.Count;

        reverse(gobList, 0, d);
        reverse(gobList, d, count);
        reverse(gobList, 0, count);
    }

    public void SaveData()
    {
        string saveData = "";

        foreach(GameObject gob in _historyContents)
        {
            saveData += gob.GetComponent<Text>().text + ","; // ,는 인덱스 구분
        }

        saveData += 'f'; // f는 마지막 인덱스처리

        PlayerPrefs.SetString("UsageHistoryLog", saveData);
        Debug.Log(PlayerPrefs.GetString("UsageHistoryLog"));
    }

    public void LoadData()
    {

        if (PlayerPrefs.HasKey("UsageHistoryLog"))
        {
            string loadData = "";

            loadData = PlayerPrefs.GetString("UsageHistoryLog");

            bool isFin = false;
            int index = -1;

            string temp = "";
            while(!isFin)
            {
                index++;

                if (loadData[index] == 'f') // f는 마지막 인덱스처리
                {
                    isFin = true;
                    break;
                }
                if (loadData[index] == ',') // ,는 인덱스 구분
                {
                    GameObject gob = Instantiate(_content, _contentParent);
                    gob.GetComponent<Text>().text = temp;

                    _historyContents.Add(gob);

                    temp = "";

                    continue;
                }
                else
                {
                    temp += loadData[index];
                }
            }
        }
    }
}
