using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RugbyLeague
{
    public class Registry
    {
        public const int TILE_SIZE_X = 16;
        public const int TILE_SIZE_Y = 16;

        public const float FOLLOW_LERP = 3.0f;

        

        //public const Vector2[] StartPositions_KickOffAttack = new Vector2[] {Vector2(

        public static Vector2[] StartPositions_KickOffAttack =
        {
            new Vector2(677, 1020),      //1. fullback
            new Vector2(90, 1020),     //2. right wing
            new Vector2(150, 1020),    //3. right centre
            new Vector2(1900, 1020),    //4. left centre
            new Vector2(2000, 1020),      //5. left wing
            new Vector2(600, 1020),     //6. five-eighth
            new Vector2(624, 1020),    //7. halfback
            new Vector2(600, 1020),    //8. left prop
            new Vector2(700, 1020),      //9. hooker 
            new Vector2(740, 1020),     //10. right prop
            new Vector2(780, 1020),    //11. left second row
            new Vector2(830, 1020),     //12. right second row
            new Vector2(900, 1020)     //13. lock
        };
        public static Vector2[] StartPositions_KickOffDefense =
        {
            new Vector2(677, 1520),      //1. fullback
            new Vector2(500, 1520),     //2. right wing
            new Vector2(600, 1520),    //3. right centre
            new Vector2(700, 1520),    //4. left centre
            new Vector2(110, 1520),      //5. left wing
            new Vector2(200, 1520),     //6. five-eighth
            new Vector2(300, 1520),    //7. halfback
            new Vector2(400, 1520),    //8. left prop
            new Vector2(500, 1520),      //9. hooker 
            new Vector2(555, 1520),     //10. right prop
            new Vector2(645, 1520),    //11. left second row
            new Vector2(688, 1520),     //12. right second row
            new Vector2(999, 1520)     //13. lock
        };


        public static GameSettings[] GameSettings =
        {
            //                          Name                Default,    GameValue,  Min,   Max,     increment   //Number id
            new GameSettings("Clouds",           10,         10,         1,      100,    1),         //0
            new GameSettings("Bombs",            4,          4,          0,      20,     1),         //1
            new GameSettings("Cave",             50,         50,         40,     60,     1),         //2
            new GameSettings("Regen Time",       10,         10,         3,      100,    1),         //3
            new GameSettings("Winning Score",    10,         10,         1,      100,    1),         //4
			new GameSettings("Run Speed",        120,        120,        50,     200,    10),        //5
            new GameSettings("Bullet Regen",     4,          4,          0,      100,    1),         //6
            new GameSettings("Power Up Time",    7,          7,          1,      20,     1),         //7
            new GameSettings("Bullet Velocity",  360,        360,        100,    600,    20),        //8
            new GameSettings("Randomonium",      0,          0,          0,      0,      0),         //9
			new GameSettings("Play Now",         0,          0,          0,      0,      0),         //10
        };


        public static void runCheat(string Cheat)
        {

        }

    }
}
