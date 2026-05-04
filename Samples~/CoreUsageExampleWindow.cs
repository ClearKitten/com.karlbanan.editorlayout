using UnityEditor;
using UnityEngine;
using KarlBanan.EditorLayout;

public sealed class CoreUsageExampleWindow : EditorWindow
{
    [MenuItem("Tools/KarlBanan/Editor Layout/Core Usage Example")]
    private static void Open()
    {
        GetWindow<CoreUsageExampleWindow>("Core Usage");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Core Usage Example", EditorStyles.boldLabel);
        EditorGUILayout.Space(4f);

        EditorDraw.EvenRow(
            new ExampleInspectorElement("Even A"),
            new ExampleInspectorElement("Even B"),
            new ExampleInspectorElement("Even C")
        );

        EditorGUILayout.Space(8f);

        EditorDraw.FixedRow(
            LayoutSettings.StretchLastSettings,
            new ExampleInspectorElement("Fixed", 100f, false),
            new ExampleInspectorElement("Expands", 120f, true)
        );

        EditorGUILayout.Space(8f);

        EditorDraw.FixedRow(
            LayoutSettings.Padded,
            new InspectorColumn(
                new LayoutSettings(4f, CrossAxisAlignment.Top),
                new ExampleInspectorElement("Column row 1"),
                new ExampleInspectorElement("Column row 2")
            ),
            new InspectorSpace(12f, debugDraw: true),
            new ExampleInspectorElement("After spacer", 160f, true)
        );
    }
}


public sealed class ExampleInspectorElement : IInspectorElement
{
    private readonly string text;
    private readonly float preferredWidth;
    private readonly bool expandWidth;

    public ExampleInspectorElement(string text, float preferredWidth = 120f, bool expandWidth = false)
    {
        this.text = text;
        this.preferredWidth = preferredWidth;
        this.expandWidth = expandWidth;
    }

    public bool CanDraw => true;
    public bool ExpandWidth => expandWidth;
    public bool ExpandHeight => false;

    public float GetMinWidth() => 40f;
    public float GetPreferredWidth() => preferredWidth;
    public float GetMinHeight() => EditorGUIUtility.singleLineHeight;
    public float GetPreferredHeight() => EditorGUIUtility.singleLineHeight;

    public void Draw(Rect rect)
    {
        EditorGUI.HelpBox(rect, text, MessageType.Info);
    }
}
