using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ayavann.World.Terrain
{
	class Chunk
	{
		public const float Scale = 10f;
		public const float TerrainScale = 10f;

		public float[] VertexValue;
		public Vector3 Position;

		private VertexPositionColor[] Vertices;

		/// <summary>
		/// Note that chunk (a: x, b: y) coordinates are world (a: x, y, b: z) coordinates
		/// </summary>
		public static Chunk GetChunk(OctaveValueNoise noise, Vector2 vector_coords)
		{
			var chunk = new Chunk() { VertexValue = new float[4] };
			chunk.VertexValue[0] = TerrainScale * MathF.Pow(noise.Noise(vector_coords), -2);
			chunk.VertexValue[1] = TerrainScale * MathF.Pow(noise.Noise(vector_coords + Vector2.UnitX), -2);
			chunk.VertexValue[2] = TerrainScale * MathF.Pow(noise.Noise(vector_coords + Vector2.UnitY), -2);
			chunk.VertexValue[3] = TerrainScale * MathF.Pow(noise.Noise(vector_coords + new Vector2(1, 1)), -2);
			chunk.Position = new(vector_coords.X * Scale, -25f, vector_coords.Y * Scale);

			chunk.GenVertices();
			return chunk;
		}

		public Chunk() { }
		public Chunk(float[] vertices, Vector3 pos)
		{
			VertexValue = vertices;
			Position = pos * new Vector3(Scale, 1, Scale);
		}

		private void GenVertices()
		{
			Vertices = new VertexPositionColor[6];
			var vertices = new VertexPositionColor[4];
			vertices[0] = new(Position + new Vector3(0, VertexValue[0], 0), Color.Red);
			vertices[1] = new(Position + new Vector3(Scale, VertexValue[1], 0), Color.Green);
			vertices[2] = new(Position + new Vector3(0, VertexValue[2], Scale), Color.Blue);
			vertices[3] = new(Position + new Vector3(Scale, VertexValue[3], Scale), Color.White);
			Vertices = new VertexPositionColor[] { vertices[0], vertices[1], vertices[2], vertices[2], vertices[1], vertices[3] };
		}
		private int[] GenIndices() => new int[6] { 1, 2, 0, 2, 3, 1 };
		public void RenderChunk(GraphicsDevice gd, BasicEffect be)
		{
			// Create a vbo of chunk surface
			VertexBuffer vbo = new(gd, typeof(VertexPositionColor), 6, BufferUsage.WriteOnly);
			vbo.SetData(Vertices, 0, 6);

			gd.SetVertexBuffer(vbo);
			foreach (var pass in be.CurrentTechnique.Passes)
			{
				pass.Apply();
				gd.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 3);
				gd.DrawPrimitives(PrimitiveType.TriangleStrip, 3, 6);
				//gd.DrawUserIndexedPrimitives(PrimitiveType.TriangleStrip, Vertices, 0, 6, GenIndices(), 0, 2);
			}
		}
	}
}
