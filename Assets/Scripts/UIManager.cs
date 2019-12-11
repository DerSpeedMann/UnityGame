using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UIManager : MonoBehaviour
{
    public GameObject editorHud;
    public GameObject gameHud;

    void Start()
    {
        editorHud.SetActive(true);
        gameHud.SetActive(false);
    }
    public void HideGameUI(bool hide)
    {
        gameHud.SetActive(!hide);
    }
    public void HideEditorUI(bool hide)
    {
        editorHud.SetActive(!hide);
    }
}
