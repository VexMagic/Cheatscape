using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cheatscape
{
    static class Text_Manager
    {
        static SpriteFont Font;
        static Texture2D Background;
        static Texture2D TextBoarder;
        static Texture2D RuleSelector;
        static Texture2D ScrollBar;

        public static int MaximumTextBoxWidth = 100;
        public static int LineSize = 9;
        public static int BetweenLineSize = 12;
        public static bool IsTextCentered = false;
        static bool IsScrollNeeded;

        public enum TextStyle { Standard, DropShadow, Boarder, Blood}
        public static TextStyle CurrentTextStyle = TextStyle.Standard;

        public static string TutorialText;

        public static Vector2 RulesPosition = new Vector2(6 + Rules_List.ScrollBarWidth, 144);
        public static Vector2 TutorialPosition = new Vector2(450, 100);

        public static void Load()
        {
            Font = Global_Info.AccessContentManager.Load<SpriteFont>("Font");
            Background = Global_Info.AccessContentManager.Load<Texture2D>("TextboxBackground");
            TextBoarder = Global_Info.AccessContentManager.Load<Texture2D>("Text Boarder");
            RuleSelector = Global_Info.AccessContentManager.Load<Texture2D>("Selector");
            ScrollBar = Global_Info.AccessContentManager.Load<Texture2D>("Scroll Bar");
        }

        public static void DrawText(string aString, int anXPos, int aYPos, SpriteBatch aSpriteBatch) //draw text
        {
            switch (CurrentTextStyle)
            {
                default:
                    aSpriteBatch.DrawString(Font, aString, new Vector2(anXPos, aYPos), Color.Black);
                    break;
                case TextStyle.DropShadow:
                    aSpriteBatch.DrawString(Font, aString, new Vector2(anXPos + 1, aYPos + 1), Color.Black);
                    aSpriteBatch.DrawString(Font, aString, new Vector2(anXPos, aYPos), Color.White);
                    break;
                case TextStyle.Boarder:
                    aSpriteBatch.DrawString(Font, aString, new Vector2(anXPos + 1, aYPos), Color.Black);
                    aSpriteBatch.DrawString(Font, aString, new Vector2(anXPos, aYPos + 1), Color.Black);
                    aSpriteBatch.DrawString(Font, aString, new Vector2(anXPos, aYPos - 1), Color.Black);
                    aSpriteBatch.DrawString(Font, aString, new Vector2(anXPos - 1, aYPos), Color.Black);
                    aSpriteBatch.DrawString(Font, aString, new Vector2(anXPos, aYPos), Color.White);
                    break;
                case TextStyle.Blood:
                    aSpriteBatch.DrawString(Font, aString, new Vector2(anXPos + 1, aYPos), Color.Black);
                    aSpriteBatch.DrawString(Font, aString, new Vector2(anXPos, aYPos + 1), Color.Black);
                    aSpriteBatch.DrawString(Font, aString, new Vector2(anXPos, aYPos - 1), Color.Black);
                    aSpriteBatch.DrawString(Font, aString, new Vector2(anXPos - 1, aYPos), Color.Black);
                    aSpriteBatch.DrawString(Font, aString, new Vector2(anXPos, aYPos), Color.Red);
                    break;
            }
        }

        public static void DrawTextBox(string aString, Vector2 aPosition, Texture2D aBoarder, SpriteBatch aSpriteBatch, bool isScalable = false)
        {
            List<string> tempTextBox = SeparateText(aString);
            int tempBoxWidth = 0;

            if (isScalable)
            {
                for (int i = 0; i < tempTextBox.Count; i++)
                {
                    if (Font.MeasureString(tempTextBox[i]).X > tempBoxWidth)
                    {
                        tempBoxWidth = (int)Font.MeasureString(tempTextBox[i]).X;
                    }
                }
            }
            else
            {
                tempBoxWidth = MaximumTextBoxWidth;
            }

            aSpriteBatch.Draw(Background, new Rectangle((int)aPosition.X - 2, (int)aPosition.Y - 2, tempBoxWidth + 4, (tempTextBox.Count * LineSize) + 4), Color.White);

            aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X - LineSize, (int)aPosition.Y - LineSize, LineSize, LineSize), new Rectangle(0, 0, LineSize, LineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X, (int)aPosition.Y - LineSize, tempBoxWidth, LineSize), new Rectangle(LineSize, 0, LineSize, LineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X + tempBoxWidth, (int)aPosition.Y - LineSize, LineSize, LineSize), new Rectangle((LineSize * 2), 0, LineSize, LineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);

            for (int j = 0; j < tempTextBox.Count; j++)
            {
                int tempOffset = 0;

                if (IsTextCentered)
                    tempOffset = (int)((Font.MeasureString(tempTextBox[j]).X - tempBoxWidth) / 2);

                DrawText(tempTextBox[j], (int)aPosition.X - tempOffset, (int)aPosition.Y - (LineSize / 4), aSpriteBatch);
                aPosition.Y += LineSize;

                aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X - LineSize, (int)aPosition.Y - LineSize, LineSize, LineSize), new Rectangle(0, LineSize, LineSize, LineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X + tempBoxWidth, (int)aPosition.Y - LineSize, LineSize, LineSize), new Rectangle((LineSize * 2), LineSize, LineSize, LineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            }

            aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X - LineSize, (int)aPosition.Y, LineSize, LineSize), new Rectangle(0, LineSize * 2, LineSize, LineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X, (int)aPosition.Y, tempBoxWidth, LineSize), new Rectangle(LineSize, LineSize * 2, LineSize, LineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X + tempBoxWidth, (int)aPosition.Y, LineSize, LineSize), new Rectangle((LineSize * 2), LineSize * 2, LineSize, LineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);

        }

        public static void DrawRuleBox(string[] aStringArray, SpriteBatch aSpriteBatch)
        {
            int tempYOffset = (int)RulesPosition.Y - Rules_List.Scrolling(aStringArray);
            for (int i = 0; i < aStringArray.Length; i++)
            {
                if (Rules_List.AccessCurrentRule != Rules_List.GetList().Length)
                {
                    if (Rules_List.GetList()[Rules_List.AccessCurrentRule] == aStringArray[i])
                        DrawTextBox(aStringArray[i], new Vector2(RulesPosition.X, tempYOffset), RuleSelector, aSpriteBatch);
                    else
                        DrawTextBox(aStringArray[i], new Vector2(RulesPosition.X, tempYOffset), TextBoarder, aSpriteBatch);
                }
                else
                    DrawTextBox(aStringArray[i], new Vector2(RulesPosition.X, tempYOffset), TextBoarder, aSpriteBatch);

                List<string> tempTextBox = SeparateText(aStringArray[i]);
                tempYOffset += tempTextBox.Count * LineSize;
                tempYOffset += BetweenLineSize;
            }

            if (tempYOffset >= (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale) - LineSize - (BetweenLineSize / 2))
            {
                tempYOffset = (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale) - LineSize - (BetweenLineSize / 2);
                IsScrollNeeded = true;
            }
            else
            {
                IsScrollNeeded = false;
            }

            if (Rules_List.AccessCurrentRule != Rules_List.GetList().Length)
                DrawTextBox("Back", new Vector2(RulesPosition.X, tempYOffset), TextBoarder, aSpriteBatch);
            else
                DrawTextBox("Back", new Vector2(RulesPosition.X, tempYOffset), RuleSelector, aSpriteBatch);

            if (IsScrollNeeded)
            {
                DrawScrollBar(aStringArray, aSpriteBatch);
            }
        }

        static void DrawScrollBar(string[] aStringArray, SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(ScrollBar, new Rectangle(0, (int)RulesPosition.Y - 6, 20, 21), new Rectangle(0, 0, 20, 21), Color.White);
            aSpriteBatch.Draw(ScrollBar, new Rectangle(0, (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale) - 21, 
                20, 21), new Rectangle(0, 22, 20, 21), Color.White);

            int tempBarFullLength = (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale) - 36 - (int)RulesPosition.Y;

            aSpriteBatch.Draw(ScrollBar, new Rectangle(0, (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale) - 
                21 - tempBarFullLength, 20, tempBarFullLength), new Rectangle(0, 21, 20, 1), Color.White);

            int tempProcent = (tempBarFullLength * 100) / ScrollPercent(aStringArray);

            int tempScrollBarLenth = (int)Math.Round((double)(25 * tempBarFullLength) / tempProcent);

            double tempBarScrollAmount = (double)(100 * Scrolling(aStringArray)) / ScrollPercent(aStringArray) / 100;

            //int tempScrollAmount = (int)RulesPosition.Y - 6 + 19 + (0);
            //int tempScrollAmount = (int)RulesPosition.Y - 6 + 19 + ((tempBarFullLength - tempScrollBarLenth) / 2);
            //int tempScrollAmount = (int)RulesPosition.Y - 6 + 19 + (tempBarFullLength - tempScrollBarLenth);
            int tempScrollAmount = (int)(RulesPosition.Y + 13 + ((tempBarFullLength - tempScrollBarLenth) * tempBarScrollAmount));

            aSpriteBatch.Draw(ScrollBar, new Rectangle(0, tempScrollAmount + 2, 20, tempScrollBarLenth), new Rectangle(0, 45, 20, 1), Color.White);
            aSpriteBatch.Draw(ScrollBar, new Rectangle(0, tempScrollAmount, 20, 2), new Rectangle(0, 43, 20, 2), Color.White);
            aSpriteBatch.Draw(ScrollBar, new Rectangle(0, tempScrollAmount + tempScrollBarLenth + 2, 20, 2), new Rectangle(0, 46, 20, 2), Color.White);
        }

        public static int Scrolling(string[] aStringArray)
        {
            int tempScrollAmount = ((int)RulesPosition.Y / 2) - (int)(Global_Info.AccessWindowSize.Y / (2 * Global_Info.AccessScreenScale));

            for (int i = 0; i < aStringArray.Length; i++)
            {
                List<string> tempTextBox = SeparateText(aStringArray[i]);

                if (Rules_List.AccessCurrentRule == i)
                {
                    tempScrollAmount += ((LineSize * tempTextBox.Count) + BetweenLineSize) / 2;
                    break;
                }

                tempScrollAmount += LineSize * tempTextBox.Count;
                tempScrollAmount += BetweenLineSize;
            }

            if (tempScrollAmount < 0)
                return 0;
            else
                return tempScrollAmount;
        }

        static int ScrollPercent(string[] aStringArray)
        {
            int tempRuleArea = ((int)RulesPosition.Y / 2) - (int)(Global_Info.AccessWindowSize.Y / (2 * Global_Info.AccessScreenScale));

            for (int i = 0; i < aStringArray.Length; i++)
            {
                List<string> tempTextBox = SeparateText(aStringArray[i]);

                tempRuleArea += LineSize * tempTextBox.Count;
                tempRuleArea += BetweenLineSize;
            }

            return tempRuleArea;
        }

        public static void DrawTutorialBox(SpriteBatch aSpriteBatch)
        {
            if (TutorialText != "" && TutorialText != null)
                DrawTextBox(TutorialText, TutorialPosition, TextBoarder, aSpriteBatch, true);
        }

        public static void DrawTurnCounter(SpriteBatch aSpriteBatch)
        {
            DrawTextBox("Turns left: " + File_Manager.turnCounter,Game_Board.BoardPosition-new Vector2(TextBoarder.Width/2,TextBoarder.Height+12), TextBoarder, aSpriteBatch);
        }

        public static List<string> SeparateText(string aString)
        {
            List<string> tempTextBox = new List<string>();
            string[] tempWords = aString.Split(' ');
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

            return tempTextBox;
        }
    }
}
