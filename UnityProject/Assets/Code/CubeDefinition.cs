using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum eCubeType
{
	Grass
}

public class CubeDefinition
{
	//	Static Interface.
	static Dictionary< eCubeType, CubeDefinition > Definitions;

	public static CubeDefinition GetDefinition( eCubeType cubeType )
	{
		return Definitions[ cubeType ];
	}

	public static void AddDefinition( eCubeType cubeType, CubeDefinition cubeDefinition )
	{
		Definitions.Add( cubeType, cubeDefinition );
	}


	//	Acutal Cube Definition.
	public int textureID = 0;
}
