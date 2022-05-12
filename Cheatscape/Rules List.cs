using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Cheatscape
{
    static class Rules_List
    {
        static SpriteFont font;
        static Texture2D banner;
        static Texture2D imageBoarder;

        static int currentRuleList = 0;
        static int currentRule = 0;
        public static int AccessCurrentRuleList { get => currentRuleList; set => currentRuleList = value; }
        public static int AccessCurrentRule { get => currentRule; set => currentRule = value; }

        public static int amountOfRuleLists = 3;
        static int lastRule;
        public static int scrollBarWidth = 20;
        static Vector2 imagePosition = new Vector2(5 + scrollBarWidth, 0);
        static Vector2 bannerPosition = new Vector2(scrollBarWidth, 101);

        public static List<Vector2> allowedRules = new List<Vector2>();

        static string[] generalRules = {
            "The white player always starts.",
            "The starting board state is shown to the right.",
            "Only one piece may be moved per turn (not including Castling).",
            "A piece can only be moved on its player's turn.",
            "A piece can only move to an unoccupied space or one occupied by an opposing piece.",
            "If a piece is moved to a space occupied by an opposing piece, the opposing piece is captured and removed from the game.",
            "If a Pawn reaches the other side of the board, it is replaced by a new queen, rook, bishop, or knight of the same color."};
        static string[] movementRules = {
            "The Pawn moves one space forward but on its first move it can move one or two spaces forward.",
            "The Pawn can only attack one space diagonally ahead and not straight forward.",
            "The Rook can move any number of spaces in a straight line vertically or horizontally.",
            "The Bishop can move any number of spaces in a straight line diagonally.",
            "The Knight can move in an L shape.",
            "The Queen can move any number of spaces in a straight line vertically, horizontally or diagonally.",
            "The King can move one space in any direction.",
            "The Knight is the only piece that can jump over other pieces"};
        static string[] extraRules = {
            "If the King is being threatened by an opposing piece, its player has to move a piece to secure the King. This is called Check.",
            "If the King is in Check and its player can't secure it on their turn, the opponent wins. This is called Checkmate.",
            "The a piece can't make a move that causes its King to be in Check.",
            "A tie happens when the same board state occurs 3 times or when both players have made 50 moves.",
            "If the King and a Rook have not moved yet, the King can move 2 spaces towards the Rook causing the Rook to move to the space next to the King on the opposite side. This is called Castling.",
            "Castling is not allowed if the King moves out of, through or into a space where it would be in Check."};

        public static void Load() //get font
        {
            font = Global_Info.AccessContentManager.Load<SpriteFont>("Font");
            banner = Global_Info.AccessContentManager.Load<Texture2D>("Rules Banner");
            imageBoarder = Global_Info.AccessContentManager.Load<Texture2D>("Rule Image Boarder");
        }

        public static void IncludeList(int aList)
        {
            string[] tempArray = GetList(aList);

            for (int i = 0; i < tempArray.Length; i++)
            {
                if (!allowedRules.Contains(new Vector2(aList, i)))
                {
                    allowedRules.Add(new Vector2(aList, i));
                }
            }
        }

        public static string[] GetList()
        {
            switch (currentRuleList)
            {
                default:
                    return generalRules;
                case 1:
                    return movementRules;
                case 2:
                    return extraRules;
            }
        }

        public static string[] GetList(int aList)
        {
            switch (aList)
            {
                default:
                    return generalRules;
                case 1:
                    return movementRules;
                case 2:
                    return extraRules;
            }
        }

        public static string[] GetAllowedRules(int aRuleList)
        {
            List<string> tempList = new List<string>(GetList());
            int tempRule = 0;

            for (int i = 0; i < tempList.Count; i++)
            {
                if (!allowedRules.Contains(new Vector2(aRuleList, tempRule)))
                {
                    tempList.RemoveAt(i);
                    i--;
                }
                tempRule++;
            }

            return tempList.ToArray();
        }

        public static void MoveThroughRules(int aMoveDirection = 0)
        {
            switch (aMoveDirection)
            {
                case 0: //Move Down
                    lastRule = currentRule;
                    if (currentRule <= GetList().Length)
                        currentRule++;
                    break;
                case 1: //Move Up
                    lastRule = currentRule;
                    if (currentRule >= 0)
                        currentRule--;
                    break;
                case 2: //Move Left
                    currentRuleList--;
                    if (currentRuleList < 0)
                        currentRuleList = amountOfRuleLists - 1;
                    currentRule = 0;
                    break;
                case 3: //Move Right
                    currentRuleList++;
                    if (currentRuleList >= amountOfRuleLists)
                        currentRuleList = 0;
                    currentRule = 0;
                    break;
            }
            SkipExcludedRules(aMoveDirection);
        }
        static void SkipExcludedRules(int aMoveDirection = 0)
        {
            if (aMoveDirection != 1)
            {
                if (GetAllowedRules(currentRuleList).Length <= 0)
                {
                    if (aMoveDirection >= 2)
                        MoveThroughRules(aMoveDirection);
                    else
                        MoveThroughRules(3);
                }
                else
                {
                    for (int i = 0; i < GetList().Length; i++)
                    {
                        if (!allowedRules.Contains(new Vector2(currentRuleList, currentRule)))
                        {
                            if (currentRule != GetList().Length)
                            {
                                if (currentRule <= GetList().Length)
                                    currentRule++;
                                else
                                    currentRule = lastRule;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < GetList().Length; i++)
                {
                    if (!allowedRules.Contains(new Vector2(currentRuleList, currentRule)))
                    {
                        if (currentRule > 0)
                            currentRule--;
                        else
                            currentRule = lastRule;
                    }
                }
            }
        }

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            currentRule--;
            MoveThroughRules(0);

            string[] tempArray = GetAllowedRules(currentRuleList);
            Text_Manager.DrawRuleBox(tempArray, aSpriteBatch);

            aSpriteBatch.Draw(banner, new Rectangle((int)bannerPosition.X, (int)bannerPosition.Y,
                Text_Manager.MaximumTextBoxWidth + (int)((Text_Manager.RulesPosition.X - scrollBarWidth) * 2), 20),
                new Rectangle(0, 0, Text_Manager.MaximumTextBoxWidth + (int)((Text_Manager.RulesPosition.X - scrollBarWidth) * 2), 20),
                Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);

            aSpriteBatch.Draw(banner, new Rectangle((int)bannerPosition.X, (int)bannerPosition.Y + 20,
                Text_Manager.MaximumTextBoxWidth + (int)((Text_Manager.RulesPosition.X - scrollBarWidth) * 2), 17),
                new Rectangle(0, (currentRuleList * 17) + 20, Text_Manager.MaximumTextBoxWidth + (int)((Text_Manager.RulesPosition.X - scrollBarWidth) * 2), 17),
                Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);

            if (currentRule < GetList().Length)
            {
                Texture2D tempRuleImage = Global_Info.AccessContentManager.Load<Texture2D>("Rule Images/" + currentRuleList + "-" + currentRule);
                aSpriteBatch.Draw(tempRuleImage, new Rectangle((int)imagePosition.X + 3, (int)imagePosition.Y + 3, 96, 96), Color.White);
                aSpriteBatch.Draw(imageBoarder, new Rectangle((int)imagePosition.X, (int)imagePosition.Y, 102, 102), Color.White);
            }
        }

        public static int Scrolling(string[] aStringArray)
        {
            int tempScrollAmount = ((int)Text_Manager.RulesPosition.Y / 2) - (int)(Global_Info.AccessWindowSize.Y / (2 * Global_Info.AccessScreenScale));

            for (int i = 0; i < aStringArray.Length; i++)
            {
                List<string> tempTextBox = Text_Manager.SeparateText(aStringArray[i]);

                if (currentRule == i)
                {
                    tempScrollAmount += ((Text_Manager.LineSize * tempTextBox.Count) + Text_Manager.BetweenLineSize) / 2;
                    break;
                }

                tempScrollAmount += Text_Manager.LineSize * tempTextBox.Count;
                tempScrollAmount += Text_Manager.BetweenLineSize;
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

        public static int MaxScroll(string[] aStringArray)
        {
            int tempMaxScroll = 0 - (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale);

            for (int i = 0; i < aStringArray.Length; i++)
            {
                List<string> tempTextBox = new List<string>();
                string[] tempWords = aStringArray[i].Split(' ');
                string tempLine = tempWords[0];

                for (int j = 1; j < tempWords.Length; j++)
                {
                    if (Text_Manager.MaximumTextBoxWidth >= font.MeasureString(tempLine + " " + tempWords[j]).X)
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

                tempMaxScroll += Text_Manager.LineSize * tempTextBox.Count;
                tempMaxScroll += Text_Manager.BetweenLineSize;
            }

            tempMaxScroll += Text_Manager.LineSize + Text_Manager.BetweenLineSize - 4;

            return tempMaxScroll + (int)Text_Manager.RulesPosition.Y - 2;
        }
    }
}
