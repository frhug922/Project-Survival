using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectionType { Diary, CollectionItem }

[CreateAssetMenu(fileName = "New Collection", menuName = "Assets/Create New Collection")]

public class CollectionSO : ScriptableObject
{
    public int CollectionId;
    public string CollectionName;
    public string CollectionDescription;
    public CollectionType CollectionType;
    public Sprite CollectionIcon;
    public Sprite SilhouetteSprite;
}
