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

namespace CommonWidget
{
    public abstract class ElementCreater : IElementCreater
    {
        private List<string> _keys;
        public virtual List<string> Keys { get { if (_keys == null) _keys = CreateDefultList(); return _keys; } }
        public abstract GameObject CreateInstence(WidgetItem info);
        public abstract Texture CreatePreview(WidgetItem info);
        protected abstract List<string> CreateDefultList();
    }
}
