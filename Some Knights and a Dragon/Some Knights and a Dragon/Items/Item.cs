﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Some_Knights_and_a_Dragon.Managers;
using Microsoft.Xna.Framework.Input;
using Some_Knights_and_a_Dragon.Entities.Creatures;

namespace Some_Knights_and_a_Dragon.Items
{
    public abstract class Item
    {
        public Sprite Sprite { get; protected set; } // The Sprite Class of the item
        public string Name { get; protected set; } // Name of the item
        public string Description { get; protected set; } // A small description
        public Vector2 Handle { get; protected set; } // Where the item is held by a creature

        public Item()
        {
        }

        protected virtual void LoadSprite(string filePath, int width, int height) // Loads the sprite of the item if it has animation
        {
            Sprite = new Sprite(filePath, width, height, 12);
        }

        protected virtual void LoadSprite(string filePath) // Loads the sprite of the item if it has no animation
        {
            Sprite = new Sprite(filePath);
        }

        public virtual void Update(GameTime gameTime)
        {
            Sprite.Update(ref gameTime);
        }
        public virtual void OnUse(GameTime gameTime) // When used do this
        {

        }

        public virtual void AfterUse() // After used do this
        {

        }

        // Draws the item on a creature
        public void DrawOn(ref SpriteBatch spriteBatch, Creature creature, Entities.TextureDirection textureDirection) // Draws the item on a creature
        {
            Sprite.Draw(ref spriteBatch,
                creature.HandPosition,
                textureDirection,
                textureDirection == Entities.TextureDirection.Left ? new Vector2(Sprite.Width - Handle.X, Handle.Y) : Handle);
        }

        public virtual void UseAnimation(GameTime gameTime) // Animation if the item is used.
        {

        }
    }
}
