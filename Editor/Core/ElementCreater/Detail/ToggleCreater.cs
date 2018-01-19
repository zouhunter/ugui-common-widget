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
    public class ToggleCreater : IElementCreater
    {
        public GameObject CreateInstence(CreateInfo info)
        {
            var ok = EditorApplication.ExecuteMenuItem("GameObject/UI/Toggle");
            if (ok)
            {
                var created = Selection.activeGameObject;
                var textureDic = info.textureDic;
                var toggle = created.GetComponent<Toggle>();
                var background = toggle.targetGraphic as Image;
                var mask = toggle.graphic as Image;
                if(textureDic.ContainsKey(KeyWord.background))
                {
                    background.sprite = textureDic[KeyWord.background];
                    background.type = Image.Type.Simple;
                    background.SetNativeSize();
                }
                if (textureDic.ContainsKey(KeyWord.mask))
                {
                    mask.sprite = textureDic[KeyWord.mask];
                    background.type = Image.Type.Simple;
                    background.SetNativeSize();
                }

                var text = toggle.GetComponentInChildren<Text>();
                text.text = info.name;
                return created;
            }
            else
            {
                return null;
            }
        }

        public Texture CreatePreview(CreateInfo info)
        {
            Texture2D texture = null;
            var textureDic = info.textureDic;
            if (textureDic.ContainsKey(KeyWord.background)){
                var sprite = textureDic[KeyWord.background];
                texture = sprite.texture as Texture2D;
            }
            return texture;
        }
    }
}