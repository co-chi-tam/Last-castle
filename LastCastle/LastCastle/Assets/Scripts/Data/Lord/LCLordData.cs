using System;
using System.Collections.Generic;

public class LCLordData : LCBaseData
{
	private string m_TimeCreate;
	private int m_Gold;
	private List<LCEnum.EEntityType> m_MapUnlock;
	private List<LCEnum.EEntityType> m_CastleUnlock;

	public string TimeCreate
	{
		get { return m_TimeCreate; }
		set { m_TimeCreate = value; }
	}

	public int Gold
	{
		get { return m_Gold; }
		set { m_Gold = value; }
	}

	public List<LCEnum.EEntityType> MapUnlock
	{
		get { return m_MapUnlock; }
		set { m_MapUnlock = value; }
	}

	public List<LCEnum.EEntityType> CastleUnlock
	{
		get { return m_CastleUnlock; }
		set { m_CastleUnlock = value; }
	}

	public LCLordData()
	{
		this.TimeCreate = string.Empty;
		this.Gold = 0;
		this.MapUnlock = null;
		this.CastleUnlock = null;
	}

	public static LCLordData Clone (LCLordData instance) {
		var tmp = new LCLordData ();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.ModelPaths = instance.ModelPaths;
		tmp.EntityType = instance.EntityType;
		tmp.Icon = instance.Icon;
		tmp.TimeCreate = instance.TimeCreate;
		tmp.Gold = instance.Gold;
		tmp.MapUnlock = new List<LCEnum.EEntityType>(instance.MapUnlock);
		tmp.CastleUnlock = new List<LCEnum.EEntityType>(instance.CastleUnlock);
		return tmp;
	}

}

