using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public Sprite disabledButtonImg;
    public Button[] levelButtons;
    public static int unlockedIndex;

    // Start is called before the first frame update
    void Start()
    {

        for(int i = levelButtons.Length-1; i > unlockedIndex; i--)
        {
            Button b = levelButtons[i].GetComponent<Button>();
            b.interactable = false;
            b.image.sprite = disabledButtonImg;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
