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
namespace CommonWidget
{
    public struct CreateInfo
    {
        public string name;
        public Dictionary<string, Sprite> textureDic;

        public CreateInfo(string name,Dictionary<string, Sprite> textureDic)
        {
            this.name = name;
            this.textureDic = textureDic;
        }
    }
}
