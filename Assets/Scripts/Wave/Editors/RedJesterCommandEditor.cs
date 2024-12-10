#if UNITY_EDITOR

using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Wave.Jesters.Red;

namespace Wave.Editors
{
    [CustomPropertyDrawer(typeof(RedJesterCommand))]
    public class RedJesterCommandEditor : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var myInspector = new VisualElement();

            var foldout = new Foldout();
            var propertyField = new PropertyField(property.FindPropertyRelative("action"), "Action");
            propertyField.RegisterValueChangeCallback(_ =>
            {
                foldout.text = Regex.Replace(
                    property.FindPropertyRelative("action").GetEnumName<RedJesterActions>(),
                    "([a-z])([A-Z])", "$1 $2");
                Populate(property, foldout);
            });
            AddProperty(property, foldout, "timestamp", "Timestamp");
            foldout.Add(propertyField);

            myInspector.Add(foldout);
            return myInspector;
        }

        private static void Populate(SerializedProperty property, VisualElement visualElement)
        {
            var jesterCommand = property.boxedValue as RedJesterCommand;

            while (visualElement.childCount > 2)
            {
                visualElement.RemoveAt(2);
            }

            AddProperty(property, visualElement, "shotData.speed", "Speed");
            AddProperty(property, visualElement, "shotData.inaccuracy", "Inaccuracy");
            AddProperty(property, visualElement, "shotData.scale", "Size");
            AddProperty(property, visualElement, "shotData.damage", "Damage");
            AddProperty(property, visualElement, "shotData.x", "X");
            AddProperty(property, visualElement, "shotData.y", "Y");
            AddProperty(property, visualElement, "shotData.randomX", "RandomX");
            AddProperty(property, visualElement, "shotData.randomY", "RandomY");

            switch (jesterCommand!.action)
            {
                case RedJesterActions.Throw:
                    AddProperty(property, visualElement, "shotData.numberOfBounces", "Number of Bounces");
                    AddProperty(property, visualElement, "shotData.throwAirTime", "Air Time");
                    AddProperty(property, visualElement, "shotData.fireBetween", "Fire Delay");
                    AddProperty(property, visualElement, "shotData.animationCurve", "Animation Curve");
                    break;
                case RedJesterActions.ThrowAndRoll:
                    AddProperty(property, visualElement, "shotData.numberOfBounces", "Number of Bounces");
                    AddProperty(property, visualElement, "shotData.throwAirTime", "Air Time");
                    AddProperty(property, visualElement, "shotData.animationCurve", "Animation Curve");
                    break;
            }

            AddProperty(property, visualElement, "shotData.breakable", "Breakable");
        }

        private static void AddProperty(SerializedProperty property, VisualElement visualElement,
            string propertyName, string labelText)
        {
            var propertyField = new PropertyField(property.FindPropertyRelative(propertyName), labelText);
            propertyField.Bind(property.serializedObject);
            visualElement.Add(propertyField);
        }
    }
}
#endif