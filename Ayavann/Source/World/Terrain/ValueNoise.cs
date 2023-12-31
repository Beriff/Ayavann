﻿using Ayavann.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ayavann.World.Terrain;

class ValueNoise
{
	public (int a, int b) Range;
	public int Frequency = 15;
	public Dictionary<Vector2, float> LatticeNodes;
	public Random NoiseSeed;
	public int Seed;
	public ValueNoise((int, int) range, int freq, int seed)
	{
		LatticeNodes = new();
		Frequency = freq;
		Range = range;
		NoiseSeed = new Random(seed);
	}

	public float GetValue(Vector2 pos)
	{
		if (LatticeNodes.ContainsKey(pos)) return LatticeNodes[pos];
		else
		{
			return LatticeNodes[pos] =
			new Random(NumExtend.Pairing(Seed, NumExtend.Pairing((int)pos.X, (int)pos.Y))).Next(Range.a, Range.b);
		}
	}

	public virtual float Noise(Vector2 point)
	{
		Func<float, float, float> mod = NumExtend.Mod;
		Vector2 topleft = new(MathF.Floor(point.X / Frequency) * Frequency, MathF.Floor(point.Y / Frequency) * Frequency);
		Vector2 topright = topleft + new Vector2(Frequency, 0);
		Vector2 bottomleft = topleft + new Vector2(0, Frequency);
		Vector2 bottomright = topleft + new Vector2(Frequency, Frequency);
		float tx = mod(point.X, Frequency) / Frequency;
		float ty = mod(point.Y, Frequency) / Frequency;
		return NumExtend.Bilerp(
			GetValue(topleft), GetValue(topright), GetValue(bottomleft), GetValue(bottomright), tx, ty
		);
	}
}

class OctaveValueNoise
{
	public List<ValueNoise> Octaves;
	public int NormalizationValue = 0;
	public OctaveValueNoise(int seed, params ((int, int) range, int freq)[] octaves)
	{
		Octaves = new();
		foreach (var (range, freq) in octaves)
		{
			NormalizationValue += range.Item2;
			Octaves.Add(new(range, freq, seed));
		}
	}
	public float Noise(Vector2 point)
	{
		float val = 0;
		foreach (var noise in Octaves) val += noise.Noise(point);
		return val / NormalizationValue;
	}
	public static OctaveValueNoise WorldNoise(int seed) => new(seed,
		((0, 255), 70), ((0, 128), 50), ((0, 64), 10), ((0, 32), 5), ((0, 16), 2)
	);
	public static OctaveValueNoise AuxiliaryNoise(int seed) => new(seed,
		((0, 128), 40), ((0, 64), 10), ((0, 32), 5), ((0, 16), 2)
	);
	public Texture2D GetTexture(GraphicsDevice gd, int size = 100)
	{
		Texture2D t = new Texture2D(gd, size, size);
		Color[] data = new Color[size * size];
		NumExtend.XY(size, size, (x, y) =>
		{
			float val = Noise(new(x, y));
			data[x * size + y] = new Color(val, val, val);
		});
		t.SetData(data);
		return t;
	}
}
