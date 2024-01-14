using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Easing : MonoBehaviour
{
    public static float easeInQuad(float t)
    {
        return t * t;
    }
    
    public static float easeOutQuad(float t)
    {
        return 1 - (1 - t) * (1 - t);
    }
}
