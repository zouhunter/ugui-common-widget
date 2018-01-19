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
    public class ImageCreater : IElementCreater
    {
        public GameObject CreateInstence(CreateInfo info)
        {
            var ok = EditorApplication.ExecuteMenuItem("GameObject/UI/Image");
            if (ok)
            {
                var created = Selection.activeGameObject;
                var image = Selection.activeGameObject.GetComponent<Image>();
                if(info.textureDic.ContainsKey(KeyWord.sprite))
                {
                    WidgetUtility.InitImage(image, info.textureDic[KeyWord.sprite], Image.Type.Simple);
                }
                return created;
            }
            return null;
        }
        public Texture CreatePreview(CreateInfo info)
        {
            Texture texture = null;
            if (info.textureDic.ContainsKey(KeyWord.sprite))
            {
                var sprite = info.textureDic[KeyWord.sprite];
                if (sprite != null)
                {
                    texture = sprite.texture;
                }
            }
            return texture;
        }
    }
}
