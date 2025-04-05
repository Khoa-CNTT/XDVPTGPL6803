using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAtlas : MonoBehaviour
{
    public static ItemAtlas Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    enum ItemTypes {
    STONE,TOMATO
    
    }
    public Sprite stoneSprite;
    public Sprite woodSprite;
}
