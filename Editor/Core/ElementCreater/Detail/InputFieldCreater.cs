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
using System;

namespace CommonWidget
{
    public class InputFieldCreater : ElementCreater
    {
        public override GameObject CreateInstence(WidgetItem info)
        {
            var ok = EditorApplication.ExecuteMenuItem("GameObject/UI/Input Field");
            if (ok)
            {
                var inputfield = Selection.activeGameObject.GetComponent<InputField>();
                if (info.spriteDic.ContainsKey(KeyWord.background)){
                    var image = inputfield.targetGraphic as Image;
                    WidgetUtility.InitImage(image, info.spriteDic[KeyWord.background]);
                }

                return inputfield.gameObject;
            }
            return null;
        }

        public override List<Sprite> GetPreviewList(WidgetItem info)
        {
            var list = new List<Sprite>();
            if(info.spriteDic.ContainsKey(KeyWord.background))
            {
                list.Add(info.spriteDic[KeyWord.background]);
            }
            return list;
        }

        protected override List<string> CreateDefultList()
        {
            return new List<string>() { KeyWord.background };
        }
    }
}
