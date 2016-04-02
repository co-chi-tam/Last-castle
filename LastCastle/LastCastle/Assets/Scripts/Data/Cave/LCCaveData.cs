using UnityEngine;
using System.Collections;

public class LCCaveData : LCBaseData
{
	private LCEnum.EEntityType m_MemberType;
	private int m_CurrentMember;
	private int m_MaxMember;
	private float m_TimeSpawnSodier;

	public LCEnum.EEntityType MemberType
	{
		get { return m_MemberType; }
		set { m_MemberType = value; }
	}

	public int CurrentMember
	{
		get { return m_CurrentMember; }
		set { m_CurrentMember = value; }
	}

	public int MaxMember
	{
		get { return m_MaxMember; }
		set { m_MaxMember = value; }
	}

	public float TimeSpawnSodier
	{
		get { return m_TimeSpawnSodier; }
		set { m_TimeSpawnSodier = value; }
	} 

	public LCCaveData()
	{
		this.MemberType = LCEnum.EEntityType.None;
		this.CurrentMember = 0;
		this.MaxMember = 0;
		this.TimeSpawnSodier = 0f;
	}

	public static LCCaveData Clone (LCCaveData instance) {
		var tmp = new LCCaveData ();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.EntityType = instance.EntityType;
		tmp.Description = instance.Description;
		tmp.Icon = instance.Icon;
		tmp.FSMPath = instance.FSMPath;
		tmp.ModelPaths = instance.ModelPaths;
		tmp.MemberType = instance.MemberType;
		tmp.MaxMember = instance.MaxMember;
		tmp.TimeSpawnSodier = instance.TimeSpawnSodier;
		return tmp;
	}

}

