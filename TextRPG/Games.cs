﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Collections;
using System.Globalization;

namespace TextRpg01
{
    
    public class Games
    {
        public const int BOARDX = 31;
        public const int BOARDY = 31;
        public int[,] gameboard = new int[BOARDX, BOARDY];
        // 몬스터 위치 저장하는 배열
        public int[,] gamemonsterboard = new int[BOARDX, BOARDY];


        public bool isGameOver;
        public bool TitleCheck;
        public bool OccupationCheck;
        public bool StatusCheck;
        public bool ChoiceCheck;
        public bool VillageCheckout = false;
        public bool AdventureCheckout = false;
        public bool AdventureTreeCheckout = false;
        public bool PlayeritemCheckout;
        public bool StoreCheckout;
        public bool CaveCheckout;

        // 전투가 끝났는지 확인
        public bool BattleCheck = false;

        // 전투나 도망가기 선택하는지 확인
        public bool BattleONCheck;

        // 피격 확인
        public bool BattleHitChk;
        public bool BattleHitplayerChk;
        public bool Battlemonsterturn;


        public int ArrowY;
        public int ArrowX;
        public int CheckStack = 0;
        public string PlayerName = string.Empty;
        public string PlayerOccupation = string.Empty;
        public int SavePlayerY;
        public int SavePlayerX;


        public int[] StatusValues = new int[6];
        // 전투 체력 / 공격력 / 방어력 / 진짜 체력 순
        public int[] PlayerBattleVal = new int[4] { 0, 0, 0, 5};
        
        public int[] MonsterBattleVal = new int[3];

        // 플레이어 인벤토리 문자들
        public List<string> Playeritem = new List<string>();
        // 돈 / 체력포션 / 산 잡몹 / 산 보스 몹 / 동굴 잡몹 / 동굴 보스 몹 /                     0~ 5개
        // 전사 무기 1 / 전사 갑옷 1 / 전사 장식 1 / 전사 무기 2 / 전사 갑옷 2 / 전사 장식 2 값    6~11개
        // 마법사 무기 1 / 마법사 갑옷 1 / 마법사 장식 1 / 마법사 무기 2 / 마법사 갑옷 2 / 마법사 장식 2 값    12~17개
        // 도적 무기 1 / 도적 갑옷 1 / 도적 장식 1 / 도적 무기 2 / 도적 갑옷 2 / 도적 장식 2 값    17~22개

        public int[] PlayeritemVal = new int[23];
        public Dictionary<string, string> playeritemsIndex = new Dictionary<string, string>();

        // 플레이어 장비 유무
        public string[] PlayerWearWeps = new string[3];

        // 상점 물품
        public List<string> StoreVal = new List<string>();




        Random random = new Random();
        Monster monster = new Monster();
        Weps weps = new Weps();

        public Games()
        {
            
            GameSet();

            StoreWepSet();
            GamePlay();
        }

        // 전 게임 보드 값 지워버리는 함수
        public void GameboardClear()
        {
            for (int y = 0; y < BOARDY; y++)
            {
                for (int x = 0; x < BOARDX; x++)
                {

                    gameboard[y, x] = 0;


                }
            }
        } // GameboardClear()
        public void GameSet()
        {
            
            for(int y = 0; y < BOARDY; y++)
            {
                for(int x = 0; x < BOARDX; x++)
                {
                    
                    if (x == 1)
                    {
                        gameboard[y, x] = -1;

                    }

                    
                    // 테두리
                    if (x == 0 || x == 30 || y == 0 || y == 30)
                    {
                        gameboard[y, x] = -2;

                    }


                    // 제목 좌표
                    if (x == 1 && y == 8)
                    {
                        gameboard[y, x] = -3;

                    }

                    

                    // 시작하기 좌표
                    if (x == 1 && y == 22)
                    {
                        gameboard[y, x] = 2;

                    }

                    if (x==2 && y == 22)
                    {
                        gameboard[y, x] = -5;

                    }




                    // 종료하기 좌표
                    if (x == 1 && y == 24)
                    {
                        gameboard[y, x] = 3;

                    }

                    if (x == 2 && y == 24)
                    {
                        gameboard[y, x] = -6;

                    }

                    
                }
            }

           
        }   // GameSet()


        public void GamePlay()
        {
            isGameOver = false;
            TitleCheck = false;
            OccupationCheck = false;
            StatusCheck = false;


            while (isGameOver == false)
            {
                while (TitleCheck == false)
                {
                    Console.SetCursorPosition(0, 0);
                    Title();


                }
                if (isGameOver == true)
                {
                    break;
                }


                // 이름 입력 받기
                GameNameSet();

                Console.SetCursorPosition(0, 0);
                GameName();

                // 직업 선택 좌표 설정
                OccupationSet();

                // 직업 선택
                while (OccupationCheck == false)
                {
                    Console.SetCursorPosition(0, 0);
                    Occupation();
                }

                // 스탯 선택 좌표 설정

                StatusSet();

                while (StatusCheck == false)
                {
                    Console.SetCursorPosition(0, 0);
                    Status();
                }


                PlayerBattleVal[0] = 5;

                if (PlayerOccupation == "전사")
                {
                    PlayerBattleVal[1] = (int)(StatusValues[0] + ((StatusValues[2] + StatusValues[4]) / 2));
                    PlayerBattleVal[2] = (int)(StatusValues[1] + ((StatusValues[3] + StatusValues[5]) / 2));
                }
                else if (PlayerOccupation == "마법사")
                {
                    PlayerBattleVal[1] = (int)(StatusValues[2] + ((StatusValues[0] + StatusValues[4]) / 2));
                    PlayerBattleVal[2] = (int)(StatusValues[3] + ((StatusValues[1] + StatusValues[5]) / 2));
                }
                else if (PlayerOccupation == "도적")
                {
                    PlayerBattleVal[1] = (int)(StatusValues[4] + ((StatusValues[0] + StatusValues[2]) / 2));
                    PlayerBattleVal[2] = (int)(StatusValues[5] + ((StatusValues[1] + StatusValues[3]) / 2));
                }



                PlayeritemVal[0] = 300;


                //마을, 모험, 아이템, 끝내기 선택지
                ChoiceActSet();

                while (ChoiceCheck == false)
                {
                    Console.SetCursorPosition(0, 0);
                    ChoiceAct();

                }





            }


        } // GamePlay()


        

