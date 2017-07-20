using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using CommonWidget;

public class WidgetTest {

    [Test]
    public void PrintPrefabInfos()
    {
       var objs = WidgetUtility.LoadAllGameObject();
        if(objs!= null)
        {
            foreach (var item in objs)
            {
                Debug.Log(item.prefab.name);
            }
        }
        else
        {

        }
       
    }
}
