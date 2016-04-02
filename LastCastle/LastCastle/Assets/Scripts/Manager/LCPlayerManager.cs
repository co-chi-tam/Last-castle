using UnityEngine;
using System.Collections;

public class LCPlayerManager {

	public static LCEntity CurrentPlayer;
	public static LCEnum.EEntityType CurrentMap = LCEnum.EEntityType.Map_1_1;
	public static LCEnum.EEntityType CurrentCastle = LCEnum.EEntityType.StoneCastle;

	public static void SaveStatus()
	{
		PlayerPrefs.SetString(Utilities.LORD_DATA, CurrentPlayer.SerializeJSON());
		PlayerPrefs.Save();
	}

}
