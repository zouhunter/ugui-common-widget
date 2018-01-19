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
    public class ButtonCreater : IElementCreater
    {
        public GameObject CreateInstence(CreateInfo info)
        {
            var ok = EditorApplication.ExecuteMenuItem("GameObject/UI/Button");
            if (ok)
            {
                var created = Selection.activeGameObject;
                var textureDic = info.textureDic;
                var btn = created.GetComponent<Button>();

                if (textureDic.ContainsKey(KeyWord.normal))
                {
                    var texture = textureDic[KeyWord.normal];

                    var image = btn.targetGraphic as Image;
                    image.sprite = texture;
                    image.type = Image.Type.Simple;
                    image.SetNativeSize();

                    var text = btn.GetComponentInChildren<Text>();
                    text.text = info.name;
                }

                foreach (var item in textureDic)
                {
                    if (item.Key == KeyWord.normal) continue;
                    btn.transition = Selectable.Transition.SpriteSwap;
                    var keyword = item.Key;
                    var sprite = item.Value;
                    var state = btn.spriteState;
                    if (keyword == KeyWord.disabled)
                    {
                        state.disabledSprite = sprite;
                    }
                    else if (keyword == KeyWord.highlighted)
                    {
                        state.highlightedSprite = sprite;
                    }
                    else if (keyword == KeyWord.pressed)
                    {
                        state.pressedSprite = sprite;
                    }
                    btn.spriteState = state;
                }

                return created;
            }
            else
            {
                return null;
            }
        }

        public Texture CreatePreview(CreateInfo info)
        {
            Texture texture = null;
            var textureDic = info.textureDic;
            if (textureDic.ContainsKey(KeyWord.normal)){
                var sprite = textureDic[KeyWord.normal];
                texture = sprite.texture;
            }
            return texture;
        }
    }
}