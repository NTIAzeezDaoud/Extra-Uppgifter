﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Some_Knights_and_a_Dragon.Managers;
using Some_Knights_and_a_Dragon.Entities.Creatures;
using Some_Knights_and_a_Dragon.Levels;
using System;
using System.Collections.Generic;
using Some_Knights_and_a_Dragon.Managers.PlayerManagement;
using Some_Knights_and_a_Dragon.Entities;

namespace Some_Knights_and_a_Dragon.Windows
{
    public class GameplayWindow : MenuWindow
    {
        public Level CurrentLevel { get; private set; } // The Current level of the game
        public Player Player { get; private set; } // The player object
        public string PlayerName { get; private set; } // The name of the player
        private SaveData saveData; // Save data to load
        public HighScoreRecorder HighScoreRecorder { get; private set; } // Records the time and bosses defeated to later save in the high score list
        public XMLManager<Level> LevelLoader { get; private set; } // Manages the loading of XML files

        private Sprite ScoreMenu;
        public GameplayWindow() : base("Gameplay Window")
        {
            // Load the level
            LevelLoader = new XMLManager<Level>();
            ScoreMenu = new Sprite("Menus/scoreList");
            ScoreMenu.Scale = 10;
        }

        public override void LoadContent() // Loads the content of the level
        {
            base.LoadContent();
            // CODE IS FOR DEBUG WILL BE CHANGED TO BE MORE DYNAMIC AND DEPENDANT ON THE XML LOADERS

            // After the saved data is loaded the data from the save file is placed into their corresponding places (fields)

            // Player name
            PlayerName = saveData.DataSaveValues["PlayerName"];

            // Load the player and its creature from the file
            Player = new Player((Creature)Activator.CreateInstance(null,saveData.DataSaveValues["Character"]).Unwrap());

            // Load the level
            NewLevel(saveData.DataSaveValues["Level"]);

            // Load the health data
            Player.Creature.ChangeMaxHealth(int.Parse(saveData.DataSaveValues["MaxHealth"]));
            Player.Creature.ChangeCurrentHealth(int.Parse(saveData.DataSaveValues["CurrentHealth"]));

            // Load the inventory
            Player.Inventory.LoadInventoryFromData(saveData.InventoryItems);

            // Initiate the high score recorder and start recording
            HighScoreRecorder = new HighScoreRecorder(PlayerName);
            HighScoreRecorder.Start();
        }

        // Load a save from a file path in the saves file
        public void LoadFromSave(string path)
        {
            saveData = new SaveData("../../../Saves/" + path);
            LoadContent();
        }

        // Load a save from a save data object
        public void LoadFromSave(SaveData saveData)
        {
            this.saveData = saveData;
            LoadContent();
        }

        // Save the game
        public void SaveGame()
        {
            // Save the game
            saveData.Save();

            // Stop the recorder and save the high score
            HighScoreRecorder.Stop();
            HighScoreRecorder.SaveHighScore();
        }

        public override void Draw(ref SpriteBatch _spriteBatch)
        {
            // Draw the level and the player
            base.Draw(ref _spriteBatch);
            CurrentLevel.Draw(ref _spriteBatch);
            Player.Draw(ref _spriteBatch);

            if (Managers.Networking.GameplayNetworkHandler.InLocalGame && Game1.InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Tab))
            {
                ScoreMenu.Draw(ref _spriteBatch, new Vector2(640, 400), TextureDirection.Right);
                Game1.FontManager.WriteTitle(_spriteBatch, "Scores", new Vector2(640, 200), Color.Red);
                int index = 0;
                foreach (KeyValuePair<string, int> score in Managers.Networking.GameplayNetworkHandler.PlayerHighscore)
                    Game1.FontManager.WriteText(_spriteBatch, $"{score.Key} - {score.Value}", new Vector2(640, 250 + index++ * 50), Color.Red);
            }
        }

        public override void Update(ref GameTime gameTime)
        {
            // Update the player and the game. If the player is dead
            base.Update(ref gameTime);
            CurrentLevel.Update(ref gameTime);
            Player.Update(ref gameTime);
        }

        // Loads a new level and calls the Garbage collector to more 
        public void NewLevel(string levelName)
        {
            try // If there is error, catch it and send it to the ErrorWindow
            {
                CurrentLevel = null;
                GC.Collect();
                CurrentLevel = LevelLoader.Get("Levels/" + levelName);
                CurrentLevel.LoadContent();
                CurrentLevel.Creatures.Add(Player.Creature);
                Game1.WindowManager.GameState = GameState.Playing;
            }
            catch (Exception e)
            {
                Game1.WindowManager.DisplayError(e, "The XML file to load this level could not be found.");
            }
            
        }

        // Resets the current level, this is used when the player dies
        public void ResetLevel()
        {
            Player.Creature.SetHealthToMax();
            NewLevel(CurrentLevel.Name + ".xml");
        }
    }
}
