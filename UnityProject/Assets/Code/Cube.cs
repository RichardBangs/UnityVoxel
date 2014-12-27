using UnityEngine;
using System.Collections;


public class Cube
{
	public WorldPosition Position;
	public Quaternion Rotation;

	public Vector3 Scale = Vector3.one;
	
	public const int numFaces = 6;
	public const int numVerticesPerFace = 4;
	public const int numVerticiesPerTriangle = 3;
	public const int numTrianglesPerFace = 2;

	public int TriangleCount
	{
		get
		{
			const int numFaces = 6;
			const int numTrisPerFace = 2;

			return numFaces * numTrisPerFace;
		}
	}

	public void WriteData( Chunk chunk, int cubeVertexStart, int cubeTriangleStart )
	{
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

				int vertexIndex = ( face * numVerticesPerFace ) + vertex + cubeVertexStart;

				var rotation = Quaternion.AngleAxis( rotationAngle, rotationAxis );
				chunk.verticies[ vertexIndex ] = Position.ToVector3() - ( rotation * new Vector3( Scale.x * axis.x, Scale.y * axis.y, Scale.z * axis.z ) ) + ( rotation * Vector3.forward * 0.5f );

				chunk.normals[ vertexIndex ] = rotation * Vector3.forward;
			}

			int vertexOffset = ( face * numVerticesPerFace ) + cubeVertexStart;

			CubeDefinition cubeDefinition = CubeDefinitionManager.GetDefinition( "Grass" );

			cubeDefinition.GetTextureCoords( 	face,
											 	out chunk.uvs[ vertexOffset + 0 ],
			                                	out chunk.uvs[ vertexOffset + 1 ],
			                                	out chunk.uvs[ vertexOffset + 2 ],
			                                	out chunk.uvs[ vertexOffset + 3 ] );

			int triangleOffset = ( face * ( numTrianglesPerFace * numVerticiesPerTriangle ) ) + cubeTriangleStart;

			chunk.triangles[ triangleOffset + 0 ] = vertexOffset + 0;
			chunk.triangles[ triangleOffset + 1 ] = vertexOffset + 1;
			chunk.triangles[ triangleOffset + 2 ] = vertexOffset + 2;

			chunk.triangles[ triangleOffset + 3 ] = vertexOffset + 3;
			chunk.triangles[ triangleOffset + 4 ] = vertexOffset + 2;
			chunk.triangles[ triangleOffset + 5 ] = vertexOffset + 1;
		}
	}
}
