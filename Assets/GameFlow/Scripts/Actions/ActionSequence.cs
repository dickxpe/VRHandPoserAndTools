
using System.Collections.Generic;
using UltEvents;
using UnityEngine;

public class ActionSequence : MonoBehaviour
{

    public int playId = 0;

    public List<UltEvent> playList;

    public void Play()
    {
        playList[playId].Invoke();
    }

}