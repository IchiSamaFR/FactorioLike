using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmeltRecipes : MonoBehaviour
{
    public static SmeltRecipes instance;
    public List<SmeltRecipe> recipes = new List<SmeltRecipe>();

    void Awake()
    {
        instance = this;

        foreach (SmeltRecipe recipe in recipes)
        {
            if(recipe.amountNeeded == 0)
            {
                recipe.amountNeeded = 1;
            }
            if (recipe.amountResulted == 0)
            {
                recipe.amountResulted = 1;
            }
        }
    }

    public SmeltRecipe GetResult(string ressourceId)
    {
        foreach (SmeltRecipe recipe in recipes)
        {
            if (recipe.basicsRessourceId == ressourceId)
            {
                return recipe;
            }
        }

        return null;
    }
}

[System.Serializable]
public class SmeltRecipe
{
    public string recipeId;
    public string basicsRessourceId;
    public int amountNeeded;
    public string resultId;
    public int amountResulted;
}
