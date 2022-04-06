using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cheatscape
{
    class Chess_Move
    {
        public Vector2 myStartingPos;
        public Vector2 myEndingPos;

        public Chess_Move(string aStart, string anEnd)
        {
            myStartingPos = DecryptPosition(aStart);
            myEndingPos = DecryptPosition(anEnd);

            //add so you can specify that a piece gets removed instead of moving
        }

        Vector2 DecryptPosition(string aPosition)
        {
            aPosition = aPosition.ToLower();
            int tempXPos = 0;
            int tempYPos = 8 - (aPosition[1] - '0');

            switch (aPosition[0])
            {
                case 'a':
                    tempXPos = 0;
                    break;
                case 'b':
                    tempXPos = 1;
                    break;
                case 'c':
                    tempXPos = 2;
                    break;
                case 'd':
                    tempXPos = 3;
                    break;
                case 'e':
                    tempXPos = 4;
                    break;
                case 'f':
                    tempXPos = 5;
                    break;
                case 'g':
                    tempXPos = 6;
                    break;
                case 'h':
                    tempXPos = 7;
                    break;
            }

            return new Vector2(tempXPos, tempYPos);
        }
    }
}
