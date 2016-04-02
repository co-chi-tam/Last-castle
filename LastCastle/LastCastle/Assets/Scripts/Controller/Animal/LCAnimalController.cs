using UnityEngine;
using System.Collections.Generic;

public class LCAnimalController : LCSodierController
{
	#region Properties

	#endregion

	#region Implementation Monobehaviour

	#endregion

	#region Main methods

	public override Dictionary<string, object> GetObjectCurrentValue()
	{
		var tmp = base.GetObjectCurrentValue();
		var resources = m_Entity.GetDropResources();
		for (int i = 0; i < resources.Length; i++)
		{
			var type = (LCEnum.EResourcesType)i;
			tmp[type.ToString()] = resources[i].ToString();
		}
		return tmp;
	}

	#endregion

	#region FSM


	#endregion

	#region Getter && Setter

	#endregion

}

