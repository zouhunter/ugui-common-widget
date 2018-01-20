using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace CommonWidget
{
    public class SliderCreater : ElementCreater
    {
        public override GameObject CreateInstence(WidgetItem info)
        {
            var ok = EditorApplication.ExecuteMenuItem("GameObject/UI/Slider");
            if (ok)
            {
                var obj = Selection.activeGameObject;
                var slider = obj.GetComponent<Slider>();
                var dic = info.spriteDic;
                if (dic.ContainsKey(KeyWord.background))
                {
                    var image = slider.transform.Find("Background").GetComponent<Image>();

                    if (image.sprite.rect.width > image.sprite.rect.height)
                    {
                        slider.SetDirection(Slider.Direction.LeftToRight, true);
                    }
                    else
                    {
                        slider.SetDirection(Slider.Direction.BottomToTop, true);
                    }
                    image.sprite = dic[KeyWord.background];
                    var sliderRect = slider.GetComponent<RectTransform>();
                    sliderRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, image.sprite.rect.width);
                    sliderRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, image.sprite.rect.height);
                    var backgroundRect = image.GetComponent<RectTransform>();
                    backgroundRect.anchorMin = Vector2.zero;
                    backgroundRect.anchorMax = Vector2.one;

                   
                }
                if(dic.ContainsKey(KeyWord.fill))
                {
                    slider.fillRect.GetComponent<Image>().sprite = dic[KeyWord.fill];
                }
                if(dic.ContainsKey(KeyWord.handle))
                {
                    slider.handleRect.GetComponent<Image>().sprite = dic[KeyWord.handle];
                }
                return obj;
            }
            return null;
        }

        public override List<Sprite> GetPreviewList(WidgetItem info)
        {
            var list = new List<Sprite>();
            var dic = info.spriteDic;
            if (dic != null)
            {
                if (dic.ContainsKey(KeyWord.background))
                {
                    list.Add(dic[KeyWord.background]);
                }

                if (dic.ContainsKey(KeyWord.fill))
                {
                    list.Add(dic[KeyWord.fill]);
                }
            }
            return list;
        }

        protected override List<string> CreateDefultList()
        {
            return new List<string>() { KeyWord.background, KeyWord.fill, KeyWord.handle };
        }
    }

}
