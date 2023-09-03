// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.46
// 

using Colyseus.Schema;

public partial class PlayerNO : Vector2NO {
	[Type(2, "string")]
	public string name = default(string);

	[Type(3, "uint8")]
	public byte size = default(byte);

	[Type(4, "string")]
	public string color = default(string);

	[Type(5, "uint16")]
	public ushort score = default(ushort);
}

