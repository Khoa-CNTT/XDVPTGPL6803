using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharSprite", menuName = "CharSprite", order = 1)]
public class CharSprite : ScriptableObject
{
    public char character;
    public Sprite sprite;
}
