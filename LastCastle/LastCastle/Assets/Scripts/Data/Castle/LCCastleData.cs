using System;
using System.Collections.Generic;

public class LCCastleData : LCBaseData
{
	private List<LCWaveData> m_ListWave;
	private LCEnum.EEntityType[] m_TroopTypes;
	private int m_ResourceBonus;
	private int m_HealthPointBonus;
	private float m_MoveSpeedBonus;
	private int m_GoldWinBonus;
	private int[] m_Resources;
	private int m_Population;
	private int m_CostGold;

	public List<LCWaveData> ListWave
	{
		get { return m_ListWave; }
		set { m_ListWave = value; }
	}

	public int Population
	{
		get { return m_Population; }
		set { m_Population = value; }
	}

	public LCEnum.EEntityType[] TroopTypes
	{
		get { return m_TroopTypes; }
		set { m_TroopTypes = value; }
	}

	public int ResourceBonus {
		get { return m_ResourceBonus; }
		set { m_ResourceBonus = value; }
	}

	public int HealthPointBonus {
		get { return m_HealthPointBonus; }
		set { m_HealthPointBonus = value; }
	}

	public float MoveSpeedBonus {
		get { return m_MoveSpeedBonus; }
		set { m_MoveSpeedBonus = value; }
	}

	public int GoldWinBonus {
		get { return m_GoldWinBonus; }
		set { m_GoldWinBonus = value; }
	}

	public int[] Resources {
		get { return m_Resources; }
		set { m_Resources = value; }
	}

	public int CostGold
	{
		get { return m_CostGold; }
		set { m_CostGold = value; }
	}

	public LCCastleData() : base()
	{
		this.Population = 0;
		this.ResourceBonus = 0;
		this.HealthPointBonus = 0;
		this.MoveSpeedBonus = 0f;
		this.GoldWinBonus = 0;
		this.Resources = null;
		this.CostGold = 0;
	}

	public static LCCastleData Clone (LCCastleData instance) {
		var tmp = new LCCastleData();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.Description = instance.Description;
		tmp.Icon = instance.Icon;
		tmp.EntityType = instance.EntityType;
		tmp.ModelPaths = instance.ModelPaths;
		tmp.FSMPath = instance.FSMPath;
		tmp.ListWave = instance.ListWave;
		tmp.Team = instance.Team;
		tmp.CurrentHealthPoint = instance.CurrentHealthPoint;
		tmp.MaxHealthPoint = instance.MaxHealthPoint;
		tmp.Resources = new int[instance.Resources.Length];
		Array.Copy (instance.Resources, tmp.Resources, instance.Resources.Length);
		tmp.CostGold = instance.CostGold;
		tmp.Population = instance.Population;
		tmp.TroopTypes = new LCEnum.EEntityType[instance.TroopTypes.Length];
		Array.Copy (instance.TroopTypes, tmp.TroopTypes, instance.TroopTypes.Length);
		tmp.ResourceBonus = instance.ResourceBonus;
		tmp.HealthPointBonus = instance.HealthPointBonus;
		tmp.MoveSpeedBonus = instance.MoveSpeedBonus;
		tmp.GoldWinBonus = instance.GoldWinBonus;
		return tmp;
	}

}

