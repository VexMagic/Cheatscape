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
        static Texture2D Banner;
        static Texture2D ImageBoarder;

        static int CurrentRuleList = 0;
        static int CurrentRule = 0;
        public static int AccessCurrentRuleList { get => CurrentRuleList; set => CurrentRuleList = value; }
        public static int AccessCurrentRule { get => CurrentRule; set => CurrentRule = value; }

        static int MaximumTextBoxWidth = 100;
        static int LineSize = 9;
        static int BetweenLineSize = 12;
        static Vector2 TextPosition = new Vector2(6, 43);

        public static List<Vector2> AllowedRules = new List<Vector2>();

        static string[] GeneralRules = { 
            "The white player always starts.", 
            "The starting board state is shown to the right.", 
            "Only one piece may be moved per turn (not including Castling).",
            "A piece can only be moved on its player's turn.",
            "A piece can only move to an unoccupied space or one occupied by an opposing piece.", 
            "If a piece is moved to a space occupied by an opposing piece, the opposing piece is captured and removed from the game.",
            "If a Pawn reaches the other side of the board, it is replaced by a new queen, rook, bishop, or knight of the same color."};
        static string[] MovementRules = { 
            "The Pawn moves one space forward but on its first move it can move one or two spaces forward.",
            "The Pawn can only attack one space diagonally ahead and not straight forward.", 
            "The Rook can move any number of spaces in a straight line vertically or horizontally.",
            "The Bishop can move any number of spaces in a straight line diagonally.",
            "The Knight can move in an L shape.",
            "The Queen can move any number of spaces in a straight line vertically, horizontally or diagonally.",
            "The King can move one space in any direction.",
            "The Knight is the only piece that can jump over other pieces"};
        static string[] ExtraRules = { 
            "If the King is being threatened by an opposing piece, its player has to move a piece to secure the King. This is called Check.",
            "If the King is in Check and its player can't secure it on their turn, the opponent wins. This is called Checkmate.",
            "The a piece can't make a move that causes its King to be in Check.",
            "A tie happens when the same board state occurs 3 times or when both players have made 50 moves.", 
            "If the King and a Rook have not moved yet, the King can move 2 spaces towards the Rook causing the Rook to move to the space next to the King on the opposite side. This is called Castling.", 
            "Castling is not allowed if the King moves out of, through or into a space where it would be in Check."};

        public static void Load() //get font
        {
            Font = Global_Info.AccessContentManager.Load<SpriteFont>("Font");
            Background = Global_Info.AccessContentManager.Load<Texture2D>("TextboxBackground");
            Selector = Global_Info.AccessContentManager.Load<Texture2D>("Selector");
            Banner = Global_Info.AccessContentManager.Load<Texture2D>("Rules Banner");
            ImageBoarder = Global_Info.AccessContentManager.Load<Texture2D>("Rule Image Boarder");
        }

        public static void IncludeList(int aList)
        {
            string[] tempArray = GetList(CurrentRuleList);

            for (int i = 0; i < tempArray.Length; i++)
            {
                if (!AllowedRules.Contains(new Vector2(aList, i)))
                {
                    AllowedRules.Add(new Vector2(aList, i));
                }
            }
        }

        public static string[] GetList(int aList)
        {
            switch (CurrentRuleList)
            {
                default:
                    return GeneralRules;
                case 1:
                    return MovementRules;
                case 2:
                    return ExtraRules;
            }
        }

        public static string[] GetAllowedRules(int aRuleList)
        {
            List<string> tempList = new List<string>(GetList(CurrentRuleList));

            for (int i = 0; i < tempList.Count; i++)
            {
                if (!AllowedRules.Contains(new Vector2(aRuleList, i)))
                {
                    tempList.RemoveAt(i);
                    i--;
                }
            }

            return tempList.ToArray();
        }

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            string[] tempArray = GetAllowedRules(CurrentRuleList);

            if (CurrentRule >= tempArray.Length)
                CurrentRule = tempArray.Length - 1;
            else if (CurrentRule < 0)
                CurrentRule = 0;

            DrawTextBox(tempArray, aSpriteBatch);

            aSpriteBatch.Draw(Banner, new Rectangle(0, 0, MaximumTextBoxWidth + (int)(TextPosition.X * 2), 20), new Rectangle(0, 0, MaximumTextBoxWidth + (int)(TextPosition.X * 2), 20), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            aSpriteBatch.Draw(Banner, new Rectangle(0, 20, MaximumTextBoxWidth + (int)(TextPosition.X * 2), 17), new Rectangle(0, (CurrentRuleList * 17) + 20, MaximumTextBoxWidth + (int)(TextPosition.X * 2), 17), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);

            Texture2D tempRuleImage = Global_Info.AccessContentManager.Load<Texture2D>("Rule Images/" + CurrentRuleList + "-" + CurrentRule);
            aSpriteBatch.Draw(tempRuleImage, new Rectangle(375, 7, 96, 96), Color.White);

            aSpriteBatch.Draw(ImageBoarder, new Rectangle(372, 4, 102, 102), Color.White);
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
                if (MaxScroll(aStringArray) > 0)
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

            return tempMaxScroll + (int)TextPosition.Y - 2;
        }
    }
}
