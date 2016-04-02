using UnityEngine;
using System.Collections.Generic;

public class LCLord : LCEntity {

	#region Properties

	private LCLordData m_Data;

	#endregion

	#region Contructor

	public LCLord(LCBaseController ctrl, LCBaseData data): base (ctrl, data)
	{
		m_Data = data as LCLordData;
	}

	#endregion

	#region Main methods

	public override void InitUI()
	{
		base.InitUI();
		if (OnGoldChange != null)
		{
			var gold = GetGold();
			OnGoldChange(gold, gold, 999999);
		}
	}

	public override string SerializeJSON()
	{
		var mapUnlocks = GetMapUnlock();
		var mapUnlockText = string.Empty;
		for (int i = 0; i < mapUnlocks.Count; i++)
		{
			var comma = i < mapUnlocks.Count - 1 ? "," : string.Empty;
			mapUnlockText += (int)mapUnlocks[i] + comma;
		}
		var castleUnlocks = GetCastleUnlock();
		var castleUnlockText = string.Empty;
		for (int i = 0; i < castleUnlocks.Count; i++)
		{
			var comma = i < castleUnlocks.Count - 1 ? "," : string.Empty;
			castleUnlockText += (int)castleUnlocks[i] + comma;
		}
		var serialize = "{"+
			"\"ID\": 0," +
			"\"Name\": \"" + GetName() + "\"," +
			"\"Icon\": \"\"," +
			"\"ModelPaths\": [\"Prefabs/Lord/Lord\"]," +
			"\"EntityType\": 1,"+
			"\"TimeCreate\": \"1/1/1970\"," +
			"\"Gold\": "+ GetGold() +"," +
			"\"MapUnlock\": [" + mapUnlockText + "]," +
			"\"CastleUnlock\": [" + castleUnlockText + "]" +
		"}";
		return serialize;
	}

	#endregion

	#region Getter && Setter

	public override LCEnum.EEntityType GetEntityType()
	{
		return m_Data.EntityType;
	}

	public override int GetGold()
	{
		return m_Data.Gold;
	}

	public override void SetGold(int value)
	{
		base.SetGold(value);
		m_Data.Gold = value < 0 ? 0 : value;
	}

	public override string GetName()
	{
		return m_Data.Name;
	}

	public override void SetName(string value)
	{
		base.SetName(value);
		m_Data.Name = value;
	}

	public override List<LCEnum.EEntityType> GetMapUnlock()
	{
		return m_Data.MapUnlock;
	}

	public override void SetMapUnlock(LCEnum.EEntityType value)
	{
		base.SetMapUnlock(value);
		m_Data.MapUnlock.Add (value);
	}

	public override List<LCEnum.EEntityType> GetCastleUnlock()
	{
		return m_Data.CastleUnlock;
	}

	public override void SetCastleUnlock(LCEnum.EEntityType value)
	{
		base.SetCastleUnlock(value);
		m_Data.CastleUnlock.Add(value);
	}

	#endregion

}
