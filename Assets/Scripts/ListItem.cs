using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListItem : MonoBehaviour {

    public SpriteDigit SpriteDigit;

    public void UpdateContent(int index)
    {
        SpriteDigit.DisplayNumber(index);
    }
}
