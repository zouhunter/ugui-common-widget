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
using System.Text.RegularExpressions;

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
        string userPath;
        string match;
        private void OnEnable()
        {
            serializeObj = new SerializedObject(this);
            scriptProp = serializeObj.FindProperty("m_Script");
            LoadObjectHolders();
        }

        private void LoadObjectHolders(string spritePath = null)
        {
            allobjhs = WidgetUtility.LoadAllGameObject(spritePath);
            if (allobjhs == null){
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
                if(menus.Count > 0)
                {
                    LoadCurrObjects();
                }
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.PropertyField(scriptProp);
            DrawUserPath();
            DrawToolBarHead();
            DrawSurchOption();
            DrawScrollViewObjs();
            DrawToolButtons();
        }

        private void DrawSurchOption()
        {
            EditorGUI.BeginChangeCheck();
            match = EditorGUILayout.TextField(match);
            if(EditorGUI.EndChangeCheck())
            {
                LoadCurrObjects();
                var containsKey = new List<ObjectHolder>(currobjhs);
                Regex regex = new Regex(match, RegexOptions.IgnoreCase);
                if (!string.IsNullOrEmpty(match)){
                    currobjhs = containsKey.FindAll(x => regex.Match( x.name).Length > 0).ToArray();
                }
            }
        }

        private void DrawUserPath()
        {
            using (var hor = new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("自定义加载路径:",GUILayout.Width(100));
                EditorGUILayout.LabelField(userPath);
                if(GUILayout.Button("选择", EditorStyles.miniButtonRight,GUILayout.Width(40)))
                {
                    if(Selection.activeObject != null && ProjectWindowUtil.IsFolder(Selection.activeInstanceID))
                    {
                        var path = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
                        userPath = path;
                        currobjhs = null;
                        LoadObjectHolders(userPath);
                    }
                }
            }
        }

        private void DrawToolButtons()
        {
            var rect = GUILayoutUtility.GetRect(position.width, EditorGUIUtility.singleLineHeight);
            var lableRect = new Rect(rect.x, rect.y, 40, rect.height);
            GUI.Label(lableRect,"Tools:");
            var toolButtons = new Rect(rect.width - 60, rect.y, 60, rect.height);
            if (GUI.Button(toolButtons, "ALINE"))
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
                    int horCount = 4;
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
                        var click = GUILayout.Button(new GUIContent(item.Preview), GUILayout.MinWidth(width), GUILayout.MinHeight(width * 0.6f));
                        var lastRect = GUILayoutUtility.GetLastRect();
                        GUI.Label(lastRect, item.name);
                        if (click)
                        {
                            item.CreateInstence();
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
            if (changed && menus.Length > 0)
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
