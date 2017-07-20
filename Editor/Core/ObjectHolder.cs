using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.Sprites;
using UnityEngine.Scripting;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.Assertions.Must;
using UnityEngine.Assertions.Comparers;
using System.Collections;
using UnityEditor;

namespace CommonWidget
{
    public class ObjectHolder
    {
        public GameObject prefab;
        public GUIContent preview;
        public string menuName;

        public ObjectHolder(string menuName, string assetpath)
        {
            this.menuName = menuName;
            prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetpath);
            var icon = AssetDatabase.LoadAssetAtPath<Texture>(assetpath.Replace(".prefab", ".png"));
            preview =new GUIContent(prefab.name, icon);
        }
    }
}

