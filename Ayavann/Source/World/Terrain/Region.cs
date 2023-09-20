using Ayavann.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ayavann.World.Terrain
{
	class Region
	{
		public const int RegionSize = 2;
		public Vector2 Position { get; set; }
		public Chunk[,] Chunks { get; set; }

		public Region(OctaveValueNoise noise, Vector2 position)
		{
			Position = position;
			Chunks = new Chunk[RegionSize, RegionSize];
			NumExtend.XY(RegionSize, RegionSize, (x, y) =>
			{
				Chunks[x, y] =
				Chunk.GetChunk(noise, new Vector2(position.X + x, position.Y + y));
			});
		}

		public void Render(GraphicsDevice gd, BasicEffect be)
		{

			NumExtend.XY(RegionSize, RegionSize, (x, y) =>
			{
				Chunks[x, y].RenderChunk(gd, be);
			});
		}
	}
}
