using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ayavann.World.Entity;

class Entity
{
  public Model model;

  public void DrawModel(Matrix world, Matrix view, Matrix projection)
  {
    foreach (ModelMesh mesh in model.Meshes)
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
