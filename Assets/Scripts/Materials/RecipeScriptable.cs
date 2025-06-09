using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Recipe")]
public class RecipeScriptable : ScriptableObject
{
    public List<int> requiredMaterials;
    public Sprite recipeSprite;
    public Sprite recipeIcon;
    public string recipeName;
    public List<string> startDialogs;
}
