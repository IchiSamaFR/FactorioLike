using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitMarker : MonoBehaviour
{
    public static HitMarker instance;
    public Image hitMarker;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    public void Change(Color color)
    {
        hitMarker.color = color;
    }
}
