using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ayavann.Scene
{
	class Camera
	{
		public Vector3 Position;
		public Vector3 Target;

		public Matrix Projection;
		public Matrix View;
		public Matrix Model;

		public bool UpdateTarget;

		public Camera(GraphicsDevice gd, bool update_target = false)
		{
			Projection = Matrix.CreatePerspectiveFieldOfView(MathF.PI / 4, gd.Viewport.AspectRatio, 1f, 1000f);
			Model = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.UnitY);
			Position = new(0f, 0f, -100f);
			Target = Vector3.UnitZ;
			View = Matrix.CreateLookAt(Position, Target, Vector3.Up);
			UpdateTarget = update_target;
		}

		public void ApplyCameraTransform(BasicEffect be)
		{
			be.Projection = Projection;
			be.World = Model;
			be.View = GetViewMatrix();
		}

		public Matrix GetViewMatrix()
		{
			return Matrix.CreateLookAt(Position, UpdateTarget ? Target : Position - Vector3.Forward, Vector3.Up);
		}
	}
}
