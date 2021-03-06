﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Some_Knights_and_a_Dragon.Windows.Menus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Some_Knights_and_a_Dragon.Windows
{
    public class DeathWindow : MenuWindow
    {
        public DeathWindow() : base("Death Window")
        {
            // Add the menu items
            MenuItems.Add("Respawn", new Button(new Vector2(640, 500), "Respawn", RespawnPlayer));
            MenuItems.Add("Main Menu", new Button(new Vector2(640, 700), "Main Menu", BackToMain));
        }

        public override void Draw(ref SpriteBatch _spriteBatch)
        {
            // Draw back ground and display death text
            _spriteBatch.Draw(Game1.TextureManager.BlankTexture, new Rectangle(0, 0, 1280, 960), Color.Crimson * 0.7f); // Draws a faint foreground on the screen
            base.Draw(ref _spriteBatch);
            Game1.FontManager.WriteTitle(_spriteBatch, "YOU ARE DEAD", new Vector2(640, 400), Color.Black);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            Loaded = true;
        }

        public override void Update(ref GameTime gameTime)
        {
            base.Update(ref gameTime);
        }

        public void RespawnPlayer() // Resets the level and changes the gamestate to playing
        {
            Game1.WindowManager.GetGameplayWindow().ResetLevel();
            Game1.WindowManager.GameState = Managers.GameState.Playing;
        }

        public void BackToMain()
        {
            // Return to main menu and play, unload gameplay data and play the intro music
            Game1.WindowManager.GameState = Managers.GameState.MainMenu;
            Game1.WindowManager.UnloadGameplay();
            Game1.SongManager.Play("intro");
        }
    }
}
