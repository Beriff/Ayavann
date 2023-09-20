using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ayavann.World.Entity;

class Entity
{
	public Model Model;

	private Vector3 _Position;
	public Vector3 Position {
		get => _Position;
		set 
		{
			_Position = value;
			LocalWorldMatrix = Matrix.CreateWorld(value, Vector3.Forward, Vector3.Up); 
		} 
	}

	private Matrix LocalWorldMatrix;

	public void DrawModel(Matrix world, Matrix view, Matrix projection)
	{
		foreach (ModelMesh mesh in Model.Meshes)
		{
			foreach (BasicEffect effect in mesh.Effects)
			{
				effect.World = LocalWorldMatrix;
				effect.View = view;
				effect.Projection = projection;
			}

			mesh.Draw();
		}
	}

	public Entity(Vector3? position = null)
	{
		Position = position ?? Vector3.Zero;
	}
}