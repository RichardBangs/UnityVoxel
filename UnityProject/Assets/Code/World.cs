using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour
{
	List< Chunk > Chunks = new List< Chunk >();

	void Awake()
	{
		var worldGO = new GameObject( "World" );

		int size = 16;

		for( int x = 0; x < size; x++ )
		{
			for( int y = 0; y < 1; y++ )
			{
				for( int z = 0; z < size; z++ )
				{
					var chunkGO = CreateChunk( x, y, z );
					
					chunkGO.transform.parent = worldGO.transform;

					Chunks.Add( chunkGO.GetComponent< Chunk >() );
				}
			}
		}
	}

	GameObject CreateChunk( int x, int y, int z )
	{
		var chunkGO = new GameObject( string.Format( "Chunk-{0},{1},{2}", x, y, z ) );
		chunkGO.AddComponent< Chunk >();

		chunkGO.transform.localPosition = new Vector3( x * Chunk.size, y * Chunk.size, z * Chunk.size );

		return chunkGO;
	}
}
