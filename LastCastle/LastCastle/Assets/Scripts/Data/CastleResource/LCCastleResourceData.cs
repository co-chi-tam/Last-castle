using System;
using System.Collections;

public class LCCastleResourceData : LCBaseData
{

	private int[] m_ResourceCollects;
	private float m_TimeCollect;

	public int[] ResourceCollects
	{
		get { return m_ResourceCollects; }
		set { m_ResourceCollects = value; }
	}

	public float TimeCollect
	{
		get { return m_TimeCollect; }
		set { m_TimeCollect = value; }
	}

	public LCCastleResourceData()
	{
		this.ResourceCollects = null;
		this.TimeCollect = 0f;
	}

	public static LCCastleResourceData Clone (LCCastleResourceData instance) {
		var tmp = new LCCastleResourceData();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.Description = instance.Description;
		tmp.Icon = instance.Icon;
		tmp.EntityType = instance.EntityType;
		tmp.ModelPaths = instance.ModelPaths;
		tmp.FSMPath = instance.FSMPath;
		tmp.Team = instance.Team;
		tmp.CurrentHealthPoint = instance.CurrentHealthPoint;
		tmp.MaxHealthPoint = instance.MaxHealthPoint;
		tmp.ResourceCollects = new int[instance.ResourceCollects.Length];
		Array.Copy (instance.ResourceCollects, tmp.ResourceCollects, instance.ResourceCollects.Length);
		tmp.TimeCollect = instance.TimeCollect;
		return tmp;
	}

}

