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
    public class CercalSliderCreater : IElementCreater
    {
        public List<string> Keys
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public GameObject CreateInstence(WidgetItem info)
        {
            throw new NotImplementedException();
        }

        public Texture CreatePreview(WidgetItem info)
        {
            throw new NotImplementedException();
        }
    }
}