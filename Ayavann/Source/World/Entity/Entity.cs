using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ayavann.World.Entity;

class Entity
{
  public Model Model;
  public Vector3 Position = new(0,0,0);

  public Matrix Update(Matrix World) {
    Position += new Vector3(0, 1f, 0);
    return Matrix.CreateTranslation(Position);
  }

  public void DrawModel(Matrix world, Matrix view, Matrix projection)
  {
    foreach (ModelMesh mesh in Model.Meshes)
    {
      foreach (BasicEffect effect in mesh.Effects)
      {
        effect.World = world;
        effect.View = view;
        effect.Projection = projection;
      }

      mesh.Draw();
    }
  }
}
