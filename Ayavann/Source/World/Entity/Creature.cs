using Ayavann.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ayavann.World.Entity;

class Creature : Entity
{
  public float Health = 100;
  private BasicEffect spriteEffect;

  public Creature(BasicEffect effect, Vector3? position = null) : base(position)
  {
    spriteEffect = effect;
  }
  public void DrawHealthBar(SpriteBatch _spriteBatch, Texture2D texture, Camera camera)
  {
    spriteEffect.World = Matrix.CreateScale(1, -1, 1) *
      Matrix.CreateTranslation(this.Position + new Vector3(0f, 1.5f, 0f));
    spriteEffect.View = camera.GetViewMatrix();
    spriteEffect.Projection = camera.Projection;
    _spriteBatch.Begin(0, null, null, DepthStencilState.DepthRead, RasterizerState.CullNone, spriteEffect);
    _spriteBatch.Draw(
      texture, Vector2.Zero, null, Color.White, 0,
      new Vector2(texture.Width / 2, texture.Height / 2), 0.005f, 0, 0);
    _spriteBatch.End();
  }
}