        // 타이틀 화면 구현 함수
        public void Title()
        {
            for (int y = 0; y < BOARDY; y++)
            {
                for (int x = 0; x < BOARDX; x++)
                {
                    switch (gameboard[y, x])
                    {

                        case -6:
                            Console.Write("종료 하기".PadLeft(8, ' ') + "".PadRight(38, ' '));

                            break;
                        case -5:
                            Console.Write("시작 하기".PadLeft(8, ' ') + "".PadRight(38, ' '));
                            break;
                        case -3:
                            Console.Write("[제목 테스트]".PadLeft(45, ' ') + "".PadRight(37, ' '));
                            break;

                        case -2:
                            Console.Write("■".PadRight(2, ' '));
                            break;

                        case -1:
                            Console.Write("".PadLeft(87, ' '));
                            break;

                        case 2:
                            Console.Write("▶".PadLeft(36, ' '));
                            break;
                        case 3:
                            Console.Write(" ".PadLeft(37, ' '));
                            break;


                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            
            ConsoleKeyInfo Move;

            Move = Console.ReadKey();

            switch (Move.Key)
            {
                case ConsoleKey.W:
                    gameboard[22, 1] = 2;
                    gameboard[24, 1] = 3;

                    break;

                case ConsoleKey.S:
                    gameboard[22, 1] = 3;
                    gameboard[24, 1] = 2;

                    break;

                case ConsoleKey.UpArrow:
                    gameboard[22, 1] = 2;
                    gameboard[24, 1] = 3;

                    break;
                case ConsoleKey.DownArrow:
                    gameboard[22, 1] = 3;
                    gameboard[24, 1] = 2;

                    break;

                default:

                    if (gameboard[22, 1] == 2)
                    {
                        // 게임 시작
                        TitleCheck = true;
                    }
                    else
                    {
                        // 게임 종료
                        TitleCheck = true;
                        isGameOver = true;
                    }
                    break;

            }
        }

        // 이름 정하는 좌표 구현
        public void GameNameSet()
        {
            for (int y = 0; y < BOARDY; y++)
            {
                for (int x = 0; x < BOARDX; x++)
                {

                    if (x == 1)
                    {
                        gameboard[y, x] = -1;

                    }


                    // 테두리
                    if (x == 0 || x == 30 || y == 0 || y == 30)
                    {
                        gameboard[y, x] = -2;

                    }


                    // 제목 좌표
                    if (x == 1 && y == 8)
                    {
                        gameboard[y, x] = -3;

                    }



                }
            }


        } // GameNameSet()


        // 이름 정하는 함수 구현
        public void GameName()
        {
            for (int y = 0; y < BOARDY; y++)
            {
                for (int x = 0; x < BOARDX; x++)
                {
                    switch (gameboard[y, x])
                    {

                        
                        case -3:
                            Console.Write("[이름을 입력해주세요]".PadLeft(45, ' ') + "".PadRight(33, ' '));
                            break;

                        case -2:
                            Console.Write("■".PadRight(2, ' '));
                            break;

                        case -1:
                            Console.Write("".PadLeft(87, ' '));
                            break;


                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            Console.SetCursorPosition(43, 12);
            PlayerName = Console.ReadLine();

        } // GameName()


        // 직업 선택 좌표 구현

        public void OccupationSet()
        {
            for (int y = 0; y < BOARDY; y++)
            {
                for (int x = 0; x < BOARDX; x++)
                {
                    gameboard[y, x] = 0;

                    if (x == 1)
                    {
                        gameboard[y, x] = -1;

                    }


                    // 테두리
                    if (x == 0 || x == 30 || y == 0 || y == 30)
                    {
                        gameboard[y, x] = -2;

                    }


                    // 제목 좌표
                    if (x == 1 && y == 8)
                    {
                        gameboard[y, x] = -3;

                    }

                    // 화살표 좌표
                    if (x == 1 && y == 12)
                    {
                        gameboard[y, x] = 2;

                    }

                    // 선택지 앞 공백 좌표
                    if ((x == 1 && y == 16) || (x == 1 && y == 20) || (x == 1 && y == 24))
                    {
                        gameboard[y, x] = 3;

                    }

                    // 전사 좌표
                    if (x == 2 && y == 12)
                    {
                        gameboard[y, x] = 4;

                    }


                    // 마법사 좌표
                    if (x == 2 && y == 16)
                    {
                        gameboard[y, x] = 5;

                    }


                    // 도적 좌표
                    if (x == 2 && y == 20)
                    {
                        gameboard[y, x] = 6;

                    }

                    // 랜덤 선택 좌표
                    if (x == 2 && y == 24)
                    {
                        gameboard[y, x] = 7;

                    }


                }
            }

            // 화살표 좌표 저장
            ArrowY = 12;
            ArrowX = 1;

        } // OccupationSet()

        // 직업 선택 구현 함수
        public void Occupation()
        {
            
            for (int y = 0; y < BOARDY; y++)
            {
                for (int x = 0; x < BOARDX; x++)
                {
                    switch (gameboard[y, x])
                    {

                        case -3:
                            Console.Write("[직업을 선택해주세요]".PadLeft(45, ' ') + "".PadRight(33, ' '));
                            break;

                        case -2:
                            Console.Write("■".PadRight(2, ' '));
                            break;

                        case -1:
                            Console.Write("".PadLeft(87, ' '));
                            break;

                        case 2:
                            Console.Write("▶".PadLeft(34, ' '));
                            break;
                        case 3:
                            Console.Write(" ".PadLeft(35, ' '));
                            break;
                        case 4:
                            Console.Write("전사".PadLeft(8, ' ') + "".PadRight(42, ' '));
                            break;
                        case 5:
                            Console.Write("마법사".PadLeft(8, ' ') + "".PadRight(41, ' '));
                            break;
                        case 6:
                            Console.Write("도적".PadLeft(8, ' ') + "".PadRight(42, ' '));
                            break;
                        case 7:
                            Console.Write("랜덤 선택".PadLeft(8, ' ') + "".PadRight(40, ' '));
                            break;

                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();


            ConsoleKeyInfo Move;

            Move = Console.ReadKey();

            switch (Move.Key)
            {
                case ConsoleKey.W:
                    if (gameboard[12, 1] == 2) { /* Do Nothing */ }
                    else
                    {
                        gameboard[ArrowY, 1] = 3;

                        ArrowY -= 4;
                        gameboard[ArrowY, 1] = 2;
                    }

                    break;

                case ConsoleKey.S:
                    
                    if (gameboard[24, 1] == 2) { /* Do Nothing */ }
                    else
                    {
                        gameboard[ArrowY, 1] = 3;

                        ArrowY += 4;
                        gameboard[ArrowY, 1] = 2;
                    }
                    break;

                case ConsoleKey.UpArrow:
                    if (gameboard[12, 1] == 2) { /* Do Nothing */ }
                    else
                    {
                        gameboard[ArrowY, 1] = 3;

                        ArrowY -= 4;
                        gameboard[ArrowY, 1] = 2;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (gameboard[24, 1] == 2) { /* Do Nothing */ }
                    else
                    {
                        gameboard[ArrowY, 1] = 3;

                        ArrowY += 4;
                        gameboard[ArrowY, 1] = 2;
                    }
                    break;

                default:

                    if (gameboard[12, 1] == 2)
                    {
                        // 전사 선택
                        PlayerOccupation = "전사";
                        PlayerWearWeps[0] = "기본용 양손검";
                        PlayeritemVal[6]++;

                        PlayerWearWeps[1] = "기본용 갑옷";
                        PlayeritemVal[7]++;

                        PlayerWearWeps[2] = "기본용 전사의 가호";
                        PlayeritemVal[8]++;


                        OccupationCheck = true;

                    }
                    else if (gameboard[16, 1] == 2)
                    {
                        // 마법사 선택
                        PlayerOccupation = "마법사";
                        PlayerWearWeps[0] = "기본용 마법봉";
                        PlayeritemVal[12]++;

                        PlayerWearWeps[1] = "기본용 마법복";
                        PlayeritemVal[13]++;

                        PlayerWearWeps[2] = "기본용 마법사의 가호";
                        PlayeritemVal[14]++;

                        OccupationCheck = true;


                    }
                    else if (gameboard[20, 1] == 2)
                    {
                        // 도적 선택
                        PlayerOccupation = "도적";
                        PlayerWearWeps[0] = "기본용 단검";
                        PlayeritemVal[18]++;

                        PlayerWearWeps[1] = "기본용 복장";
                        PlayeritemVal[19]++;

                        PlayerWearWeps[2] = "기본용 도적의 가호";
                        PlayeritemVal[20]++;

                        OccupationCheck = true;


                    }
                    else
                    {
                        // 랜덤 선택
                        
                        int RandomOcc = random.Next(1, 3 + 1);

                        switch (RandomOcc)
                        {
                            case 1:
                                PlayerOccupation = "전사";
                                PlayerWearWeps[0] = "기본용 양손검";
                                PlayeritemVal[6]++;

                                PlayerWearWeps[1] = "기본용 갑옷";
                                PlayeritemVal[7]++;

                                PlayerWearWeps[2] = "기본용 전사의 가호";
                                PlayeritemVal[8]++;
                                break;
                            case 2:
                                PlayerOccupation = "마법사";
                                PlayerWearWeps[0] = "기본용 마법봉";
                                PlayeritemVal[12]++;

                                PlayerWearWeps[1] = "기본용 마법복";
                                PlayeritemVal[13]++;

                                PlayerWearWeps[2] = "기본용 마법사의 가호";
                                PlayeritemVal[14]++;
                                break;
                            case 3:
                                PlayerOccupation = "도적";
                                PlayerWearWeps[0] = "기본용 단검";
                                PlayerWearWeps[1] = "기본용 복장";
                                PlayerWearWeps[2] = "기본용 도적의 가호";
                                break;
                        }
                        OccupationCheck = true;

                    }
                    break;

            }



        } // Occupation()

        // 스탯 선택 좌표 구현
        public void StatusSet()
        {
            StatusValues = new int[7] { 5, 5, 5, 5, 5, 5, 10 };

            for (int y = 0; y < BOARDY; y++)
            {
                for (int x = 0; x < BOARDX; x++)
                {
                    gameboard[y, x] = 0;

                    if (x == 1)
                    {
                        gameboard[y, x] = -1;

                    }


                    // 테두리
                    if (x == 0 || x == 30 || y == 0 || y == 30)
                    {
                        gameboard[y, x] = -2;

                    }


                    // 제목 좌표
                    if (x == 1 && y == 8)
                    {
                        gameboard[y, x] = -3;

                    }

                    // 왼쪽 화살표 좌표
                    if (x == 1 && y == 12)
                    {
                        gameboard[y, x] = 2;

                    }



                    // 스탯 앞 공백 좌표

                    if (x == 1 && y == 15)
                    {
                        gameboard[y, x] = 5;
                    }
                    if (x == 1 && y == 18)
                    {
                        gameboard[y, x] = 5;

                    }


                    // 스탯 오른쪽 공백 좌표
                    if ((x == 3 && y == 12) || (x == 3 && y == 15) || (x == 3 && y == 18))
                    {
                        gameboard[y, x] = -5;

                    }

                    // 남은 스탯 및 랜덤 선택 앞 공백 좌표

                    if ((x == 1 && y == 21) || (x == 1 && y == 24) || (x == 1 && y == 27))
                    {
                        gameboard[y, x] = 3;

                    }

                    // 스탯 좌표 

                    // 힘 좌표
                    if (x == 2 && y == 12)
                    {
                        gameboard[y, x] = 6;

                    }

                    // 체력 좌표
                    if (x == 4 && y == 12)
                    {
                        gameboard[y, x] = 7;

                    }

                    // 마법 좌표
                    if (x == 2 && y == 15)
                    {
                        gameboard[y, x] = 8;

                    }

                    // 지혜 좌표
                    if (x == 4 && y == 15)
                    {
                        gameboard[y, x] = 9;

                    }

                    // 행운 좌표
                    if (x == 2 && y == 18)
                    {
                        gameboard[y, x] = 10;

                    }

                    // 회피 좌표
                    if (x == 4 && y == 18)
                    {
                        gameboard[y, x] = 11;

                    }

                    // 남은 스탯 표기 좌표
                    if (x == 2 && y == 21)
                    {
                        gameboard[y, x] = 12;

                    }


                    // 랜덤 선택 좌표
                    if (x == 2 && y == 24)
                    {
                        gameboard[y, x] = 13;

                    }

                    // 스텟 초기화 좌표
                    if (x == 2 && y == 27)
                    {
                        gameboard[y, x] = 14;

                    }


                }
            }

            // 화살표 좌표 저장
            ArrowY = 12;
            ArrowX = 1;

            
        } // StatusSet()

        // 스탯 선택 구현 함수
        public void Status()
        {
                       
            
            for (int y = 0; y < BOARDY; y++)
            {
                for (int x = 0; x < BOARDX; x++)
                {
                    switch (gameboard[y, x])
                    {

                        case -3:
                            Console.Write("[스텟을 입력해주세요]".PadLeft(45, ' ') + "".PadRight(33, ' '));
                            break;

                        case -2:
                            Console.Write("■".PadRight(2, ' '));
                            break;

                        case -1:
                            Console.Write("".PadLeft(87, ' '));
                            break;

                        case 2:
                            Console.Write("▶".PadLeft(29, ' '));
                            break;
                        case 3:
                            Console.Write(" ".PadLeft(35, ' '));
                            break;

                        // 오른쪽 스탯 화살표
                        case 4:
                            Console.Write("▶".PadLeft(9, ' '));
                            break;
                        case 5:
                            Console.Write("".PadLeft(30, ' '));
                            break;
                        case -5:
                            Console.Write("".PadLeft(10, ' '));
                            break;
                        case 6:
                            if(10 <= StatusValues[0])
                            {
                                Console.Write("힘 : {0}".PadLeft(9, ' '), StatusValues[0]);
                            }
                            else
                            {
                                Console.Write("힘 : {0}".PadLeft(10, ' '), StatusValues[0]);

                            }
                            break;
                        case 7:
                            if(10 <= StatusValues[1])
                            {
                                Console.Write("체력 : ".PadLeft(6, ' ') + "{0}".PadRight(31, ' '), StatusValues[1]);

                            }
                            else
                            {
                                Console.Write("체력 : ".PadLeft(6, ' ')+"{0}".PadRight(32, ' '), StatusValues[1]);
                            }
                            break;
                        case 8:
                            if (10 <= StatusValues[2])
                            {
                                Console.Write(" 마법 : {0}".PadLeft(9, ' '), StatusValues[2]);

                            }
                            else
                            {
                                Console.Write(" 마법 : {0}".PadLeft(10, ' '), StatusValues[2]);

                            }
                            break;
                        case 9:
                            if(10 <= StatusValues[3])
                            {
                                Console.Write("지혜 : {0}".PadRight(36, ' '), StatusValues[3]);
                                                                
                            }
                            else
                            {
                                Console.Write("지혜 : {0}".PadRight(37, ' '), StatusValues[3]);
                            }
                            break;
                        case 10:
                            if (10 <= StatusValues[4])
                            {
                                Console.Write("행운 : {0}".PadLeft(9, ' '), StatusValues[4]);

                            }
                            else
                            {
                                Console.Write("행운 : {0}".PadLeft(10, ' '), StatusValues[4]);

                            }

                            break;
                        case 11:
                            if(10 <= StatusValues[5] )
                            {
                                Console.Write("회피 : {0}".PadRight(36, ' '), StatusValues[5]);

                                
                            }
                            else
                            {
                                Console.Write("회피 : {0}".PadRight(37, ' '), StatusValues[5]);

                            }
                            break;
                        case 12:
                            if (StatusValues[6] < 10)
                            {
                                Console.Write("남은 스탯 : {0}".PadLeft(13, ' ') + "".PadRight(37, ' '), StatusValues[6]);

                            }
                            else
                            {
                                Console.Write("남은 스탯 : {0}".PadLeft(13, ' ') + "".PadRight(36, ' '), StatusValues[6]);
                            }
                            break;

                        case 13:
                            Console.Write("랜덤 선택".PadLeft(9, ' ') + "".PadRight(39, ' '));
                            break;
                        case 14:
                            Console.Write("초기화".PadLeft(8, ' ') + "".PadRight(41, ' '));
                            break;
                        case 15:
                            Console.Write("▶".PadLeft(34, ' '));
                            break;
                        
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();


            ConsoleKeyInfo Move;

            Move = Console.ReadKey();

            switch (Move.Key)
            {
                case ConsoleKey.W:
                    if (ArrowY == 12) { /* Do Nothing */ }
                    else
                    {
                        if (ArrowX == 1)
                        {
                            if (ArrowY < 21)
                            {
                                                                
                                gameboard[ArrowY, 1] = 5;
                                ArrowY -= 3;
                                gameboard[ArrowY, 1] = 2;

                            }

                            else if (ArrowY == 21)
                            {
                                gameboard[ArrowY, 1] = 3;
                                ArrowY -= 3;
                                gameboard[ArrowY, 1] = 2;
                            }
                            else
                            {
                                gameboard[ArrowY, 1] = 3;
                                ArrowY -= 3;
                                gameboard[ArrowY, 1] = 15;
                            }

                        }

                        else
                        {
                            gameboard[ArrowY, ArrowX] = -5;

                            ArrowY -= 3;
                            gameboard[ArrowY, ArrowX] = 4;
                            
                        }
                    }

                    break;
                case ConsoleKey.A:
                    if(ArrowX == 1) { /* Do Nothing */ }
                    else
                    {
                        gameboard[ArrowY, ArrowX] = -5;

                        ArrowX -= 2;
                        gameboard[ArrowY, ArrowX] = 2;
                    }
                    break;

                case ConsoleKey.S:
                    if (ArrowY == 27) { /* Do Nothing */ }
                    else
                    {
                        if (ArrowX == 1)
                        {
                            if ( ArrowY < 18)
                            {
                                gameboard[ArrowY, 1] = 5;
                                ArrowY += 3;
                                gameboard[ArrowY, 1] = 2;

                            }

                            else if (ArrowY == 18)
                            {
                                gameboard[ArrowY, 1] = 5;
                                ArrowY += 3;
                                gameboard[ArrowY, 1] = 15;
                            }
                            else
                            {
                                gameboard[ArrowY, 1] = 3;
                                ArrowY += 3;
                                gameboard[ArrowY, 1] = 15;
                            }

                        }

                        else
                        {
                            if (ArrowY == 18)
                            {
                                gameboard[ArrowY, ArrowX] = -5;
                                ArrowY += 3;
                                ArrowX -= 2;
                                gameboard[ArrowY, ArrowX] = 15;
                            }
                            else
                            {
                                gameboard[ArrowY, ArrowX] = -5;

                                ArrowY += 3;
                                gameboard[ArrowY, ArrowX] = 4;
                            }
                        }
                    }


                    break;
                case ConsoleKey.D:
                    if (ArrowX == 3) { /* Do Nothing */ }
                    else
                    {
                        if (21 <= ArrowY) { /* Do Nothing */ }
                        else
                        {

                            gameboard[ArrowY, ArrowX] = 5;

                            ArrowX += 2;
                            gameboard[ArrowY, ArrowX] = 4;
                        }
                    }
                    break;

                case ConsoleKey.UpArrow:
                    if (ArrowY == 12) { /* Do Nothing */ }
                    else
                    {
                        if (ArrowX == 1)
                        {
                            if (ArrowY < 21)
                            {
                                gameboard[ArrowY, 1] = 5;
                                ArrowY -= 3;
                                gameboard[ArrowY, 1] = 2;

                            }

                            else if (ArrowY == 21)
                            {
                                gameboard[ArrowY, 1] = 3;
                                ArrowY -= 3;
                                gameboard[ArrowY, 1] = 2;
                            }
                            else
                            {
                                gameboard[ArrowY, 1] = 3;
                                ArrowY -= 3;
                                gameboard[ArrowY, 1] = 15;
                            }

                        }

                        else
                        {
                            gameboard[ArrowY, ArrowX] = -5;

                            ArrowY -= 3;
                            gameboard[ArrowY, ArrowX] = 4;

                        }
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    if (ArrowX == 1) { /* Do Nothing */ }
                    else
                    {
                        gameboard[ArrowY, ArrowX] = -5;

                        ArrowX -= 2;
                        gameboard[ArrowY, ArrowX] = 2;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (ArrowY == 27) { /* Do Nothing */ }
                    else
                    {
                        if (ArrowX == 1)
                        {
                            if (ArrowY < 18)
                            {
                                gameboard[ArrowY, 1] = 5;
                                ArrowY += 3;
                                gameboard[ArrowY, 1] = 2;

                            }

                            else if (ArrowY == 18)
                            {
                                gameboard[ArrowY, 1] = 5;
                                ArrowY += 3;
                                gameboard[ArrowY, 1] = 15;
                            }
                            else
                            {
                                gameboard[ArrowY, 1] = 3;
                                ArrowY += 3;
                                gameboard[ArrowY, 1] = 15;
                            }

                        }

                        else
                        {
                            if (ArrowY == 18)
                            {
                                gameboard[ArrowY, ArrowX] = -5;
                                ArrowY += 3;
                                ArrowX -= 2;
                                gameboard[ArrowY, ArrowX] = 15;
                            }
                            else
                            {
                                gameboard[ArrowY, ArrowX] = -5;

                                ArrowY += 3;
                                gameboard[ArrowY, ArrowX] = 4;
                            }
                        }
                    }
                    break;

                case ConsoleKey.RightArrow:
                    if (ArrowX == 3) { /* Do Nothing */ }
                    else
                    {
                        if (21 <= ArrowY) { /* Do Nothing */ }
                        else
                        {

                            gameboard[ArrowY, ArrowX] = 5;

                            ArrowX += 2;
                            gameboard[ArrowY, ArrowX] = 4;
                        }
                    }
                    break;
                default:

                    if (gameboard[12, 1] == 2)
                    {
                        // 힘 스텟 선택
                        StatusValues[0]++;
                        CheckStack++;
                        StatusValues[6]--;
                    }
                    else if (gameboard[12, 3] == 4)
                    {
                        // 체력 선택
                        StatusValues[1]++;
                        CheckStack++;
                        StatusValues[6]--;


                    }
                    else if (gameboard[15, 1] == 2)
                    {
                        // 마법 선택
                        StatusValues[2]++;
                        CheckStack++;
                        StatusValues[6]--;

                    }
                    else if (gameboard[15, 3] == 4)
                    {
                        // 지혜 선택
                        StatusValues[3]++;
                        CheckStack++;
                        StatusValues[6]--;

                    }
                    else if (gameboard[18, 1] == 2)
                    {
                        // 행운 선택
                        StatusValues[4]++;
                        CheckStack++;
                        StatusValues[6]--;

                    }
                    else if (gameboard[18, 3] == 4)
                    {
                        // 회피 선택
                        StatusValues[5]++;
                        CheckStack++;
                        StatusValues[6]--;

                    }

                    else if (gameboard[20, 1] == 2)
                    {
                        // 도적 선택
                        PlayerOccupation = "도적";
                        OccupationCheck = true;


                    }
                    // 랜덤 선택
                    else if (gameboard[24, 1] == 15)
                    {
                        // 랜덤 선택
                        Random random = new Random();

                       
                        for (int i = 0; i < StatusValues[6]; i++)
                        {
                            int RandomOcc = random.Next(1, 6 + 1);
                            switch (RandomOcc)
                            {
                                case 1:
                                    StatusValues[0]++;
                                    break;
                                case 2:
                                    StatusValues[1]++;
                                    break;
                                case 3:
                                    StatusValues[2]++;
                                    break;
                                case 4:
                                    StatusValues[3]++;
                                    break;
                                case 5:
                                    StatusValues[4]++;
                                    break;
                                case 6:
                                    StatusValues[5]++;
                                    break;
                            }
                        }
                        StatusCheck = true;
                    }
                    // 초기화
                    else if (gameboard[27, 1] == 15)
                    {
                        CheckStack = 0;
                        StatusSet();
                    }
                    break;

            }

            if (CheckStack == 10)
            {
                StatusCheck = true;
            }

        } // Status()

        // 다음 행동 선택 좌표 구현
        public void ChoiceActSet()
        {

            for (int y = 0; y < BOARDY; y++)
            {
                for (int x = 0; x < BOARDX; x++)
                {
                    gameboard[y, x] = 0;

                    if (x == 1)
                    {
                        gameboard[y, x] = -1;

                    }


                    // 테두리
                    if (x == 0 || x == 30 || y == 0 || y == 30)
                    {
                        gameboard[y, x] = -2;

                    }


                    // 제목 좌표
                    if (x == 1 && y == 8)
                    {
                        gameboard[y, x] = -3;

                    }

                    // 화살표 좌표
                    if (x == 1 && y == 12)
                    {
                        gameboard[y, x] = 2;

                    }

                    // 선택지 앞 공백 좌표
                    if ((x == 1 && y == 16) || (x == 1 && y == 20) || (x == 1 && y == 24))
                    {
                        gameboard[y, x] = 3;

                    }

                    // 마을 좌표
                    if (x == 2 && y == 12)
                    {
                        gameboard[y, x] = 4;

                    }


                    // 모험 좌표
                    if (x == 2 && y == 16)
                    {
                        gameboard[y, x] = 5;

                    }


                    // 아이템 좌표
                    if (x == 2 && y == 20)
                    {
                        gameboard[y, x] = 6;

                    }

                    // 끝내기 좌표
                    if (x == 2 && y == 24)
                    {
                        gameboard[y, x] = 7;

                    }


                }
            }

            // 화살표 좌표 저장
            ArrowY = 12;
            ArrowX = 1;



        } // ChoiceActSet()

        // 다음 행동 선택 구현
        public void ChoiceAct()
        {
            for (int y = 0; y < BOARDY; y++)
            {
                for (int x = 0; x < BOARDX; x++)
                {
                    switch (gameboard[y, x])
                    {

                        case -3:
                            Console.Write("[다음 행동을 선택해주세요]".PadLeft(45, ' ') + "".PadRight(31, ' '));
                            break;

                        case -2:
                            Console.Write("■".PadRight(2, ' '));
                            break;

                        case -1:
                            Console.Write("".PadLeft(87, ' '));
                            break;

                        case 2:
                            Console.Write("▶".PadLeft(34, ' '));
                            break;
                        case 3:
                            Console.Write(" ".PadLeft(35, ' '));
                            break;
                        case 4:
                            Console.Write("마을로 항하기".PadLeft(9, ' ') + "".PadRight(37, ' '));
                            break;
                        case 5:
                            Console.Write("모험 떠나기".PadLeft(9, ' ') + "".PadRight(38, ' '));
                            break;
                        case 6:
                            Console.Write("아이템 확인하기".PadLeft(9, ' ') + "".PadRight(36, ' '));
                            break;
                        case 7:
                            Console.Write("종료하기".PadLeft(8, ' ') + "".PadRight(40, ' '));
                            break;

                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();


            ConsoleKeyInfo Move;

            Move = Console.ReadKey();

            switch (Move.Key)
            {
                case ConsoleKey.W:
                    if (gameboard[12, 1] == 2) { /* Do Nothing */ }
                    else
                    {
                        gameboard[ArrowY, 1] = 3;

                        ArrowY -= 4;
                        gameboard[ArrowY, 1] = 2;
                    }

                    break;

                case ConsoleKey.S:

                    if (gameboard[24, 1] == 2) { /* Do Nothing */ }
                    else
                    {
                        gameboard[ArrowY, 1] = 3;

                        ArrowY += 4;
                        gameboard[ArrowY, 1] = 2;
                    }
                    break;

                case ConsoleKey.UpArrow:
                    if (gameboard[12, 1] == 2) { /* Do Nothing */ }
                    else
                    {
                        gameboard[ArrowY, 1] = 3;

                        ArrowY -= 4;
                        gameboard[ArrowY, 1] = 2;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (gameboard[24, 1] == 2) { /* Do Nothing */ }
                    else
                    {
                        gameboard[ArrowY, 1] = 3;

                        ArrowY += 4;
                        gameboard[ArrowY, 1] = 2;
                    }
                    break;

                default:

                    if (gameboard[12, 1] == 2)
                    {
                        // 마을로 떠나기
                        // 마을 구현
                        GameboardClear();
                        VillageCheckout = false;
                        VillageSet();

                        while (VillageCheckout == false)
                        {
                            Console.SetCursorPosition(0, 0);
                            Village();
                        }

                    }
                    else if (gameboard[16, 1] == 2)
                    {
                        // 모험 떠나기
                        // 모험 구현
                        GameboardClear();
                        AdventureCheckout = false;
                        AdventureSet();
                        while (AdventureCheckout == false)
                        {
                            Console.SetCursorPosition(0, 0);

                            Adventure();
                        }


                    }
                    else if (gameboard[20, 1] == 2)
                    {
                        // 아이템 확인하기
                        PlayeritemCheckout = false;
                        GameboardClear();
                        PlayeritemsSet();
                       
                        while (PlayeritemCheckout == false)
                        {
                            Console.SetCursorPosition(0, 0);
                            Playeritems();
                        }

                    }
                    else
                    {

                        // 게임 종료
                        VillageCheckout = true;
                        AdventureCheckout = true;
                        AdventureTreeCheckout = true;

                        PlayeritemCheckout = true;
                        StoreCheckout = true;
                        CaveCheckout = true;
                        ChoiceCheck = true;
                        isGameOver = true;

                    }
                    break;

            }
        } // ChoiceAct()


        // 마을 좌표 구현
        public void VillageSet()
        {
            for (int y = 0; y < BOARDY; y++)
            {
                for (int x = 0; x < BOARDX; x++)
                {
                    gameboard[y, x] = -1;

                    

                    // 테두리
                    if (x == 0 || x == 30 || y == 0 || y == 30)
                    {
                        gameboard[y, x] = -2;

                    }

                    

                    // 선택지로 돌아가는 포탈 좌표
                    if ((x == 14 || x == 15 || x == 16) && y == 24)
                    {
                        gameboard[y, x] = 20;
                    }

                    //// npc 좌표
                    //if (x == 16 && y == 10)
                    //{
                    //    gameboard[y, x] = 4;

                    //}

                    // 상점 좌표
                    if (x == 21 && y == 16)
                    {
                        gameboard[y, x] = 10;

                    }

                    
                    // 벽 취급 좌표
                    if (y == 25 || y==1)
                    {
                        gameboard[y, x] = -2;
                    }

                    if( x==11 &&(y==26 || y==27 || y == 28 || y == 29))
                    {
                        gameboard[y, x] = -2;

                    }

                    if (y==24&&(0 < x&& x < 14) || y == 24&&(16<x && x < 31))
                    {
                        gameboard[y, x] = -2;

                    }
                    if((x == 20 || x == 22 || x == 23) && y == 16 ||
                       (x == 20 || x == 21 || x == 22 || x == 23) && y == 15)
                    {
                        gameboard[y, x] = -2;

                    }
                    if ((x == 20 || x == 21 || x == 22 || x == 23) && y == 14 ||
                       (x == 20 || x == 21 || x == 22 || x == 23) && y == 13)
                    {
                        gameboard[y, x] = 20;

                    }

                    // 바닥 타일 좌표
                    if ((x == 13 || x== 14 || x == 15 || x == 16 || x == 17)&& (1 < y && y < 24))
                    {
                        gameboard[y, x] = -5;

                    }

                    // 플레이어 좌표
                    if (x == 15 && y == 23)
                    {
                        gameboard[y, x] = 2;

                        ArrowY = y;
                        ArrowX = x;

                    }

                }
            }


        } //  VillageSet()

        // 마을 구현 함수
        public void Village()
        {
            for (int y = 0; y < BOARDY; y++)
            {
                for (int x = 0; x < BOARDX; x++)
                {
                    switch (gameboard[y, x])
                    {
                        case 20:
                            Console.ForegroundColor = ConsoleColor.Blue;

                            Console.Write("■".PadRight(2, ' '));
                            Console.ForegroundColor = ConsoleColor.White;

                            break;
                        case 10:
                            Console.ForegroundColor = ConsoleColor.Gray;

                            Console.Write("▥".PadRight(2, ' '));
                            Console.ForegroundColor = ConsoleColor.White;

                            break;
                        case 4:
                            Console.ForegroundColor = ConsoleColor.Yellow;

                            Console.Write("●".PadRight(2, ' '));
                            Console.ForegroundColor = ConsoleColor.White;

                            break;
                        case -2:
                            Console.Write("■".PadRight(2, ' '));
                            break;

                        case -5:
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write("■".PadRight(2, ' '));
                            Console.ForegroundColor = ConsoleColor.White;

                            break;
                        case -1:
                            Console.Write(" ".PadRight(3, ' '));
                            break;

                        case 2:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("◎".PadRight(2, ' '));
                            Console.ForegroundColor = ConsoleColor.White;
                            break;

                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            DownSet1();
            DownSet2();


            ConsoleKeyInfo Move;

            Move = Console.ReadKey();

            switch (Move.Key)
            {
                case ConsoleKey.W:
                    if (ArrowY == 2 || gameboard[ArrowY-1, ArrowX] == -5) 
                    {
                        if (gameboard[ArrowY - 1, ArrowX] == -5)
                        {
                            gameboard[ArrowY, ArrowX] = -5;
                            ArrowY--;
                            gameboard[ArrowY, ArrowX] = 2;
                        }
                        else { /* Do Nothing */}
                    }
                    else if(gameboard[ArrowY - 1, ArrowX] == -2 || gameboard[ArrowY - 1, ArrowX] == 20) { /* Do Nothing */ }
                    else if (gameboard[ArrowY - 1, ArrowX] == 10)
                    {
                        // 상점 오픈
                        GameboardClear();
                        StoreCheckout = false;
                        StoreSet();

                        while (StoreCheckout == false)
                        {
                            Console.SetCursorPosition(0, 0);

                            Store();
                        }

                    }
                    else
                    {
                        gameboard[ArrowY, ArrowX] = -1;
                        ArrowY--;
                        gameboard[ArrowY, ArrowX] = 2;
                    }

                    break;
                case ConsoleKey.A:
                    if (ArrowX == 2 || gameboard[ArrowY, ArrowX - 1] == -5)
                    {
                        if (gameboard[ArrowY, ArrowX-1] == -5)
                        {
                            if (ArrowX == 17 + 1)
                            {
                                gameboard[ArrowY, ArrowX] = -1;
                                ArrowX--;
                                gameboard[ArrowY, ArrowX] = 2;
                            }
                            else
                            {
                                gameboard[ArrowY, ArrowX] = -5;
                                ArrowX--;
                                gameboard[ArrowY, ArrowX] = 2;
                            }
                        }
                        
                        else { /* Do Nothing */}
                    }
                    else if (gameboard[ArrowY, ArrowX - 1] == -2 || gameboard[ArrowY, ArrowX - 1] == 20) { /* Do Nothing */ }
                    else if ((gameboard[ArrowY+1, ArrowX] == -5 || gameboard[ArrowY - 1, ArrowX] == -5) && gameboard[ArrowY, ArrowX - 1] == -1)
                    {
                        gameboard[ArrowY, ArrowX] = -5;
                        ArrowX--;
                        gameboard[ArrowY, ArrowX] = 2;

                    }
                    else
                    {
                        gameboard[ArrowY, ArrowX] = -1;
                        ArrowX--;
                        gameboard[ArrowY, ArrowX] = 2;
                    }
                    break;

                case ConsoleKey.S:

                    if (ArrowY == 23 || gameboard[ArrowY + 1, ArrowX] == -5)
                    {
                        
                        if (ArrowY == 23 &&(ArrowX == 14 || ArrowX == 15 || ArrowX == 16))
                        {
                            // 마을, 모험, 아이템, 끝내기 선택지
                            ChoiceActSet();

                            while (ChoiceCheck == false)
                            {
                                Console.SetCursorPosition(0, 0);
                                ChoiceAct();

                            }
                        }
                        else if (gameboard[ArrowY + 1, ArrowX] == -5)
                        {
                            gameboard[ArrowY, ArrowX] = -5;
                            ArrowY++;
                            gameboard[ArrowY, ArrowX] = 2;
                        }
                        
                    }
                    else if (gameboard[ArrowY + 1, ArrowX] == -2 || gameboard[ArrowY + 1, ArrowX] == 20) { /* Do Nothing */ }
                    else
                    {
                        gameboard[ArrowY, ArrowX] = -1;
                        ArrowY++;
                        gameboard[ArrowY, ArrowX] = 2;
                    }
                    break;

                case ConsoleKey.D:
                    if (ArrowX == 28 || gameboard[ArrowY, ArrowX + 1] == -5)
                    {
                        if (gameboard[ArrowY, ArrowX+1] == -5)
                        {

                            if (ArrowX == 12)
                            {
                                gameboard[ArrowY, ArrowX] = -1;
                                ArrowX++;
                                gameboard[ArrowY, ArrowX] = 2;
                            }
                            else
                            {

                                gameboard[ArrowY, ArrowX] = -5;
                                ArrowX++;
                                gameboard[ArrowY, ArrowX] = 2;
                            }
                        }
                    }
                    else if (gameboard[ArrowY, ArrowX + 1] == -2 || gameboard[ArrowY, ArrowX + 1] == 20) { /* Do Nothing */ }
                    else if ((gameboard[ArrowY + 1, ArrowX] == -5 || gameboard[ArrowY - 1, ArrowX] == -5) && gameboard[ArrowY, ArrowX + 1] == -1)
                    {
                        gameboard[ArrowY, ArrowX] = -5;
                        ArrowX++;
                        gameboard[ArrowY, ArrowX] = 2;

                    }
                    else
                    {
                        gameboard[ArrowY, ArrowX] = -1;
                        ArrowX++;
                        gameboard[ArrowY, ArrowX] = 2;
                    }
                    break;


            }

        } // Village()


        // 모험 선택 좌표 구현
        public void AdventureSet()
        {
            for (int y = 0; y < BOARDY; y++)
            {
                for (int x = 0; x < BOARDX; x++)
                {
                    gameboard[y, x] = -1;

                    // 테두리
                    if (x == 0 || x == 30 || y == 0 || y == 30)
                    {
                        gameboard[y, x] = -2;

                    }

                    if((11 <= x && x <= 19)&&(y == 9 || y == 17) ||
                        (9 <= y && y <= 17)&&(x == 11 || x == 19) )
                    {
                        gameboard[y, x] = -2;
                        
                        // 산, 바다, 동굴, 돌아가기 포탈 구현
                        if((14 <= x && x <= 16) && (y == 9 || y == 17) ||
                        (12 <= y && y <= 14) && (x == 11 || x == 19))
                        {
                            gameboard[y, x] = -3;

                        }
                    }

                    // 플레이어 좌표
                    if (x == 15 && y == 13)
                    {
                        gameboard[y, x] = 2;

                        ArrowY = y;
                        ArrowX = x;
                    }



                    // 벽 취급 좌표
                    if (y == 25)
                    {
                        gameboard[y, x] = -2;
                    }

                    if (x == 11 && (y == 26 || y == 27 || y == 28 || y == 29))
                    {
                        gameboard[y, x] = -2;

                    }




                }
            }


        } //  AdventureSet()

        public void Adventure()
        {
            for (int y = 0; y < BOARDY; y++)
            {
                for (int x = 0; x < BOARDX; x++)
                {
                    switch (gameboard[y, x])
                    {
                        case -3:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("■".PadRight(2, ' '));
                            Console.ForegroundColor = ConsoleColor.White;

                            break;
                        case -2:
                            Console.Write("■".PadRight(2, ' '));
                            break;

                        case -1:
                            Console.Write(" ".PadRight(3, ' '));
                            break;

                        case 2:
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.Write("◎".PadRight(2, ' '));
                            Console.ForegroundColor = ConsoleColor.White;
                            break;

                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            DownSet1();
            DownSet2();


            ConsoleKeyInfo Move;

            Move = Console.ReadKey();

            switch (Move.Key)
            {
                case ConsoleKey.W:
                    if (ArrowY == 2 || gameboard[ArrowY - 1, ArrowX] == -2 ||
                        gameboard[ArrowY - 1, ArrowX] == -3)
                    {
                        if(gameboard[ArrowY - 1, ArrowX] == -3)
                        {
                            // 산으로 가기
                            GameboardClear();
                            AdventureTreeSet();
                            MonsterCreateSet();

                            while (AdventureTreeCheckout == false)
                            {
                                Console.SetCursorPosition(0, 0);
                                AdventureTree();
                            }
                        }
                    }
                    else
                    {
                        gameboard[ArrowY, ArrowX] = -1;
                        ArrowY--;
                        gameboard[ArrowY, ArrowX] = 2;
                    }

                    break;
                case ConsoleKey.A:
                    if (ArrowX == 2 || gameboard[ArrowY, ArrowX - 1] == -2 ||
                        gameboard[ArrowY, ArrowX - 1] == -3)
                    { 
                        if(gameboard[ArrowY, ArrowX - 1] == -2)
                        {
                            // 바다로 가기
                        }
                    }
                    else
                    {
                        gameboard[ArrowY, ArrowX] = -1;
                        ArrowX--;
                        gameboard[ArrowY, ArrowX] = 2;
                    }
                    break;

                case ConsoleKey.S:

                    if (ArrowY == 23 || gameboard[ArrowY + 1, ArrowX] == -2 ||
                        gameboard[ArrowY + 1, ArrowX] == -3) 
                    {

                        if (gameboard[ArrowY + 1, ArrowX] == -3)
                        {
                            // 마을, 모험, 아이템, 끝내기 선택지
                            GameboardClear();

                            ChoiceActSet();

                            while (ChoiceCheck == false)
                            {
                                Console.SetCursorPosition(0, 0);
                                ChoiceAct();

                            }
                        }
                    }
                    else
                    {
                        gameboard[ArrowY, ArrowX] = -1;
                        ArrowY++;
                        gameboard[ArrowY, ArrowX] = 2;
                    }
                    break;

                case ConsoleKey.D:
                    if (ArrowX == 28 || gameboard[ArrowY, ArrowX + 1] == -2 ||
                        gameboard[ArrowY, ArrowX + 1] == -3)
                    {
                        if(gameboard[ArrowY, ArrowX + 1] == -3)
                        {
                            // 동굴로 가기
                            GameboardClear();

                            AdventureCaveSet();
                            MonsterCreateSet();
                            CaveCheckout = false;
                            while (CaveCheckout == false)
                            {
                                Console.SetCursorPosition(0, 0);

                                AdventureCave();
                            }

                        }
                    }
                    else
                    {
                        gameboard[ArrowY, ArrowX] = -1;
                        ArrowX++;
                        gameboard[ArrowY, ArrowX] = 2;
                    }
                    break;

                

            }

        } // Adventure()


        // 몬스터 생성 구현 함수
        public void MonsterCreateSet()
        {
            int monsterCount = 0;

            while (monsterCount < 5) {

                int randomVal1 = random.Next(0, 23 );
                int randomVal2 = random.Next(0, 30 );

                for (int y = 0; y < 23; y++)
                {
                    for (int x = 0; x < BOARDX; x++)
                    {
                        if (gameboard[randomVal1, randomVal2] == -2 || gameboard[randomVal1, randomVal2] == 2 ||
                            gameboard[randomVal1, randomVal2] == 10 || gameboard[randomVal1, randomVal2] == -3 ||
                            gameboard[randomVal1, randomVal2] == 20 || gameboard[randomVal1, randomVal2] == -10)
                        {
                            /* Do Nothing */
                        }
                        else
                        {
                            gameboard[randomVal1, randomVal2] = 10;
                            gamemonsterboard[randomVal1, randomVal2] = 10;
                            monsterCount++;
                        }
                    }
                }
            }


        } // MonsterCreateSet()

        // 몬스터 위치 저장한걸 다시 출력하는 함수

        public void MonsterReSetting()
        {
            for (int y = 0; y < BOARDY; y++)
            {
                for (int x = 0; x < BOARDX; x++)
                {
                    if(gamemonsterboard[y,x] == 10)
                    {
                        gameboard[y, x] = 10;
                    }
                }
            }

        } // MonsterCreateSet()



        // 동굴 구현 좌표 함수
        public void AdventureCaveSet()
        {
            for (int y = 0; y < BOARDY; y++)
            {
                for (int x = 0; x < BOARDX; x++)
                {
                    gameboard[y, x] = -1;

                    // 테두리
                    if (x == 0 || x == 30 || y == 0 || y == 30 ||
                        x == 1 && (0 <= y && y <= 24) || x == 29 && (0 <= y && y <= 24))
                    {
                        gameboard[y, x] = -2;

                    }


                    // 플레이어 좌표
                    if (x == 2 && y == 13)
                    {
                        gameboard[y, x] = 2;

                        ArrowY = y;
                        ArrowX = x;

                    }


                    // 벽 취급 좌표
                    if (y == 25 || y == 1)
                    {
                        gameboard[y, x] = -2;
                    }

                    if (x == 11 && (y == 26 || y == 27 || y == 28 || y == 29))
                    {
                        gameboard[y, x] = -2;

                    }

                    if (x == 3 && (2 <= y && y <= 7))
                    {
                        gameboard[y, x] = -10;

                    }
                    if (y == 1 && (0 < x && x < 30))
                    {
                        gameboard[y, x] = -10;

                    }

                    if (x == 29 && (0 < y && y < 25))
                    {
                        gameboard[y, x] = -10;

                    }

                    if (x == 1 && (0 < y && y < 12))
                    {
                        gameboard[y, x] = -10;

                    }

                    if (x == 1 && (14 < y && y < 25))
                    {
                        gameboard[y, x] = -10;

                    }

                    if (x == 5 && (3 <= y && y <= 8))
                    {
                        gameboard[y, x] = -10;

                    }

                    if (x == 7 && (2 <= y && y <= 7))
                    {
                        gameboard[y, x] = -10;

                    }
                    if((9<=x&&x<=11) && (4 <= y && y <= 5))
                    {
                        gameboard[y, x] = -10;

                    }

                    if(y==8 && (9 <= x && x <= 13))
                    {
                        gameboard[y, x] = -10;

                    }
                    if (x == 13 && (4 <= y && y <= 7))
                    {
                        gameboard[y, x] = -10;

                    }
                    if ((14 <= x && x <= 16) && (20 <= y && y <= 23))
                    {
                        gameboard[y, x] = -10;

                    }
                    if (y==24&&(0<x&&x<30))
                    {
                        gameboard[y, x] = -10;

                    }

                    if((17<=y&&y<=18) && (9 <= x && x <= 12))
                    {
                        gameboard[y, x] = -10;

                    }
                    if ((11 <= y && y <= 14) && (11 <= x && x <= 13))
                    {
                        gameboard[y, x] = -10;

                    }

                    if (y == 11 &&(1 < x && x < 5))
                    {
                        gameboard[y, x] = -10;

                    }

                    if(y == 9 &&( 2 < x && x < 7))
                    {
                        gameboard[y, x] = -10;

                    }

                    if((14 <= y&& y <=16) && (x == 3 || x == 4))
                    {
                        gameboard[y, x] = -10;

                    }
                    if (y == 18 && (2 <= x&& x<=5))
                    {
                        gameboard[y, x] = -10;

                    }

                    if (y == 21 &&(3 <= x&& x <= 6))
                    {
                        gameboard[y, x] = -10;

                    }

                    if (x == 5 && (22 <= y && y <= 24))
                    {
                        gameboard[y, x] = -10;

                    }

                    if((12<=y&& y<= 14) && x == 15)
                    {
                        gameboard[y, x] = -10;

                    }

                    if((6<= x&& x<=9) && (11 <= y && y <= 12))
                    {
                        gameboard[y, x] = -10;

                    }

                    if ((y == 12 || y==14) && (15 <= x && x <= 17))
                    {
                        gameboard[y, x] = -10;

                    }

                    if((8<=x&&x<=12) && y==22)
                    {
                        gameboard[y, x] = -10;

                    }

                    if (x == 7 && (14 <= y && y <= 19))
                    {
                        gameboard[y, x] = -10;

                    }
                    if ((18<=x&&x<=19) && (8 <= y && y <= 12))
                    {
                        gameboard[y, x] = -10;

                    }

                    if ((17<=y&&y<= 18) && (14 <= x && x <= 20))
                    {
                        gameboard[y, x] = -10;

                    }

                    if ((15 <= x && x <= 16) && (2 <= y && y <= 8))
                    {
                        gameboard[y, x] = -10;

                    }
                    if ((17 <= x && x <= 18) && (14 <= y && y <= 16))
                    {
                        gameboard[y, x] = -10;

                    }

                    if (x == 27 && (18 <= y && y <= 23))
                    {
                        gameboard[y, x] = -10;

                    }

                    if (x == 23 && (18 <= y && y <= 23))
                    {
                        gameboard[y, x] = -10;

                    }
                    if (x == 25 && (15 <= y && y <= 22))
                    {
                        gameboard[y, x] = -10;

                    }
                    if (y == 14 && (21 <= x && x <= 29))
                    {
                        gameboard[y, x] = -10;

                    }

                    if ((4<=y&&y<=6) && (21 <= x && x <= 25))
                    {
                        gameboard[y, x] = -10;

                    }

                    if ((23 <= x && x <= 27) && (9 <= y && y <= 11))
                    {
                        gameboard[y, x] = -10;

                    }
                    // 기믹 트리거 좌표
                    if (y == 13 && x == 16)
                    {
                        gameboard[y, x] = 20;
                    }

                    // 포탈 취급 좌표
                    if ((y == 12 || y == 13 || y == 14) && x == 1)
                    {
                        gameboard[y, x] = -3;
                    }


                }
            }

        } //  AdventureCaveSet()

        // 동굴 구현 함수
        public void AdventureCave()
        {
            for (int y = 0; y < BOARDY; y++)
            {
                for (int x = 0; x < BOARDX; x++)
                {
                    switch (gameboard[y, x])
                    {
                        // 기믹 트리거
                        case 20:
                            if (-2 <= ArrowY - y && ArrowY - y <= 2)
                            {
                                if (-2 <= ArrowX - x && ArrowX - x <= 2)
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.Write("▣".PadRight(2, ' '));
                                    Console.ForegroundColor = ConsoleColor.White;

                                }
                                else
                                {
                                    Console.Write(" ".PadRight(3, ' '));

                                }
                            }
                            else
                            {
                                Console.Write(" ".PadRight(3, ' '));
                            }


                            break;
                        case -10:

                            if(-2 <= ArrowY - y && ArrowY - y <= 2)
                            {
                                if(-2<= ArrowX - x && ArrowX - x <= 2)
                                {
                                    Console.Write("■".PadRight(2, ' '));

                                }
                                else
                                {
                                    Console.Write(" ".PadRight(3, ' '));

                                }
                            }
                            else
                            {
                                Console.Write(" ".PadRight(3, ' '));
                            }


                            break;

                        // 몬스터 값
                        case 10:
                            if (-2 <= ArrowY - y && ArrowY - y <= 2)
                            {
                                if (-2 <= ArrowX - x && ArrowX - x <= 2)
                                {
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    Console.Write("▣".PadRight(2, ' '));
                                    Console.ForegroundColor = ConsoleColor.White;

                                }
                                else
                                {
                                    Console.Write(" ".PadRight(3, ' '));

                                }
                            }
                            else
                            {
                                Console.Write(" ".PadRight(3, ' '));
                            }

                            break;
                        case -3:
                            Console.ForegroundColor = ConsoleColor.Blue;

                            Console.Write("■".PadRight(2, ' '));
                            Console.ForegroundColor = ConsoleColor.White;

                            break;
                        case -2:
                            Console.Write("■".PadRight(2, ' '));
                            break;

                        case -1:
                            Console.Write(" ".PadRight(3, ' '));
                            break;

                        case 2:
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.Write("◎".PadRight(2, ' '));
                            Console.ForegroundColor = ConsoleColor.White;
                            break;

                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            DownSet1();
            DownSet2();


            ConsoleKeyInfo Move;

            Move = Console.ReadKey();

            switch (Move.Key)
            {
                case ConsoleKey.W:
                    if (ArrowY == 2 || gameboard[ArrowY - 1, ArrowX] == -2 ||
                        gameboard[ArrowY - 1, ArrowX] == 10)
                    {
                        if (gameboard[ArrowY - 1, ArrowX] == 10)
                        {

                            BattleCheck = false;
                            BattleONCheck = false;
                            Battlemonsterturn = false;
                            SavePlayerY = ArrowY;
                            SavePlayerX = ArrowX;

                            gamemonsterboard[ArrowY - 1, ArrowX] = 0;
                            monster = new Golem();

                            GameboardClear();
                            PlayerBattleVal[0] = 5;
                            MonsterBattleVal[0] = monster.Monsterhp();
                            MonsterBattleVal[1] = monster.Monsterdamage();
                            MonsterBattleVal[2] = monster.Monsterdefence();

                            BattleSet();

                            while (BattleCheck == false)
                            {
                                Console.SetCursorPosition(0, 0);
                                Battle();

                            }

                            GameboardClear();
                            AdventureCaveSet();
                            MonsterReSetting();

                            gameboard[ArrowY, ArrowX] = -1;
                            ArrowY = SavePlayerY;
                            ArrowX = SavePlayerX;
                            gameboard[ArrowY, ArrowX] = 2;
                        }
                    }
                    else if(gameboard[ArrowY - 1, ArrowX] == -10) { /* Do Nothing */ }
                    else
                    {
                        gameboard[ArrowY, ArrowX] = -1;
                        ArrowY--;
                        gameboard[ArrowY, ArrowX] = 2;
                    }

                    break;
                case ConsoleKey.A:
                    if (ArrowX == 2 || gameboard[ArrowY, ArrowX - 1] == -2 || gameboard[ArrowY, ArrowX - 1] == 10)
                    {
                        if (gameboard[ArrowY, ArrowX - 1] == 10)
                        {

                            BattleCheck = false;
                            BattleONCheck = false;
                            Battlemonsterturn = false;
                            SavePlayerY = ArrowY;
                            SavePlayerX = ArrowX;

                            gamemonsterboard[ArrowY, ArrowX - 1] = 0;
                            monster = new Golem();

                            GameboardClear();
                            PlayerBattleVal[0] = 5;
                            MonsterBattleVal[0] = monster.Monsterhp();
                            MonsterBattleVal[1] = monster.Monsterdamage();
                            MonsterBattleVal[2] = monster.Monsterdefence();

                            BattleSet();

                            while (BattleCheck == false)
                            {
                                Console.SetCursorPosition(0, 0);
                                Battle();

                            }

                            GameboardClear();
                            AdventureCaveSet();
                            MonsterReSetting();

                            gameboard[ArrowY, ArrowX] = -1;
                            ArrowY = SavePlayerY;
                            ArrowX = SavePlayerX;
                            gameboard[ArrowY, ArrowX] = 2;
                        }
                        else if (ArrowY == 12 || ArrowY == 13 || ArrowY == 14)
                        {
                            // 모험 떠나기
                            // 모험 구현
                            GameboardClear();
                            AdventureCheckout = false;
                            AdventureSet();
                            while (AdventureCheckout == false)
                            {
                                Console.SetCursorPosition(0, 0);

                                Adventure();
                            }
                        }
                    }
                    else if (gameboard[ArrowY, ArrowX-1] == -10) { /* Do Nothing */ }
                    else
                    {
                        gameboard[ArrowY, ArrowX] = -1;
                        ArrowX--;
                        gameboard[ArrowY, ArrowX] = 2;
                    }
                    break;

                case ConsoleKey.S:

                    if (ArrowY == 23 || gameboard[ArrowY + 1, ArrowX] == -2 || gameboard[ArrowY + 1, ArrowX] == 10)
                    {

                        if (gameboard[ArrowY + 1, ArrowX] == 10)
                        {

                            BattleCheck = false;
                            BattleONCheck = false;
                            Battlemonsterturn = false;
                            SavePlayerY = ArrowY;
                            SavePlayerX = ArrowX;

                            gamemonsterboard[ArrowY + 1, ArrowX] = 0;
                            monster = new Golem();

                            GameboardClear();
                            PlayerBattleVal[0] = 5;
                            MonsterBattleVal[0] = monster.Monsterhp();
                            MonsterBattleVal[1] = monster.Monsterdamage();
                            MonsterBattleVal[2] = monster.Monsterdefence();

                            BattleSet();

                            while (BattleCheck == false)
                            {
                                Console.SetCursorPosition(0, 0);
                                Battle();

                            }

                            GameboardClear();
                            AdventureCaveSet();
                            MonsterReSetting();

                            gameboard[ArrowY, ArrowX] = -1;
                            ArrowY = SavePlayerY;
                            ArrowX = SavePlayerX;
                            gameboard[ArrowY, ArrowX] = 2;
                        }

                    }
                    else if (gameboard[ArrowY + 1, ArrowX] == -10) { /* Do Nothing */ }
                    else
                    {
                        gameboard[ArrowY, ArrowX] = -1;
                        ArrowY++;
                        gameboard[ArrowY, ArrowX] = 2;
                    }
                    break;

                case ConsoleKey.D:
                    if (ArrowX == 28 || gameboard[ArrowY, ArrowX + 1] == -2 || gameboard[ArrowY, ArrowX + 1] == 10)
                    {
                        if (gameboard[ArrowY, ArrowX + 1] == 10)
                        {

                            BattleCheck = false;
                            BattleONCheck = false;
                            Battlemonsterturn = false;
                            SavePlayerY = ArrowY;
                            SavePlayerX = ArrowX;

                            gamemonsterboard[ArrowY, ArrowX + 1] = 0;
                            monster = new Golem();

                            GameboardClear();
                            PlayerBattleVal[0] = 5;
                            MonsterBattleVal[0] = monster.Monsterhp();
                            MonsterBattleVal[1] = monster.Monsterdamage();
                            MonsterBattleVal[2] = monster.Monsterdefence();

                            BattleSet();

                            while (BattleCheck == false)
                            {
                                Console.SetCursorPosition(0, 0);
                                Battle();

                            }

                            GameboardClear();
                            AdventureCaveSet();
                            MonsterReSetting();

                            gameboard[ArrowY, ArrowX] = -1;
                            ArrowY = SavePlayerY;
                            ArrowX = SavePlayerX;
                            gameboard[ArrowY, ArrowX] = 2;
                        }
                    }
                    else if (gameboard[ArrowY, ArrowX + 1] == -10) { /* Do Nothing */ }
                    else
                    {
                        gameboard[ArrowY, ArrowX] = -1;
                        ArrowX++;
                        gameboard[ArrowY, ArrowX] = 2;
                    }
                    break;

            }



        } // AdventureCave()



        // 산 구현 좌표 함수
        public void AdventureTreeSet()
        {
            for (int y = 0; y < BOARDY; y++)
            {
                for (int x = 0; x < BOARDX; x++)
                {
                    gameboard[y, x] = -1;

                    // 테두리
                    if (x == 0 || x == 30 || y == 0 || y == 30 ||
                        x == 1 && (0 <= y && y <= 24) || x == 29 && (0 <= y && y <= 24) )
                    {
                        gameboard[y, x] = -2;

                    }


                    // 플레이어 좌표
                    if (x == 15 && y == 23)
                    {
                        gameboard[y, x] = 2;

                        ArrowY = y;
                        ArrowX = x;

                    }


                    // 벽 취급 좌표
                    if (y == 25 || y == 1)
                    {
                        gameboard[y, x] = -2;
                    }

                    if (x == 11 && (y == 26 || y == 27 || y == 28 || y == 29))
                    {
                        gameboard[y, x] = -2;

                    }

                    if ((x == 14 || x == 15 || x == 16) && y == 24)
                    {
                        gameboard[y, x] = -3;
                    }


                }
            }

        } //  AdventureTreeSet()

        // 산 구현 함수
        public void AdventureTree()
        {
            for (int y = 0; y < BOARDY; y++)
            {
                for (int x = 0; x < BOARDX; x++)
                {
                    switch (gameboard[y, x])
                    {
                        case 10:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("◈".PadRight(2, ' '));
                            Console.ForegroundColor = ConsoleColor.White;

                            break;
                        case -3:
                            Console.ForegroundColor = ConsoleColor.Blue;

                            Console.Write("■".PadRight(2, ' '));
                            Console.ForegroundColor = ConsoleColor.White;

                            break;
                        case -2:
                            Console.Write("■".PadRight(2, ' '));
                            break;

                        case -1:
                            Console.Write(" ".PadRight(3, ' '));
                            break;

                        case 2:
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.Write("◎".PadRight(2, ' '));
                            Console.ForegroundColor = ConsoleColor.White;
                            break;

                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            DownSet1();
            DownSet2();


            ConsoleKeyInfo Move;

            Move = Console.ReadKey();

            switch (Move.Key)
            {
                case ConsoleKey.W:
                    if (ArrowY == 2 || gameboard[ArrowY-1, ArrowX] == -2 ||
                        gameboard[ArrowY - 1, ArrowX] == 10 )
                    {
                        if (gameboard[ArrowY - 1, ArrowX] == 10)
                        {
                            
                            BattleCheck = false;
                            BattleONCheck = false;
                            Battlemonsterturn = false;
                            SavePlayerY = ArrowY;
                            SavePlayerX = ArrowX;

                            gamemonsterboard[ArrowY - 1, ArrowX] = 0;
                            monster = new Wolf();

                            GameboardClear();
                            PlayerBattleVal[0] = 5;
                            MonsterBattleVal[0] = monster.Monsterhp();
                            MonsterBattleVal[1] = monster.Monsterdamage();
                            MonsterBattleVal[2] = monster.Monsterdefence();

                            BattleSet();

                            while (BattleCheck == false)
                            {
                                Console.SetCursorPosition(0, 0);
                                Battle();

                            }

                            GameboardClear();
                            AdventureTreeSet();
                            MonsterReSetting();

                            gameboard[ArrowY, ArrowX] = -1;
                            ArrowY = SavePlayerY;
                            ArrowX = SavePlayerX;
                            gameboard[ArrowY, ArrowX] = 2;
                        }
                    }
                    else
                    {
                        gameboard[ArrowY, ArrowX] = -1;
                        ArrowY--;
                        gameboard[ArrowY, ArrowX] = 2;
                    }

                    break;
                case ConsoleKey.A:
                    if (ArrowX == 2 || gameboard[ArrowY, ArrowX-1] == -2 || gameboard[ArrowY, ArrowX - 1] == 10) 
                    {
                        if (gameboard[ArrowY, ArrowX - 1] == 10)
                        {

                            BattleCheck = false;
                            BattleONCheck = false;
                            Battlemonsterturn = false;
                            SavePlayerY = ArrowY;
                            SavePlayerX = ArrowX;

                            gamemonsterboard[ArrowY, ArrowX-1] = 0;
                            monster = new Wolf();

                            GameboardClear();
                            PlayerBattleVal[0] = 5;
                            MonsterBattleVal[0] = monster.Monsterhp();
                            MonsterBattleVal[1] = monster.Monsterdamage();
                            MonsterBattleVal[2] = monster.Monsterdefence();

                            BattleSet();

                            while (BattleCheck == false)
                            {
                                Console.SetCursorPosition(0, 0);
                                Battle();

                            }

                            GameboardClear();
                            AdventureTreeSet();
                            MonsterReSetting();

                            gameboard[ArrowY, ArrowX] = -1;
                            ArrowY = SavePlayerY;
                            ArrowX = SavePlayerX;
                            gameboard[ArrowY, ArrowX] = 2;
                        }
                    }
                    else
                    {
                        gameboard[ArrowY, ArrowX] = -1;
                        ArrowX--;
                        gameboard[ArrowY, ArrowX] = 2;
                    }
                    break;

                case ConsoleKey.S:

                    if (ArrowY == 23 || gameboard[ArrowY + 1, ArrowX] == -2 || gameboard[ArrowY + 1, ArrowX] == 10)
                    {

                        if (gameboard[ArrowY+1, ArrowX] == 10)
                        {

                            BattleCheck = false;
                            BattleONCheck = false;
                            Battlemonsterturn = false;
                            SavePlayerY = ArrowY;
                            SavePlayerX = ArrowX;

                            gamemonsterboard[ArrowY + 1, ArrowX] = 0;
                            monster = new Wolf();

                            GameboardClear();
                            PlayerBattleVal[0] = 5;
                            MonsterBattleVal[0] = monster.Monsterhp();
                            MonsterBattleVal[1] = monster.Monsterdamage();
                            MonsterBattleVal[2] = monster.Monsterdefence();

                            BattleSet();

                            while (BattleCheck == false)
                            {
                                Console.SetCursorPosition(0, 0);
                                Battle();

                            }

                            GameboardClear();
                            AdventureTreeSet();
                            MonsterReSetting();

                            gameboard[ArrowY, ArrowX] = -1;
                            ArrowY = SavePlayerY;
                            ArrowX = SavePlayerX;
                            gameboard[ArrowY, ArrowX] = 2;
                        }


                        if (ArrowX == 14 || ArrowX == 15 || ArrowX == 16)
                        {
                            // 모험 떠나기
                            // 모험 구현
                            GameboardClear();
                            AdventureCheckout = false;
                            AdventureSet();
                            while (AdventureCheckout == false)
                            {
                                Console.SetCursorPosition(0, 0);

                                Adventure();
                            }
                        }
                    }
                    else
                    {
                        gameboard[ArrowY, ArrowX] = -1;
                        ArrowY++;
                        gameboard[ArrowY, ArrowX] = 2;
                    }
                    break;

                case ConsoleKey.D:
                    if (ArrowX == 28 || gameboard[ArrowY, ArrowX + 1] == -2 || gameboard[ArrowY, ArrowX + 1] == 10) 
                    {
                        if (gameboard[ArrowY, ArrowX + 1] == 10)
                        {

                            BattleCheck = false;
                            BattleONCheck = false;
                            Battlemonsterturn = false;
                            SavePlayerY = ArrowY;
                            SavePlayerX = ArrowX;

                            gamemonsterboard[ArrowY, ArrowX + 1] = 0;
                            monster = new Wolf();

                            GameboardClear();
                            PlayerBattleVal[0] = 5;
                            MonsterBattleVal[0] = monster.Monsterhp();
                            MonsterBattleVal[1] = monster.Monsterdamage();
                            MonsterBattleVal[2] = monster.Monsterdefence();

                            BattleSet();

                            while (BattleCheck == false)
                            {
                                Console.SetCursorPosition(0, 0);
                                Battle();

                            }

                            GameboardClear();
                            AdventureTreeSet();
                            MonsterReSetting();

                            gameboard[ArrowY, ArrowX] = -1;
                            ArrowY = SavePlayerY;
                            ArrowX = SavePlayerX;
                            gameboard[ArrowY, ArrowX] = 2;
                        }
                    }
                    else
                    {
                        gameboard[ArrowY, ArrowX] = -1;
                        ArrowX++;
                        gameboard[ArrowY, ArrowX] = 2;
                    }
                    break;

            }



        } // AdventureTree()



        // 오류 뜨니 보류 -> 커서 위치 오류 문제를 해결해야할듯;
        // 갑자기 해결됨...?
        // 다 블록으로 채우는 문제였나?
        public void DownSet1()
        {
            Console.SetCursorPosition(3, 26);
            Console.WriteLine("이름 : {0} ", PlayerName);
            Console.SetCursorPosition(3, 27);
            PlayerHpVal();

            Console.SetCursorPosition(3, 28);
            Console.WriteLine(" 힘  : {0} 마법 : {1} 행운 : {2}", StatusValues[0], StatusValues[2], StatusValues[4]);
            Console.SetCursorPosition(3, 29);
            Console.WriteLine("체력 : {0} 지혜 : {1} 회피 : {2}", StatusValues[1], StatusValues[3], StatusValues[5]);

        } // DownSet1()


        public void DownSet2()
        {
            
            Console.SetCursorPosition(36, 26);
            Console.WriteLine("[장비]");
            Console.SetCursorPosition(36, 27);
            Console.WriteLine($"무기 : {PlayerWearWeps[0]}");
            Console.SetCursorPosition(36, 28);

            Console.WriteLine($" 옷  : {PlayerWearWeps[1]}");

            Console.SetCursorPosition(36, 29);
            Console.WriteLine($"장식 : {PlayerWearWeps[2]}");


        } // DownSet2()


        // 배틀 시작시 구현 좌표 함수 
        public void BattleSet()
        {
            

            for (int y = 0; y < BOARDY; y++)
            {
                for(int x = 0; x < BOARDX; x++)
                {
                    gameboard[y, x] = 0;

                    if (x == 1)
                    {
                        gameboard[y, x] = -1;

                    }

                    
                    // 테두리
                    if (x == 0 || x == 30 || y == 0 || y == 30)
                    {
                        gameboard[y, x] = -2;

                    }


                    // VS 출력 좌표
                    if (x == 1 && y == 8)
                    {
                        gameboard[y, x] = -3;

                    }

                    // 플레이어 출력 좌표

                    if (x == 1 && y == 12)
                    {
                        gameboard[y, x] = 10;
                    }

                    // 몬스터 출력 좌표
                    if (x == 3 && y == 12)
                    {
                        gameboard[y, x] = 20;
                    }

                    // 공격 방향을 알려주는 좌표 플레이어 -> 몬스터
                    if (x == 2 && y == 12)
                    {
                        if(BattleONCheck == true)
                        {
                            gameboard[y, x] = 30;

                            if(Battlemonsterturn == true)
                            {
                                gameboard[y, x] = 40;

                            }
                        }
                        else 
                        {
                            gameboard[y, x] = 0;
                        }
                    }
                    


                    // 시작하기 좌표
                    if (x == 1 && y == 22)
                    {
                        gameboard[y, x] = 2;

                    }

                    if (x==2 && y == 22)
                    {
                        gameboard[y, x] = -5;
                        
                    }


                    // 종료하기 좌표
                    if (x == 1 && y == 24)
                    {
                        gameboard[y, x] = 3;

                    }

                    if (x == 2 && y == 24)
                    {
                        gameboard[y, x] = -6;
                        
                    }

                    
                }
            }


        }   // BattleSet()


        // 배틀 선택지 구현 함수
        public void Battle()
        {


            
            BattleView();
            if(BattleONCheck == true)
            {
                BattleVal();
            }

            ConsoleKeyInfo Move;

            Console.SetCursorPosition(0, 30);
            Move = Console.ReadKey();

            switch (Move.Key)
            {
                case ConsoleKey.W:
                    gameboard[22, 1] = 2;
                    gameboard[24, 1] = 3;

                    break;

                case ConsoleKey.S:
                    gameboard[22, 1] = 3;
                    gameboard[24, 1] = 2;

                    break;

                case ConsoleKey.UpArrow:
                    gameboard[22, 1] = 2;
                    gameboard[24, 1] = 3;

                    break;
                case ConsoleKey.DownArrow:
                    gameboard[22, 1] = 3;
                    gameboard[24, 1] = 2;

                    break;

                default:

                    if (gameboard[22, 1] == 2)
                    {
                        // 전투 시작
                        BattleONCheck = true;
                        GameboardClear();
                        BattleSet();

                    }
                    else
                    {

                        int Runrandom = random.Next(1, 20 + 1);
                        if(9 < Runrandom)
                        {
                            BattleONCheck = false;
                            BattleCheck = true;

                            Console.SetCursorPosition(30, 15);
                            Console.WriteLine($"{PlayerOccupation} {PlayerName} 의 도망이 성공했습니다.");
                            
                            Task.Delay(900).Wait();
                        }
                        else
                        {
                            // 도망 가기 실패
                            // 전투 시작
                            BattleONCheck = true;
                            Console.SetCursorPosition(30, 15);
                            Console.WriteLine($"{PlayerOccupation} {PlayerName} 의 도망이 실패했습니다.");
                            Console.SetCursorPosition(30, 16);
                            Console.WriteLine($"{monster.Monstername()} 와의 전투를 시작합니다.");
                            Task.Delay(900).Wait();
                            GameboardClear();
                            BattleSet();
                        }
                        
                    }
                    break;

            }


        } // Battle()

        // 배틀 장면 구현 함수
        public void BattleVal()
        {
            int TotalDamage = 0;
            int Totaldefence = 0;

            if (BattleONCheck == true)
            {
                if (PlayerWearWeps[0] == "기본용 양손검" || PlayerWearWeps[0] == "기본용 마법봉" || PlayerWearWeps[0] == "기본용 단검")
                {
                    if (PlayerWearWeps[0] == "기본용 양손검")
                    {
                        weps = new warrior1();
                        TotalDamage += weps.Wepdamage();
                        Totaldefence += weps.Wepdefence();
                    }
                    else if (PlayerWearWeps[0] == "기본용 마법봉")
                    {

                    }
                    else if (PlayerWearWeps[0] == "기본용 단검")
                    {

                    }
                }
                else if (PlayerWearWeps[0] == "그레이트 소드")
                {
                    if (PlayerWearWeps[0] == "그레이트 소드")
                    {
                        weps = new warrior4();
                        TotalDamage += weps.Wepdamage();
                        Totaldefence += weps.Wepdefence();

                    }
                }
                else { }

                if(PlayerWearWeps[1] == "기본용 갑옷" || PlayerWearWeps[1] == "기본용 마법복" || PlayerWearWeps[1] == "기본용 복장")
                {
                    if (PlayerWearWeps[1] == "기본용 갑옷")
                    {
                        weps = new warrior2();
                        TotalDamage += weps.Wepdamage();
                        Totaldefence += weps.Wepdefence();
                    }
                    else if (PlayerWearWeps[1] == "기본용 마법복")
                    {

                    }
                    else if (PlayerWearWeps[1] == "기본용 복장")
                    {

                    }
                }
                else if (PlayerWearWeps[1] == "판금 갑옷")
                {
                    if (PlayerWearWeps[1] == "판금 갑옷")
                    {
                        weps = new warrior5();
                        TotalDamage += weps.Wepdamage();
                        Totaldefence += weps.Wepdefence();
                    }
                }
                else { }

                if (PlayerWearWeps[2] == "기본용 전사의 가호" || PlayerWearWeps[2] == "기본용 마법사의 가호" || PlayerWearWeps[2] == "기본용 도적의 가호")
                {
                    if (PlayerWearWeps[2] == "기본용 전사의 가호")
                    {
                        weps = new warrior3();
                        TotalDamage += weps.Wepdamage();
                        Totaldefence += weps.Wepdefence();
                    }
                    else if (PlayerWearWeps[2] == "기본용 마법사의 가호")
                    {

                    }
                    else if (PlayerWearWeps[2] == "기본용 도적의 가호")
                    {

                    }
                }
                else if (PlayerWearWeps[2] == "숙련된 전사의 가호")
                {
                    if (PlayerWearWeps[2] == "숙련된 전사의 가호")
                    {
                        weps = new warrior6();
                        TotalDamage += weps.Wepdamage();
                        Totaldefence += weps.Wepdefence();
                    }
                }
                else { }

                Console.SetCursorPosition(30, 15);
                Console.WriteLine($"{PlayerOccupation} {PlayerName} 이(가) {monster.Monstername()} 을 공격했다.");

                int dice = random.Next(1, 10 + 1);

                if (MonsterBattleVal[2] < PlayerBattleVal[1] + TotalDamage + dice)
                {
                    int dice2 = random.Next(1, 10 + 1);

                    if (MonsterBattleVal[2] + dice2 < PlayerBattleVal[1] + dice)
                    {
                        BattleHitChk = true;
                        BattleSet();
                        Console.SetCursorPosition(0, 0);
                        BattleView();

                        MonsterBattleVal[0]--;
                        Console.SetCursorPosition(30, 17);
                        Console.WriteLine($"{PlayerOccupation} {PlayerName} 의 공격이 성공했다.".PadLeft(20, ' '));
                        Console.SetCursorPosition(30, 18);

                        Console.WriteLine($"{monster.Monstername()} 의 체력이 줄었다.".PadLeft(20, ' '));

                        BattleHitChk = false;
                    }
                    else
                    {
                        Console.SetCursorPosition(30, 17);
                        Console.WriteLine($"{PlayerOccupation} {PlayerName} 의 공격이 실패했다.".PadLeft(20, ' '));
                        Console.SetCursorPosition(30, 18);

                        Console.WriteLine($"{monster.Monstername()} 의 체력은 그대로다.".PadLeft(20, ' '));
                        

                    }
                }
                else
                {
                    Console.SetCursorPosition(30, 17);
                    Console.WriteLine($"{PlayerOccupation} {PlayerName} 의 공격이 실패했다.".PadLeft(20, ' '));
                    Console.SetCursorPosition(30, 18);

                    Console.WriteLine($"{monster.Monstername()} 의 체력은 그대로다.".PadLeft(20, ' '));
                    

                }

                // 플레이어 이름 표기
                Console.SetCursorPosition(30, 8);
                Console.Write($"{PlayerName}");
                Console.SetCursorPosition(25, 10);
                PlayerbattleHpVal();


                // 몬스터 이름 표기
                Console.SetCursorPosition(58, 8);
                Console.Write($"{monster.Monstername()}");
                Console.SetCursorPosition(53, 10);

                MonsterHpVal();

                // ========
                // 몬스터의 공격 턴
                Task.Delay(900).Wait();
                Battlemonsterturn = true;
                BattleSet();
                Console.SetCursorPosition(0, 0);
                BattleView();

                Console.SetCursorPosition(30, 15);
                Console.WriteLine($"{monster.Monstername()} 이(가) {PlayerOccupation} {PlayerName} 을 공격했다.");

                dice = random.Next(1, 10 + 1);

                if (PlayerBattleVal[2] + Totaldefence < MonsterBattleVal[1] + dice)
                {
                    int dice2 = random.Next(1, 10 + 1);

                    if (PlayerBattleVal[2] + Totaldefence + dice2 < MonsterBattleVal[1] + dice)
                    {
                        BattleHitplayerChk = true;
                        BattleSet();
                        Console.SetCursorPosition(0, 0);
                        BattleView();

                        PlayerBattleVal[0]--;
                        Console.SetCursorPosition(30, 17);
                        Console.WriteLine($"{monster.Monstername()} 의 공격이 성공했다.".PadLeft(20, ' '));
                        Console.SetCursorPosition(30, 18);

                        Console.WriteLine($"{PlayerOccupation} {PlayerName} 의 체력이 줄었다.".PadLeft(20, ' '));

                        BattleHitplayerChk = false;
                    }
                    else
                    {
                        Console.SetCursorPosition(30, 17);
                        Console.WriteLine($"{monster.Monstername()} 의 공격이 실패했다.".PadLeft(20, ' '));
                        Console.SetCursorPosition(30, 18);

                        Console.WriteLine($"{PlayerOccupation} {PlayerName} 의 체력은 그대로다.".PadLeft(20, ' '));


                    }
                }
                else
                {
                    Console.SetCursorPosition(30, 17);
                    Console.WriteLine($"{monster.Monstername()} 의 공격이 실패했다.".PadLeft(20, ' '));
                    Console.SetCursorPosition(30, 18);

                    Console.WriteLine($"{PlayerOccupation} {PlayerName} 의 체력은 그대로다.".PadLeft(20, ' '));

                }

                // 플레이어 이름 표기
                Console.SetCursorPosition(30, 8);
                Console.Write($"{PlayerName}");
                Console.SetCursorPosition(25, 10);
                PlayerbattleHpVal();


                // 몬스터 이름 표기
                Console.SetCursorPosition(58, 8);
                Console.Write($"{monster.Monstername()}");
                Console.SetCursorPosition(53, 10);

                MonsterHpVal();

                Battlemonsterturn = false;
                BattleONCheck = false;


                Task.Delay(900).Wait();

                GameboardClear();
                BattleSet();
                Console.SetCursorPosition(0, 0);
                BattleView();

                if (PlayerBattleVal[0] == 0 || MonsterBattleVal[0] == 0)
                {
                    if(PlayerBattleVal[0] == 0)
                    {
                        PlayerBattleVal[3]--;
                        Console.SetCursorPosition(30, 15);
                        Console.WriteLine($"{PlayerOccupation} {PlayerName} 은 {monster.Monstername()} 와의 전투에서 패배했다.".PadLeft(20, ' '));
                        if(PlayerBattleVal[3] == 0)
                        {
                            Console.SetCursorPosition(30, 16);
                            Console.WriteLine($"{PlayerOccupation} {PlayerName} 의 남은 체력이 없습니다.".PadLeft(20, ' '));
                            Console.SetCursorPosition(30, 17);
                            Console.WriteLine($"게임에서 패배하셨습니다.".PadLeft(20, ' '));
                            
                            isGameOver = true;

                        }
                        else
                        {
                            Console.SetCursorPosition(30, 16);
                            Console.WriteLine($"{PlayerOccupation} {PlayerName} 의 남은 체력은 {PlayerBattleVal[3]} 입니다.".PadLeft(20, ' '));
                        }
                    }
                    else
                    {
                        bool copyitem = false;
                        int randomitem = random.Next(1, 20 + 1);

                        Console.SetCursorPosition(26, 15);
                        Console.WriteLine($"{PlayerOccupation} {PlayerName} 은 {monster.Monstername()} 와의 전투에서 승리했다.".PadLeft(20, ' '));
                       

                        if(9 < randomitem)
                        {
                            Console.SetCursorPosition(30, 16);
                            Console.WriteLine($"{monster.Monstergold()} 골드와 {monster.Monsteritem()} 을 획득했다.".PadLeft(20, ' '));
                            PlayeritemVal[0] += monster.Monstergold();

                            if (monster.Monsteritem() == "늑대의 가죽")
                            {
                                PlayeritemVal[2]++;
                            }
                            else if(monster.Monsteritem() == "유적 골렘의 파편")
                            {
                                PlayeritemVal[4]++;
                            }


                            for (int i = 0; i < Playeritem.Count; i++)
                            {
                                if (Playeritem[i] == monster.Monsteritem())
                                {
                                    copyitem = true;
                                }
                                else { /* Do Nothing */ }
                                
                            }

                            if (copyitem == false)
                            {
                                Playeritem.Add(monster.Monsteritem());
                                playeritemsIndex.Add(monster.Monsteritem(), monster.MonsteritemIndex());
                            }
                            else
                            {
                                /* Do Nothing */
                            }

                        }
                        else
                        {
                            Console.SetCursorPosition(30, 16);
                            Console.WriteLine($"{monster.Monstergold()} 골드를 획득했다.".PadLeft(20, ' '));

                            PlayeritemVal[0] += monster.Monstergold();

                        }


                    }
                    BattleCheck = true;
                }
            }
            else { }

            GameboardClear();
            BattleSet();
        } // BattleVal()


        // 배틀 화면 함수
        public void BattleView()
        {
            for (int y = 0; y < BOARDY; y++)
            {
                for (int x = 0; x < BOARDX; x++)
                {
                    switch (gameboard[y, x])
                    {

                        case -6:
                            Console.Write("도망 가기".PadLeft(8, ' ') + "".PadRight(38, ' '));
                            break;
                        case -5:
                            Console.Write("전투 하기".PadLeft(8, ' ') + "".PadRight(38, ' '));
                            break;
                        case -3:
                            Console.Write("  VS  ".PadLeft(47, ' ') + "".PadRight(40, ' '));
                            break;

                        case -2:
                            Console.Write("■".PadRight(2, ' '));
                            break;

                        case -1:
                            Console.Write("".PadLeft(87, ' '));
                            break;

                        case 2:
                            Console.Write("▶".PadLeft(36, ' '));
                            break;
                        case 3:
                            Console.Write(" ".PadLeft(37, ' '));
                            break;
                        case 10:
                            if (BattleONCheck == true)
                            {
                                if(BattleHitplayerChk == true)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.Write("∫◎".PadLeft(32, ' '));
                                    Console.ForegroundColor = ConsoleColor.White;

                                }
                                else
                                {
                                    Console.Write("∫◎".PadLeft(32, ' '));

                                }

                            }
                            else
                            {
                                Console.Write("∫◎".PadLeft(30, ' '));

                            }

                            //Console.Write("∮◎".PadLeft(30, ' '));
                            //Console.Write("§◎".PadLeft(30, ' '));
                            break;
                        case 20:

                            if (BattleONCheck == true)
                            {
                                if (BattleHitChk == true)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.Write($"{monster.Monsterface()}".PadLeft(13, ' ') + "".PadRight(25, ' '));
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                                else if(Battlemonsterturn == true)
                                {
                                    Console.Write($"{monster.Monsterface()}".PadLeft(11, ' ') + "".PadRight(27, ' '));

                                }
                                else
                                {
                                    Console.Write($"{monster.Monsterface()}".PadLeft(13, ' ') + "".PadRight(25, ' '));

                                }

                            }
                            else
                            {
                                Console.Write($"{monster.Monsterface()}".PadLeft(27, ' ') + "".PadRight(25, ' '));

                            }

                            break;
                        case 30:
                            Console.Write("▶▶".PadLeft(10, ' '));
                            break;
                        case 40:
                            Console.Write("◀◀".PadLeft(10, ' '));
                            break;

                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();


            // 플레이어 이름 표기
            Console.SetCursorPosition(30, 8);
            Console.Write($"{PlayerName}");
            Console.SetCursorPosition(25, 10);
            PlayerbattleHpVal();


            // 몬스터 이름 표기
            Console.SetCursorPosition(58, 8);
            Console.Write($"{monster.Monstername()}");
            Console.SetCursorPosition(53, 10);

            MonsterHpVal();
        } // BattleView()


        // 전투중인 플레이어 hp 보여주는 함수
        public void PlayerbattleHpVal()
        {
            Console.Write("HP : ");
            for (int i = 0; i < 5; i++)
            {
                if (i < PlayerBattleVal[0])
                {
                    Console.Write("■");

                }
                else
                {
                    Console.Write("□");

                }
            }
            Console.WriteLine();
        } // PlayerHpVal()

        // 플레이어 hp 보여주는 함수
        public void PlayerHpVal()
        {
            Console.Write("HP".PadLeft(3, ' ')+": ".PadLeft(4, ' '));
            for (int i = 0; i < 5; i++)
            {
                if (i < PlayerBattleVal[3])
                {
                    Console.Write("■");

                }
                else
                {
                    Console.Write("□");

                }
            }
            Console.WriteLine();
        } // PlayerHpVal()

        // 몬스터 hp 보여주는 함수
        public void MonsterHpVal()
        {
            Console.Write("HP : ");
            for (int i = 0; i < monster.Monsterhp(); i++)
            {
                if (i < MonsterBattleVal[0])
                {
                    Console.Write("■");
                }
                else
                {
                    Console.Write("□");

                }
            }
            Console.WriteLine();
        } // MonsterHpVal()



        // 플레이어 인벤토리 확인
        public void PlayeritemsSet()
        {
            for (int y = 0; y < BOARDY; y++)
            {
                for (int x = 0; x < BOARDX; x++)
                {
                    gameboard[y, x] = 0;

                    if (x == 1)
                    {
                        gameboard[y, x] = -1;

                    }


                    // 테두리
                    if (x == 0 || x == 30 || y == 0 || y == 30)
                    {
                        gameboard[y, x] = -2;

                    }


                    //// 제목 좌표
                    //if (x == 1 && y == 8)
                    //{
                    //    gameboard[y, x] = -3;

                    //}

                    // 왼쪽 화살표 좌표
                    if (x == 1 && y == 12)
                    {
                        gameboard[y, x] = 2;

                    }



                    // 아이템 앞 공백 좌표

                    if (x == 1 && ( y == 13 || y == 14 || y == 15 || y == 16 || y == 17 || y == 18 || y == 19 || y == 20))
                    {
                        gameboard[y, x] = 5;
                    }


                    // 아이템 오른쪽 공백 좌표
                    if (x == 2 && ( y == 12 || y == 13 || y == 14 || y == 15 || y == 16 || y == 17 || y == 18 || y == 19 || y == 20))
                    {
                        gameboard[y, x] = -5;

                    }

                    // 돌아가기 앞 공백 좌표

                    if (x == 1 && y == 27)
                    {
                        gameboard[y, x] = 3;

                    }

                    

                    // 돌아가기 좌표
                    if (x == 2 && y == 27)
                    {
                        gameboard[y, x] = 13;

                    }



                }
            }

            // 화살표 좌표 저장
            ArrowY = 12;
            ArrowX = 1;

            
        } // PlayeritemsSet()

        public void Playeritems()
        {
            

            for (int y = 0; y < BOARDY; y++)
            {
                for (int x = 0; x < BOARDX; x++)
                {
                    switch (gameboard[y, x])
                    {

                        //case -3:
                        //    Console.Write($"[{PlayerName} 의 인벤토리]".PadLeft(45, ' ') + "".PadRight(33, ' '));
                        //    break;

                        case -2:
                            Console.Write("■".PadRight(2, ' '));
                            break;

                        case -1:
                            Console.Write("".PadLeft(87, ' '));
                            break;

                        case 2:
                            Console.Write("▶".PadLeft(21, ' '));
                            break;
                        case 3:
                            Console.Write(" ".PadLeft(35, ' '));
                            break;

                        // 오른쪽 스탯 화살표
                        case 4:
                            Console.Write("▶".PadLeft(29, ' ')+"".PadRight(35, ' '));
                            break;
                        case 5:
                            Console.Write(" ".PadLeft(22, ' '));
                            break;
                        case -5:
                            Console.Write("".PadLeft(30, ' ') + "".PadRight(35, ' '));
                            break;
                        
                        
                        case 13:
                            Console.Write("돌아 가기".PadLeft(9, ' ') + "".PadRight(39, ' '));
                            break;
                        case 15:
                            Console.Write("▶".PadLeft(34, ' '));
                            break;

                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.SetCursorPosition(40, 8);
            Console.WriteLine($"[{PlayerName} 의 인벤토리]");
            Console.SetCursorPosition(35, 10);
            Console.WriteLine($"[현재 {PlayerName} 의 소지한 골드 : {PlayeritemVal[0]}]");

            int loopCount = 0;
            

            for (int i = 0; i < Playeritem.Count; i++)
            {

                if (i % 2 == 0)
                {
                    Console.SetCursorPosition(27, 12 + loopCount);
                    Console.Write($"{Playeritem[i]} : ");

                    if(Playeritem[i] == "늑대의 가죽")
                    {
                        Console.Write($"{PlayeritemVal[2]}");
                    }
                    else if(Playeritem[i] == "그레이트 소드")
                    {
                        Console.Write($"{PlayeritemVal[9]}");

                    }
                    else if (Playeritem[i] == "판금 갑옷")
                    {
                        Console.Write($"{PlayeritemVal[10]}");

                    }
                    else if(Playeritem[i] == "숙련된 전사의 가호")
                    {
                        Console.Write($"{PlayeritemVal[11]}");

                    }
                    else if (Playeritem[i] == "기본용 양손검")
                    {
                        Console.Write($"{PlayeritemVal[6]}");

                    }
                    else if (Playeritem[i] == "기본용 갑옷")
                    {
                        Console.Write($"{PlayeritemVal[7]}");

                    }
                    else if (Playeritem[i] == "기본용 전사의 가호")
                    {
                        Console.Write($"{PlayeritemVal[8]}");

                    }
                }
                else
                {
                    Console.SetCursorPosition(57, 12 + loopCount);
                    Console.Write($"{Playeritem[i]} : ");
                    if (Playeritem[i] == "늑대의 가죽")
                    {
                        Console.Write($"{PlayeritemVal[2]}");
                    }
                    else if (Playeritem[i] == "그레이트 소드")
                    {
                        Console.Write($"{PlayeritemVal[9]}");

                    }
                    else if (Playeritem[i] == "판금 갑옷")
                    {
                        Console.Write($"{PlayeritemVal[10]}");

                    }
                    else if (Playeritem[i] == "숙련된 전사의 가호")
                    {
                        Console.Write($"{PlayeritemVal[11]}");

                    }
                    else if (Playeritem[i] == "기본용 양손검")
                    {
                        Console.Write($"{PlayeritemVal[6]}");

                    }
                    else if (Playeritem[i] == "기본용 갑옷")
                    {
                        Console.Write($"{PlayeritemVal[7]}");

                    }
                    else if (Playeritem[i] == "기본용 전사의 가호")
                    {
                        Console.Write($"{PlayeritemVal[8]}");

                    }
                    loopCount++;
                }
            }



            Console.SetCursorPosition(0, 31);

            ConsoleKeyInfo Move;

            Move = Console.ReadKey();

            switch (Move.Key)
            {
                case ConsoleKey.W:
                    if (ArrowY == 12) { /* Do Nothing */ }
                    else
                    {
                        if (ArrowX == 1)
                        {
                            if (ArrowY < 21)
                            {

                                gameboard[ArrowY, 1] = 5;
                                ArrowY--;
                                gameboard[ArrowY, 1] = 2;

                            }

                            else if (ArrowY == 27)
                            {
                                gameboard[ArrowY, 1] = 3;
                                ArrowY -= 7;
                                gameboard[ArrowY, 1] = 2;
                            }

                        }

                        else
                        {
                            gameboard[ArrowY, ArrowX] = -5;

                            ArrowY--;
                            gameboard[ArrowY, ArrowX] = 4;

                        }
                    }

                    break;
                case ConsoleKey.A:
                    if (ArrowX == 1) { /* Do Nothing */ }
                    else
                    {
                        gameboard[ArrowY, ArrowX] = -5;

                        ArrowX--;
                        gameboard[ArrowY, ArrowX] = 2;
                    }
                    break;

                case ConsoleKey.S:
                    if (ArrowY == 27) { /* Do Nothing */ }
                    else
                    {
                        if (ArrowX == 1)
                        {
                            if (ArrowY < 20)
                            {
                                gameboard[ArrowY, 1] = 5;
                                ArrowY++;
                                gameboard[ArrowY, 1] = 2;

                            }

                            else if (ArrowY == 20)
                            {
                                gameboard[ArrowY, 1] = 5;
                                ArrowY += 7;
                                gameboard[ArrowY, 1] = 15;
                            }

                        }

                        else
                        {
                            if (ArrowY == 20)
                            {
                                gameboard[ArrowY, ArrowX] = -5;
                                ArrowY += 7;
                                ArrowX--;
                                gameboard[ArrowY, ArrowX] = 15;
                            }
                            else
                            {
                                gameboard[ArrowY, ArrowX] = -5;

                                ArrowY++;
                                gameboard[ArrowY, ArrowX] = 4;
                            }
                        }
                    }


                    break;
                case ConsoleKey.D:
                    if (ArrowX == 2) { /* Do Nothing */ }
                    else
                    {
                        if (ArrowY == 27) { /* Do Nothing */ }
                        else
                        {

                            gameboard[ArrowY, ArrowX] = 5;

                            ArrowX++;
                            gameboard[ArrowY, ArrowX] = 4;
                        }
                    }
                    break;

                default:
                    
                    if (gameboard[ArrowY, ArrowX] == 2)
                    {
                        int chkY = ArrowY - 12;
                        int ArrayVal = chkY * 2;

                        if (ArrayVal < Playeritem.Count)
                        {
                            Console.SetCursorPosition(33, 22);
                            Console.WriteLine($"{playeritemsIndex[Playeritem[ArrayVal]]}");


                            if (Playeritem[ArrayVal] == "기본용 양손검")
                            {
                                Console.SetCursorPosition(33, 23);

                                Console.WriteLine("장착하시겠습니까?");
                                Console.SetCursorPosition(33, 24);
                                Console.WriteLine("왼쪽 입력 : 장착하기, 오른쪽 입력 : 아니요.");

                                Console.SetCursorPosition(0, 31);
                                Move = Console.ReadKey();

                                switch (Move.Key)
                                {
                                    case ConsoleKey.A:
                                        string temp;
                                        temp = PlayerWearWeps[0];
                                        PlayerWearWeps[0] = Playeritem[ArrayVal];
                                        Playeritem[ArrayVal] = temp;

                                        Console.SetCursorPosition(33, 25);
                                        Console.WriteLine($"{PlayerWearWeps[0]} 장착 완료되었습니다.");

                                        Console.SetCursorPosition(0, 31);
                                        Move = Console.ReadKey();
                                        break;

                                    case ConsoleKey.D:
                                        Console.SetCursorPosition(33, 25);
                                        Console.WriteLine($"장착하지 않았습니다.");

                                        Console.SetCursorPosition(0, 31);
                                        Move = Console.ReadKey();
                                        break;
                                }
                            }

                            else if (Playeritem[ArrayVal] == "그레이트 소드")
                            {
                                Console.SetCursorPosition(33, 23);

                                Console.WriteLine("장착하시겠습니까?");
                                Console.SetCursorPosition(33, 24);
                                Console.WriteLine("왼쪽 입력 : 장착하기, 오른쪽 입력 : 아니요.");

                                Console.SetCursorPosition(0, 31);
                                Move = Console.ReadKey();

                                switch (Move.Key)
                                {
                                    case ConsoleKey.A:
                                        string temp;
                                        temp = PlayerWearWeps[0];
                                        PlayerWearWeps[0] = Playeritem[ArrayVal];
                                        Playeritem[ArrayVal] = temp;

                                        Console.SetCursorPosition(33, 25);
                                        Console.WriteLine($"{PlayerWearWeps[0]} 장착 완료되었습니다.");

                                        Console.SetCursorPosition(0, 31);
                                        Move = Console.ReadKey();
                                        break;

                                    case ConsoleKey.D:
                                        Console.SetCursorPosition(33, 25);
                                        Console.WriteLine($"장착하지 않았습니다.");

                                        Console.SetCursorPosition(0, 31);
                                        Move = Console.ReadKey();
                                        break;
                                }
                            }

                            else if (Playeritem[ArrayVal] == "기본용 갑옷")
                            {
                                Console.SetCursorPosition(33, 23);

                                Console.WriteLine("장착하시겠습니까?");
                                Console.SetCursorPosition(33, 24);
                                Console.WriteLine("왼쪽 입력 : 장착하기, 오른쪽 입력 : 아니요.");

                                Console.SetCursorPosition(0, 31);
                                Move = Console.ReadKey();

                                switch (Move.Key)
                                {
                                    case ConsoleKey.A:
                                        string temp;
                                        temp = PlayerWearWeps[1];
                                        PlayerWearWeps[1] = Playeritem[ArrayVal];
                                        Playeritem[ArrayVal] = temp;

                                        Console.SetCursorPosition(33, 25);
                                        Console.WriteLine($"{PlayerWearWeps[1]} 장착 완료되었습니다.");

                                        Console.SetCursorPosition(0, 31);
                                        Move = Console.ReadKey();
                                        break;

                                    case ConsoleKey.D:
                                        Console.SetCursorPosition(33, 25);
                                        Console.WriteLine($"장착하지 않았습니다.");

                                        Console.SetCursorPosition(0, 31);
                                        Move = Console.ReadKey();
                                        break;
                                }
                            }
                            else if (Playeritem[ArrayVal] == "판금 갑옷")
                            {
                                Console.SetCursorPosition(33, 23);

                                Console.WriteLine("장착하시겠습니까?");
                                Console.SetCursorPosition(33, 24);
                                Console.WriteLine("왼쪽 입력 : 장착하기, 오른쪽 입력 : 아니요.");

                                Console.SetCursorPosition(0, 31);
                                Move = Console.ReadKey();

                                switch (Move.Key)
                                {
                                    case ConsoleKey.A:
                                        string temp;
                                        temp = PlayerWearWeps[1];
                                        PlayerWearWeps[1] = Playeritem[ArrayVal];
                                        Playeritem[ArrayVal] = temp;

                                        Console.SetCursorPosition(33, 25);
                                        Console.WriteLine($"{PlayerWearWeps[1]} 장착 완료되었습니다.");

                                        Console.SetCursorPosition(0, 31);
                                        Move = Console.ReadKey();
                                        break;

                                    case ConsoleKey.D:
                                        Console.SetCursorPosition(33, 25);
                                        Console.WriteLine($"장착하지 않았습니다.");

                                        Console.SetCursorPosition(0, 31);
                                        Move = Console.ReadKey();
                                        break;
                                }
                            }
                            else if (Playeritem[ArrayVal] == "기본용 전사의 가호")
                            {
                                Console.SetCursorPosition(33, 23);

                                Console.WriteLine("장착하시겠습니까?");
                                Console.SetCursorPosition(33, 24);
                                Console.WriteLine("왼쪽 입력 : 장착하기, 오른쪽 입력 : 아니요.");

                                Console.SetCursorPosition(0, 31);
                                Move = Console.ReadKey();

                                switch (Move.Key)
                                {
                                    case ConsoleKey.A:
                                        string temp;
                                        temp = PlayerWearWeps[2];
                                        PlayerWearWeps[2] = Playeritem[ArrayVal];
                                        Playeritem[ArrayVal] = temp;

                                        Console.SetCursorPosition(33, 25);
                                        Console.WriteLine($"{PlayerWearWeps[2]} 장착 완료되었습니다.");

                                        Console.SetCursorPosition(0, 31);
                                        Move = Console.ReadKey();
                                        break;

                                    case ConsoleKey.D:
                                        Console.SetCursorPosition(33, 25);
                                        Console.WriteLine($"장착하지 않았습니다.");

                                        Console.SetCursorPosition(0, 31);
                                        Move = Console.ReadKey();
                                        break;
                                }
                            }

                            else if (Playeritem[ArrayVal] == "숙련된 전사의 가호")
                            {
                                Console.SetCursorPosition(33, 23);

                                Console.WriteLine("장착하시겠습니까?");
                                Console.SetCursorPosition(33, 24);
                                Console.WriteLine("왼쪽 입력 : 장착하기, 오른쪽 입력 : 아니요.");

                                Console.SetCursorPosition(0, 31);
                                Move = Console.ReadKey();

                                switch (Move.Key)
                                {
                                    case ConsoleKey.A:
                                        string temp;
                                        temp = PlayerWearWeps[2];
                                        PlayerWearWeps[2] = Playeritem[ArrayVal];
                                        Playeritem[ArrayVal] = temp;

                                        Console.SetCursorPosition(33, 25);
                                        Console.WriteLine($"{PlayerWearWeps[2]} 장착 완료되었습니다.");

                                        Console.SetCursorPosition(0, 31);
                                        Move = Console.ReadKey();
                                        break;

                                    case ConsoleKey.D:
                                        Console.SetCursorPosition(33, 25);
                                        Console.WriteLine($"장착하지 않았습니다.");

                                        Console.SetCursorPosition(0, 31);
                                        Move = Console.ReadKey();
                                        break;
                                }
                            }

                        }
                        else
                        {
                            /* Do Nothing */
                        }
                    }

                    else if (gameboard[ArrowY, ArrowX] == 4)
                    {
                        int chkY = ArrowY - 12;
                        int ArrayVal = (chkY * 2) + 1;

                        if (ArrayVal < Playeritem.Count)
                        {
                            Console.SetCursorPosition(33, 22);
                            Console.WriteLine($"{playeritemsIndex[Playeritem[ArrayVal]]}");

                            if (Playeritem[ArrayVal] == "기본용 양손검")
                            {
                                Console.SetCursorPosition(33, 23);

                                Console.WriteLine("장착하시겠습니까?");
                                Console.SetCursorPosition(33, 24);
                                Console.WriteLine("왼쪽 입력 : 장착하기, 오른쪽 입력 : 아니요.");

                                Console.SetCursorPosition(0, 31);
                                Move = Console.ReadKey();

                                switch (Move.Key)
                                {
                                    case ConsoleKey.A:
                                        string temp;
                                        temp = PlayerWearWeps[0];
                                        PlayerWearWeps[0] = Playeritem[ArrayVal];
                                        Playeritem[ArrayVal] = temp;

                                        Console.SetCursorPosition(33, 25);
                                        Console.WriteLine($"{PlayerWearWeps[0]} 장착 완료되었습니다.");

                                        Console.SetCursorPosition(0, 31);
                                        Move = Console.ReadKey();
                                        break;

                                    case ConsoleKey.D:
                                        Console.SetCursorPosition(33, 25);
                                        Console.WriteLine($"장착하지 않았습니다.");

                                        Console.SetCursorPosition(0, 31);
                                        Move = Console.ReadKey();
                                        break;
                                }
                            }

                            else if (Playeritem[ArrayVal] == "그레이트 소드")
                            {
                                Console.SetCursorPosition(33, 23);

                                Console.WriteLine("장착하시겠습니까?");
                                Console.SetCursorPosition(33, 24);
                                Console.WriteLine("왼쪽 입력 : 장착하기, 오른쪽 입력 : 아니요.");

                                Console.SetCursorPosition(0, 31);
                                Move = Console.ReadKey();

                                switch (Move.Key)
                                {
                                    case ConsoleKey.A:
                                        string temp;
                                        temp = PlayerWearWeps[0];
                                        PlayerWearWeps[0] = Playeritem[ArrayVal];
                                        Playeritem[ArrayVal] = temp;

                                        Console.SetCursorPosition(33, 25);
                                        Console.WriteLine($"{PlayerWearWeps[0]} 장착 완료되었습니다.");

                                        Console.SetCursorPosition(0, 31);
                                        Move = Console.ReadKey();
                                        break;

                                    case ConsoleKey.D:
                                        Console.SetCursorPosition(33, 25);
                                        Console.WriteLine($"장착하지 않았습니다.");

                                        Console.SetCursorPosition(0, 31);
                                        Move = Console.ReadKey();
                                        break;
                                }
                            }

                            else if (Playeritem[ArrayVal] == "기본용 갑옷")
                            {
                                Console.SetCursorPosition(33, 23);

                                Console.WriteLine("장착하시겠습니까?");
                                Console.SetCursorPosition(33, 24);
                                Console.WriteLine("왼쪽 입력 : 장착하기, 오른쪽 입력 : 아니요.");

                                Console.SetCursorPosition(0, 31);
                                Move = Console.ReadKey();

                                switch (Move.Key)
                                {
                                    case ConsoleKey.A:
                                        string temp;
                                        temp = PlayerWearWeps[1];
                                        PlayerWearWeps[1] = Playeritem[ArrayVal];
                                        Playeritem[ArrayVal] = temp;

                                        Console.SetCursorPosition(33, 25);
                                        Console.WriteLine($"{PlayerWearWeps[1]} 장착 완료되었습니다.");

                                        Console.SetCursorPosition(0, 31);
                                        Move = Console.ReadKey();
                                        break;

                                    case ConsoleKey.D:
                                        Console.SetCursorPosition(33, 25);
                                        Console.WriteLine($"장착하지 않았습니다.");

                                        Console.SetCursorPosition(0, 31);
                                        Move = Console.ReadKey();
                                        break;
                                }
                            }
                            else if (Playeritem[ArrayVal] == "판금 갑옷")
                            {
                                Console.SetCursorPosition(33, 23);

                                Console.WriteLine("장착하시겠습니까?");
                                Console.SetCursorPosition(33, 24);
                                Console.WriteLine("왼쪽 입력 : 장착하기, 오른쪽 입력 : 아니요.");

                                Console.SetCursorPosition(0, 31);
                                Move = Console.ReadKey();

                                switch (Move.Key)
                                {
                                    case ConsoleKey.A:
                                        string temp;
                                        temp = PlayerWearWeps[1];
                                        PlayerWearWeps[1] = Playeritem[ArrayVal];
                                        Playeritem[ArrayVal] = temp;

                                        Console.SetCursorPosition(33, 25);
                                        Console.WriteLine($"{PlayerWearWeps[1]} 장착 완료되었습니다.");

                                        Console.SetCursorPosition(0, 31);
                                        Move = Console.ReadKey();
                                        break;

                                    case ConsoleKey.D:
                                        Console.SetCursorPosition(33, 25);
                                        Console.WriteLine($"장착하지 않았습니다.");

                                        Console.SetCursorPosition(0, 31);
                                        Move = Console.ReadKey();
                                        break;
                                }
                            }
                            else if (Playeritem[ArrayVal] == "기본용 전사의 가호")
                            {
                                Console.SetCursorPosition(33, 23);

                                Console.WriteLine("장착하시겠습니까?");
                                Console.SetCursorPosition(33, 24);
                                Console.WriteLine("왼쪽 입력 : 장착하기, 오른쪽 입력 : 아니요.");

                                Console.SetCursorPosition(0, 31);
                                Move = Console.ReadKey();

                                switch (Move.Key)
                                {
                                    case ConsoleKey.A:
                                        string temp;
                                        temp = PlayerWearWeps[2];
                                        PlayerWearWeps[2] = Playeritem[ArrayVal];
                                        Playeritem[ArrayVal] = temp;

                                        Console.SetCursorPosition(33, 25);
                                        Console.WriteLine($"{PlayerWearWeps[2]} 장착 완료되었습니다.");

                                        Console.SetCursorPosition(0, 31);
                                        Move = Console.ReadKey();
                                        break;

                                    case ConsoleKey.D:
                                        Console.SetCursorPosition(33, 25);
                                        Console.WriteLine($"장착하지 않았습니다.");

                                        Console.SetCursorPosition(0, 31);
                                        Move = Console.ReadKey();
                                        break;
                                }
                            }

                            else if (Playeritem[ArrayVal] == "숙련된 전사의 가호")
                            {
                                Console.SetCursorPosition(33, 23);

                                Console.WriteLine("장착하시겠습니까?");
                                Console.SetCursorPosition(33, 24);
                                Console.WriteLine("왼쪽 입력 : 장착하기, 오른쪽 입력 : 아니요.");

                                Console.SetCursorPosition(0, 31);
                                Move = Console.ReadKey();

                                switch (Move.Key)
                                {
                                    case ConsoleKey.A:
                                        string temp;
                                        temp = PlayerWearWeps[2];
                                        PlayerWearWeps[2] = Playeritem[ArrayVal];
                                        Playeritem[ArrayVal] = temp;

                                        Console.SetCursorPosition(33, 25);
                                        Console.WriteLine($"{PlayerWearWeps[2]} 장착 완료되었습니다.");

                                        Console.SetCursorPosition(0, 31);
                                        Move = Console.ReadKey();
                                        break;

                                    case ConsoleKey.D:
                                        Console.SetCursorPosition(33, 25);
                                        Console.WriteLine($"장착하지 않았습니다.");

                                        Console.SetCursorPosition(0, 31);
                                        Move = Console.ReadKey();
                                        break;
                                }
                            }
                        }
                        else
                        {
                            /* Do Nothing */
                        }
                    }

                    


                    // 돌아 가기
                    else if (gameboard[27, 1] == 15)
                    {
                        // 마을, 모험, 아이템, 끝내기 선택지
                        GameboardClear();
                        ChoiceActSet();

                        while (ChoiceCheck == false)
                        {
                            Console.SetCursorPosition(0, 0);
                            ChoiceAct();

                        }
                    }
                    break;

            }

        } // Playeritems()





        // 상점 좌표 구현
        public void StoreSet()
        {
            for (int y = 0; y < BOARDY; y++)
            {
                for (int x = 0; x < BOARDX; x++)
                {
                    gameboard[y, x] = 0;

                    if (x == 1)
                    {
                        gameboard[y, x] = -1;

                    }


                    // 테두리
                    if (x == 0 || x == 30 || y == 0 || y == 30)
                    {
                        gameboard[y, x] = -2;

                    }


                    //// 제목 좌표
                    //if (x == 1 && y == 8)
                    //{
                    //    gameboard[y, x] = -3;

                    //}

                    // 왼쪽 화살표 좌표
                    if (x == 1 && y == 12)
                    {
                        gameboard[y, x] = 2;

                    }



                    // 아이템 앞 공백 좌표

                    if (x == 1 && (y == 13 || y == 14 || y == 15 || y == 16 || y == 17 || y == 18 || y == 19 || y == 20))
                    {
                        gameboard[y, x] = 5;
                    }


                    // 아이템 오른쪽 공백 좌표
                    if (x == 2 && (y == 12 || y == 13 || y == 14 || y == 15 || y == 16 || y == 17 || y == 18 || y == 19 || y == 20))
                    {
                        gameboard[y, x] = -5;

                    }

                    // 돌아가기 앞 공백 좌표

                    if (x == 1 && y == 27)
                    {
                        gameboard[y, x] = 3;

                    }



                    // 마을로 돌아가기 좌표
                    if (x == 2 && y == 27)
                    {
                        gameboard[y, x] = 13;

                    }



                }
            }

            // 화살표 좌표 저장
            ArrowY = 12;
            ArrowX = 1;
        } // StoreSet()



        // 상점 구현 좌표
        public void Store()
        {
            for (int y = 0; y < BOARDY; y++)
            {
                for (int x = 0; x < BOARDX; x++)
                {
                    switch (gameboard[y, x])
                    {

                        case -2:
                            Console.Write("■".PadRight(2, ' '));
                            break;

                        case -1:
                            Console.Write("".PadLeft(87, ' '));
                            break;

                        case 2:
                            Console.Write("▶".PadLeft(21, ' '));
                            break;
                        case 3:
                            Console.Write(" ".PadLeft(35, ' '));
                            break;

                        // 오른쪽 스탯 화살표
                        case 4:
                            Console.Write("▶".PadLeft(29, ' ') + "".PadRight(35, ' '));
                            break;
                        case 5:
                            Console.Write(" ".PadLeft(22, ' '));
                            break;
                        case -5:
                            Console.Write("".PadLeft(30, ' ') + "".PadRight(35, ' '));
                            break;


                        case 13:
                            Console.Write("돌아 가기".PadLeft(9, ' ') + "".PadRight(39, ' '));
                            break;
                        case 15:
                            Console.Write("▶".PadLeft(34, ' '));
                            break;

                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.SetCursorPosition(40, 8);
            Console.WriteLine($"[마을 상점]");
            Console.SetCursorPosition(35, 10);
            Console.WriteLine($"[현재 {PlayerName} 의 소지한 골드 : {PlayeritemVal[0]}]");


            int loopCount = 0;

            for (int i = 0; i < StoreVal.Count; i++)
            {

                if (i % 2 == 0)
                {
                    Console.SetCursorPosition(27, 12 + loopCount);
                    Console.Write($"{StoreVal[i]}");
                }
                else
                {
                    Console.SetCursorPosition(57, 12 + loopCount); 
                    Console.Write($"{StoreVal[i]}");
                    loopCount++;
                }
            }



            Console.SetCursorPosition(0, 31);

            ConsoleKeyInfo Move;

            Move = Console.ReadKey();

            switch (Move.Key)
            {
                case ConsoleKey.W:
                    if (ArrowY == 12) { /* Do Nothing */ }
                    else
                    {
                        if (ArrowX == 1)
                        {
                            if (ArrowY < 21)
                            {

                                gameboard[ArrowY, 1] = 5;
                                ArrowY--;
                                gameboard[ArrowY, 1] = 2;

                            }

                            else if (ArrowY == 27)
                            {
                                gameboard[ArrowY, 1] = 3;
                                ArrowY -= 7;
                                gameboard[ArrowY, 1] = 2;
                            }

                        }

                        else
                        {
                            gameboard[ArrowY, ArrowX] = -5;

                            ArrowY--;
                            gameboard[ArrowY, ArrowX] = 4;

                        }
                    }

                    break;
                case ConsoleKey.A:
                    if (ArrowX == 1) { /* Do Nothing */ }
                    else
                    {
                        gameboard[ArrowY, ArrowX] = -5;

                        ArrowX--;
                        gameboard[ArrowY, ArrowX] = 2;
                    }
                    break;

                case ConsoleKey.S:
                    if (ArrowY == 27) { /* Do Nothing */ }
                    else
                    {
                        if (ArrowX == 1)
                        {
                            if (ArrowY < 20)
                            {
                                gameboard[ArrowY, 1] = 5;
                                ArrowY++;
                                gameboard[ArrowY, 1] = 2;

                            }

                            else if (ArrowY == 20)
                            {
                                gameboard[ArrowY, 1] = 5;
                                ArrowY += 7;
                                gameboard[ArrowY, 1] = 15;
                            }

                        }

                        else
                        {
                            if (ArrowY == 20)
                            {
                                gameboard[ArrowY, ArrowX] = -5;
                                ArrowY += 7;
                                ArrowX--;
                                gameboard[ArrowY, ArrowX] = 15;
                            }
                            else
                            {
                                gameboard[ArrowY, ArrowX] = -5;

                                ArrowY++;
                                gameboard[ArrowY, ArrowX] = 4;
                            }
                        }
                    }


                    break;
                case ConsoleKey.D:
                    if (ArrowX == 2) { /* Do Nothing */ }
                    else
                    {
                        if (ArrowY == 27) { /* Do Nothing */ }
                        else
                        {

                            gameboard[ArrowY, ArrowX] = 5;

                            ArrowX++;
                            gameboard[ArrowY, ArrowX] = 4;
                        }
                    }
                    break;

                default:

                    if (gameboard[ArrowY, ArrowX] == 2)
                    {
                        
                        int chkY = ArrowY - 12;
                        int ArrayVal = chkY * 2;

                        if (ArrayVal < StoreVal.Count)
                        {
                            if (ArrowY == 12)
                            {
                                weps = new warrior4();
                            }
                            else if (ArrowY == 13)
                            {
                                weps = new warrior6();
                            }


                            Console.SetCursorPosition(33, 22);
                            Console.WriteLine($"{playeritemsIndex[StoreVal[ArrayVal]]}");
                            Console.SetCursorPosition(33, 23);
                            Console.WriteLine($"구매하시겠습니까? (가격 : {weps.Wepgold()} 골드 )");
                            Console.SetCursorPosition(33, 24);
                            Console.WriteLine("왼쪽 입력 -> 구매, 오른쪽 입력 -> 구매 취소");
                            Console.SetCursorPosition(0, 31);

                            Move = Console.ReadKey();


                            bool twocheck = false;
                            switch (Move.Key)
                            {
                                case ConsoleKey.A:
                                    for (int i = 0; i < Playeritem.Count; i++)
                                    {
                                        if (Playeritem[i] == weps.Wepitem())
                                        {
                                            twocheck = true;


                                        }

                                    }
                                    if (twocheck == true)
                                    {
                                        Console.SetCursorPosition(33, 25);

                                        Console.WriteLine("이미 같은 물건을 가지고 있습니다.");
                                        Console.SetCursorPosition(0, 31);

                                        Move = Console.ReadKey();
                                    }

                                    else if (PlayerWearWeps[0] == weps.Wepitem() || PlayerWearWeps[1] == weps.Wepitem() || PlayerWearWeps[2] == weps.Wepitem())
                                    {
                                        Console.SetCursorPosition(33, 25);

                                        Console.WriteLine("이미 같은 물건을 가지고 있습니다.");
                                        Console.SetCursorPosition(0, 31);

                                        Move = Console.ReadKey();
                                    }

                                    else if (weps.Wepgold() <= PlayeritemVal[0])
                                    {
                                        PlayeritemVal[0] -= weps.Wepgold();
                                        Playeritem.Add(weps.Wepitem());

                                        if (weps.Wepitem() == "그레이트 소드")
                                        {
                                            PlayeritemVal[9]++;
                                        }

                                        else if (weps.Wepitem() == "숙련된 전사의 가호")
                                        {
                                            PlayeritemVal[11]++;

                                        }


                                        Console.SetCursorPosition(33, 25);
                                        Console.WriteLine($"{weps.Wepitem()}. 구매에 성공했습니다.");
                                        Console.SetCursorPosition(0, 31);

                                        Move = Console.ReadKey();


                                    }
                                    else
                                    {
                                        Console.SetCursorPosition(33, 25);
                                        Console.WriteLine("돈이 부족합니다. 구매에 실패했습니다.");
                                        Console.SetCursorPosition(0, 31);

                                        Move = Console.ReadKey();

                                    }
                                    break;
                                case ConsoleKey.D:
                                    Console.SetCursorPosition(33, 25);
                                    Console.WriteLine("구매를 취소했습니다.");
                                    Console.SetCursorPosition(0, 31);

                                    Move = Console.ReadKey();
                                    break;

                            }

                        }
                        else
                        {
                            /* Do Nothing */
                        }
                    }

                    else if (gameboard[ArrowY, ArrowX] == 4)
                    {

                        
                        int chkY = ArrowY - 12;
                        int ArrayVal = (chkY * 2) + 1;


                        if (ArrayVal < StoreVal.Count)
                        {
                            if (ArrowY == 12)
                            {
                                weps = new warrior5();
                            }


                            Console.SetCursorPosition(33, 22);
                            Console.WriteLine($"{playeritemsIndex[StoreVal[ArrayVal]]}");
                            Console.SetCursorPosition(33, 23);
                            Console.WriteLine($"구매하시겠습니까? (가격 : {weps.Wepgold()} 골드 )");
                            Console.SetCursorPosition(33, 24);
                            Console.WriteLine("왼쪽 입력 -> 구매, 오른쪽 입력 -> 구매 취소");
                            Console.SetCursorPosition(0, 31);

                            Move = Console.ReadKey();


                            bool twocheck = false;
                            switch (Move.Key)
                            {
                                case ConsoleKey.A:
                                    for(int i = 0; i < Playeritem.Count; i++)
                                    {
                                        if(Playeritem[i] == weps.Wepitem())
                                        {
                                            twocheck = true;
                                            
                                            
                                        }
                                        
                                    }
                                    if(twocheck == true)
                                    {
                                        Console.SetCursorPosition(33, 25);

                                        Console.WriteLine("이미 같은 물건을 가지고 있습니다.");
                                        Console.SetCursorPosition(0, 31);

                                        Move = Console.ReadKey();
                                    }

                                    else if(PlayerWearWeps[0] == weps.Wepitem() || PlayerWearWeps[1] == weps.Wepitem() || PlayerWearWeps[2] == weps.Wepitem())
                                    {
                                        Console.SetCursorPosition(33, 25);

                                        Console.WriteLine("이미 같은 물건을 가지고 있습니다.");
                                        Console.SetCursorPosition(0, 31);

                                        Move = Console.ReadKey();
                                    }

                                    else if (weps.Wepgold() <= PlayeritemVal[0])
                                    {
                                        PlayeritemVal[0] -= weps.Wepgold();
                                        Playeritem.Add(weps.Wepitem());

                                        if (weps.Wepitem() == "판금 갑옷")
                                        {
                                            PlayeritemVal[10]++;
                                        }



                                        Console.SetCursorPosition(33, 25);
                                        Console.WriteLine($"{weps.Wepitem()}. 구매에 성공했습니다.");
                                        Console.SetCursorPosition(0, 31);

                                        Move = Console.ReadKey();


                                    }
                                    else
                                    {
                                        Console.SetCursorPosition(33, 25);
                                        Console.WriteLine("돈이 부족합니다. 구매에 실패했습니다.");
                                        Console.SetCursorPosition(0, 31);

                                        Move = Console.ReadKey();

                                    }
                                    break;
                                case ConsoleKey.D:
                                    Console.SetCursorPosition(33, 25);
                                    Console.WriteLine("구매를 취소했습니다.");
                                    Console.SetCursorPosition(0, 31);

                                    Move = Console.ReadKey();
                                    break;

                            }
                        }

                        else
                        {
                            /* Do Nothing */
                        }
                    }




                    // 마을로 돌아 가기
                    else if (gameboard[27, 1] == 15)
                    {


                        GameboardClear();
                        VillageCheckout = false;
                        VillageSet();
                        gameboard[23, 15] = -5;
                        gameboard[17, 21] = 2;

                        ArrowY = 17;
                        ArrowX = 21;
                        while (VillageCheckout == false)
                        {
                            Console.SetCursorPosition(0, 0);
                            Village();
                        }
                    }
                    break;

            }
        }


        public void StoreWepSet()
        {

            
            weps = new warrior1();
            playeritemsIndex.Add(weps.Wepitem(), weps.WepitemIndex());
            weps = new warrior2();
            playeritemsIndex.Add(weps.Wepitem(), weps.WepitemIndex());

            weps = new warrior3();
            playeritemsIndex.Add(weps.Wepitem(), weps.WepitemIndex());


            weps = new warrior4();
            StoreVal.Add(weps.Wepitem());
            playeritemsIndex.Add(weps.Wepitem(), weps.WepitemIndex());

            weps = new warrior5();
            StoreVal.Add(weps.Wepitem());
            playeritemsIndex.Add(weps.Wepitem(), weps.WepitemIndex());
            weps = new warrior6();
            StoreVal.Add(weps.Wepitem());
            playeritemsIndex.Add(weps.Wepitem(), weps.WepitemIndex());
        }

    } // class Games

    // 무기 클래스
    public class Weps
    {
        protected int wepdamage;
        protected int wepdefence;
        protected string wepitem;
        protected string wepitemIndex;
        protected int wepgold;
        protected int wepsoldgold;

        public string Wepitem()
        {
            return this.wepitem;
        }

        public int Wepdamage()
        {
            return this.wepdamage;
        }

        public int Wepdefence()
        {
            return this.wepdefence;
        }

        public string WepitemIndex()
        {
            return this.wepitemIndex;
        }

        public int Wepgold()
        {
            return this.wepgold;
        }
        public int Wepsoldgold()
        {
            return this.wepsoldgold;
        }

    }

    class warrior1 : Weps
    {
        public warrior1()
        {
            
            this.wepdamage = 5;
            this.wepdefence = 0;
            this.wepitem = "기본용 양손검";
            this.wepitemIndex = "초보자용 전사 무기";
            this.wepgold = 5;
            this.wepsoldgold = 5;
        }

    }

    class warrior2 : Weps
    {
        public warrior2()
        {

            this.wepdamage = 0;
            this.wepdefence = 5;
            this.wepitem = "기본용 갑옷";
            this.wepitemIndex = "초보자용 전사 갑옷";
            this.wepgold = 5;
            this.wepsoldgold = 5;
        }

    }

    class warrior3 : Weps
    {
        public warrior3()
        {

            this.wepdamage = 3;
            this.wepdefence = 3;
            this.wepitem = "기본용 전사의 가호";
            this.wepitemIndex = "초보자용 전사의 부적";
            this.wepgold = 5;
            this.wepsoldgold = 5;
        }

    }


    class warrior4 : Weps
    {
        public warrior4()
        {

            this.wepdamage = 15;
            this.wepdefence = 0;
            this.wepitem = "그레이트 소드";
            this.wepitemIndex = "숙련자 전사용 양손검";
            this.wepgold = 100;
            this.wepsoldgold = 10;
        }

    }

    class warrior5 : Weps
    {
        public warrior5()
        {

            this.wepdamage = 0;
            this.wepdefence = 15;
            this.wepitem = "판금 갑옷";
            this.wepitemIndex = "판금이 덮입혀진 갑옷";
            this.wepgold = 100;
            this.wepsoldgold = 10;
        }

    }

    class warrior6 : Weps
    {
        public warrior6()
        {

            this.wepdamage = 8;
            this.wepdefence = 8;
            this.wepitem = "숙련된 전사의 가호";
            this.wepitemIndex = "숙련된 전사의 부적";
            this.wepgold = 100;
            this.wepsoldgold = 10;
        }

    }



    // 몬스터 클래스
    public class Monster
    {
        protected string monstername;
        protected int monsterhp;
        protected int monsterdamage;
        protected int monsterdefence;
        protected string monsteritem;
        protected string monsteritemIndex;
        protected int monstergold;
        protected string monsterface;

        public void MonsterAtk()
        {
            Console.WriteLine($"{monstername} 이 공격했다.");
        }

        public string Monstername()
        {
            return this.monstername;
        }

        public int Monsterhp()
        {
            return this.monsterhp;
        }

        public int Monsterdamage()
        {
            return this.monsterdamage;
        }

        public int Monsterdefence()
        {
            return this.monsterdefence;
        }

        public string Monsteritem()
        {
            return this.monsteritem;
        }

        public string MonsteritemIndex()
        {
            return this.monsteritemIndex;
        }

        public int Monstergold()
        {
            return this.monstergold;
        }
        public string Monsterface()
        {
            return this.monsterface;
        }
    } // class Monster

    class Wolf : Monster
    {
        public Wolf()
        {
            this.monstername = "실버 울프";
            this.monsterhp = 5;
            this.monsterdamage = 10;
            this.monsterdefence = 10;
            this.monsteritem = "늑대의 가죽";
            this.monsteritemIndex = "회색빛이 감도는 늑대의 가죽.";
            this.monstergold = 50;
            this.monsterface = "◈▽◈";
        }
    }

    class Golem : Monster
    {
        public Golem()
        {
            this.monstername = "유적 골렘";
            this.monsterhp = 5;
            this.monsterdamage = 15;
            this.monsterdefence = 15;
            this.monsteritem = "유적 골렘의 파편";
            this.monsteritemIndex = "세월이 느껴지는 골렘의 파편.";
            this.monstergold = 100;
            this.monsterface = "▣ㅁ▣";
        }
    }


}
