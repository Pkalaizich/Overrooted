using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "My Assets/Material")]
public class MaterialScriptable : ScriptableObject
{
    public Sprite materialIcon;
    public Sprite materialSprite;
    public string materialName;
    public int materialIndex;
}
