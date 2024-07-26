using UnityEditor;
using Animation;

[CustomEditor(typeof(TweenAnimation))]
public class TweenAnimationEditor : Editor
{
    TweenAnimation tweenAnimation;

    const string prop_ease = "_ease";
    const string prop_easeCurve = "_easeCurve";
    const string prop_jumpPower = "_jumpPower";
    const string prop_rotationMode = "_rotationMode";

    public void OnEnable()
    {
        tweenAnimation = (TweenAnimation)target;
    }

    private void DisplayPropertyConditionally(SerializedProperty property, bool condition = true)
    { if (condition) { EditorGUILayout.PropertyField(property); } }

    private void DisablePropertyConditionally(SerializedProperty property, bool condition = true)
    {
        EditorGUI.BeginDisabledGroup(condition);
        EditorGUILayout.PropertyField(property);
        EditorGUI.EndDisabledGroup();
    }

    public override void OnInspectorGUI()
    {
        SerializedProperty property = serializedObject.GetIterator();
        property.NextVisible(true);

        do
        {
            switch(property.name)
            {
                case prop_jumpPower:
                    DisplayPropertyConditionally(property, tweenAnimation.type == TweenAnimation.Type.Jump);
                    break;
                case prop_ease:
                    DisablePropertyConditionally(property, tweenAnimation.useCustomEaseCurve == true);
                    break;
                case prop_easeCurve:
                    EditorGUI.indentLevel++;
                    DisablePropertyConditionally(property, tweenAnimation.useCustomEaseCurve == false);
                    EditorGUI.indentLevel--;
                    break;
                case prop_rotationMode:
                    DisplayPropertyConditionally(property, tweenAnimation.type == TweenAnimation.Type.Rotate);
                    break;
                default:
                    EditorGUILayout.PropertyField(property);
                    break;
            }
        }
        while (property.NextVisible(false));

        serializedObject.ApplyModifiedProperties();
    }
}
