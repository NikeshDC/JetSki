using UnityEditor;
using UnityEngine;
using Animation;

[CustomPropertyDrawer(typeof(AnimationLabelAttribute))]
public class AnimationLabelAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Object propertyReferenceObject = property.objectReferenceValue;
        if (propertyReferenceObject != null)
        {
            IAnimation animation = propertyReferenceObject as IAnimation;
            label.text = animation.name;
        }
        EditorGUI.PropertyField(position, property, label);
    }
}