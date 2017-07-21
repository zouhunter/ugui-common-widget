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
    public class WidgetWindow : EditorWindow
    {
        [MenuItem(WidgetUtility.Menu_widgetWindow)]
        static void OpenWidow()
        {
            GetWindow<WidgetWindow>();
        }

        SerializedObject serializeObj;
        SerializedProperty scriptProp;
        int currToolbar;
        string[] menus = null;
        ObjectHolder[] allobjhs;
        ObjectHolder[] currobjhs;
        Vector3 scrollpos;
        ObjectHolder activeObjHolder;
        private void OnEnable()
        {
            serializeObj = new SerializedObject(this);
            scriptProp = serializeObj.FindProperty("m_Script");
            LoadObjectHolders();
        }

        private void LoadObjectHolders()
        {
            allobjhs = WidgetUtility.LoadAllGameObject();
            if (allobjhs == null)
            {
                Close();
            }
            else
            {
                List<string> menus = new List<string>();
                for (int i = 0; i < allobjhs.Length; i++)
                {
                    if (!menus.Contains(allobjhs[i].menuName))
                    {
                        menus.Add(allobjhs[i].menuName);
                    }
                }
                this.menus = menus.ToArray();
                LoadCurrObjects();
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.PropertyField(scriptProp);
            DrawToolBarHead();
            DrawScrollViewObjs();
        }

        private void DrawScrollViewObjs()
        {
            using (var scro = new EditorGUILayout.ScrollViewScope(scrollpos))
            {
                scrollpos = scro.scrollPosition;
               
                if (currobjhs != null)
                {
                    int horCount = 3;
                    var width = (this.position.width - 20) / horCount;
                    EditorGUILayout.BeginHorizontal();
                    for (int i = 0; i < currobjhs.Length; i++)
                    {
                        if((i + horCount) % horCount == 0)
                        {
                            EditorGUILayout.EndHorizontal();
                        }
                        if (i % horCount == 0)
                        {
                            EditorGUILayout.BeginHorizontal();
                        }
                        var item = currobjhs[i];
                        var click = GUILayout.Button(new GUIContent(item.prefab.name, item.preview), GUILayout.Width(width),GUILayout.Height(width * 0.6f));
       
                        if (click)
                        {
                            var obj = Selection.activeTransform;
                            if (obj != null)
                            {
                                var rectTrans = obj.GetComponent<RectTransform>();
                                if (rectTrans != null)
                                {
                                    var instence = GameObject.Instantiate(item.prefab);
                                    instence.transform.SetParent(rectTrans, false);
                                    instence.name = item.prefab.name;
                                }
                            }
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        private void DrawToolBarHead()
        {
            EditorGUI.BeginChangeCheck();
            currToolbar = GUILayout.Toolbar(currToolbar, menus);
            var changed = EditorGUI.EndChangeCheck();
            if (changed)
            {
                LoadCurrObjects();
            }
        }

        private void LoadCurrObjects()
        {
            string menu = menus[currToolbar];
            var targetItems = new List<ObjectHolder>(allobjhs);
            var items = targetItems.FindAll(x => x.menuName == menu);
            if (items != null)
            {
                currobjhs = items.ToArray();
            }
            else
            {
                Debug.Log("找不到" + menu);
            }
        }
    }
}
