using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public static HUDController instance;
    public Text carrotCountText;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Ya hay una instancia de HUDController");
            return;
        }

        instance = this;
    }

    public void UpdateCarrotCount(int count)
    {
        carrotCountText.text = count.ToString();
    }
}