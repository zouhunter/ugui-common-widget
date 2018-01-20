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
using System;
using UnityEditor;

namespace CommonWidget
{
    public class ImageCreater : ElementCreater
    {
        public override GameObject CreateInstence(WidgetItem info)
        {
            var ok = EditorApplication.ExecuteMenuItem("GameObject/UI/Image");
            if (ok)
            {
                var created = Selection.activeGameObject;
                var image = Selection.activeGameObject.GetComponent<Image>();
                if(info.spriteDic.ContainsKey(KeyWord.sprite))
                {
                    WidgetUtility.InitImage(image, info.spriteDic[KeyWord.sprite], Image.Type.Simple);
                }
                return created;
            }
            return null;
        }
        public override Texture CreatePreview(WidgetItem info)
        {
            Texture texture = null;
            if (info.spriteDic.ContainsKey(KeyWord.sprite))
            {
                var sprite = info.spriteDic[KeyWord.sprite];
                if (sprite != null)
                {
                    texture = sprite.texture;
                }
            }
            return texture;
        }
        protected override List<string> CreateDefultList()
        {
            return new List<string>() { KeyWord.sprite };
        }
    }
}
