using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ColorScheme : ScriptableObject
{
    public Color[] Colors;

    public int Length => Colors.Length;
    public Color this[int index] => Colors[index];
}
