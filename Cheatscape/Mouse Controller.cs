using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cheatscape
{
    static class Mouse_Controller
    {
        public static MouseState CurrentMS, PreviousMS;
        static Vector2 MousePosition = new Vector2();
        static Rectangle[,] BoardTiles = new Rectangle[8, 8];
        static Rectangle[,] LevelButtons = new Rectangle[5, 2];
        static Rectangle OptionsButton = new Rectangle(50, 200, 32, 32);
        static Rectangle[] ScrollButtons = { new Rectangle(0, 138, 20, 20), new Rectangle(0, 340, 20, 20) };
        static Texture2D TileSelect;
        static Vector2 SelectedTile;

        public static void Load()
        {
            TileSelect = Global_Info.AccessContentManager.Load<Texture2D>("Tile Select");

            Vector2 tempBoardPos = Game_Board.AccessBoardPosition;
            int tempTileSize = Game_Board.AccessTileSize;

            for (int i = 0; i < BoardTiles.GetLength(0); i++)
            {
                for (int j = 0; j < BoardTiles.GetLength(1); j++)
                {
                    BoardTiles[i, j] = new Rectangle((int)tempBoardPos.X + (i * tempTileSize), 
                        (int)tempBoardPos.Y + (j * tempTileSize), tempTileSize, tempTileSize);
                }
            }

            for (int i = 0; i < LevelButtons.GetLength(0); i++)
            {
                for (int j = 0; j < LevelButtons.GetLength(1); j++)
                {
                    LevelButtons[i, j] = new Rectangle(50 + i * 100, 50 + j * 75, 96, 64);
                }
            }
        }

        public static void Update()
        {
            PreviousMS = CurrentMS;
            CurrentMS = Mouse.GetState();
            MousePosition = new Vector2(CurrentMS.X, CurrentMS.Y) / Global_Info.AccessScreenScale;

            switch (Global_Info.AccessCurrentGameState)
            {
                case Global_Info.GameState.LevelSelect:
                    LevelSelectUpdate();
                    break;
                case Global_Info.GameState.PlayingLevel:
                    LevelUpdate();
                    break;
                case Global_Info.GameState.MainMenu:
                    if (CurrentMS.LeftButton == ButtonState.Pressed && PreviousMS.LeftButton == ButtonState.Released)
                        Main_Menu.animating = true;
                    break;
                case Global_Info.GameState.Options:
                    break;
            }
        }

        static void LevelSelectUpdate()
        {
            for (int i = 0; i < LevelButtons.GetLength(0); i++)
            {
                for (int j = 0; j < LevelButtons.GetLength(1); j++)
                {
                    if (LevelButtons[i, j].Contains(MousePosition))
                    {
                        Level_Select_Menu.SelectedBundleX = i;
                        Level_Select_Menu.SelectedBundleY = j;
                        Level_Select_Menu.optionHighlight = false;
                    }
                }
            }

            if (OptionsButton.Contains(MousePosition))
            {
                Level_Select_Menu.optionHighlight = true;
            }

            if (CurrentMS.LeftButton == ButtonState.Pressed && PreviousMS.LeftButton == ButtonState.Released)
            {
                if (Level_Select_Menu.optionHighlight)
                    Transition.StartTransition(Transition.TransitionState.ToOptions);
                else
                    Transition.StartTransition(Transition.TransitionState.ToLevel);
            }
        }

        static void LevelUpdate()
        {
            SelectedTile.X = 100;

            for (int i = 0; i < BoardTiles.GetLength(0); i++)
            {
                for (int j = 0; j < BoardTiles.GetLength(1); j++)
                {
                    if (BoardTiles[i, j].Contains(MousePosition))
                        SelectedTile = new Vector2(i, j);
                }
            }

            if (CurrentMS.LeftButton == ButtonState.Pressed && PreviousMS.LeftButton == ButtonState.Released)
            {
                if (Level_Manager.FindingCheat && (ScrollButtons[0].Contains(MousePosition) || ScrollButtons[1].Contains(MousePosition)))
                {
                    if (ScrollButtons[0].Contains(MousePosition))
                        Rules_List.MoveThroughRules(1);
                    else
                        Rules_List.AccessCurrentRule++;
                }
                else if (SelectedTile.X != 100)
                {
                    if (!Level_Manager.FindingCheat)
                        Level_Manager.FindingCheat = true;
                }
                else
                    Level_Manager.FindingCheat = false;
            }

            if (Level_Manager.FindingCheat)
            {
                if (CurrentMS.ScrollWheelValue > PreviousMS.ScrollWheelValue)
                {
                    Rules_List.MoveThroughRules(1);
                }
                if (CurrentMS.ScrollWheelValue < PreviousMS.ScrollWheelValue)
                {
                    Rules_List.AccessCurrentRule++;
                }
            }
        }

        public static void LevelDraw(SpriteBatch aSpriteBatch)
        {
            if (SelectedTile.X != 100)
            {
                Vector2 tempPosition = new Vector2((int)(Game_Board.AccessBoardPosition.X + (SelectedTile.X * Game_Board.AccessTileSize)),
                        (int)(Game_Board.AccessBoardPosition.Y + (SelectedTile.Y * Game_Board.AccessTileSize)));
                aSpriteBatch.Draw(TileSelect, tempPosition, Color.White);
            }
        }
    }
}
