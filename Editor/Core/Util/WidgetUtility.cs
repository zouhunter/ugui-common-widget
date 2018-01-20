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
        /// <summary>
        /// 从json文件中加载出配制
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static WidgetItem[] LoadWidgeItems(string json,string assetDir)
        {
            var items = new List<WidgetItem>();
            var jsonarray = JSONNode.Parse(json).AsArray;
            foreach (var nodeItem in jsonarray)
            {
                var jsonClass = nodeItem as JSONClass;
                if (nodeItem != null && jsonClass != null)
                {
                    var item = new WidgetItem();
                    item.type = (WidgetType)Enum.Parse(typeof(WidgetType), jsonClass[KeyWord.type].Value);
                    item.name = jsonClass[KeyWord.name].Value;
                    item.spriteDic = LoadTextures(jsonClass, assetDir);
                    items.Add(item);
                }
            }
            return items.ToArray();
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

                var jsonarray = JSONArray.Parse(jsonString).AsArray;
                if (jsonarray == null) continue;
                
                foreach (var nodeItem in jsonarray)
                {
                    if (nodeItem != null && nodeItem is JSONClass)
                    {
                        var holder = new ObjectHolder(assetDir, (JSONClass)nodeItem);
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

        public static GameObject CreateInstence(WidgetType type,WidgetItem info)
        {
            var creater = GetCreater(type);
            return creater.CreateInstence(info);
        }
        public static Texture CreatePreview(WidgetType type, WidgetItem info)
        {
            var creater = GetCreater(type);
            return creater.CreatePreview(info);
        }
        internal static List<string> GetKeys(WidgetType type)
        {
            var creater = GetCreater(type);
            return creater.Keys;
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
        public static Dictionary<string, Sprite> LoadTextures(JSONClass json,string assetDir)
        {
            var spriteDic = new Dictionary<string, Sprite>();
            if (json[KeyWord.image] != null && json[KeyWord.image].AsObject != null)
            {
                var obj = json[KeyWord.image].AsObject;
                foreach (var item in obj)
                {
                    var keyValue = JSONArray.Parse(item.ToString());
                    if (keyValue.Count < 2 || string.IsNullOrEmpty(keyValue[0]) || string.IsNullOrEmpty(keyValue[1])) continue;
                    var texturePath = assetDir + keyValue[1];
                    var texture = AssetDatabase.LoadAssetAtPath<Sprite>(texturePath);
                    if (texture != null)
                    {
                        spriteDic.Add(keyValue[0], texture);
                    }
                }
            }

            return spriteDic;
        }


    }
}