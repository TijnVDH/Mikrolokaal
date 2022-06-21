using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterForm
{
    public string Name; // only for editor purposes
    public Sprite Sprite;
    public UpgradeType UpgradeType;
    //public List<CharacterFormRequirement> Requirements = new List<CharacterFormRequirement>();
}
