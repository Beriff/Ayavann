using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ayavann.Physics
{
	struct ColorGradient
	{
		public Color Start;
		public Color End;

		public ColorGradient(Color start, Color end)
		{
			Start = start;
			End = end;
		}

		public Color GetColor(float a)
		{
			Func<float, float, float, float> lerp = NumExtend.Lerp;
			return new Color(lerp(Start.R, End.R, a), lerp(Start.G, End.G, a), lerp(Start.B, End.B, a));
		}

	}
}
