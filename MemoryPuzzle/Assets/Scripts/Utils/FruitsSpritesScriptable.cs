using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FruitsSpriteList", menuName = "ScriptableObjects/FruitsSpriteList", order = 2)]
public class FruitsSpritesScriptable : ScriptableObject
{
    /// <summary>
    /// card sprite collection
    /// </summary>
    [SerializeField] List<Sprite> cardSpriites;
    /// <summary>
    /// back face sprite
    /// </summary>
    [SerializeField] Sprite backSprite;

    public int Count => cardSpriites.Count;

    //back sprite property
    public Sprite GetBackSprite => backSprite;

    /// <summary>
    /// Get the sprite from index
    /// </summary>
    /// <param name="index">index of sprite for now its also refred as id</param>
    /// <returns></returns>
    public Sprite GetSprite(int index)
    {
        if (index < 0 || index >= cardSpriites.Count)
        {
            Debug.Log("Item you are trying to access  isn't available" + index);
            return null;
        }
        return cardSpriites[index];
    }
}
