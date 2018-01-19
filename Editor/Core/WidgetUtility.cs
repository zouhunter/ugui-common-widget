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
using System.Linq;

namespace CommonWidget
{
    public static class WidgetUtility
    {
        //保存预制体的文件夹自动生成的guid（如果发生了变化请重写）
        private const string SpritePathGUID = "f336f1d680afb0d4b8ca64b381dd3d6c";
        //打开窗口的猜到，可自定义
        public const string Menu_widgetWindow = "Window/Widget/Common";
        public const string Menu_widgetConfig = "Window/Widget/Config";
        private static Dictionary<WidgetType, IElementCreater> createrDic;
        /// <summary>
        /// 从PrefabPathGUID加载所有的预制体
        /// </summary>
        /// <returns></returns>
        public static ObjectHolder[] LoadAllGameObject(string spritePath = null)
        {
            var holders = new List<ObjectHolder>();
            if(string.IsNullOrEmpty(spritePath))
                spritePath = AssetDatabase.GUIDToAssetPath(SpritePathGUID);
            if (!string.IsNullOrEmpty(spritePath))
            {
                var fullPath = Path.GetFullPath(spritePath);
                holders.AddRange( LoadAllUserDefine(fullPath));
                holders.AddRange(LoadAllSprites(fullPath));
                return holders.ToArray();
            }
            else
            {
                Debug.LogError("[加载对象失败] " + "保存预制体的文件夹.meta发生了变化，请重新设置本脚本中的PrefabPathGUID");
                return null;
            }
        }
        private static List<ObjectHolder> LoadAllSprites(string fullpath)
        {
            var holders = new List<ObjectHolder>();
            string[] spritepaths = Directory.GetFiles(fullpath, "*.png", SearchOption.AllDirectories);
            foreach (var spritepath in spritepaths)
            {
                var assetpath = spritepath.Replace("\\", "/").Replace(Application.dataPath, "Assets");
                var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetpath);
                if (sprite != null)
                {
                    var holder = new ObjectHolder(sprite);
                    holders.Add(holder);
                }
            }
            return holders;
        }
        private static List<ObjectHolder> LoadAllUserDefine(string fullPath)
        {
            var holders = new List<ObjectHolder>();
            string[] rules = Directory.GetFiles(fullPath, "*.json", SearchOption.AllDirectories);
            foreach (var rulepath in rules)
            {
                var assetpath = rulepath.Replace("\\", "/").Replace(Application.dataPath, "Assets");
                var jsonname = System.IO.Path.GetFileName(assetpath);
                var assetDir = assetpath.Replace(jsonname, "");

                var jsonString = AssetDatabase.LoadAssetAtPath<TextAsset>(assetpath).text;
                if (string.IsNullOrEmpty(jsonString)){
                    continue;
                }

                var node = JSONNode.Parse(jsonString);
                List<JSONClass> nodes = new List<JSONClass>();
                if(node.AsObject != null)
                {
                    nodes.Add(node.AsObject);
                }
                else if(node.AsArray != null)
                {
                    foreach (var item in node.AsArray)
                    {
                        var jsonClass = (JSONClass)item;
                        nodes.Add(jsonClass);
                    }
                }

                foreach (var nodeItem in nodes)
                {
                    if (nodeItem != null)
                    {
                        var holder = new ObjectHolder(assetDir, nodeItem);
                        holders.Add(holder);
                    }
                }

            }
            return holders;
        }

        internal static void InitImage(Image image,Sprite sprite, Image.Type simple = Image.Type.Simple)
        {
            image.sprite = sprite;
            image.type = Image.Type.Simple;
            image.SetNativeSize();
        }

        public static GameObject CreateInstence(WidgetType type,CreateInfo info)
        {
            var creater = GetCreater(type);
            return creater.CreateInstence(info);
        }
        public static Texture CreatePreview(WidgetType type, CreateInfo info)
        {
            var creater = GetCreater(type);
            return creater.CreatePreview(info);
        }
        private static IElementCreater GetCreater(WidgetType type)
        {
            if(createrDic == null)
            {
                createrDic = new Dictionary<WidgetType, IElementCreater>();
            }
            
            if(!createrDic.ContainsKey(type))
            {
                var typeName = "CommonWidget." + type.ToString() + "Creater";
                var createrType = typeof(IElementCreater).Assembly.GetType(typeName);
                if(createrType == null)
                {
                    Debug.LogError("请编写:" + typeName);
                    return null;
                }
                createrDic.Add(type,System.Activator.CreateInstance(createrType) as IElementCreater);
            }

            return createrDic[type];

        }

       
    }
}