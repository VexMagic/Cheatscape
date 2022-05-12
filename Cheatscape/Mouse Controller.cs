using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cheatscape
{
    static class Mouse_Controller
    {
        public static MouseState currentMS, previousMS;
        static Vector2 mousePosition = new Vector2();
        static Rectangle[,] boardTiles = new Rectangle[8, 8];
        static Rectangle[,] levelButtons = new Rectangle[5, 2];
        static Rectangle optionsButton = new Rectangle(50, 200, 32, 32);
        static Rectangle[] scrollButtons = { new Rectangle(0, 138, 20, 20), new Rectangle(0, 340, 20, 20) };
        static Texture2D tileSelect;
        static Vector2 selectedTile;

        public static void Load()
        {
            tileSelect = Global_Info.AccessContentManager.Load<Texture2D>("Tile Select");

            Vector2 tempBoardPos = Game_Board.AccessBoardPosition;
            int tempTileSize = Game_Board.AccessTileSize;

            for (int i = 0; i < boardTiles.GetLength(0); i++)
            {
                for (int j = 0; j < boardTiles.GetLength(1); j++)
                {
                    boardTiles[i, j] = new Rectangle((int)tempBoardPos.X + (i * tempTileSize),
                        (int)tempBoardPos.Y + (j * tempTileSize), tempTileSize, tempTileSize);
                }
            }

            for (int i = 0; i < levelButtons.GetLength(0); i++)
            {
                for (int j = 0; j < levelButtons.GetLength(1); j++)
                {
                    levelButtons[i, j] = new Rectangle(50 + i * 100, 50 + j * 75, 96, 64);
                }
            }
        }

        public static void Update()
        {
            previousMS = currentMS;
            currentMS = Mouse.GetState();
            mousePosition = new Vector2(currentMS.X, currentMS.Y) / Global_Info.AccessScreenScale;

            switch (Global_Info.AccessCurrentGameState)
            {
                case Global_Info.GameState.levelSelect:
                    LevelSelectUpdate();
                    break;
                case Global_Info.GameState.playingLevel:
                    LevelUpdate();
                    break;
                case Global_Info.GameState.mainMenu:
                    if (currentMS.LeftButton == ButtonState.Pressed && previousMS.LeftButton == ButtonState.Released)
                        Main_Menu.animating = true;
                    break;
                case Global_Info.GameState.options:
                    break;
            }
        }

        static void LevelSelectUpdate()
        {
            for (int i = 0; i < levelButtons.GetLength(0); i++)
            {
                for (int j = 0; j < levelButtons.GetLength(1); j++)
                {
                    if (levelButtons[i, j].Contains(mousePosition))
                    {
                        Level_Select_Menu.selectedBundleX = i;
                        Level_Select_Menu.selectedBundleY = j;
                        Level_Select_Menu.optionHighlight = false;
                    }
                }
            }

            if (optionsButton.Contains(mousePosition))
            {
                Level_Select_Menu.optionHighlight = true;
            }

            if (currentMS.LeftButton == ButtonState.Pressed && previousMS.LeftButton == ButtonState.Released)
            {
                if (Level_Select_Menu.optionHighlight)
                {
                    Transition.AccessNextTransitionState = Transition.TransitionState.ToLvSelect;

                    Transition.StartTransition(Transition.TransitionState.ToOptions);
                }
                else
                {
                    Music_Player.PlayMusic();
                    Level_Manager.AccessRating = 1000;
                    Transition.StartTransition(Transition.TransitionState.ToLevel);
                }
            }
        }

        static void LevelUpdate()
        {
            selectedTile.X = 100;

            for (int i = 0; i < boardTiles.GetLength(0); i++)
            {
                for (int j = 0; j < boardTiles.GetLength(1); j++)
                {
                    if (boardTiles[i, j].Contains(mousePosition))
                        selectedTile = new Vector2(i, j);
                }
            }

            if (currentMS.LeftButton == ButtonState.Pressed && previousMS.LeftButton == ButtonState.Released)
            {
                if (Level_Manager.findingCheat && (scrollButtons[0].Contains(mousePosition) || scrollButtons[1].Contains(mousePosition)))
                {
                    if (scrollButtons[0].Contains(mousePosition))
                        Rules_List.MoveThroughRules(1);
                    else
                        Rules_List.AccessCurrentRule++;
                }
                else if (selectedTile.X != 100)
                {
                    if (!Level_Manager.findingCheat)
                        Level_Manager.findingCheat = true;
                }
                else
                    Level_Manager.findingCheat = false;
            }

            if (Level_Manager.findingCheat)
            {
                if (currentMS.ScrollWheelValue > previousMS.ScrollWheelValue)
                {
                    Rules_List.MoveThroughRules(1);
                }
                if (currentMS.ScrollWheelValue < previousMS.ScrollWheelValue)
                {
                    Rules_List.AccessCurrentRule++;
                }
            }
        }

        public static void LevelDraw(SpriteBatch aSpriteBatch)
        {
            if (selectedTile.X != 100)
            {
                Vector2 tempPosition = new Vector2((int)(Game_Board.AccessBoardPosition.X + (selectedTile.X * Game_Board.AccessTileSize)),
                        (int)(Game_Board.AccessBoardPosition.Y + (selectedTile.Y * Game_Board.AccessTileSize)));
                aSpriteBatch.Draw(tileSelect, tempPosition, Color.White);
            }
        }
    }
}
