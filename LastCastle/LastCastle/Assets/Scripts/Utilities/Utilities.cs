using UnityEngine;
using System.Collections;

public class Utilities {

	public const string LORD_DATA = "LORD_DATA";

	public static Vector3 ConvertStringToV3(string value) {
		var result = Vector3.zero;
		var tmp = value.Split(',');
		result.x = float.Parse(tmp[0].ToString());
		result.y = float.Parse(tmp[1].ToString());
		result.z = float.Parse(tmp[2].ToString());
		return result;
	}

	public static Quaternion ConvertStringToQuaternion(string value) {
		var tmp = ConvertStringToV3(value);
		var result = Quaternion.Euler(tmp);
		return result;
	}

	public static T GetImage<T> (string path, string name) where T : class
	{
		var resources = Resources.LoadAll(path);
		for (int i = 0; i < resources.Length; i++)
		{
			if (resources[i].name == name)
				return resources[i] as T;
		}
		return default(T);
	}

}
