using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(uObjectCurrentValue))]
public class ObjectCurrentValueInspector : Editor 
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();
		var myTarget = (uObjectCurrentValue)target;
		var ctrl = myTarget.GetComponent<LCBaseController>();
		if (ctrl != null)
		{
			var objectValues = ctrl.GetObjectCurrentValue();
			foreach (var item in objectValues)
			{
				EditorGUILayout.LabelField(item.Key.ToString(), item.Value == null ? "None" : item.Value.ToString());
			}
		}
	}
}








