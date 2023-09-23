using Ayavann.Scene;
using Ayavann.World.Terrain;
using Ayavann.World.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ayavann.Physics;

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
	private Texture2D healthTexture;
	private BasicEffect slimeEffect;
	private Creature Slime;
	private readonly List<Creature> creatures = new();

	private World.Terrain.World world = new(OctaveValueNoise.AuxiliaryNoise(1));

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

		slimeEffect = new BasicEffect(GraphicsDevice)
		{
			TextureEnabled = true,
			VertexColorEnabled = true,
		};
		Slime = new(slimeEffect);
		creatures.Add(Slime);
		base.Initialize();
	}

	protected override void LoadContent()
	{
		_spriteBatch = new SpriteBatch(GraphicsDevice);
		texture = OctaveValueNoise.AuxiliaryNoise(0).GetTexture(GraphicsDevice);
		Slime.Model = Content.Load<Model>("slime");
		healthTexture = Content.Load<Texture2D>("HealthBar");
	}

	protected override void Update(GameTime gameTime)
	{
		if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
		if (Keyboard.GetState().IsKeyDown(Keys.W)) camera.Position -= Vector3.Forward;
		if (Keyboard.GetState().IsKeyDown(Keys.S)) camera.Position += Vector3.Forward;
		if (Keyboard.GetState().IsKeyDown(Keys.A)) camera.Position -= Vector3.Left;
		if (Keyboard.GetState().IsKeyDown(Keys.D)) camera.Position -= Vector3.Right;
		if (Keyboard.GetState().IsKeyDown(Keys.Space)) camera.Position += Vector3.Up;
		if (Keyboard.GetState().IsKeyDown(Keys.LeftShift)) camera.Position += Vector3.Down;
		if (Keyboard.GetState().IsKeyDown(Keys.G))
		{
			RasterizerState rasterizerState = new();
			rasterizerState.FillMode = FillMode.WireFrame;
			rasterizerState.CullMode = CullMode.None;
			GraphicsDevice.RasterizerState = rasterizerState;
		}

		Slime.Position += new Vector3(new Random().NextSingle() * 0.01f, 0, new Random().NextSingle() * 0.01f);
		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		
		camera.ApplyCameraTransform(basicEffect);
		GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1f, 0);
		
		world.RenderAt(GraphicsDevice, basicEffect, camera.Position.XZ()/10);
        Console.WriteLine(camera.GetForward());


		foreach (Creature creature in creatures) creature.DrawHealthBar(_spriteBatch, healthTexture, camera);
		Slime.DrawModel(camera.GetViewMatrix(), camera.Projection);

		_spriteBatch.Begin(depthStencilState: DepthStencilState.Default);
		_spriteBatch.Draw(texture, Vector2.Zero, Color.White);
		_spriteBatch.End();
		base.Draw(gameTime);
	}
}
