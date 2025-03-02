using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(LevelSettings), menuName = "LevelSettings")]
public class LevelSettings : ScriptableObject
{
    public LevelView[] Levels;
}