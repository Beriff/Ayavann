using Ayavann.Physics;
using Ayavann.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ayavann.World.Terrain
{
	/*
	 * How coordinates work:
	 * There are innate opengl space coordinates, which rose from assigning
	 * camera's near and far planes with numbers (0.1 and 500)
	 * 
	 * However there are actual world coordinates, which are simply
	 * innate / scale, since each chunk is scaled by the factor of Chunk.Scale
	 * 
	 * The coordinates are referred to as innate and actual coordinates (ic & ac)
	 */
	class World
	{
		private readonly OctaveValueNoise WorldNoise;
		public int RenderDistance = 10;
		public Dictionary<Vector2, Region> LoadedRegions;
		public Camera ActiveCamera;

		public World(OctaveValueNoise noise, Camera cam)
		{
			WorldNoise = noise;
			LoadedRegions = new();
			ActiveCamera = cam;
		}

		public Region RequestRegion(Vector2 pos)
		{
			if(LoadedRegions.ContainsKey(pos)) return LoadedRegions[pos];
			LoadedRegions[pos] = new Region(WorldNoise, pos);
			return LoadedRegions[pos];
		}

		public void Render(GraphicsDevice gd, BasicEffect be)
		{
			//Get FOV borders
			var forward = -ActiveCamera.GetForward().XZ();
			var position = ActiveCamera.Position.XZ() / Chunk.Scale;

			var border_1 = forward.Rotated(-ActiveCamera.FOV);
			var border_2 = forward.Rotated(ActiveCamera.FOV);

			var boxpos_f = forward * RenderDistance + position;
			var boxpos_b1 = border_1 * RenderDistance + position;
			var boxpos_b2 = border_2 * RenderDistance + position;

			int max_x = (int) Math.Max(boxpos_f.X, Math.Max(Math.Max(boxpos_b1.X, boxpos_b2.X), position.X));
			int max_y = (int) Math.Max(boxpos_f.Y, Math.Max(Math.Max(boxpos_b1.Y, boxpos_b2.Y), position.Y));
			int min_x = (int) Math.Min(boxpos_f.X, Math.Min(Math.Min(boxpos_b1.X, boxpos_b2.X), position.X));
			int min_y = (int) Math.Min(boxpos_f.Y, Math.Min(Math.Min(boxpos_b1.Y, boxpos_b2.Y), position.Y));

            for (int x = min_x; x <= max_x; x++)
			{
				for(int y = min_y; y <= max_y; y++)
				{
					Vector2 coordinate = new(x, y);
					if(coordinate.IsBetween(boxpos_b1, boxpos_b2))
					{
						RequestRegion(coordinate).Render(gd, be);
					}
				}
			}
		}
	}
}
