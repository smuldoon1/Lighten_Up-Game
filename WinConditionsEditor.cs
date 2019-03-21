using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WinConditions))]
public class WinConditionsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        WinConditions wc = (WinConditions)target;

        wc.burnTillCompletion = (WinConditions.CompletionAmount)EditorGUILayout.EnumPopup("BurnTillCompletion", wc.burnTillCompletion);
        if (wc.burnTillCompletion == WinConditions.CompletionAmount.Percentage)
        {
            wc.burnCompletion = EditorGUILayout.Slider("CompletionPercentage", wc.burnCompletion, 0, 100);
        }
        EditorGUILayout.Space();

        wc.tasksTillCompletion = (WinConditions.CompletionAmount)EditorGUILayout.EnumPopup("TasksTillCompletion", wc.tasksTillCompletion);
        if (wc.tasksTillCompletion == WinConditions.CompletionAmount.Percentage)
        {
            wc.taskCompletion = EditorGUILayout.Slider("CompletionPercentage", wc.taskCompletion, 0, 100);
        }
        EditorGUILayout.Space();

        wc.timeOutWinner = (WinConditions.Player)EditorGUILayout.EnumPopup("WinnerOnTimeUp", wc.timeOutWinner);
        if (wc.timeOutWinner != WinConditions.Player.No_Condition)
        {
            EditorGUILayout.BeginHorizontal();
            wc.timeCompletion = EditorGUILayout.IntField("TimeTillCompletion", wc.timeCompletion);
            if (wc.timeCompletion < 0)
            {
                wc.timeCompletion = 0;
            }
            EditorGUILayout.LabelField(Mathf.Floor(wc.timeCompletion / 60).ToString().PadLeft(2, '0') + ":" + (wc.timeCompletion % 60).ToString().PadLeft(2, '0'));
            EditorGUILayout.EndHorizontal();
        }
    }
}