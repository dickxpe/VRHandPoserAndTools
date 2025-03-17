// Author: Peter Dickx https://github.com/dickxpe
// MIT License - Copyright (c) 2024 Peter Dickx

using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using UltEvents.Editor;

namespace com.zebugames.meantween.unity
{

    [CustomEditor(typeof(KeyboardEvents))]
    [InitializeOnLoad]
    public class KeyboardEventsCustomEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            // Get reference to the serialized object
            serializedObject.Update();

            // Get properties
            SerializedProperty allKeysProp = serializedObject.FindProperty("allKeys");
            SerializedProperty keyCodeProp = serializedObject.FindProperty("keyCode");
            SerializedProperty onKeyDownProp = serializedObject.FindProperty("onKeyDown");
            SerializedProperty onKeyUpProp = serializedObject.FindProperty("onKeyUp");
            SerializedProperty onKeyProp = serializedObject.FindProperty("onKey");

            // Draw 'allKeys' toggle
            EditorGUILayout.PropertyField(allKeysProp);

            // Show 'keyCode' only when allKeys is false
            if (!allKeysProp.boolValue)
            {
                EditorGUILayout.PropertyField(keyCodeProp);
            }

            // Always show event properties
            EditorGUILayout.PropertyField(onKeyDownProp);
            EditorGUILayout.PropertyField(onKeyUpProp);
            EditorGUILayout.PropertyField(onKeyProp);

            // Apply modified properties
            serializedObject.ApplyModifiedProperties();
        }
    }
}