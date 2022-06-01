using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cheatscape
{
    static class Game_Board
    {
        static Chess_Piece[,] ChessPiecesOnBoard = new Chess_Piece[8, 8];
        static List<Chess_Piece> CapturedWhitePieces = new List<Chess_Piece>();
        static List<Chess_Piece> CapturedBlackPieces = new List<Chess_Piece>();
        public static Vector2 BoardPosition = new Vector2(Global_Info.AccessWindowSize.X / 4 - 128, Global_Info.AccessWindowSize.Y / 4 - 128);
        static int TileSize = 32;
        static float timer;
        static float timeLimit = 90f;
        static int currentFrame;

        static Rectangle mapRectangle;
        public static Chess_Piece[,] AccessChessPiecesOnBoard { get => ChessPiecesOnBoard; set => ChessPiecesOnBoard = value; }
        public static Vector2 AccessBoardPosition { get => BoardPosition; set => BoardPosition = value; }
        public static int AccessTileSize { get => TileSize; set => TileSize = value; }

        public static List<Texture2D> MapPack = new List<Texture2D>();
        static Texture2D currentMap;

        static Rectangle SplashArtSize = new Rectangle(400, 0, 200, 100);

        static Texture2D ChessBoard;
        static Texture2D CheckArt;
        static Texture2D CheckmateArt;
        static Texture2D fogOfWarTex;
        static Texture2D SlideButtons;       

        enum SplashArt { None, Check, Checkmate }
        static SplashArt CurrentSplashArt = SplashArt.None;

        static int PausedMove;

        public static void Load()
        {
            Texture2D Kindergarten = Global_Info.AccessContentManager.Load<Texture2D>("Kindergarten");
            Texture2D Desk = Global_Info.AccessContentManager.Load<Texture2D>("Desk");
            Texture2D Park = Global_Info.AccessContentManager.Load<Texture2D>("Park");
            Texture2D Train = Global_Info.AccessContentManager.Load<Texture2D>("cheatscape Train");

            MapPack.Add(Kindergarten);
            MapPack.Add(Kindergarten);
            MapPack.Add(Desk);
            MapPack.Add(Park);
            MapPack.Add(Train);
            ChessBoard = Global_Info.AccessContentManager.Load<Texture2D>("Chess Board");          
            CheckArt = Global_Info.AccessContentManager.Load<Texture2D>("Check Art");
            CheckmateArt = Global_Info.AccessContentManager.Load<Texture2D>("Checkmate Art");
            fogOfWarTex = Global_Info.AccessContentManager.Load<Texture2D>("FogOfWar");
            SlideButtons = Global_Info.AccessContentManager.Load<Texture2D>("Slide Buttons");

            SetBasicBoardState();
        }

        public static void AdjustMap(int map)
        {
            try
            {
                currentMap = MapPack[map];
            }
            catch
            {
                currentMap = MapPack[0];
            }
        }
        public static void DrawMap(GameTime gameTime)
        {
            int MaxFrame = 4;
            mapRectangle = new Rectangle(0, 1080* currentFrame, 1920, 1080);
            if (currentMap == MapPack[4])
            {
                timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (timer >= timeLimit)
                {
                    currentFrame++;
                    timer = 0;
                    if (currentFrame == MaxFrame)
                    {
                        currentFrame = 0;
                    }
                }
            }
        }

        public static void ResetBoard()
        {
            CapturedWhitePieces.Clear();
            CapturedBlackPieces.Clear();
            Level_Manager.AccessCurrentSlide = 1;
            Rules_List.AccessCurrentRule = 0;
            Rules_List.AccessCurrentRuleList = 0;
            Text_Manager.TutorialText = null;
            Hand_Animation_Manager.ResetAllHands();
            SetBasicBoardState();

            for (int i = 0; i < Level_Manager.AccessAllMoves[0].Count; i++)
            {
                MakeAMove(Level_Manager.AccessAllMoves[0][i], true);
            }
        }

        public static void SetBasicBoardState()
        {
            Level_Manager.AccessAllAnswers.Clear();
            Rules_List.AllowedRules.Clear();

            switch (Level_Manager.AccessCurrentBundle)
            {
                default:

                    break;
            }

            for (int i = 0; i < Level_Manager.AccessAllMoves.Count; i++)
            {
                for (int j = 0; j < Level_Manager.AccessAllMoves[i].Count; j++)
                {
                    switch (Level_Manager.AccessAllMoves[i][j].MyMoveType)
                    {
                        case Chess_Move.MoveType.SpecialRule:
                            if (Level_Manager.CurrentLevel == 0)
                            {
                                Level_Transition.specialRule = Level_Manager.AccessAllMoves[i][j].myText.ToLower();
                            }                        
                            break;
                        case Chess_Move.MoveType.AnswerCheat:
                            Level_Manager.AccessAllAnswers.Add(new Tuple<Chess_Move, int>(Level_Manager.AccessAllMoves[i][j], i + 1));
                            break;
                        case Chess_Move.MoveType.IncludeRule:
                            if (!Rules_List.AllowedRules.Contains(Level_Manager.AccessAllMoves[i][j].myRule))
                                Rules_List.AllowedRules.Add(Level_Manager.AccessAllMoves[i][j].myRule);
                            break;
                        case Chess_Move.MoveType.IncludeList:
                            Rules_List.IncludeList(Level_Manager.AccessAllMoves[i][j].myRuleList);
                            break;
                        case Chess_Move.MoveType.ChessBoard:
                            switch (Level_Manager.AccessAllMoves[i][j].myText.ToLower())
                            {
                                default:
                                    ChessBoard = Global_Info.AccessContentManager.Load<Texture2D>("Chess Board");
                                    break;
                            }
                            break;
                    }
                }
            }

            if (Rules_List.AllowedRules.Count == 0)
            {
                for (int i = 0; i < Rules_List.AmountOfRuleLists; i++)
                {
                    Rules_List.IncludeList(i);
                }
            }

            for (int x = 0; x < ChessPiecesOnBoard.GetLength(0); x++)
            {
                for (int y = 0; y < ChessPiecesOnBoard.GetLength(1); y++)
                {
                    int tempPiece = 0;
                    bool tempIsWhite = true;

                    if (y <= 3)
                        tempIsWhite = false;
                    else
                        tempIsWhite = true;

                    switch (y)
                    {
                        case 0:
                        case 7:
                            switch (x)
                            {
                                case 0:
                                case 7:
                                    tempPiece = 2;
                                    break;
                                case 1:
                                case 6:
                                    tempPiece = 4;
                                    break;
                                case 2:
                                case 5:
                                    tempPiece = 3;
                                    break;
                                case 3:
                                    tempPiece = 5;
                                    break;
                                case 4:
                                    tempPiece = 6;
                                    break;
                            }
                            break;
                        case 1:
                        case 6:
                            tempPiece = 1;
                            break;
                        default:
                            tempPiece = 0;
                            break;
                    }

                    ChessPiecesOnBoard[x, y] = new Chess_Piece(tempPiece, tempIsWhite);
                }
            }
        }

        public static void CapturePiece(Vector2 aPosition)
        {
            if (ChessPiecesOnBoard[(int)aPosition.X, (int)aPosition.Y].myPieceType != 0)
            {
                if (ChessPiecesOnBoard[(int)aPosition.X, (int)aPosition.Y].isWhitePiece)
                    CapturedWhitePieces.Add(new Chess_Piece(ChessPiecesOnBoard[(int)aPosition.X, (int)aPosition.Y]));
                else if (!ChessPiecesOnBoard[(int)aPosition.X, (int)aPosition.Y].isWhitePiece)
                    CapturedBlackPieces.Add(new Chess_Piece(ChessPiecesOnBoard[(int)aPosition.X, (int)aPosition.Y]));
            }
        }

        public static void MakeAMove(Chess_Move aMove, bool isCurrentTurn)
        {
            if (isCurrentTurn)
                CurrentSplashArt = SplashArt.None;

            switch (aMove.MyMoveType)
            {
                case Chess_Move.MoveType.MovePiece:
                    if (isCurrentTurn)
                        Hand_Animation_Manager.GiveHandDirection(aMove);
                    else
                    {
                        CapturePiece(aMove.myEndingPos);
                        ChessPiecesOnBoard[(int)aMove.myEndingPos.X, (int)aMove.myEndingPos.Y].myPieceType = ChessPiecesOnBoard[(int)aMove.myStartingPos.X, (int)aMove.myStartingPos.Y].myPieceType;
                        ChessPiecesOnBoard[(int)aMove.myEndingPos.X, (int)aMove.myEndingPos.Y].isWhitePiece = ChessPiecesOnBoard[(int)aMove.myStartingPos.X, (int)aMove.myStartingPos.Y].isWhitePiece;
                        ChessPiecesOnBoard[(int)aMove.myStartingPos.X, (int)aMove.myStartingPos.Y].myPieceType = 0;
                    }
                    break;
                case Chess_Move.MoveType.AddPiece:
                    ChessPiecesOnBoard[(int)aMove.myStartingPos.X, (int)aMove.myStartingPos.Y] = new Chess_Piece(aMove.myPiece);
                    break;
                case Chess_Move.MoveType.RemovePiece:
                    ChessPiecesOnBoard[(int)aMove.myStartingPos.X, (int)aMove.myStartingPos.Y].myPieceType = 0;
                    break;
                case Chess_Move.MoveType.CapturePiece:
                    if (aMove.myPiece.isWhitePiece)
                        CapturedWhitePieces.Add(new Chess_Piece(aMove.myPiece));
                    else if (!aMove.myPiece.isWhitePiece)
                        CapturedBlackPieces.Add(new Chess_Piece(aMove.myPiece));
                    break;
                case Chess_Move.MoveType.AnswerCheat:
                    break;
                case Chess_Move.MoveType.TutorialText:
                    if (isCurrentTurn)
                        Text_Manager.TutorialText = aMove.myText;
                    else
                        Text_Manager.TutorialText = "";
                    break;
                case Chess_Move.MoveType.CallCheck:
                    if (isCurrentTurn)
                        CurrentSplashArt = SplashArt.Check;
                    break;
                case Chess_Move.MoveType.CallCheckmate:
                    if (isCurrentTurn)
                        CurrentSplashArt = SplashArt.Checkmate;
                    break;
            }
        }

        public static void SetBoardState()
        {
            SetBasicBoardState();
            CapturedWhitePieces.Clear();
            CapturedBlackPieces.Clear();
            PausedMove = 0;

            for (int i = 0; i < Level_Manager.AccessCurrentSlide - 1; i++)
            {
                for (int j = 0; j < Level_Manager.AccessAllMoves[i].Count; j++)
                {
                    MakeAMove(Level_Manager.AccessAllMoves[i][j], false);
                }
            }

            CurrentTurnMoves();
        }

        public static void CurrentTurnMoves()
        {
            for (int i = PausedMove; i < Level_Manager.AccessAllMoves[Level_Manager.AccessCurrentSlide - 1].Count; i++)
            {
                MakeAMove(Level_Manager.AccessAllMoves[Level_Manager.AccessCurrentSlide - 1][i], true);
                if (Level_Manager.AccessAllMoves[Level_Manager.AccessCurrentSlide - 1][i].MyMoveType == Chess_Move.MoveType.MovePiece)
                {
                    PausedMove = i + 1;
                    break;
                }
            }
        }

        //public static void GenerateFogOfWar(int distance, int mode)
        //{
        //    fogOfWar = new bool[8, 8];

        //    fogOfWarMode = mode;
        //    fogOfWarDistance = distance;

        //    if (mode == 1) //From the Left
        //    {
        //        for (int y = 0; y < 8; y++)
        //        {
        //            for (int x = 0; x < distance; x++)
        //            {
        //                fogOfWar[x, y] = true;
        //            }
        //        }
        //    }
        //    else if (mode == 2) //From the Right
        //    {
        //        for (int y = 0; y < 8; y++)
        //        {
        //            for (int x = 8 - distance; x < 8; x++)
        //            {
        //                fogOfWar[x, y] = true;
        //            }
        //        }
        //    }
        //    else if (mode == 3) //From the Top
        //    {
        //        for (int y = 0; y < distance; y++)
        //        {
        //            for (int x = 0; x < 8; x++)
        //            {
        //                fogOfWar[x, y] = true;
        //            }
        //        }
        //    }
        //    else if (mode == 4) //from the Bottom
        //    {
        //        for (int y = 8 - distance; y < 8; y++)
        //        {
        //            for (int x = 0; x < 8; x++)
        //            {
        //                fogOfWar[x, y] = true;
        //            }
        //        }
        //    }
        //}

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            if(currentMap == MapPack[4])
            aSpriteBatch.Draw(currentMap, new Rectangle(0, 0, (int)(Global_Info.AccessWindowSize.X / Global_Info.AccessScreenScale), (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale)), mapRectangle, Color.White);
            else
            aSpriteBatch.Draw(currentMap, new Rectangle(0, 0, (int)(Global_Info.AccessWindowSize.X / Global_Info.AccessScreenScale), (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale)), Color.White);
            Vector2 BoardOffset = new Vector2(BoardPosition.X - ((ChessBoard.Width - (TileSize * 8)) / 2), 
                BoardPosition.Y - ((ChessBoard.Height - (TileSize * 8)) / 2));

            aSpriteBatch.Draw(ChessBoard, BoardOffset, Color.White);

            for (int x = 0; x < ChessPiecesOnBoard.GetLength(0); x++)
            {
                for (int y = 0; y < ChessPiecesOnBoard.GetLength(1); y++)
                {
                    Vector2 tempPiecePos = new Vector2(BoardPosition.X + (x * TileSize), BoardPosition.Y + (y * TileSize));
                    ChessPiecesOnBoard[x, y].Draw(aSpriteBatch, tempPiecePos);
                }
            }

            for (int i = 0; i < CapturedWhitePieces.Count; i++)
            {
                Vector2 tempPiecePos = new Vector2(BoardOffset.X - TileSize, BoardOffset.Y + (i * (TileSize / 2)));
                CapturedWhitePieces[i].Draw(aSpriteBatch, tempPiecePos);
            }

            for (int i = CapturedBlackPieces.Count - 1; i >= 0; i--)
            {
                Vector2 tempPiecePos = new Vector2(BoardOffset.X + ChessBoard.Width,
                    BoardOffset.Y + (ChessBoard.Height - TileSize) - (i * (TileSize / 2)));
                CapturedBlackPieces[i].Draw(aSpriteBatch, tempPiecePos);
            }
            if (!Level_Manager.FindingCheat)
            {
                if (Level_Manager.AccessCurrentSlide > 1)
                    aSpriteBatch.Draw(SlideButtons, new Rectangle(113, 148, 31, 64), new Rectangle(0, 0, 31, 64), Color.White);
                if (Level_Manager.AccessCurrentSlide < Level_Manager.AccessAllMoves.Count)
                    aSpriteBatch.Draw(SlideButtons, new Rectangle(456, 148, 31, 64), new Rectangle(33, 0, 31, 64), Color.White);
            }

            if (CurrentSplashArt == SplashArt.Check)
                aSpriteBatch.Draw(CheckArt, SplashArtSize, Color.White);
            else if (CurrentSplashArt == SplashArt.Checkmate)
                aSpriteBatch.Draw(CheckmateArt, SplashArtSize, Color.White);
        }
    }
}
