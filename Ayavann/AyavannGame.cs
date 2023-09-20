using Ayavann.Scene;
using Ayavann.World.Terrain;
using Ayavann.World.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Ayavann;
public class AyavannGame : Game
{
	private readonly GraphicsDeviceManager _graphics;
	private SpriteBatch _spriteBatch;
	private BasicEffect basicEffect;
	private VertexBuffer vbo;
	private Camera camera;
	private Region c;
	private Texture2D texture;
	private Entity Ship = new();

	public AyavannGame()
	{
		_graphics = new(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;
	}

	protected override void Initialize()
	{
		basicEffect = new(GraphicsDevice)
		{
			Alpha = 1f,
			VertexColorEnabled = true,
			LightingEnabled = false
		};

		VertexPositionColor[] triangle = new VertexPositionColor[3]
		{
			new(new(0, 20, 10), Color.Red),
			new(new(-20, -20, 10), Color.Green),
			new(new(20, -20, 10), Color.Blue)
		};
		vbo = new(GraphicsDevice, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
		vbo.SetData(triangle);
		camera = new(GraphicsDevice);

		c = new Region(OctaveValueNoise.AuxiliaryNoise(1), Vector2.Zero);
		Console.WriteLine($"{new Vector3(1, 0, 1) * 10f}");

		base.Initialize();
	}

	protected override void LoadContent()
	{
		_spriteBatch = new SpriteBatch(GraphicsDevice);
		texture = OctaveValueNoise.AuxiliaryNoise(0).GetTexture(GraphicsDevice);
		Ship.Model = Content.Load<Model>("ship2");
	}

	protected override void Update(GameTime gameTime)
	{
		if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
		if (Keyboard.GetState().IsKeyDown(Keys.W)) camera.Position -= Vector3.Forward;
		if (Keyboard.GetState().IsKeyDown(Keys.S)) camera.Position += Vector3.Forward;
		if (Keyboard.GetState().IsKeyDown(Keys.A)) camera.Position -= Vector3.Left;
		if (Keyboard.GetState().IsKeyDown(Keys.D)) camera.Position -= Vector3.Right;
		if (Keyboard.GetState().IsKeyDown(Keys.G))
		{
			RasterizerState rasterizerState = new();
			rasterizerState.FillMode = FillMode.WireFrame;
			rasterizerState.CullMode = CullMode.None;
			GraphicsDevice.RasterizerState = rasterizerState;
		}

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		camera.ApplyCameraTransform(basicEffect);
		//GraphicsDevice.SetVertexBuffer(vbo);
		GraphicsDevice.Clear(ClearOptions.DepthBuffer | ClearOptions.Target, Color.CornflowerBlue, 1f, 0);
		/*foreach(var pass in basicEffect.CurrentTechnique.Passes)
		{
			pass.Apply();
			GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 3);
		}*/
		c.Render(GraphicsDevice, basicEffect);

		_spriteBatch.Begin();
		_spriteBatch.Draw(texture, Vector2.Zero, Color.White);
		_spriteBatch.End();

		Ship.DrawModel(camera.Model, camera.GetViewMatrix(), camera.Projection);

		base.Draw(gameTime);
	}
}
