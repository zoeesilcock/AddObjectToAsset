using UnityEngine;
using UnityEditor;
using System.Collections;

public class AddObjectToAsset : EditorWindow {
    public Object selectedObject;

    [MenuItem("Assets/Add Object To Asset")]
    private static void ShowParentPicker() {
        string childPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        AddObjectToAsset window = GetWindow<AddObjectToAsset>();
        window.selectedObject = AssetDatabase.LoadAssetAtPath(childPath, typeof(Object)) as Object;
    }

    void AddObjectTo(Object parentAsset, Object childObject) {
        Object objectCopy = Object.Instantiate(childObject);
        objectCopy.name = childObject.name;

        AssetDatabase.AddObjectToAsset(objectCopy, AssetDatabase.GetAssetPath(parentAsset));
        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(childObject));
        AssetDatabase.SaveAssets();
    }

    void ShowPicker() {
        int controlID = EditorGUIUtility.GetControlID(FocusType.Passive);
        EditorGUIUtility.ShowObjectPicker<Object>(null, false, "", controlID);
    }

    void OnGUI() {
        string commandName = Event.current.commandName;

        GUILayout.Box("Chose an asset which you would like this object to be a child of.");

        if (GUILayout.Button("Pick a parent asset")) {
            ShowPicker();
        }

        if (commandName == "ObjectSelectorClosed") {
            Object selectedAsset = EditorGUIUtility.GetObjectPickerObject();
            if (selectedAsset) {
                AddObjectTo(selectedAsset, selectedObject);
            }
            Close();
        }
    }
}
