
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LvlCreator)), CanEditMultipleObjects]
public class LvlCreatorEditorExtention : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LvlCreator lvlCreator = (LvlCreator)target;

        lvlCreator.DefaultColor = EditorGUILayout.ColorField("Default Color", lvlCreator.DefaultColor);

        lvlCreator.AlternativeColor = EditorGUILayout.ColorField("Alternative Color", lvlCreator.AlternativeColor);
    }
}

