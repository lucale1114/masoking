using System;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(JesterSpawner))]
public class WaveEditor : Editor
{
    public VisualTreeAsset m_InspectorXML;
    public float y;
    public float timestamp;

    Waves waves;

    public override VisualElement CreateInspectorGUI()
    {
        // Create a new VisualElement to be the root of our Inspector UI.
        VisualElement myInspector = new VisualElement();
        // Add a simple label.
        myInspector.Add(new Label("This is a custom Inspector"));
        // Load the UXML file.
        m_InspectorXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/WaveEditorUI.uxml");
        // Instantiate the UXML.
        myInspector = m_InspectorXML.Instantiate();
   
        var b = myInspector.Q<Button>();
        Slider ySlider;

        myInspector.Query<Slider>().ForEach((s) =>
        {
            if (s.name == "YSlider")
            {
                Label l = (Label)myInspector.Query("YValue").First();
                ySlider = s;
                ySlider.RegisterValueChangedCallback(v =>
                {
                    y = v.newValue;
                    l.text = y.ToString();
                });
            }
        });

        FloatField floatField = (FloatField)myInspector.Query("FloatField").First();
        floatField.RegisterValueChangedCallback<float>(v =>
        {
            timestamp = v.newValue;   
        });

        Debug.Log(serializedObject.FindProperty("Waves").objectReferenceValue);
        ObjectField waveField = myInspector.Query<ObjectField>().First();
        waveField.RegisterValueChangedCallback(v =>
        {
            serializedObject.FindProperty("Waves").objectReferenceValue = waveField.value;
        });

        TreeView treeView = myInspector.Query<TreeView>().First();
       
        b.clickable.clicked += () =>
        CreateNewJester();

        // Return the finished Inspector UI.
        return myInspector;
    }

    private void CreateNewJester()
    {
        Debug.Log(waves);
    }
}