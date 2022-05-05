using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Cheatscape
{
    static class Text_Manager
    {
        static SpriteFont font;
        static Texture2D background;
        static Texture2D textBoarder;
        static Texture2D ruleSelector;

        public static int maximumTextBoxWidth = 100;
        public static int lineSize = 9;
        public static int betweenLineSize = 12;

        public static string tutorialText;

        public static Vector2 rulesPosition = new Vector2(6, 144);
        public static Vector2 tutorialPosition = new Vector2(450, 100);

        public static void Load()
        {
            font = Global_Info.AccessContentManager.Load<SpriteFont>("Font");
            background = Global_Info.AccessContentManager.Load<Texture2D>("TextboxBackground");
            textBoarder = Global_Info.AccessContentManager.Load<Texture2D>("Text Boarder");
            ruleSelector = Global_Info.AccessContentManager.Load<Texture2D>("Selector");
        }

        public static void DrawText(string aString, int anXPos, int aYPos, SpriteBatch aSpriteBatch) //draw text
        {
            aSpriteBatch.DrawString(font, aString, new Vector2(anXPos, aYPos), Color.Black);
        }

        public static void DrawTextBox(string aString, Vector2 aPosition, Texture2D aBoarder, SpriteBatch aSpriteBatch)
        {
            List<string> tempTextBox = SeparateText(aString);

            aSpriteBatch.Draw(background, new Rectangle((int)aPosition.X - 2, (int)aPosition.Y - 2, maximumTextBoxWidth + 4, (tempTextBox.Count * lineSize) + 4), Color.White);

            aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X - lineSize, (int)aPosition.Y - lineSize, lineSize, lineSize), new Rectangle(0, 0, lineSize, lineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X, (int)aPosition.Y - lineSize, maximumTextBoxWidth, lineSize), new Rectangle(lineSize, 0, lineSize, lineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X + maximumTextBoxWidth, (int)aPosition.Y - lineSize, lineSize, lineSize), new Rectangle((lineSize * 2), 0, lineSize, lineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);

            for (int j = 0; j < tempTextBox.Count; j++)
            {
                Text_Manager.DrawText(tempTextBox[j], (int)aPosition.X, (int)aPosition.Y - (lineSize / 4), aSpriteBatch);
                aPosition.Y += lineSize;

                aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X - lineSize, (int)aPosition.Y - lineSize, lineSize, lineSize), new Rectangle(0, lineSize, lineSize, lineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X + maximumTextBoxWidth, (int)aPosition.Y - lineSize, lineSize, lineSize), new Rectangle((lineSize * 2), lineSize, lineSize, lineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            }

            aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X - lineSize, (int)aPosition.Y, lineSize, lineSize), new Rectangle(0, lineSize * 2, lineSize, lineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X, (int)aPosition.Y, maximumTextBoxWidth, lineSize), new Rectangle(lineSize, lineSize * 2, lineSize, lineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            aSpriteBatch.Draw(aBoarder, new Rectangle((int)aPosition.X + maximumTextBoxWidth, (int)aPosition.Y, lineSize, lineSize), new Rectangle((lineSize * 2), lineSize * 2, lineSize, lineSize), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);

        }

        public static void DrawRuleBox(string[] aStringArray, SpriteBatch aSpriteBatch)
        {
            int tempYOffset = (int)rulesPosition.Y - Rules_List.Scrolling(aStringArray);
            for (int i = 0; i < aStringArray.Length; i++)
            {
                if (Rules_List.AccessCurrentRule != Rules_List.GetList().Length)
                {
                    if (Rules_List.GetList()[Rules_List.AccessCurrentRule] == aStringArray[i])
                        DrawTextBox(aStringArray[i], new Vector2(rulesPosition.X, tempYOffset), ruleSelector, aSpriteBatch);
                    else
                        DrawTextBox(aStringArray[i], new Vector2(rulesPosition.X, tempYOffset), textBoarder, aSpriteBatch);
                }
                else
                    DrawTextBox(aStringArray[i], new Vector2(rulesPosition.X, tempYOffset), textBoarder, aSpriteBatch);

                List<string> tempTextBox = SeparateText(aStringArray[i]);
                tempYOffset += tempTextBox.Count * lineSize;
                tempYOffset += betweenLineSize;
            }

            if (tempYOffset > (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale) - lineSize - (betweenLineSize / 2))
            {
                tempYOffset = (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale) - lineSize - (betweenLineSize / 2);
            }

            if (Rules_List.AccessCurrentRule != Rules_List.GetList().Length)
                DrawTextBox("Back", new Vector2(rulesPosition.X, tempYOffset), textBoarder, aSpriteBatch);
            else
                DrawTextBox("Back", new Vector2(rulesPosition.X, tempYOffset), ruleSelector, aSpriteBatch);
        }

        public static void DrawTutorialBox(SpriteBatch aSpriteBatch)
        {
            if (tutorialText != "" && tutorialText != null)
                DrawTextBox(tutorialText, tutorialPosition, textBoarder, aSpriteBatch);
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
