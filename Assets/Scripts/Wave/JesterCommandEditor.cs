using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using PopupWindow = UnityEngine.UIElements.PopupWindow;

namespace Wave
{
    [CustomPropertyDrawer(typeof(WaveData.JesterCommand))]
    public class JesterCommandEditor : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var myInspector = new VisualElement();

            var foldout = new Foldout();
            var propertyField = new PropertyField(property.FindPropertyRelative("action"), "Action");
            propertyField.RegisterValueChangeCallback(_ =>
            {

                foldout.text = Regex.Replace(
                    property.FindPropertyRelative("action").GetEnumName<WaveData.Actions>(),
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
            var jesterCommand = property.boxedValue as WaveData.JesterCommand;

            while (visualElement.childCount > 2)
            {
                visualElement.RemoveAt(2);
            }

            switch (jesterCommand!.action)
            {
                case WaveData.Actions.FireAimed:
                    AddProperty(property, visualElement, "shotData.speed", "Speed");
                    AddProperty(property, visualElement, "shotData.inaccuracy", "Inaccuracy");
                    AddProperty(property, visualElement, "shotData.size", "Size");
                    AddProperty(property, visualElement, "shotData.damage", "Damage");
                    AddProperty(property, visualElement, "shotData.x", "X");
                    AddProperty(property, visualElement, "shotData.y", "Y");
                    AddProperty(property, visualElement, "shotData.randomX", "RandomX");
                    AddProperty(property, visualElement, "shotData.randomY", "RandomY");
                    AddProperty(property, visualElement, "shotData.amount", "Amount");
                    AddProperty(property, visualElement, "shotData.fireBetween", "Fire Delay");
                    AddProperty(property, visualElement, "shotData.spin", "Spin");
                    AddProperty(property, visualElement, "shotData.straight", "Straight");
                    break;
                case WaveData.Actions.FireStorm:
                    AddProperty(property, visualElement, "shotData.speed", "Speed");
                    AddProperty(property, visualElement, "shotData.inaccuracy", "Inaccuracy");
                    AddProperty(property, visualElement, "shotData.size", "Size");
                    AddProperty(property, visualElement, "shotData.damage", "Damage");
                    AddProperty(property, visualElement, "shotData.x", "X");
                    AddProperty(property, visualElement, "shotData.y", "Y");
                    AddProperty(property, visualElement, "shotData.randomX", "RandomX");
                    AddProperty(property, visualElement, "shotData.randomY", "RandomY");
                    AddProperty(property, visualElement, "shotData.amount", "Amount");
                    AddProperty(property, visualElement, "shotData.fireBetween", "Fire Delay");
                    AddProperty(property, visualElement, "shotData.spin", "Spin");
                    AddProperty(property, visualElement, "shotData.straight", "Straight");
                    break;
                case WaveData.Actions.FireBurst:
                    AddProperty(property, visualElement, "shotData.speed", "Speed");
                    AddProperty(property, visualElement, "shotData.inaccuracy", "Inaccuracy");
                    AddProperty(property, visualElement, "shotData.size", "Size");
                    AddProperty(property, visualElement, "shotData.damage", "Damage");
                    AddProperty(property, visualElement, "shotData.x", "X");
                    AddProperty(property, visualElement, "shotData.y", "Y");
                    AddProperty(property, visualElement, "shotData.randomX", "RandomX");
                    AddProperty(property, visualElement, "shotData.randomY", "RandomY");
                    AddProperty(property, visualElement, "shotData.timer", "Timer");
                    AddProperty(property, visualElement, "shotData.amount", "Amount");
                    AddProperty(property, visualElement, "shotData.speed2", "Burst projectile speed");
                    AddProperty(property, visualElement, "shotData.spin", "Spin");
                    AddProperty(property, visualElement, "shotData.straight", "Straight");
                    break;
                case WaveData.Actions.FireCurved:
                    AddProperty(property, visualElement, "shotData.speed", "Speed");
                    AddProperty(property, visualElement, "shotData.inaccuracy", "Inaccuracy");
                    AddProperty(property, visualElement, "shotData.size", "Size");
                    AddProperty(property, visualElement, "shotData.damage", "Damage");
                    AddProperty(property, visualElement, "shotData.x", "X");
                    AddProperty(property, visualElement, "shotData.y", "Y");
                    AddProperty(property, visualElement, "shotData.randomX", "RandomX");
                    AddProperty(property, visualElement, "shotData.randomY", "RandomY");
                    AddProperty(property, visualElement, "shotData.timer", "Timer");
                    AddProperty(property, visualElement, "shotData.gravityDir", "Gravity Direction");
                    AddProperty(property, visualElement, "shotData.spin", "Spin");
                    AddProperty(property, visualElement, "shotData.straight", "Straight");
                    break;
                case WaveData.Actions.FireWavy:
                    AddProperty(property, visualElement, "shotData.speed", "Speed");
                    AddProperty(property, visualElement, "shotData.inaccuracy", "Inaccuracy");
                    AddProperty(property, visualElement, "shotData.size", "Size");
                    AddProperty(property, visualElement, "shotData.damage", "Damage");
                    AddProperty(property, visualElement, "shotData.x", "X");
                    AddProperty(property, visualElement, "shotData.y", "Y");
                    AddProperty(property, visualElement, "shotData.randomX", "RandomX");
                    AddProperty(property, visualElement, "shotData.randomY", "RandomY");
                    AddProperty(property, visualElement, "shotData.frequency", "Frequency");
                    AddProperty(property, visualElement, "shotData.amp", "Amplitude");
                    AddProperty(property, visualElement, "shotData.spin", "Spin");
                    AddProperty(property, visualElement, "shotData.straight", "Straight");
                    break;
                case WaveData.Actions.FireRow:
                    AddProperty(property, visualElement, "shotData.speed", "Speed");
                    AddProperty(property, visualElement, "shotData.inaccuracy", "Inaccuracy");
                    AddProperty(property, visualElement, "shotData.size", "Size");
                    AddProperty(property, visualElement, "shotData.damage", "Damage");
                    AddProperty(property, visualElement, "shotData.x", "X");
                    AddProperty(property, visualElement, "shotData.y", "Y");
                    AddProperty(property, visualElement, "shotData.randomX", "RandomX");
                    AddProperty(property, visualElement, "shotData.randomY", "RandomY");
                    AddProperty(property, visualElement, "shotData.amount", "Amount");
                    AddProperty(property, visualElement, "shotData.radius", "Radius");
                    AddProperty(property, visualElement, "shotData.spin", "Spin");
                    AddProperty(property, visualElement, "shotData.straight", "Straight");
                    break;
                case WaveData.Actions.FireSniper:
                    AddProperty(property, visualElement, "shotData.speed", "Speed");
                    AddProperty(property, visualElement, "shotData.inaccuracy", "Inaccuracy");
                    AddProperty(property, visualElement, "shotData.size", "Size");
                    AddProperty(property, visualElement, "shotData.damage", "Damage");
                    AddProperty(property, visualElement, "shotData.x", "X");
                    AddProperty(property, visualElement, "shotData.y", "Y");
                    AddProperty(property, visualElement, "shotData.randomX", "RandomX");
                    AddProperty(property, visualElement, "shotData.randomY", "RandomY");
                    AddProperty(property, visualElement, "shotData.fireBetween", "Fire Delay");
                    AddProperty(property, visualElement, "shotData.spin", "Spin");
                    AddProperty(property, visualElement, "shotData.straight", "Straight");
                    break;
                case WaveData.Actions.Throw:
                    AddProperty(property, visualElement, "shotData.damage", "Damage");
                    AddProperty(property, visualElement, "shotData.x", "X");
                    AddProperty(property, visualElement, "shotData.y", "Y");
                    AddProperty(property, visualElement, "shotData.throwAirTime", "Air Time");
                    AddProperty(property, visualElement, "shotData.animationCurve", "Animation Curve");
                    break;
                case WaveData.Actions.ThrowAndRoll:
                    AddProperty(property, visualElement, "shotData.damage", "Damage");
                    AddProperty(property, visualElement, "shotData.speed", "Speed");
                    AddProperty(property, visualElement, "shotData.x", "X");
                    AddProperty(property, visualElement, "shotData.y", "Y");
                    AddProperty(property, visualElement, "shotData.throwAirTime", "Air Time");
                    AddProperty(property, visualElement, "shotData.animationCurve", "Animation Curve");
                    break;
            }
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