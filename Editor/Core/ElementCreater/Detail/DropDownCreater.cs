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
    public class DropDownCreater : ElementCreater
    {
        public override GameObject CreateInstence(WidgetItem info)
        {
            throw new NotImplementedException();
        }

        public override List<Sprite> GetPreviewList(WidgetItem info)
        {
            throw new NotImplementedException();
        }

        protected override List<string> CreateDefultList()
        {
            throw new NotImplementedException();
        }
    }

}
