using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public Button[] levelButtons;
    public static int unlockedIndex;

    // Start is called before the first frame update
    void Start()
    {

        for(int i = levelButtons.Length-1; i > unlockedIndex; i--)
        {
            levelButtons[i].GetComponent<Button>().interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
