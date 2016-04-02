
public class LCAnimalData : LCSodierData
{
	
	public LCAnimalData()
	{
		this.DropResources = null;
	}

	public static LCAnimalData Clone (LCAnimalData instance) {
		var tmp = new LCAnimalData();
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
		tmp.DropResources = instance.DropResources;
		return tmp;
	}

}

