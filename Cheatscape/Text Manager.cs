using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Cheatscape
{
    static class Text_Manager
    {
        static SpriteFont font;
        public static SpriteFont largeFont;
        static Texture2D background;
        public static Texture2D textBoarder;
        static Texture2D ruleSelector;
        static Texture2D scrollBar;

        public static int maximumTextBoxWidth = 100;
        public static int lineSize = 9;
        public static int betweenLineSize = 12;
        public static bool isTextCentered = false;

        public enum TextStyle
        {
            standard, dropShadow, boarder, blood
        }
        public static TextStyle currentTextStyle = TextStyle.boarder;

        public static string tutorialText;

        public static Vector2 rulesPosition = new Vector2(6 + Rules_List.scrollBarWidth, 144);
        public static Vector2 tutorialPosition = new Vector2(470, 50);

        public static void Load()
        {
            font = Global_Info.AccessContentManager.Load<SpriteFont>("Font");
            largeFont = Global_Info.AccessContentManager.Load<SpriteFont>("LargeFont");
            background = Global_Info.AccessContentManager.Load<Texture2D>("TextboxBackground");
            textBoarder = Global_Info.AccessContentManager.Load<Texture2D>("Text Boarder");
            ruleSelector = Global_Info.AccessContentManager.Load<Texture2D>("Selector");
            scrollBar = Global_Info.AccessContentManager.Load<Texture2D>("Scroll Bar");
        }

        public static void DrawText(string aString, int anXPos, int aYPos, SpriteBatch aSpriteBatch) //draw text
        {
            switch (currentTextStyle)
            {
                default:
                    aSpriteBatch.DrawString(font, aString, new Vector2(anXPos, aYPos), Color.Black);
                    break;
                case TextStyle.dropShadow:
                    aSpriteBatch.DrawString(font, aString, new Vector2(anXPos + 1, aYPos + 1), Color.Black);
                    aSpriteBatch.DrawString(font, aString, new Vector2(anXPos, aYPos), Color.White);
                    break;
                case TextStyle.boarder:
                    aSpriteBatch.DrawString(font, aString, new Vector2(anXPos + 1, aYPos), Color.Black);
                    aSpriteBatch.DrawString(font, aString, new Vector2(anXPos, aYPos + 1), Color.Black);
                    aSpriteBatch.DrawString(font, aString, new Vector2(anXPos, aYPos - 1), Color.Black);
                    aSpriteBatch.DrawString(font, aString, new Vector2(anXPos - 1, aYPos), Color.Black);
                    aSpriteBatch.DrawString(font, aString, new Vector2(anXPos, aYPos), Color.White);
                    break;
                case TextStyle.blood:
                    aSpriteBatch.DrawString(font, aString, new Vector2(anXPos + 1, aYPos), Color.Black);
                    aSpriteBatch.DrawString(font, aString, new Vector2(anXPos, aYPos + 1), Color.Black);
                    aSpriteBatch.DrawString(font, aString, new Vector2(anXPos, aYPos - 1), Color.Black);
                    aSpriteBatch.DrawString(font, aString, new Vector2(anXPos - 1, aYPos), Color.Black);
                    aSpriteBatch.DrawString(font, aString, new Vector2(anXPos, aYPos), Color.Red);
                    break;
            }
        }

        public static void DrawLargeText(string aString, int anXPos, int aYPos, SpriteBatch aSpriteBatch) //draw white text
        {
            aSpriteBatch.DrawString(largeFont, aString, new Vector2(anXPos + 1, aYPos), Color.Black);
            aSpriteBatch.DrawString(largeFont, aString, new Vector2(anXPos, aYPos + 1), Color.Black);
            aSpriteBatch.DrawString(largeFont, aString, new Vector2(anXPos, aYPos - 1), Color.Black);
            aSpriteBatch.DrawString(largeFont, aString, new Vector2(anXPos - 1, aYPos), Color.Black);
            aSpriteBatch.DrawString(largeFont, aString, new Vector2(anXPos, aYPos), Color.White);
        }

        public static void DrawTextBox(string aString, Vector2 aPosition, Texture2D aBoarder, SpriteBatch aSpriteBatch, bool isScalable = false, bool isRuleText = false)
        {
            List<string> tempTextBox = SeparateText(aString);
            int tempBoxWidth = 0;

            if (isScalable)
            {
                for (int i = 0; i < tempTextBox.Count; i++)
                {
                    if (font.MeasureString(tempTextBox[i]).X > tempBoxWidth)
                    {
                        tempBoxWidth = (int)font.MeasureString(tempTextBox[i]).X;
                    }
                }
            }
            else
            {
                tempBoxWidth = maximumTextBoxWidth;
            }

            aSpriteBatch.Draw(background, new Rectangle((int)aPosition.X - 2, (int)aPosition.Y - 2, tempBoxWidth + 4, (tempTextBox.Count * lineSize) + 4), Color.White);

            aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X - lineSize, (int)aPosition.Y - lineSize, lineSize, lineSize), new Rectangle(0, 0, lineSize, lineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X, (int)aPosition.Y - lineSize, tempBoxWidth, lineSize), new Rectangle(lineSize, 0, lineSize, lineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X + tempBoxWidth, (int)aPosition.Y - lineSize, lineSize, lineSize), new Rectangle((lineSize * 2), 0, lineSize, lineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);

            int tempYPositionOffset = (int)aPosition.Y;

            for (int j = 0; j < tempTextBox.Count; j++)
            {
                int tempOffset = 0;

                if (isTextCentered)
                    tempOffset = (int)((font.MeasureString(tempTextBox[j]).X - tempBoxWidth) / 2);

                DrawText(tempTextBox[j], (int)aPosition.X - tempOffset, tempYPositionOffset - (lineSize / 4), aSpriteBatch);
                tempYPositionOffset += lineSize;

                aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X - lineSize, tempYPositionOffset - lineSize, lineSize, lineSize), new Rectangle(0, lineSize, lineSize, lineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X + tempBoxWidth, tempYPositionOffset - lineSize, lineSize, lineSize), new Rectangle((lineSize * 2), lineSize, lineSize, lineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            }

            aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X - lineSize, tempYPositionOffset, lineSize, lineSize), new Rectangle(0, lineSize * 2, lineSize, lineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X, tempYPositionOffset, tempBoxWidth, lineSize), new Rectangle(lineSize, lineSize * 2, lineSize, lineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X + tempBoxWidth, tempYPositionOffset, lineSize, lineSize), new Rectangle((lineSize * 2), lineSize * 2, lineSize, lineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);

            if (isRuleText)
            {
                Mouse_Controller.ruleBoxes.Add(new Rectangle((int)aPosition.X - (lineSize / 2), (int)aPosition.Y - (lineSize / 2),
                        tempBoxWidth + lineSize, tempYPositionOffset - (int)aPosition.Y + lineSize));
            }
        }

        public static void DrawRuleBox(string[] aStringArray, SpriteBatch aSpriteBatch)
        {
            Mouse_Controller.ruleBoxes.Clear();
            int tempYOffset = (int)rulesPosition.Y - Rules_List.Scrolling(aStringArray);
            for (int i = 0; i < aStringArray.Length; i++)
            {
                if (Rules_List.AccessCurrentRule != Rules_List.GetList().Length)
                {
                    if (Rules_List.GetList()[Rules_List.AccessCurrentRule] == aStringArray[i])
                        DrawTextBox(aStringArray[i], new Vector2(rulesPosition.X, tempYOffset), ruleSelector, aSpriteBatch, false, true);
                    else
                        DrawTextBox(aStringArray[i], new Vector2(rulesPosition.X, tempYOffset), textBoarder, aSpriteBatch, false, true);
                }
                else
                    DrawTextBox(aStringArray[i], new Vector2(rulesPosition.X, tempYOffset), textBoarder, aSpriteBatch, false, true);

                List<string> tempTextBox = SeparateText(aStringArray[i]);
                tempYOffset += tempTextBox.Count * lineSize;
                tempYOffset += betweenLineSize;
            }

            if (tempYOffset >= (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale) - lineSize - (betweenLineSize / 2))
            {
                tempYOffset = (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale) - lineSize - (betweenLineSize / 2);
                DrawScrollBar(aStringArray, aSpriteBatch);
            }

            if (Level_Manager.currentBundle != 0 || Level_Manager.currentLevel != 0)
            {
                if (Rules_List.AccessCurrentRule != Rules_List.GetList().Length)
                    DrawTextBox("Back", new Vector2(rulesPosition.X, tempYOffset), textBoarder, aSpriteBatch, false, true);
                else
                    DrawTextBox("Back", new Vector2(rulesPosition.X, tempYOffset), ruleSelector, aSpriteBatch, false, true);
            }
        }

        static void DrawScrollBar(string[] aStringArray, SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(scrollBar, new Rectangle(0, (int)rulesPosition.Y - 6, 20, 21), new Rectangle(0, 0, 20, 21), Color.White);
            aSpriteBatch.Draw(scrollBar, new Rectangle(0, (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale) - 21,
                20, 21), new Rectangle(0, 22, 20, 21), Color.White);

            int tempBarFullLength = (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale) - 36 - (int)rulesPosition.Y;

            aSpriteBatch.Draw(scrollBar, new Rectangle(0, (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale) -
                21 - tempBarFullLength, 20, tempBarFullLength), new Rectangle(0, 21, 20, 1), Color.White);

            int tempProcent = (tempBarFullLength * 100) / ScrollPercent(aStringArray);

            int tempScrollBarLenth = (int)Math.Round((double)(25 * tempBarFullLength) / tempProcent);

            double tempBarScrollAmount = (double)(100 * Scrolling(aStringArray)) / ScrollPercent(aStringArray) / 100;

            int tempScrollAmount = (int)(rulesPosition.Y + 13 + ((tempBarFullLength - tempScrollBarLenth) * tempBarScrollAmount));

            aSpriteBatch.Draw(scrollBar, new Rectangle(0, tempScrollAmount + 2, 20, tempScrollBarLenth), new Rectangle(0, 45, 20, 1), Color.White);
            aSpriteBatch.Draw(scrollBar, new Rectangle(0, tempScrollAmount, 20, 2), new Rectangle(0, 43, 20, 2), Color.White);
            aSpriteBatch.Draw(scrollBar, new Rectangle(0, tempScrollAmount + tempScrollBarLenth + 2, 20, 2), new Rectangle(0, 46, 20, 2), Color.White);
        }

        public static int Scrolling(string[] aStringArray)
        {
            int tempScrollAmount = ((int)rulesPosition.Y / 2) - (int)(Global_Info.AccessWindowSize.Y / (2 * Global_Info.AccessScreenScale));

            for (int i = 0; i < aStringArray.Length; i++)
            {
                List<string> tempTextBox = SeparateText(aStringArray[i]);

                tempScrollAmount += lineSize * tempTextBox.Count;
                tempScrollAmount += betweenLineSize;

                if (Rules_List.AccessCurrentRule == i)
                    break;
            }

            if (tempScrollAmount < 0)
                return 0;
            else
                return tempScrollAmount;
        }

        static int ScrollPercent(string[] aStringArray)
        {
            int tempRuleArea = ((int)rulesPosition.Y / 2) - (int)(Global_Info.AccessWindowSize.Y / (2 * Global_Info.AccessScreenScale));

            for (int i = 0; i < aStringArray.Length; i++)
            {
                List<string> tempTextBox = SeparateText(aStringArray[i]);

                tempRuleArea += lineSize * tempTextBox.Count;
                tempRuleArea += betweenLineSize;
            }

            return tempRuleArea;
        }

        public static void DrawTutorialBox(SpriteBatch aSpriteBatch)
        {
            if (tutorialText != "" && tutorialText != null && Level_Manager.isOnTransitionScreen == false)
                DrawTextBox(tutorialText, tutorialPosition, textBoarder, aSpriteBatch, true);
        }

        public static List<string> SeparateText(string aString)
        {
            List<string> tempTextBox = new List<string>();
            string[] tempWords = aString.Split(' ');
            string tempLine = tempWords[0];

            for (int j = 1; j < tempWords.Length; j++)
            {
                if (maximumTextBoxWidth >= font.MeasureString(tempLine + " " + tempWords[j]).X)
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
