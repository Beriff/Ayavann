using Ayavann.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ayavann.Scene
{
	class Camera
	{
		public Vector3 Position;
		public Vector3 Target;
		public Matrix Projection;
		public Matrix View;
		public Matrix Model;
		public readonly float FOV = MathF.PI / 4f;

		public bool UpdateTarget;
		public Vector3 GetForward()
		{
			var forward = (Position - Target).Normalized();
			return forward == Vector3.Zero ? forward : Vector3.Forward;
		}

		public Camera(GraphicsDevice gd, bool update_target = false)
		{
			Projection = Matrix.CreatePerspectiveFieldOfView(FOV, gd.Viewport.AspectRatio, .1f, 500f);
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
