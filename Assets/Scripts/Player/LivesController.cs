using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesController : MonoBehaviour
{
    public Image[] lifeImages;
    public Sprite fullLifeSprite;
    public Sprite lostLifeSprite;

    public void UpdateLivesDisplay(int lives)
    {
        for (int i = 0; i < lifeImages.Length; i++)
        {
            if (i < lives)
            {
                lifeImages[i].sprite = fullLifeSprite;
            }
            else
            {
                lifeImages[i].sprite = lostLifeSprite;
            }
        }
    }
}
