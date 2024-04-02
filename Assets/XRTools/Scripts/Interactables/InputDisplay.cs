// Author: Peter Dickx https://github.com/dickxpe
// MIT License - Copyright (c) 2024 Peter Dickx

using UnityEngine;
using TMPro;

public class InputDisplay : MonoBehaviour
{

    int count = 0;
    [SerializeField]
    public int maxCount = 4;
    TMP_Text text;

    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    public void EnterStar()
    {
        count++;
        text.text = text.text + "*";
        if (count == maxCount)
        {
            count = 0;
            text.text = "";
        }
    }
}
