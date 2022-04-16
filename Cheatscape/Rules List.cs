using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cheatscape
{
    static class Rules_List
    {
        static SpriteFont Font;
        static Texture2D Background;
        static Texture2D Selector;

        static int CurrentRuleList = 0;
        static int CurrentRule = 0;
        public static int AccessCurrentRuleList { get => CurrentRuleList; set => CurrentRuleList = value; }
        public static int AccessCurrentRule { get => CurrentRule; set => CurrentRule = value; }

        static int MaximumTextBoxWidth = 100;
        static int LineSize = 9;
        static int BetweenLineSize = 12;
        static Vector2 TextPosition = new Vector2(6, 6);

        static string[] GeneralRules = { 
            "The white player always starts.", 
            "The starting board state is shown below.", 
            "Only one piece may be moved per turn (not including castling).", 
            "A piece can only move to an unoccupied space or one occupied by an opposing piece.", 
            "If a piece is moved to a space occupied by an opposing piece, the opposing piece is captured and removed from the game.",
            "If a Pawn reaches the other side of the board, it is replaced by a new queen, rook, bishop, or knight of the same color.",
            "A tie happens when the same board state occurs 3 times or when both players have made 50 moves."};
        static string[] MovementRules = { 
            "The Pawn moves one space forward but on its first move it can move one or two spaces forward.",
            "The Pawn can only attack one space diagonally ahead and not straight forward.", 
            "The Rook can move any number of spaces in a straight line vertically or horizontally.",
            "The Bishop can move any number of spaces in a straight line diagonally.",
            ""};
        static string[] KingRules = { };
        static string[] Castling = { };

        public static void Load() //get font
        {
            Font = Global_Info.AccessContentManager.Load<SpriteFont>("Font");
            Background = Global_Info.AccessContentManager.Load<Texture2D>("TextboxBackground");
            Selector = Global_Info.AccessContentManager.Load<Texture2D>("Selector");
        }

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            switch (CurrentRuleList)
            {
                case 0:
                    DrawTextBox(GeneralRules, aSpriteBatch);
                    break;
                case 1:
                    DrawTextBox(MovementRules, aSpriteBatch);
                    break;
                case 2:
                    DrawTextBox(KingRules, aSpriteBatch);
                    break;
                case 3:
                    DrawTextBox(Castling, aSpriteBatch);
                    break;
            }
        }

        public static void DrawText(string aString, int anXPos, int aYPos, SpriteBatch aSpriteBatch) //draw text
        {
            aSpriteBatch.DrawString(Font, aString, new Vector2(anXPos, aYPos), Color.Black);
        }

        public static void DrawTextBox(string[] aStringArray, SpriteBatch aSpriteBatch)
        {
            int tempYOffset = (int)TextPosition.Y - Scrolling(aStringArray);

            for (int i = 0; i < aStringArray.Length; i++)
            {
                List<string> tempTextBox = new List<string>();
                string[] tempWords = aStringArray[i].Split(' ');
                string tempLine = tempWords[0];

                for (int j = 1; j < tempWords.Length; j++)
                {
                    if (MaximumTextBoxWidth >= Font.MeasureString(tempLine + " " + tempWords[j]).X)
                    {
                        tempLine += " " + tempWords[j];
                    }
                    else
                    {
                        tempTextBox.Add(tempLine);
                        tempLine = tempWords[j];
                    }
                }
                tempTextBox.Add(tempLine);

                int tempSelected = 0;
                if (CurrentRule == i)
                {
                    tempSelected = Selector.Width / 2;
                }

                aSpriteBatch.Draw(Background, new Rectangle((int)TextPosition.X - 2, tempYOffset - 2, MaximumTextBoxWidth + 4, (tempTextBox.Count * LineSize) + 4), Color.White);

                aSpriteBatch.Draw(Selector, new Rectangle((int)TextPosition.X - LineSize, tempYOffset - LineSize, LineSize, LineSize), new Rectangle(tempSelected, 0, LineSize, LineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                aSpriteBatch.Draw(Selector, new Rectangle((int)TextPosition.X, tempYOffset - LineSize, MaximumTextBoxWidth, LineSize), new Rectangle(tempSelected + LineSize, 0, LineSize, LineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                aSpriteBatch.Draw(Selector, new Rectangle((int)TextPosition.X + MaximumTextBoxWidth, tempYOffset - LineSize, LineSize, LineSize), new Rectangle(tempSelected + (LineSize * 2), 0, LineSize, LineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);

                for (int j = 0; j < tempTextBox.Count; j++)
                {
                    DrawText(tempTextBox[j], (int)TextPosition.X, tempYOffset - (LineSize / 4), aSpriteBatch);
                    tempYOffset += LineSize;

                    aSpriteBatch.Draw(Selector, new Rectangle((int)TextPosition.X - LineSize, tempYOffset - LineSize, LineSize, LineSize), new Rectangle(tempSelected, LineSize, LineSize, LineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                    aSpriteBatch.Draw(Selector, new Rectangle((int)TextPosition.X + MaximumTextBoxWidth, tempYOffset - LineSize, LineSize, LineSize), new Rectangle(tempSelected + (LineSize * 2), LineSize, LineSize, LineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                }

                aSpriteBatch.Draw(Selector, new Rectangle((int)TextPosition.X - LineSize, tempYOffset, LineSize, LineSize), new Rectangle(tempSelected, LineSize * 2, LineSize, LineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                aSpriteBatch.Draw(Selector, new Rectangle((int)TextPosition.X, tempYOffset, MaximumTextBoxWidth, LineSize), new Rectangle(tempSelected + LineSize, LineSize * 2, LineSize, LineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                aSpriteBatch.Draw(Selector, new Rectangle((int)TextPosition.X + MaximumTextBoxWidth, tempYOffset, LineSize, LineSize), new Rectangle(tempSelected + (LineSize * 2), LineSize * 2, LineSize, LineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);

                tempYOffset += BetweenLineSize;
            }
        }

        static int Scrolling(string[] aStringArray)
        {
            int tempScrollAmount = 0 - (int)(Global_Info.AccessWindowSize.Y / (2 * Global_Info.AccessScreenScale));

            for (int i = 0; i < aStringArray.Length; i++)
            {

                List<string> tempTextBox = new List<string>();
                string[] tempWords = aStringArray[i].Split(' ');
                string tempLine = tempWords[0];

                for (int j = 1; j < tempWords.Length; j++)
                {
                    if (MaximumTextBoxWidth >= Font.MeasureString(tempLine + " " + tempWords[j]).X)
                    {
                        tempLine += " " + tempWords[j];
                    }
                    else
                    {
                        tempTextBox.Add(tempLine);
                        tempLine = tempWords[j];
                    }
                }
                tempTextBox.Add(tempLine);

                if (CurrentRule == i)
                {
                    tempScrollAmount += ((LineSize * tempTextBox.Count) + BetweenLineSize ) / 2;
                    break;
                }

                tempScrollAmount += LineSize * tempTextBox.Count;
                tempScrollAmount += BetweenLineSize;
            }

            if (tempScrollAmount < 0)
                return 0;
            else if (tempScrollAmount > MaxScroll(aStringArray))
            {
                if (MaxScroll(aStringArray) > (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale))
                    return MaxScroll(aStringArray);
                else
                    return 0;
            }
            else
                return tempScrollAmount;
        }

        static int MaxScroll(string[] aStringArray)
        {
            int tempMaxScroll = 0 - (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale);

            for (int i = 0; i < aStringArray.Length; i++)
            {

                List<string> tempTextBox = new List<string>();
                string[] tempWords = aStringArray[i].Split(' ');
                string tempLine = tempWords[0];

                for (int j = 1; j < tempWords.Length; j++)
                {
                    if (MaximumTextBoxWidth >= Font.MeasureString(tempLine + " " + tempWords[j]).X)
                    {
                        tempLine += " " + tempWords[j];
                    }
                    else
                    {
                        tempTextBox.Add(tempLine);
                        tempLine = tempWords[j];
                    }
                }
                tempTextBox.Add(tempLine);

                tempMaxScroll += LineSize * tempTextBox.Count;
                tempMaxScroll += BetweenLineSize;
            }

            return tempMaxScroll + 4;
        }
    }
}
