using UnityEngine;
using System.Collections;

public static class AOCalculation
{
	static World world;

	public static void Initialise( World world )
	{
		AOCalculation.world = world;
	}

	public static void ExecuteOnChunk( Chunk chunk )
	{
		WorldPosition chunkPosition = chunk.WorldPosition;

		foreach( var cube in chunk.Cubes )
		{
			if( cube == null )
				continue;

			for( int index = 0; index < cube.Verticies.Length; index++ )
			{
				float accumulatedLight = 0.0f;

				Vector3 direction = ( cube.Verticies[ index ] - cube.Position.ToVector3() ).normalized;
				Vector3 normal = cube.Normals[ index ].normalized;

				const int distanceMax = 2;
				for( int distance = 1; distance < distanceMax; distance++ )
				{
					WorldPosition midPoint = cube.Position + ( GetDeltaWorldPositionFromVector3( normal ) * distance );

					WorldPosition axisX;
					WorldPosition axisZ;
					GetOtherAxis( normal, out axisX, out axisZ );

					for( int xOffset = -distance; xOffset <= distance; xOffset++ )
					{
						for( int zOffset = -distance; zOffset <= distance; zOffset++ )
						{
							WorldPosition iterationPosition = midPoint + ( axisX * xOffset ) + ( axisZ * zOffset );

							if( world.WorldCoordinatesToCube( iterationPosition + chunkPosition ) == null )
							{
								accumulatedLight += 0.1f;
							}
						}
					}
				}

				cube.Colours[ index ] = new Color( accumulatedLight, accumulatedLight, accumulatedLight );
			}
		}
	}

	static WorldPosition GetDeltaWorldPositionFromVector3( Vector3 deltaPosition )
	{
		WorldPosition deltaWorldPosition = new WorldPosition();
		const float nonZeroMargin = 0.1f;
		
		if( deltaPosition.x > nonZeroMargin )
			deltaWorldPosition.WorldX = Mathf.CeilToInt( deltaPosition.x );
		if( deltaPosition.y > nonZeroMargin )
			deltaWorldPosition.WorldY = Mathf.CeilToInt( deltaPosition.y );
		if( deltaPosition.z > nonZeroMargin )
			deltaWorldPosition.WorldZ = Mathf.CeilToInt( deltaPosition.z );
		
		if( deltaPosition.x < -nonZeroMargin )
			deltaWorldPosition.WorldX = Mathf.FloorToInt( deltaPosition.x );
		if( deltaPosition.y < -nonZeroMargin )
			deltaWorldPosition.WorldY = Mathf.FloorToInt( deltaPosition.y );
		if( deltaPosition.z < -nonZeroMargin )
			deltaWorldPosition.WorldZ = Mathf.FloorToInt( deltaPosition.z );

		return deltaWorldPosition;
	}

	static void GetOtherAxis( Vector3 vector, out WorldPosition outAxis1, out WorldPosition outAxis2 )
	{
		const float nonZeroMargin = 0.1f;

		Vector3 axis1, axis2;

		if( Mathf.Abs( vector.x ) > nonZeroMargin )
		{
			axis1 = new Vector3( 0.0f, 1.0f, 0.0f );
			axis2 = new Vector3( 0.0f, 0.0f, 1.0f );
		}
		else if( Mathf.Abs( vector.y ) > nonZeroMargin )
		{
			axis1 = new Vector3( 1.0f, 0.0f, 0.0f );
			axis2 = new Vector3( 0.0f, 0.0f, 1.0f );
		}
		else
		{
			axis1 = new Vector3( 0.0f, 1.0f, 0.0f );
			axis2 = new Vector3( 1.0f, 0.0f, 0.0f );
		}

		outAxis1 = GetDeltaWorldPositionFromVector3( axis1 );
		outAxis2 = GetDeltaWorldPositionFromVector3( axis2 );
	}
}
