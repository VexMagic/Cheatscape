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

        public static int MaximumTextBoxWidth = 100;
        public static int LineSize = 9;
        public static int BetweenLineSize = 12;
        public static bool IsTextCentered = false;

        public static string TutorialText;

        public static Vector2 RulesPosition = new Vector2(6, 144);
        public static Vector2 TutorialPosition = new Vector2(450, 100);

        public static void Load()
        {
            Font = Global_Info.AccessContentManager.Load<SpriteFont>("File");
            Background = Global_Info.AccessContentManager.Load<Texture2D>("TextboxBackground");
            TextBoarder = Global_Info.AccessContentManager.Load<Texture2D>("Text Boarder");
            RuleSelector = Global_Info.AccessContentManager.Load<Texture2D>("Selector");
        }

        public static void DrawText(string aString, int anXPos, int aYPos, SpriteBatch aSpriteBatch) //draw text
        {
            aSpriteBatch.DrawString(Font, aString, new Vector2(anXPos, aYPos), Color.Black);
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

            if (tempYOffset > (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale) - LineSize - (BetweenLineSize / 2))
            {
                tempYOffset = (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale) - LineSize - (BetweenLineSize / 2);
            }

            if (Rules_List.AccessCurrentRule != Rules_List.GetList().Length)
                DrawTextBox("Back", new Vector2(RulesPosition.X, tempYOffset), TextBoarder, aSpriteBatch);
            else
                DrawTextBox("Back", new Vector2(RulesPosition.X, tempYOffset), RuleSelector, aSpriteBatch);
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
