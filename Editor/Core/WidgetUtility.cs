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
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System;

namespace CommonWidget
{
    public static class WidgetUtility
    {
        //保存预制体的文件夹自动生成的guid（如果发生了变化请重写）
        private const string PrefabPathGUID = "4b13b0086f90c994f8feb37cd7a6e0be";
        //打开窗口的猜到，可自定义
        public const string Menu_widgetWindow = "Window/CommonWidget";
        //根目录对象层名
        public const string defultMenuName = "Other";
        /// <summary>
        /// 从PrefabPathGUID加载所有的预制体
        /// </summary>
        /// <returns></returns>
        public static ObjectHolder[] LoadAllGameObject()
        {
            var holders = new List<ObjectHolder>();
            var assetsPath = AssetDatabase.GUIDToAssetPath(PrefabPathGUID);
            if (!string.IsNullOrEmpty(assetsPath))
            {
                var fullPath = Path.GetFullPath(assetsPath);
                string[] info = Directory.GetFiles(fullPath, "*.prefab", SearchOption.AllDirectories);
                foreach (var item in info)
                {
                    var fullDir = Path.GetDirectoryName(item);
                    var assetpath = item.Replace("\\","/").Replace(Application.dataPath, "Assets");
                    var menuName= fullDir.Replace("\\", "/").Replace(Application.dataPath, "Assets").Replace(assetsPath, "").TrimStart('/');
                    menuName = string.IsNullOrEmpty(menuName) ? defultMenuName : menuName;
                    var holder = new ObjectHolder(menuName,assetpath);
                    holders.Add(holder);
                }
                return holders.ToArray();
            }
            else
            {
                Debug.LogError("[加载对象失败] " + "保存预制体的文件夹.meta发生了变化，请重新设置本脚本中的PrefabPathGUID");
                return null;
            }
        }
        
    }
}