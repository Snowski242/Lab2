using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathManager))]
public class PathManagerEditor : Editor
{
    [SerializeField]
    PathManager pathManager;

    [SerializeField] List<Waypoint> thePath;

    List<int> toDelete;
    Waypoint selectedpoint = null;
    bool doRepaint = true;
    private void OnSceneGUI()
    {
        thePath = pathManager.GetPath();
        DrawPath(thePath);

    }

    private void OnEnable()
    {
        pathManager = target as PathManager;
        toDelete = new List<int>();
    }

    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();
        thePath = pathManager.GetPath();

        base.OnInspectorGUI();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("path");

        DrawGUIForPoints();

        if (GUILayout.Button("Add point to path"))
        {
            pathManager.CreateAddPoint();
        }

        EditorGUILayout.EndVertical();
        SceneView.RepaintAll();
    }

    void DrawGUIForPoints()
    {
        if (thePath != null && thePath.Count > 0)
        {

            for (int i = 0; i < thePath.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                Waypoint p = thePath[i];

                Color c = GUI.color;
                if (selectedpoint == p) GUI.color = Color.green;

                Vector3 oldPos = p.GetPos();
                Vector3 newPos = EditorGUILayout.Vector3Field("", oldPos);

                if (EditorGUI.EndChangeCheck()) p.SetPos(newPos);
                if (GUILayout.Button("-", GUILayout.Width(25)))
                {
                    toDelete.Add(i);
                }
                EditorGUILayout.EndHorizontal();
            }
            if (toDelete.Count > 0)
            {
                foreach (int i in toDelete)
                {
                    thePath.RemoveAt(i);
                    toDelete.Clear();
                }
            }

        }

    }

    public void DrawPath(List<Waypoint> path)
    {
        if (path != null)
        {
            int current = 0;
            foreach (Waypoint wp in path)
            {
                doRepaint = DrawPoint(wp);
                int next = (current + 1) % path.Count;
                Waypoint wpNext = path[next];
                DrawPathLine(wp, wpNext);
                current += 1;

            }
            if (doRepaint) Repaint();
        }
    }
    public void DrawPathLine(Waypoint wp1, Waypoint wp2)
    {
        Color c = Handles.color;
        Handles.color = Color.gray;
        Handles.DrawLine(wp1.GetPos(), wp2.GetPos());
        Handles.color = c;
    }
    bool DrawPoint(Waypoint point)
    {
        bool isChanged = false;

        if (selectedpoint == point)
        {
            Color c = Color.green;
            Handles.color = Color.green;

            EditorGUI.BeginChangeCheck();
            Vector3 oldPos = point.GetPos();
            Vector3 newPos = Handles.PositionHandle(oldPos, Quaternion.identity);

            float handleSize = HandleUtility.GetHandleSize(newPos);
            Handles.SphereHandleCap(-1, newPos, Quaternion.identity, 0.25f * handleSize, EventType.Repaint);

            if (EditorGUI.EndChangeCheck())
            {
                point.SetPos(newPos);
            }

            Handles.color = c;

        }
        else
        {
            Vector3 currPos = point.GetPos();
            float handleSize = HandleUtility.GetHandleSize(currPos);
            if (Handles.Button(currPos, Quaternion.identity, 0.25f * handleSize,
                0.25f * handleSize, Handles.SphereHandleCap))
            {
                isChanged = true;
                selectedpoint = point;
            }
        }
        return isChanged;

    }


}
