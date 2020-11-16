﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Some_Knights_and_a_Dragon.Entities;
using Some_Knights_and_a_Dragon.Entities.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Some_Knights_and_a_Dragon.Managers
{
    public static class HealthBar
    {

        private static Texture2D barBackground;
        private static Texture2D bar;
        public static void Setup(ref SpriteBatch spriteBatch)
        {
            barBackground = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            barBackground.SetData(new Color[] { Color.White });
            bar = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            bar.SetData(new Color[] { Color.White });
        }
        public static void BossHealthBar(Creature creature, ref SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(barBackground, new Rectangle(150, 100, 980, 50), Color.Red);
            spriteBatch.Draw(bar, new Rectangle(150, 100, 980 * creature.CurrentHealth / creature.MaxHealth, 50), Color.Lime);
        }

        public static void FloatingBar(Creature creature, ref SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(barBackground, new Rectangle((int)creature.Position.X - 25, (int)creature.Position.Y - creature.Sprite.Height * creature.Sprite.Scale / 2, 50, 10), Color.Red);
            spriteBatch.Draw(bar, new Rectangle((int)creature.Position.X - 25, (int)creature.Position.Y - creature.Sprite.Height * creature.Sprite.Scale / 2,
                                                50 * creature.CurrentHealth / creature.MaxHealth, 10), Color.Lime);
        }
    }
}
