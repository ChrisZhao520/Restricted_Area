using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itemmodel
{
    #region  beibao
    private static Dictionary<string, item> GridItem = new Dictionary<string, item>();
    public static void StoreItem(string name, item item)
    {
        if (GridItem.ContainsKey(name))
        {
            DeleteItem(name);
        }
        GridItem.Add(name, item);
    }
    public static item GetItem(string name)
    {
        if (GridItem.ContainsKey(name))
        {
            return GridItem[name];
        }
        else
        {
            return null;
        }
    }
    public static void DeleteItem(string name)
    {
        if (GridItem.ContainsKey(name))
        {
            GridItem.Remove(name);
        }
    }
    #endregion

    #region 道具栏存储数据
    private static Dictionary<string, item> GridItemFour = new Dictionary<string, item>();
    public static void StoreItemFour(string name, item item)
    {
        if (GridItemFour.ContainsKey(name))
        {
            DeleteItemFour(name);
        }
        GridItemFour.Add(name, item);
    }
    public static item GetItemFour(string name)
    {
        if (GridItemFour.ContainsKey(name))
        {
            return GridItemFour[name];
        }
        else
        {
            return null;
        }
    }
    public static void DeleteItemFour(string name)
    {
        if (GridItemFour.ContainsKey(name))
        {
            GridItemFour.Remove(name);
        }
    }
    #endregion
}
