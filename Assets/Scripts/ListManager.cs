using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LISTDIRECTION
{
    HORIZONTAL = 0,
    VERTICAL = 1
}

public class ListManager : MonoBehaviour {

    public RectTransform Container;
    public Mask Mask;
    public RectTransform Prefab;
    public int Num = 1;
    public float Spacing;
    public LISTDIRECTION Direction = LISTDIRECTION.HORIZONTAL;
    public bool Optimize;

    private RectTransform maskRT;
    private int numVisible;
    private int numVisibleHalf;
    private float containerHalfSize;
    private float prefabSize;

    private Dictionary<int, int[]> itemDict = new Dictionary<int, int[]>();
    private List<RectTransform> listItemRect = new List<RectTransform>();
    private List<ListItem> listItems = new List<ListItem>();
    private int numItems = 0;
    private Vector3 startPos;
    private Vector3 offsetVec;

    // Use this for initialization
    void Start ()
    {
        maskRT = Mask.GetComponent<RectTransform>();
   
        Vector2 prefabScale = Prefab.sizeDelta;
        prefabSize = (Direction == LISTDIRECTION.HORIZONTAL ? prefabScale.x: prefabScale.y) + Spacing;

        Container.sizeDelta = Direction == LISTDIRECTION.HORIZONTAL ? (new Vector2(prefabSize * Num, prefabScale.y)) : (new Vector2(prefabScale.x, prefabSize * Num));
        containerHalfSize = Direction == LISTDIRECTION.HORIZONTAL ? (Container.sizeDelta.x * 0.5f):(Container.sizeDelta.y * 0.5f);
  
        numVisible = Mathf.CeilToInt((Direction == LISTDIRECTION.HORIZONTAL ? maskRT.sizeDelta.x: maskRT.sizeDelta.y) / prefabSize);
        numVisibleHalf = Mathf.CeilToInt(numVisible * 0.5f);

        offsetVec = Direction == LISTDIRECTION.HORIZONTAL ? Vector3.right : Vector3.down;
        startPos = Container.anchoredPosition3D - (offsetVec * containerHalfSize) + (offsetVec * ((Direction == LISTDIRECTION.HORIZONTAL?prefabScale.x : prefabScale.y) * 0.5f));
        numItems = Optimize? Mathf.Min(Num, numVisible + 2) : Num;
        for (int i=0; i< numItems; i++)
        {
            GameObject obj = Instantiate(Prefab.gameObject, Container.transform);
            RectTransform t = obj.GetComponent<RectTransform>();
            t.anchoredPosition3D = startPos + (offsetVec * i * prefabSize);
            listItemRect.Add(t);
            itemDict.Add(t.GetInstanceID(), new int[] {i,i});
            obj.SetActive(true);

            ListItem li = obj.GetComponentInChildren<ListItem>();
            listItems.Add(li);
            li.UpdateContent(i);
        }
        Container.anchoredPosition3D += offsetVec * (containerHalfSize - ((Direction == LISTDIRECTION.HORIZONTAL ? maskRT.sizeDelta.x : maskRT.sizeDelta.y) * 0.5f));
    }

    public void ReorderItemsByPos(float normPos)
    {
        if (Direction == LISTDIRECTION.VERTICAL) normPos = 1f - normPos;
        int numOutOfView = Mathf.CeilToInt(normPos * (Num - numVisible));
        int firstIndex = Mathf.Max(0, numOutOfView - numVisibleHalf);
        int originalIndex = firstIndex % numItems;

        int newIndex = firstIndex;
        for (int i=originalIndex; i< numItems; i++)
        {
            RectTransform item = listItemRect[i];
            moveItemByIndex(listItemRect[i], newIndex);
            listItems[i].UpdateContent(newIndex);
            newIndex++;
        }
        for (int i = 0; i < originalIndex; i++)
        {
            RectTransform item = listItemRect[i];
            moveItemByIndex(listItemRect[i], newIndex);
            listItems[i].UpdateContent(newIndex);
            newIndex++;
        }
    }

    private void moveItemByIndex(RectTransform item, int index)
    {
        int id = item.GetInstanceID();
        itemDict[id][0] = index;
        item.anchoredPosition3D = startPos + (offsetVec * index * prefabSize);
    }
}
