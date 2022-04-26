﻿using Microsoft.Xna.Framework;
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

        public static Vector2 RulesPosition = new Vector2(6, 144);

        public static void Load()
        {
            Font = Global_Info.AccessContentManager.Load<SpriteFont>("Font");
            Background = Global_Info.AccessContentManager.Load<Texture2D>("TextboxBackground");
            TextBoarder = Global_Info.AccessContentManager.Load<Texture2D>("Text Boarder");
            RuleSelector = Global_Info.AccessContentManager.Load<Texture2D>("Selector");
        }

        public static void DrawText(string aString, int anXPos, int aYPos, SpriteBatch aSpriteBatch) //draw text
        {
            aSpriteBatch.DrawString(Font, aString, new Vector2(anXPos, aYPos), Color.Black);
        }

        public static void DrawTextBox(string aString, Vector2 aPosition, Texture2D aBoarder, SpriteBatch aSpriteBatch)
        {
            List<string> tempTextBox = SeparateText(aString);

            aSpriteBatch.Draw(Background, new Rectangle((int)aPosition.X - 2, (int)aPosition.Y - 2, MaximumTextBoxWidth + 4, (tempTextBox.Count * LineSize) + 4), Color.White);

            aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X - LineSize, (int)aPosition.Y - LineSize, LineSize, LineSize), new Rectangle(0, 0, LineSize, LineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X, (int)aPosition.Y - LineSize, MaximumTextBoxWidth, LineSize), new Rectangle(LineSize, 0, LineSize, LineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X + MaximumTextBoxWidth, (int)aPosition.Y - LineSize, LineSize, LineSize), new Rectangle((LineSize * 2), 0, LineSize, LineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);

            for (int j = 0; j < tempTextBox.Count; j++)
            {
                Text_Manager.DrawText(tempTextBox[j], (int)aPosition.X, (int)aPosition.Y - (LineSize / 4), aSpriteBatch);
                aPosition.Y += LineSize;

                aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X - LineSize, (int)aPosition.Y - LineSize, LineSize, LineSize), new Rectangle(0, LineSize, LineSize, LineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X + MaximumTextBoxWidth, (int)aPosition.Y - LineSize, LineSize, LineSize), new Rectangle((LineSize * 2), LineSize, LineSize, LineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            }

            aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X - LineSize, (int)aPosition.Y, LineSize, LineSize), new Rectangle(0, LineSize * 2, LineSize, LineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X, (int)aPosition.Y, MaximumTextBoxWidth, LineSize), new Rectangle(LineSize, LineSize * 2, LineSize, LineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X + MaximumTextBoxWidth, (int)aPosition.Y, LineSize, LineSize), new Rectangle((LineSize * 2), LineSize * 2, LineSize, LineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);

        }

        public static void DrawRuleBox(string[] aStringArray, SpriteBatch aSpriteBatch)
        {
            int tempYOffset = (int)RulesPosition.Y - Rules_List.Scrolling(aStringArray);

            for (int i = 0; i < aStringArray.Length; i++)
            {
                if (Rules_List.GetList()[Rules_List.CurrentRule] == aStringArray[i])
                    DrawTextBox(aStringArray[i], new Vector2(RulesPosition.X, tempYOffset), RuleSelector, aSpriteBatch);
                else
                    DrawTextBox(aStringArray[i], new Vector2(RulesPosition.X, tempYOffset), TextBoarder, aSpriteBatch);

                tempYOffset += BetweenLineSize;
            }
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
