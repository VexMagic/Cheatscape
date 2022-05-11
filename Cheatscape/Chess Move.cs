using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cheatscape
{
    class Chess_Move
    {
        public enum MoveType { movePiece, addPiece, removePiece, capturePiece, answerCheat, includeRule, includeList, tutorialText,
            callCheck, callCheckmate};
        public MoveType myMoveType;

        public Vector2 myStartingPos;
        public Vector2 myEndingPos;
        public Chess_Piece myPiece;
        public Vector2 myRule;
        public int myRuleList;
        public string myText;

        public Chess_Move(string[] anArray)
        {
            anArray[0] = anArray[0].ToLower();

            switch (anArray[0])
            {
                case "add":
                    myMoveType = MoveType.addPiece;
                    myPiece = DecryptPiece(anArray[1]);
                    myStartingPos = DecryptPosition(anArray[2]);
                    break;
                case "remove":
                    myMoveType = MoveType.removePiece;
                    myStartingPos = DecryptPosition(anArray[1]);
                    break;
                case "capture":
                    myMoveType = MoveType.capturePiece;
                    myPiece = DecryptPiece(anArray[1]);
                    break;
                case "move":
                    myMoveType = MoveType.movePiece;
                    myStartingPos = DecryptPosition(anArray[1]);
                    myEndingPos = DecryptPosition(anArray[2]);
                    break;
                case "answer":
                    myMoveType = MoveType.answerCheat;
                    myRule = DecryptRule(anArray[1]);
                    break;
                case "include rule":
                    myMoveType = MoveType.includeRule;
                    myRule = DecryptRule(anArray[1]);
                    break;
                case "include list":
                    myMoveType = MoveType.includeList;
                    myRuleList = Int32.Parse(anArray[1]);
                    break;
                case "text":
                    myMoveType = MoveType.tutorialText;
                    myText = anArray[1];
                    break;
                case "check":
                    myMoveType = MoveType.callCheck;
                    break;
                case "checkmate":
                    myMoveType = MoveType.callCheckmate;
                    break;
            }
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

        Chess_Piece DecryptPiece(string aPiece)
        {
            aPiece = aPiece.ToLower();

            int tempPieceType = 0;
            bool tempIsWhite = true;

            switch (aPiece[0])
            {
                case 'w':
                    tempIsWhite = true;
                    break;
                case 'b':
                    tempIsWhite = false;
                    break;
            }

            switch (aPiece[1])
            {
                case 'p':
                    tempPieceType = 1;
                    break;
                case 'r':
                    tempPieceType = 2;
                    break;
                case 'b':
                    tempPieceType = 3;
                    break;
                case 'n':
                    tempPieceType = 4;
                    break;
                case 'q':
                    tempPieceType = 5;
                    break;
                case 'k':
                    tempPieceType = 6;
                    break;
            }

            return new Chess_Piece(tempPieceType, tempIsWhite);
        }

        Vector2 DecryptRule(string aRule)
        {
            int tempRuleList = aRule[0] - '0';
            int tempRule = aRule[2] - '0';

            return new Vector2(tempRuleList, tempRule);
        }
    }
}
