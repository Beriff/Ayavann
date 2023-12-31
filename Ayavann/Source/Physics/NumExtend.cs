﻿using Microsoft.Xna.Framework;

namespace Ayavann.Physics;
static class NumExtend
{
	public const float Sqrt2 = 1.41421356237f;
	public static Vector2 Normalized(this Vector2 a)
	{
		var b = a;
		b.Normalize();
		return b;
	}
	public static Vector3 Normalized(this Vector3 a)
	{
		var b = a;
		b.Normalize();
		return b;
	}
	public static Vector2 XY(this Vector3 a) => new(a.X, a.Y);
	public static Vector2 XZ(this Vector3 a) => new(a.X, a.Z);
	public static Vector3 Vec3(this Vector2 a) => new(a.X, a.Y, 0);
	public static Vector2 Apply(this Vector2 a, Func<float, float> f) => new(f(a.X), f(a.Y));
	public static Vector2 Apply(this Vector2 a, Func<float, int> f) => new(f(a.X), f(a.Y));
	public static Vector3 Apply(this Vector3 a, Func<float, float> f) => new(f(a.X), f(a.Y), f(a.Z));
	public static Vector3 Apply(this Vector3 a, Func<float, int> f) => new(f(a.X), f(a.Y), f(a.Z));
	public static Vector2 Sign(this Vector2 a) => a.Apply(MathF.Sign);
	public static float Cross(this Vector2 a, Vector2 b) => a.X * b.Y - a.Y * b.X;
	public static bool IsBetween(this Vector2 target, Vector2 a, Vector2 b)
	{
		return Math.Sign(target.Cross(a)) == Math.Sign(b.Cross(target));
	}
	public static Vector2 Rotated(this Vector2 a, double theta)
	{
		return new((float)(a.X * Math.Cos(theta) - a.Y * Math.Sin(theta)), (float)(a.X * Math.Sin(theta) + a.Y * Math.Cos(theta)));
	}
	public static Vector2 PointTo(this Vector2 a, Vector2 b) => b - a;
	public static Vector3 PointTo(this Vector3 a, Vector3 b) => b - a;
	public static float Distance(this Vector2 a, Vector2 b) => a.PointTo(b).Length();
	public static float Bilerp(float c00, float c10, float c01, float c11, float tx, float ty)
	{
		float r1 = Lerp(c00, c10, tx);
		float r2 = Lerp(c01, c11, tx);
		return Lerp(r1, r2, ty);
	}
	public static float Mod(float x, float m) => (x % m + m) % m;
	public static float Lerp(float v0, float v1, float t) => v0 + t * (v1 - v0);
	public static Vector2 Lerp(Vector2 v0, Vector2 v1, float t) =>
		new(Lerp(v0.X, v1.X, t), Lerp(v0.Y, v1.Y, t));
	public static float FloatRange(this Random r, float min, float max) =>
		Lerp(min, max, (float)r.NextDouble());
	public static int Flatten(int x, int y, int w) => w * y + x;
	public static void XY(int endx, int endy, int startx, int starty, Action<int, int> act)
	{
		for (int x = startx; x < endx; x++)
		{
			for (int y = starty; y < endy; y++)
				act(x, y);
		}
	}
	public static void XY(int endx, int endy, Action<int, int> act) => XY(endx, endy, 0, 0, act);

	public static void XYZ(int endx, int endy, int endz, int startx, int starty, int startz, Action<int, int, int> act)
	{
		for (int x = startx; x < endx; x++)
		{
			for (int y = starty; y < endy; y++)
			{
				for (int z = startz; z < endz; z++)
				{
					act(x, y, z);
				}

			}
		}
	}
	public static void XYZ(int endx, int endy, int endz, Action<int, int, int> act) => XYZ(endx, endy, endz, 0, 0, 0, act);
	public static int Pairing(int a, int b) => (int)(.5f * (a + b) * (a + b + 1) + b);
}
