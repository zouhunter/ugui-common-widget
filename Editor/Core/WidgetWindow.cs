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
            DrawToolButtons();
        }

        private void DrawToolButtons()
        {
            var rect = new Rect(position.width * 0.92f,50, position.width*0.05f, 100);
            if (GUI.Button(rect,"A\nL\nI\nN\nE"))
            {
                var obj = Selection.activeTransform;
                if (obj != null && obj.parent != null)
                {
                    var rectTrans = obj.GetComponent<RectTransform>();
                    var parentTrans = obj.transform.parent.GetComponent<RectTransform>();
                    if (rectTrans != null && parentTrans != null)
                    {
                        SetCustomAnchor(parentTrans, rectTrans);
                        return;
                    }
                }
                EditorUtility.DisplayDialog("未选中", "请先选中需要Aline的对象", "确定");
            }
        }

        private void DrawScrollViewObjs()
        {
            using (var scro = new EditorGUILayout.ScrollViewScope(scrollpos))
            {
                scrollpos = scro.scrollPosition;
               
                if (currobjhs != null)
                {
                    int horCount = 3;
                    var width = (this.position.width * 0.9f) / horCount;
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
        public static void SetCustomAnchor(RectTransform parentRectt, RectTransform rectt)
        {
            Vector2 sizeDelta = rectt.sizeDelta;
            Vector2 p_sizeDelta = parentRectt.sizeDelta;
            Vector2 anchoredPosition = rectt.anchoredPosition;
            float xmin = p_sizeDelta.x * 0.5f + anchoredPosition.x - sizeDelta.x * 0.5f;
            float xmax = p_sizeDelta.x * 0.5f + anchoredPosition.x + sizeDelta.x * 0.5f;
            float ymin = p_sizeDelta.y * 0.5f + anchoredPosition.y - sizeDelta.y * 0.5f;
            float ymax = p_sizeDelta.y * 0.5f + anchoredPosition.y + sizeDelta.y * 0.5f;
            float xSize = 0;
            float ySize = 0;
            float xanchored = 0;
            float yanchored = 0;
            rectt.anchorMin = new Vector2(xmin / p_sizeDelta.x, ymin / p_sizeDelta.y);
            rectt.anchorMax = new Vector2(xmax / p_sizeDelta.x, ymax / p_sizeDelta.y);
            rectt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sizeDelta.x);
            rectt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sizeDelta.y);
            rectt.sizeDelta = new Vector2(xSize, ySize);
            rectt.anchoredPosition = new Vector2(xanchored, yanchored);
        }
    }
}
