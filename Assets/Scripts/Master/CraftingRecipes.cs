using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingRecipes : MonoBehaviour
{
    public List<CraftingRecipe> recipes = new List<CraftingRecipe>();
}


[System.Serializable]
public class CraftingRecipe
{
    public string recipeId;
    public List<string> placeToCraft = new List<string>();
    public List<string> basicsRessourceId = new List<string>();
    public string resultId;
}
