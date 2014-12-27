using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chunk : MonoBehaviour
{
	//	A chunk:
	//		- Is a collection of cubes.
	//		- Is of fixed size in world space.
	//		- Is cuboid.
	//		- Contains a maximum number of cubes: size * size * size
	//		- Constructs buffers to render all contained Cubes.
	//		- Is rendered only if visible (in view frustrum).
	//		- When we need to do processing in the world, we normally split it up to processing in Chunks, as a fast way to split the problem and only update what's visible.
	
	//	Unity restrictions mean a single draw call can only contain a maximum of 65000 verticies. With our current brute force approach regarding a single cube, this restricts us to
	//	8*8*8 (*24 verts per cube). If it makes sense to optimise the number of verts per cube down, or do inner Chunk visibility culling, we could increase this size to 16.
	
	public const int size = 8;
	
	public Cube[] Cubes = new Cube[ size * size * size ];

	public WorldPosition WorldPosition;

	MeshFilter meshFilter;
	MeshRenderer meshRenderer;

	bool dirty = false;

	void Awake()
	{
		meshFilter = gameObject.AddComponent< MeshFilter >();
		meshRenderer = gameObject.AddComponent< MeshRenderer >();

		var material = Resources.Load< Material >( "Cube" );
		meshRenderer.sharedMaterial = material;

		//	TODO: Remove me - just here for testing purposes.
		//Random.seed = 42;
		for( int x = 0; x < size; x++ )
		{
			for( int y = 0; y < size; y++ )
			{
				for( int z = 0; z < size; z++ )
				{
					//if( x % 2 == 0 || y % 2 == 0 || z % 2 == 0 )
					//	continue;

					if( Random.value > 0.5f )
						continue;

					var testCube = new Cube();
					testCube.Position = new WorldPosition( x, y, z );
					testCube.GenerateData();
					Cubes[ GetIndex( x, y, z ) ] = testCube;
				}
			}
		}

		dirty = true;
	}

	int GetIndex( int x, int y, int z )
	{
		return x + ( y * size ) + ( z * size * size );
	}

	void GenerateMesh()
	{
		var mesh = new Mesh ();
		mesh.name = "CubeRenderer Mesh";

		var verticies = new List< Vector3 >();
		var normals = new List< Vector3 >();
		var uvs = new List< Vector2 >();
		var triangles = new List< int >();
		var colours = new List< Color >();
		foreach( var cube in Cubes )
		{
			if( cube == null )
				continue;

			int vertexOffset = verticies.Count;

			verticies.AddRange( cube.Verticies );
			uvs.AddRange( cube.UVs );
			normals.AddRange( cube.Normals );
			colours.AddRange( cube.Colours );

			for( int index = 0; index < cube.Triangles.Length; index++ )
			{
				triangles.Add( cube.Triangles[ index ] + vertexOffset );
			}
		}

		mesh.vertices = verticies.ToArray();
		mesh.normals = normals.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.colors = colours.ToArray();
		mesh.SetTriangles( triangles.ToArray(), 0 );

		meshFilter.sharedMesh = mesh;

		dirty = false;
	}

	void Update()
	{
		if( dirty )
			GenerateMesh();
	}

	public Cube GetCubeAtChunkPosition( int worldX, int worldY, int worldZ )
	{
		return Cubes[ GetIndex( worldX, worldY, worldZ ) ];
	}
}
