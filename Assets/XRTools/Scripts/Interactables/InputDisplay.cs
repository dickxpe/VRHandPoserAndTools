using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputDisplay : MonoBehaviour
{

    int count = 0;
    TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    public void EnterStar()
    {
        count++;
        text.text = text.text + "*";
        if (count == 4)
        {
            count = 0;
            text.text = "";
        }
    }
}
