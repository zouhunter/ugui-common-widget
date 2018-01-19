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
                Debug.Log(item.name);
            }
        }
        else
        {

        }
       
    }
    [Test]
    public void CreateUI()
    {
        var canvas = Object.FindObjectOfType<Canvas>();
        Selection.activeObject = canvas;
        EditorApplication.ExecuteMenuItem("GameObject/UI/Image");
        EditorApplication.ExecuteMenuItem("GameObject/UI/Image");
    }
}
