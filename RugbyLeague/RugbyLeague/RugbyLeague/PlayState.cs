using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using org.flixel;

using System.Linq;
using System.Xml.Linq;

namespace RugbyLeague
{
    public class PlayState : org.flixel.examples.BaseExampleState
    {

        //Define a playing field
        Field playingField;

        Ball ball;

        Team team1;
        Team team2;



        override public void create()
        {
            base.create();

            FlxG.mouse.hide();
            FlxG.hideHud();

            playingField = new Field(0, 0);
            add(playingField);


            ball = new Ball(78 * 8, 132 * 8);
            
            FlxG.follow(ball, Registry.FOLLOW_LERP);
            FlxG.followBounds(0, 0, 2048, 2048);



            team1 = new Team();
            team2 = new Team();

            // Create two teams of 7 robots;
            for (int i = 0; i < 13; i++)
            {
                //Player player = new Player(90 + (i * 80), (int)ball.y - 50, i + 1);
                Player player = new Player(
                    (int)Registry.StartPositions_KickOffAttack[i].X, 
                    (int)Registry.StartPositions_KickOffAttack[i].Y, 
                    i + 1, ball, team1);

                player.color = Color.LightBlue;
                team1.add(player);

                if (i == 6)
                {
                    player.isSelected = true;

                }
            }
            for (int i = 0; i < 13; i++)
            {
                //Player player = new Player(90 + (i * 80), (int)ball.y + 50, i + 1);

                Player player = new Player(
                (int)Registry.StartPositions_KickOffDefense[i].X,
                (int)Registry.StartPositions_KickOffDefense[i].Y,
                i + 1, ball, team2);

                player.color = Color.LightCyan;
                player.mode = Player.MODE_DEFENSE;
                team2.add(player);


                //if (i == 6) player.isSelected = true;
            }

            add(team1);
            add(team2);
            add(ball);
        }

        override public void update()
        {

            //if (FlxG.keys.justPressed(Keys.OemComma))
            //{
            //    for (int i = 0; i < team1.members.Count; i++)
            //    {
            //        if (((Player)team1.members[i]).isSelected == true)
            //        {
            //            ((Player)team1.members[i]).isSelected = false;
            //            ((Player)team1.members[i+1]).isSelected = true;
            //        }
            //    }
            //}
            //if (FlxG.keys.justPressed(Keys.OemPeriod))
            //{
            //    for (int i = 0; i < team1.members.Count; i++)
            //    {
            //        if (((Player)team1.members[i]).isSelected == true)
            //        {
            //            ((Player)team1.members[i]).isSelected = false;
            //            ((Player)team1.members[i - 1]).isSelected = true;
            //        }
            //    }
            //}

            FlxU.overlap(team1, team2, overlapped);

            FlxU.overlap(team1, ball, overlappedBall);
            FlxU.overlap(team2, ball, overlappedBall);


            base.update();
        }

        protected bool overlappedBall(object Sender, FlxSpriteCollisionEvent e)
        {
            //Console.WriteLine("Overlapped Ball " + e.Object2.ToString());

            //you can fire functions on each object.
            ((FlxObject)(e.Object1)).overlapped(e.Object2);
            ((FlxObject)(e.Object2)).overlapped(e.Object1);



            if (((Ball)(e.Object2)).timeSincePass > 0.25f && ((Ball)(e.Object2)).isHeld == false)
            {
                ((Player)(e.Object1)).hasBall = true;
                ((Player)(e.Object1)).isSelected = true;
                //team2.setPlayerModeTo(Player.MODE_ATTACK);
                
            }

            return true;
        }

        protected bool overlapped(object Sender, FlxSpriteCollisionEvent e)
        {
            //you can fire functions on each object.
            ((FlxObject)(e.Object1)).overlapped(e.Object2);
            ((FlxObject)(e.Object2)).overlapped(e.Object1);

            if (team1.teamHasBall())
            {
                //team2.setPlayerModeTo(Player.MODE_TACKLED);
                //team1.setPlayerModeTo(Player.MODE_PLAYTHEBALL);

            }
            if (team2.teamHasBall())
            {
                //team2.setPlayerModeTo(Player.MODE_PLAYTHEBALL);
                //team1.setPlayerModeTo(Player.MODE_TACKLED);
            }

            return true;
        }


    }
}
