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
    public class ToggleCreater : ElementCreater
    {
        public override GameObject CreateInstence(WidgetItem info)
        {
            var ok = EditorApplication.ExecuteMenuItem("GameObject/UI/Toggle");
            if (ok)
            {
                var created = Selection.activeGameObject;
                var spriteDic = info.spriteDic;
                var toggle = created.GetComponent<Toggle>();
                var background = toggle.targetGraphic as Image;
                var mask = toggle.graphic as Image;
                if(spriteDic.ContainsKey(KeyWord.background))
                {
                    background.sprite = spriteDic[KeyWord.background];
                    background.type = Image.Type.Simple;
                    background.SetNativeSize();
                }
                if (spriteDic.ContainsKey(KeyWord.mask))
                {
                    mask.sprite = spriteDic[KeyWord.mask];
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
        public override Texture CreatePreview(WidgetItem info)
        {
            Texture2D texture = null;
            var spriteDic = info.spriteDic;
            if (spriteDic.ContainsKey(KeyWord.background)){
                var sprite = spriteDic[KeyWord.background];
                texture = sprite.texture as Texture2D;
            }
            return texture;
        }
        protected override List<string> CreateDefultList()
        {
            return new List<string>() { KeyWord.background, KeyWord.mask};
        }
    }
}