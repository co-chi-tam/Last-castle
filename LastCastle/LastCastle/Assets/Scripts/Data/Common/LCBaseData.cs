
public class LCBaseData : LCInfo
{
	private string m_Icon;
	private LCEnum.EEntityType m_EntityType;
	private string m_FSMPath;
	private string[] m_ModelPaths;
	private LCEnum.EClass m_Team;
	private int m_CurrentHealthPoint;
	private int m_MaxHealthPoint;

	public string Icon {
		get { return m_Icon; }
		set { m_Icon = value; }
	}

	public LCEnum.EEntityType EntityType { 
		get { return m_EntityType; } 
		set { m_EntityType = value; } 
	}

	public string FSMPath {
		get { return m_FSMPath; }
		set { m_FSMPath = value; }
	}

	public string[] ModelPaths {
		get { return m_ModelPaths; }
		set { m_ModelPaths = value; }
	}

	public LCEnum.EClass Team
	{
		get { return m_Team; }
		set { m_Team = value; }
	}

	public int CurrentHealthPoint {
		get { return m_CurrentHealthPoint; }
		set { m_CurrentHealthPoint = value; }
	}

	public int MaxHealthPoint {
		get { return m_MaxHealthPoint; }
		set { m_MaxHealthPoint = value; }
	}

	public LCBaseData() : base()
    {
		this.m_EntityType	= LCEnum.EEntityType.None;
		this.m_Icon		= string.Empty;
		this.m_FSMPath 	= string.Empty;
		this.m_ModelPaths = null;
		this.Team = LCEnum.EClass.None;
		this.CurrentHealthPoint = 0;
		this.MaxHealthPoint = 0;
    }

	public static LCBaseData Clone (LCBaseData instance) {
		var tmp = new LCBaseData ();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.EntityType = instance.EntityType;
		tmp.Description = instance.Description;
		tmp.Icon = instance.Icon;
		tmp.FSMPath = instance.FSMPath;
		tmp.ModelPaths = instance.ModelPaths;
		tmp.Team = instance.Team;
		tmp.CurrentHealthPoint = instance.CurrentHealthPoint;
		tmp.MaxHealthPoint = instance.MaxHealthPoint;
		return tmp;
	}
}
