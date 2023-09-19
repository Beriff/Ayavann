﻿using Ayavann.Scene;
using Ayavann.World.Terrain;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Ayavann
{
	public class AyavannGame : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		BasicEffect basicEffect;
		VertexBuffer vbo;
		Camera camera;
		Chunk c;
		Texture2D texture;

		public AyavannGame()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
			basicEffect = new(GraphicsDevice);
			basicEffect.Alpha = 1f;
			basicEffect.VertexColorEnabled = true;
			basicEffect.LightingEnabled = false;

			VertexPositionColor[] triangle = new VertexPositionColor[3]
			{ 
				new(new(0, 20, 10), Color.Red),
				new(new(-20, -20, 10), Color.Green),
				new(new(20, -20, 10), Color.Blue)
			};
			vbo = new(GraphicsDevice, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
			vbo.SetData(triangle);
			camera = new(GraphicsDevice);

			c = Chunk.GetChunk(OctaveValueNoise.AuxiliaryNoise(0), new(0, 0));
			System.Console.WriteLine($"{new Vector3(1, 0, 1) * 10f}");

			base.Initialize();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			texture = OctaveValueNoise.AuxiliaryNoise(0).GetTexture(GraphicsDevice);
			// TODO: use this.Content to load your game content here
		}

		protected override void Update(GameTime gameTime)
		{
			
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			if(Keyboard.GetState().IsKeyDown(Keys.W))
			{
				camera.Position -= Vector3.Forward;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.S))
			{
				camera.Position += Vector3.Forward;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.A))
			{
				camera.Position -= Vector3.Left;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.D))
			{
				camera.Position -= Vector3.Right;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.G))
			{
				RasterizerState rasterizerState = new RasterizerState();
				rasterizerState.FillMode = FillMode.WireFrame;
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
			c.RenderChunk(GraphicsDevice, basicEffect);

			_spriteBatch.Begin();
			_spriteBatch.Draw(texture, Vector2.Zero, Color.White);
			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}