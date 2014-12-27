using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
	public const int numCubes = size * size * size;
	
	public Cube[] Cubes = new Cube[ numCubes ];

	public WorldPosition WorldPosition;

	MeshFilter meshFilter;
	MeshRenderer meshRenderer;

	public const int numVertices = numCubes * Cube.numFaces * Cube.numVerticesPerFace;
	public const int numTrianglePoints = numCubes * Cube.numFaces * ( Cube.numTrianglesPerFace * Cube.numVerticiesPerTriangle );

	public Vector3[] verticies = new Vector3[ numVertices ];
	public Vector3[] normals = new Vector3[ numVertices ];
	public Vector2[] uvs = new Vector2[ numVertices ];
	public int[] triangles = new int[ numTrianglePoints ];
	public Color[] colours = new Color[ numVertices ];

	bool dirty = false;

	public void ConstructDummyChunk()
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
					if( Random.value > 0.5f )
						continue;

					var testCube = new Cube();
					testCube.Position = new WorldPosition( x, y, z );

					Cubes[ GetIndex( x, y, z ) ] = testCube;
				}
			}
		}

		UpdateMeshData();
	}

	int GetIndex( int x, int y, int z )
	{
		return x + ( y * size ) + ( z * size * size );
	}

	void WriteToMeshFilter()
	{
		var mesh = new Mesh ();
		mesh.name = "CubeRenderer Mesh";

		mesh.vertices = verticies;
		mesh.normals = normals;
		mesh.uv = uvs;
		mesh.colors = colours;
		mesh.SetTriangles( triangles, 0 );
		
		meshFilter.sharedMesh = mesh;
		
		dirty = false;
	}

	void UpdateMeshData()
	{
		for( int cubeIndex = 0; cubeIndex < Cubes.Length; cubeIndex++ )
		{
			Cube myCube = Cubes[ cubeIndex ];

			if( myCube == null )
				continue;

			int cubeVertexStart;
			int cubeTriangleStart;

			GetCubeOffsets( cubeIndex, out cubeVertexStart, out cubeTriangleStart );

			myCube.WriteData( this, cubeVertexStart, cubeTriangleStart );
		}

		dirty = true;
	}

	void GetCubeOffsets( int cubeIndex, out int cubeVertexStart, out int cubeTriangleStart )
	{
		cubeVertexStart = cubeIndex * ( Cube.numFaces * Cube.numVerticesPerFace );
		cubeTriangleStart = cubeIndex * ( Cube.numFaces * ( Cube.numTrianglesPerFace * Cube.numVerticiesPerTriangle ) );
	}

	public Cube GetCubeFromVertexIndex( int vertexIndex )
	{
		return Cubes[ vertexIndex / ( Cube.numFaces * Cube.numVerticesPerFace ) ];
	}

	void Update()
	{
		if( dirty )
			WriteToMeshFilter();
	}

	public Cube GetCubeAtChunkPosition( int worldX, int worldY, int worldZ )
	{
		return Cubes[ GetIndex( worldX, worldY, worldZ ) ];
	}
}
