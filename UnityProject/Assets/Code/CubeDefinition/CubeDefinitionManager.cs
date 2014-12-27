using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public static class CubeDefinitionManager
{
	static Dictionary< string, CubeDefinition > Definitions = new Dictionary< string, CubeDefinition >();

	static CubeDefinitionManager()
	{
		//	TODO: Load from XML or JSON.
		var grass = new CubeDefinition();
		grass.defaultTextureIndex = new TextureIndex( 2, 15 );	//	Default to MUD!
		grass.perFaceOverrideForTextureIndex.Add( 5, new TextureIndex( 0, 15 ) );	//	Grass on the top!

		AddDefinition( "Grass", grass );
	}

	public static CubeDefinition GetDefinition( string cubeType )
	{
		return Definitions[ cubeType ];
	}
	
	public static void AddDefinition( string cubeType, CubeDefinition cubeDefinition )
	{
		Definitions.Add( cubeType, cubeDefinition );
	}
}
