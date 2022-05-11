using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cheatscape
{
    class Hand
    {
        Vector2 myMoveDirection;
        Vector2 myPosition;
        Vector2 myStartPos;
        Vector2 myEndPos;
        Vector2 myHomePos;
        Chess_Move myMove;
        bool isHolding = false;
        Texture2D myTexture;
        public bool isDone = true;
        int myMoveStage = 0;
        Chess_Piece myHoldingPiece;

        int myMoveSpeed = 35;
        int myMoveAmount = 0;

        bool myHandIsFlipped;

        public Hand(Vector2 aHomePos, bool aHandIsFlipped)
        {
            myTexture = Global_Info.AccessContentManager.Load<Texture2D>("Hands");
            myHomePos = aHomePos;            
            myPosition = myHomePos;
            myHandIsFlipped = aHandIsFlipped;
        }

        public void GainDirection(Chess_Move aMove)
        {
            myMoveStage = 0;
            isDone = false;
            myMove = aMove;

            myStartPos = new Vector2(Game_Board.AccessBoardPosition.X + (aMove.myStartingPos.X * Game_Board.AccessTileSize) - (Game_Board.AccessTileSize / 2),
                Game_Board.AccessBoardPosition.Y + (aMove.myStartingPos.Y * Game_Board.AccessTileSize) - (Game_Board.AccessTileSize / 2));

            myEndPos = new Vector2(Game_Board.AccessBoardPosition.X + (aMove.myEndingPos.X * Game_Board.AccessTileSize) - (Game_Board.AccessTileSize / 2),
                Game_Board.AccessBoardPosition.Y + (aMove.myEndingPos.Y * Game_Board.AccessTileSize) - (Game_Board.AccessTileSize / 2));

            CalculateDirection(myPosition, myStartPos);
        }

        public void ResetHand()
        {
            myPosition = myHomePos;
            isDone = true;
            isHolding = false;
            myMoveAmount = 0;
            myMoveStage = 0;
        }

        public void Update()
        {
            if (!isDone)
            {
                myPosition += myMoveDirection;
                myMoveAmount++;
                if (myMoveAmount == myMoveSpeed)
                {
                    myMoveAmount = 0;
                    myMoveStage++;

                    switch (myMoveStage)
                    {
                        case 1: //after arriving at the piece that should move
                            CalculateDirection(myStartPos, myEndPos);
                            isHolding = true;
                            myHoldingPiece = new Chess_Piece(Game_Board.AccessChessPiecesOnBoard[(int)myMove.myStartingPos.X, (int)myMove.myStartingPos.Y]);
                            Game_Board.AccessChessPiecesOnBoard[(int)myMove.myStartingPos.X, (int)myMove.myStartingPos.Y].myPieceType = 0;
                            

                            break;
                        case 2: //after dropping the piece in its new spot
                            CalculateDirection(myEndPos, myHomePos);
                            isHolding = false;
                            Music_Player.MoveEffect();

                            Game_Board.CapturePiece(myMove.myEndingPos);//add captured piece to the captured list
                            Game_Board.AccessChessPiecesOnBoard[(int)myMove.myEndingPos.X, (int)myMove.myEndingPos.Y] = new Chess_Piece(myHoldingPiece);
                            Game_Board.CurrentTurnMoves();
                            break;
                        case 3: //after arriving home
                            ResetHand();
                            break;
                    }
                }
            }
        }

        void CalculateDirection(Vector2 aStart, Vector2 anEnd)
        {
            myMoveDirection = new Vector2((anEnd.X - aStart.X) / myMoveSpeed, (anEnd.Y - aStart.Y) / myMoveSpeed);
        }

        public void Draw(SpriteBatch aSpriteBatch)
        {
            if (myHandIsFlipped == true)
            {
                if (isHolding)
                    aSpriteBatch.Draw(myTexture, new Rectangle((int)myPosition.X, (int)myPosition.Y, 64, 64), new Rectangle(0, 0, 64, 64), Color.White, 0, new Vector2(0, 0), SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally, 0);
                else
                    aSpriteBatch.Draw(myTexture, new Rectangle((int)myPosition.X, (int)myPosition.Y, 64, 64), new Rectangle(64, 0, 64, 64), Color.White, 0, new Vector2(0, 0), SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally, 0);
            }
            else if (myHandIsFlipped == false)
            {
                if (isHolding)
                    aSpriteBatch.Draw(myTexture, new Rectangle((int)myPosition.X, (int)myPosition.Y, 64, 64), new Rectangle(0, 0, 64, 64), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                else
                    aSpriteBatch.Draw(myTexture, new Rectangle((int)myPosition.X, (int)myPosition.Y, 64, 64), new Rectangle(64, 0, 64, 64), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            }
        }
    }
}
