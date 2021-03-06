﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Some_Knights_and_a_Dragon.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Some_Knights_and_a_Dragon.Managers
{
    public class Sprite
    {
        public Texture2D SpriteTexture { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Scale = 5;

        public int ScaledWidth { get => Width * Scale; }
        public int ScaledHeight { get => Height * Scale; }
        public float Rotation { get; set; }

        Timer animationTimer;

        int currentFrame = 0;
        int currentRow = 0;
        int frames = 1;
        bool oneTimeAnimationOn = false; // If the animation is on state that is does an animation once such as attack
        bool freeze = false;

        public Rectangle BoundaryBox;

        // Load a sprite with no other animations
        public Sprite(string filepath)
        {
            SpriteTexture = Game1.TextureManager.GetTexture(filepath);

            Width = SpriteTexture.Width;
            Height = SpriteTexture.Height;

            animationTimer = new Timer(1000 / 60);
        }

        // Load spritesheet and work on it
        public Sprite(string filepath, Rectangle frame, int framesPerSecond = 12)
        {
            SpriteTexture = Game1.TextureManager.GetTexture(filepath);
            Width = frame.Width;
            Height = frame.Height;
            animationTimer = new Timer(1000 / framesPerSecond);
        }

        public Sprite(string filepath, int xFrames, int yFrames, int framesPerSecond = 12)
        {
            SpriteTexture = Game1.TextureManager.GetTexture(filepath);
            Width = SpriteTexture.Width / xFrames;
            Height = SpriteTexture.Height / yFrames;
            animationTimer = new Timer(1000 / framesPerSecond);
        }

        // New texture
        public void NewTexture(Texture2D texture, int width, int height)
        {
            SpriteTexture = texture;
            Width = width;
            Height = height;
        }

        // Animate certain row and how many frames in the row, forever
        public void Animate(int row, int frames)
        {
            if (!oneTimeAnimationOn)
            {
                currentRow = row;
                this.frames = frames;
            }
        }

        // Animate certain row and how many frames in the row, once
        public void OneTimeAnimation(int row, int frames)
        {
            if (!oneTimeAnimationOn)
            {
                oneTimeAnimationOn = true;
                currentFrame = 0;
                currentRow = row;
                this.frames = frames;
            }
        }

        // Freeze the sprite at a certain frame.
        public void Freeze(int row, int frame)
        {
            freeze = true;
            oneTimeAnimationOn = false;
            currentRow = row;
            currentFrame = frame;
        }

        // Freezes the sprite at the current frame
        public void Freeze()
        {
            freeze = true;
            oneTimeAnimationOn = false;
        }

        public void AnimateAndFreeze(int row, int frames)
        {
            if (!oneTimeAnimationOn)
            {
                oneTimeAnimationOn = true;
                currentFrame = 0;
                currentRow = row;
                this.frames = frames;
                freeze = true;
            }
        }

        // Unfreeze the sprite from the Freeze method
        public void Unfreeze()
        {
            oneTimeAnimationOn = false;
            freeze = false;
        }
        

        // Update the sprite
        public void Update(ref GameTime gameTime)
        {
            animationTimer.CheckTimer(gameTime);
            if (animationTimer.TimerOn && oneTimeAnimationOn && freeze)
            {
                currentFrame = (currentFrame == frames - 1) ? currentFrame : (currentFrame + 1);
            }
            else if (animationTimer.TimerOn && !oneTimeAnimationOn && !freeze)
            {
                currentFrame = (currentFrame + 1) % frames;
            }
            else if (animationTimer.TimerOn && oneTimeAnimationOn && !freeze)
            {
                if (currentFrame >= frames - 1)
                {
                    oneTimeAnimationOn = false;
                    currentRow = 0;
                    currentFrame = 0;
                    frames = 1;
                }
                else
                    currentFrame++;
            }
        }

        // Draws the sprite
        public void Draw(ref SpriteBatch spriteBatch, Vector2 position, TextureDirection textureDirection, Vector2? origin = null)
        {

            spriteBatch.Draw(
                SpriteTexture,
                new Rectangle((int)position.X, (int)position.Y, Width * Scale, Height * Scale),
                new Rectangle(currentFrame * Width, currentRow * Height, Width, Height),
                Color.White,
                Rotation,
                origin ?? new Vector2(Width / 2, Height / 2),
                textureDirection == TextureDirection.Left ? SpriteEffects.FlipHorizontally : textureDirection == TextureDirection.Down ? SpriteEffects.FlipVertically : SpriteEffects.None,
                0);
        }

        // Draws a frame of the sprite
        public void DrawFrame(ref SpriteBatch spriteBatch, Vector2 position, int row, int column, float widthFactor, float heightFactor, TextureDirection textureDirection = TextureDirection.Right, Vector2? origin = null)
        {
            spriteBatch.Draw(
                SpriteTexture,
                new Rectangle((int)position.X, (int)position.Y, (int)Math.Round(Width * Scale * widthFactor), (int)Math.Round(Height * Scale * heightFactor)),
                new Rectangle(column * Width, row * Height, (int)Math.Round(Width * widthFactor), (int)Math.Round(Height * heightFactor)),
                Color.White,
                Rotation,
                origin ?? new Vector2((float)Width / 2, (float)Height / 2),
                textureDirection == TextureDirection.Left ? SpriteEffects.FlipHorizontally : textureDirection == TextureDirection.Down ? SpriteEffects.FlipVertically : SpriteEffects.None,
                0);
        }

        public void DrawFrame(ref SpriteBatch spriteBatch, Vector2 position, int row, int column, TextureDirection textureDirection = TextureDirection.Right, Vector2? origin = null)
        {

            spriteBatch.Draw(
                SpriteTexture,
                new Rectangle((int)position.X, (int)position.Y, Width * Scale, Height * Scale),
                new Rectangle(column * Width, row * Height, Width, Height),
                Color.White,
                Rotation,
                origin ?? new Vector2(Width / 2, Height / 2),
                textureDirection == TextureDirection.Left ? SpriteEffects.FlipHorizontally : textureDirection == TextureDirection.Down ? SpriteEffects.FlipVertically : SpriteEffects.None,
                0);
        }

        // Draws the sprite on a given rectangle
        public void DrawOnArea(ref SpriteBatch spriteBatch, Rectangle rectangle, int row, int column, TextureDirection textureDirection = TextureDirection.Right)
        {
            spriteBatch.Draw(
                SpriteTexture,
                rectangle,
                new Rectangle(column * Width, row * Height, Width, Height),
                Color.White,
                Rotation,
                Vector2.Zero,
                textureDirection == TextureDirection.Left ? SpriteEffects.FlipHorizontally : textureDirection == TextureDirection.Down ? SpriteEffects.FlipVertically : SpriteEffects.None,
                0);
        }

        public Rectangle GetBoundaryBoxAt(Vector2 position)
        {
            return new Rectangle((int)position.X - Width * Scale / 2, (int)position.Y - Height * Scale / 2,
                                 Width * Scale, Height * Scale);
        }
    }
}
