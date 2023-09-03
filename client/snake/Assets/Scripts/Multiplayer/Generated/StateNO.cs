// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.46
// 

using Colyseus.Schema;

public partial class StateNO : Schema {
	[Type(0, "map", typeof(MapSchema<PlayerNO>))]
	public MapSchema<PlayerNO> players = new MapSchema<PlayerNO>();

	[Type(1, "map", typeof(MapSchema<AppleNO>))]
	public MapSchema<AppleNO> apples = new MapSchema<AppleNO>();
}

