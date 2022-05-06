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
        }

        public static void Update()
        {
            PreviousMS = CurrentMS;
            CurrentMS = Mouse.GetState();
            MousePosition = new Vector2(CurrentMS.X, CurrentMS.Y) / Global_Info.AccessScreenScale;
        }

        public static void LevelUpdate()
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
                if (SelectedTile.X != 100)
                {
                    if (!Level_Manager.FindingCheat)
                        Level_Manager.FindingCheat = true;
                }
                else
                    Level_Manager.FindingCheat = false;
            }

            if (CurrentMS.ScrollWheelValue > PreviousMS.ScrollWheelValue)
            {
                Rules_List.MoveThroughRules(1);
            }
            if (CurrentMS.ScrollWheelValue < PreviousMS.ScrollWheelValue)
            {
                Rules_List.AccessCurrentRule++;
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
