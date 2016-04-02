using System;
public class LCEnum
{
    public enum EEntityType : int {
        None = 0,
		Lord = 1,

		// Human
		Human = 100,
		Warrior = 101,
		Mage = 102,
		Archer = 103,
		Paladin = 104,
		BeastHunter = 105,

		// Demon
		Demon = 200,
		DemonWarrior = 201,
		DemonMage = 202,
		DemonArcher = 203,
		DeathKnight = 204,
		DemonHunter = 205,

		// Castle
		Castle = 300,
		StoneCastle = 301,
		FireCastle = 302,
			
		// CastleResources
		CastleResources = 400,
		Farm = 401,
		LakeFishing = 402,
		Mine = 403,
		CattleStation = 404,

		// Cave
		Cave = 500,
		BunnyCave = 501,

		// Animal
		Animal = 600,
		Bunny = 601,

		// Map
		Map = 1000,
		Map_1_1 = 1001,
		Map_1_2 = 1002,
		Map_1_3 = 1003,
		Map_1_4 = 1004,
		Map_1_5 = 1005,
		Map_1_6 = 1006,
		Map_1_7 = 1007,
		Map_1_8 = 1008,
		Map_1_9 = 1009
    }

	public enum EClass : int {
		None = 0,
		Human = 1,
		Demon = 2,
		Natural = 3
	}

	public enum ELayer: int {
		None = 0,
		Plane = 8,
		Troop = 9, 
		Castle = 10,
		CastleResource = 11,
		Natural = 12,
		Animal = 13
	}

	public enum ECommand : int {
		AI = 0,
		Manual = 1
	}

	public enum EResourcesType : int {
		Cereal = 0,
		Fish = 1,
		Meat = 2,
		Metal = 3,

		None = 100
	}

	public enum EScene : int 
	{
		StartScene = 1,
		BattleScene = 2
	}

	public enum EObjectType : int
	{
		Neutral = 0,
		Vilain = 1,
		Partner = 2
	}
    
}
