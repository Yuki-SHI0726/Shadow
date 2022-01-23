using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Data set for speaker in UI
/// </summary>
public class SpeakerData
{
    public Sprite Sprite { get; private set; }
    public string Name { get; private set; }

    public SpeakerData()
    {
        Sprite = null;
        Name = "";
    }

    public SpeakerData(Sprite sprite, string name)
    {
        Sprite = sprite;
        Name = name;
    }
}
