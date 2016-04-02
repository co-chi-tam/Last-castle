using System;

public class LCSodierData : LCBaseData {

	private float m_MoveSpeed;
	private float m_DetectRange;
	private int m_AttackDamage;
	private float m_AttackRange;
	private float m_AttackSpeed;
	private int m_Population;
	private int[] m_CostResources;
	private int[] m_DropResources;

	public float MoveSpeed {
		get { return m_MoveSpeed; }
		set { m_MoveSpeed = value; }
	}

	public float DetectRange {
		get { return m_DetectRange; }
		set { m_DetectRange = value; }
	}

	public int AttackDamage {
		get { return m_AttackDamage; }
		set { m_AttackDamage = value; }
	}

	public float AttackRange {
		get { return m_AttackRange; }
		set { m_AttackRange = value; }
	}

	public float AttackSpeed {
		get { return m_AttackSpeed; }
		set { m_AttackSpeed = value; }
	}

	public int Population
	{
		get { return m_Population; }
		set { m_Population = value; }
	}

	public int[] CostResources {
		get { return m_CostResources; }
		set { m_CostResources = value; }
	}

	public int[] DropResources
	{
		get { return m_DropResources; }
		set { m_DropResources = value; }
	}

	public LCSodierData() : base()
	{
		this.MoveSpeed = 0f;
		this.DetectRange = 0f;
		this.AttackDamage = 0;
		this.AttackRange = 0f;
		this.AttackSpeed = 0f;
		this.CostResources = null;
		this.Population = 0;
	}

	public static LCSodierData Clone (LCSodierData instance) {
		var tmp = new LCSodierData();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.Description = instance.Description;
		tmp.Icon = instance.Icon;
		tmp.EntityType = instance.EntityType;
		tmp.ModelPaths = instance.ModelPaths;
		tmp.FSMPath = instance.FSMPath;
		tmp.Team = instance.Team;
		tmp.MoveSpeed = instance.MoveSpeed;
		tmp.DetectRange = instance.DetectRange;
		tmp.AttackDamage = instance.AttackDamage;
		tmp.AttackRange = instance.AttackRange;
		tmp.AttackSpeed = instance.AttackSpeed;
		tmp.CurrentHealthPoint = instance.CurrentHealthPoint;
		tmp.MaxHealthPoint = instance.MaxHealthPoint;
		tmp.Population = instance.Population;
		tmp.CostResources = new int[instance.CostResources.Length];
		Array.Copy (instance.CostResources, tmp.CostResources, instance.CostResources.Length);
		tmp.DropResources = new int[instance.DropResources.Length];
		Array.Copy (instance.DropResources, tmp.DropResources, instance.DropResources.Length);
		return tmp;
	}
}
