using UnityEngine;
using System.Collections;


public class Cube
{
	public WorldPosition Position;
	public Quaternion Rotation;

	public Vector3 Scale = Vector3.one;

	public Vector3[] Verticies { get; private set; }
	public Vector3[] Normals { get; private set; }
	public Vector2[] UVs { get; private set; }
	public int[] Triangles { get; private set; }
	public Color[] Colours { get; private set; }

	public int TriangleCount
	{
		get
		{
			const int numFaces = 6;
			const int numTrisPerFace = 2;

			return numFaces * numTrisPerFace;
		}
	}

	void UpdateVerticies()
	{
		const int numFaces = 6;
		const int numVerticesPerFace = 4;
		const int numVerticiesPerTriangle = 3;
		const int numTrianglesPerFace = 2;

		int vertexCount = numFaces * numVerticesPerFace;
		int indexCount = numFaces * numTrianglesPerFace * numVerticiesPerTriangle;

		Verticies = new Vector3[ vertexCount ];
		Normals = new Vector3[ vertexCount ];
		UVs = new Vector2[ vertexCount ];
		Triangles = new int[ indexCount ];
		Colours = new Color[ vertexCount ];

		for( int face = 0; face < numFaces; face++ )
		{
			for( int vertex = 0; vertex < numVerticesPerFace; vertex++ )
			{
				var axis = new Vector3( ( ( vertex / 1 ) % 2 ) == 0 ? -0.5f : 0.5f, ( ( vertex / 2 ) % 2 ) == 0 ? -0.5f : 0.5f, 0.0f );

				Vector3 rotationAxis = Vector3.up;
				float rotationAngle = face * 90.0f;
				if( face >= 4 )
				{
					rotationAxis = Vector3.right;
					rotationAngle = ( face * 180.0f ) + 90.0f;
				}

				int vertexIndex = ( face * numVerticesPerFace ) + vertex;

				var rotation = Quaternion.AngleAxis( rotationAngle, rotationAxis );
				Verticies[ vertexIndex ] = Position.ToVector3() - ( rotation * new Vector3( Scale.x * axis.x, Scale.y * axis.y, Scale.z * axis.z ) ) + ( rotation * Vector3.forward * 0.5f );

				Normals[ vertexIndex ] = rotation * Vector3.forward;
			}

			int vertexOffset = face * numVerticesPerFace;

			CubeDefinition cubeDefinition = CubeDefinitionManager.GetDefinition( "Grass" );

			cubeDefinition.GetTextureCoords( face,
											 out UVs[ vertexOffset + 0 ],
			                                 out UVs[ vertexOffset + 1 ],
			                                 out UVs[ vertexOffset + 2 ],
			                                 out UVs[ vertexOffset + 3 ] );

			int triangleOffset = face * ( numTrianglesPerFace * numVerticiesPerTriangle );

			Triangles[ triangleOffset + 0 ] = vertexOffset + 0;
			Triangles[ triangleOffset + 1 ] = vertexOffset + 1;
			Triangles[ triangleOffset + 2 ] = vertexOffset + 2;

			Triangles[ triangleOffset + 3 ] = vertexOffset + 3;
			Triangles[ triangleOffset + 4 ] = vertexOffset + 2;
			Triangles[ triangleOffset + 5 ] = vertexOffset + 1;
		}
	}
	
	public void GenerateData()
	{
		UpdateVerticies();
	}
}
