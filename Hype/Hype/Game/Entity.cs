using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Hype.Game
{
    abstract class Entity
    {
        public Vector2 Position = Vector2.Zero;
        public Vector2 Speed = Vector2.Zero;
        public Texture2D texture;
        public String ResourceName;

        public Entity() { }
        public Entity(Texture2D t)
        {
            this.texture = t;
        }
        public Entity(Texture2D t, Vector2 p, Vector2 s)
        {
            this.Position = p;
            this.Speed = s;
            this.texture = t;
        }

        public virtual void Update(GameTime gt, GraphicsDeviceManager graphics)
        {
            
        }
        public virtual void Draw(SpriteBatch sp)
        {
            sp.Draw(texture, Position, Color.White);
        }
    }
}
