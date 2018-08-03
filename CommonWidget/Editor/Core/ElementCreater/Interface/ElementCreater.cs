using UnityEngine;
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

    public abstract class DefultElementCreater : ElementCreater
    {
        protected abstract string MenuPath { get; }
        protected abstract Type CommponentType { get; }
        public override GameObject CreateOrCharge(WidgetItem info)
        {
            var oldBtn = info.parent.GetComponent(CommponentType);
            if (oldBtn != null)
            {
                Undo.RecordObject(oldBtn, "charge:" + oldBtn.name);
                ChargeWidgetInfo(oldBtn, info);
                return oldBtn.gameObject;
            }
            else
            {
                var ok = EditorApplication.ExecuteMenuItem(MenuPath);
                if (ok)
                {
                    var created = Selection.activeGameObject;
                    var btn = created.GetComponent(CommponentType);
                    btn.name = info.name;
                    Undo.RecordObject(btn, "charge:" + btn.name);
                    ChargeWidgetInfo(btn, info);
                    return btn.gameObject;
                }
            }

            return null;
        }
        protected abstract void ChargeWidgetInfo(Component component, WidgetItem info);
    }

    public abstract class ElementCreater : IElementCreater
    {
        protected List<string> _keys;
        public virtual List<string> Keys { get { if (_keys == null) _keys = CreateDefultList(); return _keys; } }
        public abstract GameObject CreateOrCharge(WidgetItem info);
        public abstract List<Sprite> GetPreviewList(WidgetItem info);
        protected abstract List<string> CreateDefultList();
    }
}
