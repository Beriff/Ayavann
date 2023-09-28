using Ayavann.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ayavann.World.Terrain
{
	class World
	{
		private readonly OctaveValueNoise WorldNoise;
		public int RenderDistance = 10;
		public Dictionary<Vector2, Region> LoadedRegions;

		public World(OctaveValueNoise noise)
		{
			WorldNoise = noise;
			LoadedRegions = new();
		}

		public Region RequestRegion(Vector2 pos)
		{
			if(LoadedRegions.ContainsKey(pos)) return LoadedRegions[pos];
			LoadedRegions[pos] = new Region(WorldNoise, pos);
			return LoadedRegions[pos];
		}

		public void RenderAt(GraphicsDevice gd, BasicEffect be, Vector2 position)
		{
			NumExtend.XY(RenderDistance / 2, RenderDistance / 2, -RenderDistance / 2, -RenderDistance / 2, (x,y) =>
			{
				var regionpos = position + new Vector2(x,y);
				RequestRegion(regionpos).Render(gd, be);
			});
		}
	}
}
