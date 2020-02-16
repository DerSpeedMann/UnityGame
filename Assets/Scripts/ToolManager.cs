using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public enum tools { Draw, Move, Erase };

    public tools activeTool = tools.Draw;

}
