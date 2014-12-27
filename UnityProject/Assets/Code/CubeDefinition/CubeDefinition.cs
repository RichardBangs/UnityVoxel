using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct TextureIndex
{
	public int IndexX;
	public int IndexY;

	public TextureIndex( int indexX, int indexY )
	{
		this.IndexX = indexX;
		this.IndexY = indexY;
	}
}

public class CubeDefinition
{
	const int numPackedTexturesInEachAxis = 16;

	public TextureIndex defaultTextureIndex;

	public Dictionary< int, TextureIndex > perFaceOverrideForTextureIndex = new Dictionary< int, TextureIndex >();
	
	public void GetTextureCoords( int face, out Vector2 bottomLeft, out Vector2 bottomRight, out Vector2 topLeft, out Vector2 topRight )
	{
		TextureIndex faceTextureIndex;
		if( !perFaceOverrideForTextureIndex.TryGetValue( face, out faceTextureIndex ) )
		{
			faceTextureIndex = defaultTextureIndex;
		}

		const float singleTextureSizeUV = 1.0f / numPackedTexturesInEachAxis;

		var baseUV = new Vector2( singleTextureSizeUV * faceTextureIndex.IndexX, singleTextureSizeUV * faceTextureIndex.IndexY );

		bottomLeft = baseUV;
		bottomRight = baseUV + new Vector2( singleTextureSizeUV, 0.0f );
		topLeft = baseUV + new Vector2( 0.0f, singleTextureSizeUV );
		topRight = baseUV + new Vector2( singleTextureSizeUV, singleTextureSizeUV );
	}
}
