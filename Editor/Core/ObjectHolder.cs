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
using System.Collections.Generic;

namespace CommonWidget
{
    public class ObjectHolder
    {
        private Texture _preview;
        private Dictionary<string, Sprite> _textures;
        public bool effective { get; private set; }
        public string name { get; private set; }
        public WidgetType widgetType { get; private set; }
        public Texture Preview
        {
            get
            {
                if (_preview == null)
                    _preview = WidgetUtility. CreatePreview(widgetType,new CreateInfo(name, textureDic));
                return _preview;
            }
        }
        public string menuName { get { return widgetType.ToString(); } }
        public Dictionary<string, Sprite> textureDic { get { if (_textures == null) _textures = LoadTextures(); return _textures; } }

        private JSONClass json;
        private string assetDir;

        public ObjectHolder(string dir,JSONClass json)
        {
            if(json[KeyWord.name] != null)
            {
                this.name = json[KeyWord.name];
            }
            else
            {
                this.name = json[KeyWord.type].Value;
            }
            this.assetDir = dir;
            effective = true;
          
            this.json = json; 

            if (string.IsNullOrEmpty(json.ToString()) || string.IsNullOrEmpty(json[KeyWord.type].Value))
            {
                effective = false;
                return;
            }

            var type = System.Enum.Parse(typeof(WidgetType), json[KeyWord.type]);
            if (type == null)
            {
                effective = false;
                return;
            }

            widgetType = (WidgetType)type;
            effective = true;
        }

        public ObjectHolder(Sprite sprite)
        {
            widgetType = WidgetType.Image;
            name = sprite.name;
            _textures = new Dictionary<string, Sprite>();
            _textures.Add(KeyWord.sprite, sprite);
        }

        public GameObject CreateInstence()
        {
            RectTransform parent = null;
            var lastTrans = Selection.activeTransform;
            if(lastTrans != null && lastTrans is RectTransform){
                parent = lastTrans as RectTransform;
            }
            else
            {
                var canvas = GameObject.FindObjectOfType<Canvas>();
                if(canvas != null)
                {
                    parent = canvas.GetComponent<RectTransform>();
                }
                else
                {
                    var ok = EditorApplication.ExecuteMenuItem("GameObject/UI/Canvas");
                    if(ok)
                    {
                        parent = Selection.activeGameObject.GetComponent<RectTransform>();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            Selection.activeObject = parent;
            var info = new CreateInfo(name, textureDic);
            var created = WidgetUtility.CreateInstence(widgetType,info);
            if (created != null) {
                created.transform.SetParent(parent, false);
                created.name = name;
                created.transform.localPosition = Vector3.zero;
            }
            return created;
        }

  

        private Dictionary<string, Sprite> LoadTextures()
        {
            var textureDic = new Dictionary<string, Sprite>();

            var temp = JSONClass.Parse(json.ToString()).AsObject;
            temp.Remove("type");
            foreach (var item in temp)
            {
                var keyValue = JSONArray.Parse(item.ToString());
                if (keyValue.Count < 2 || string.IsNullOrEmpty(keyValue[0]) || string.IsNullOrEmpty(keyValue[1])) continue;
                var texturePath = assetDir + keyValue[1];
                var texture = AssetDatabase.LoadAssetAtPath<Sprite>(texturePath);
                if (texture != null)
                {
                    textureDic.Add(keyValue[0], texture);
                }
            }
            return textureDic;
        }

    }
}

