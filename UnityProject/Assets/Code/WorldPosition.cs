using System;
using UnityEngine;

public struct WorldPosition
{
	public int WorldX;
	public int WorldY;
	public int WorldZ;
	
	public WorldPosition( int worldX, int worldY, int worldZ )
	{
		this.WorldX = worldX;
		this.WorldY = worldY;
		this.WorldZ = worldZ;
	}
	
	public Vector3 ToVector3()
	{
		return new Vector3( WorldX, WorldY, WorldZ );
	}

	public static WorldPosition operator +( WorldPosition left, WorldPosition right )
	{
		return new WorldPosition( left.WorldX + right.WorldX, left.WorldY + right.WorldY, left.WorldZ + right.WorldZ );
	}

	public static WorldPosition operator *( WorldPosition left, int multiplier )
	{
		return new WorldPosition( left.WorldX * multiplier, left.WorldY * multiplier, left.WorldZ * multiplier );
	}

	public static WorldPosition operator +( WorldPosition left, Vector3 right )
	{
		return new WorldPosition( left.WorldX + (int)right.x, left.WorldY + (int)right.y, left.WorldZ + (int)right.z );
	}

	public override string ToString ()
	{
		return string.Format( "[WorldPosition] WorldX={0}, WorldY={1}, WorldZ={2}", WorldX, WorldY, WorldZ );
	}
}