using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Utility
{
    [CustomEditor(typeof(DoActionsAfterXSeconds))]
    [System.Serializable]
    public class DoActionsAfterXSecondsEditor : Editor {

        // Attributes
        private DoActionsAfterXSeconds actionsExecuter;

        void OnEnable()
        {
            actionsExecuter = target as DoActionsAfterXSeconds;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            #region Attributes
            actionsExecuter.SetActionsName(EditorGUILayout.TextField(new GUIContent("Actions Name"), actionsExecuter.GetActionsName()));
            actionsExecuter.SetCountSeconds(EditorGUILayout.Toggle(new GUIContent("Count seconds"), actionsExecuter.GetCountSeconds()));
            actionsExecuter.SetSecondsToCount(EditorGUILayout.FloatField(new GUIContent("Seconds to count"), actionsExecuter.GetSecondsToCount()));
            actionsExecuter.SetLoop(EditorGUILayout.Toggle(new GUIContent("Loop"), actionsExecuter.GetLoop()));
            #endregion

            #region Actions
            List<GenericAction> actions = new List<GenericAction>(actionsExecuter.GetActions());

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Actions to Do", EditorStyles.boldLabel);
            if (GUILayout.Button("+"))
                actions.Add(new GenericAction(GenericActionKind.Instantiate));
            if (GUILayout.Button("-"))
                actions.RemoveAt(actions.Count - 1);
            EditorGUILayout.EndHorizontal();
            for (int i = 0; i < actions.Count; i++)
                actions[i] = GenericActionField(actions[i]);

            actionsExecuter.SetActions(actions);
            #endregion

            EditorUtility.SetDirty(actionsExecuter);
            serializedObject.ApplyModifiedProperties();
        }

        public GenericAction GenericActionField(GenericAction _genericAction)
        {
            GenericAction genericAction = new GenericAction();
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
            genericAction.SetActionKind((GenericActionKind)EditorGUILayout.EnumPopup(_genericAction.GetActionKind()));
            genericAction.SetTarget((GameObject)EditorGUILayout.ObjectField(_genericAction.GetTarget(), typeof(GameObject), true));
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
            return genericAction;
        }
    }
}
#endif