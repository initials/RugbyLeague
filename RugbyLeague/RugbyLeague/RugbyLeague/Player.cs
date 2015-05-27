using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.flixel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace RugbyLeague
{
    class Player : FlxSprite
    {
        private int jerseyNumber;

        private FlxText jerseyText;
        private SelectedPlayerIcon selectedPlayerIcon;
        private Ball ball;
        private Team team;

        public bool isSelected;
        public bool hasBall;

        public const string MODE_ATTACK = "MODE_ATTACK";
        public const string MODE_DEFENSE= "MODE_DEFENSE";
        public const string MODE_TACKLED = "MODE_TACKLED";
        public const string MODE_PLAYTHEBALL = "MODE_PLAYTHEBALL";
        public const string MODE_WAIT = "MODE_WAIT";


        public float runSpeed;
        public float sideStep;

        public Player(int xPos, int yPos, int JerseyNumber, Ball ReferenceToBall, Team ReferenceToTeam)
            : base(xPos, yPos)
        {

            jerseyNumber = JerseyNumber;
            team = ReferenceToTeam;

            jerseyText = new FlxText(xPos, yPos, 50);
            jerseyText.text = jerseyNumber.ToString();
            jerseyText.setFormat(null, 1, Color.White, FlxJustification.Center, Color.Black);
            jerseyText.setScrollFactors(1, 1);

            selectedPlayerIcon = new SelectedPlayerIcon(xPos, yPos);

            ball = ReferenceToBall;


            //loadGraphic(FlxG.Content.Load<Texture2D>("player"), true, false, 32, 32);

            loadGraphic(FlxG.Content.Load<Texture2D>("run_hs"), true, false, 96/2, 96/2);


            //addAnimation("idle", new int[] { 0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52 }, (int)FlxU.random(12,24), true);
            //addAnimation("run", new int[] { 53, 54, 55, 56, 57, 58, 59, 60,61,62,63,64,65,66,67,68,69,70,71 }, 36, true);
            //addAnimation("tackled", new int[] { 53, 63 }, 24, true);
            //play("idle");

            addAnimation("idle", new int[] { 32 }, (int)FlxU.random(12,24), true);
            //addAnimation("run", new int[] { 32,33,34,35,36,37,38,39 }, 12, true);

            runSpeed = FlxU.random(150, 200);
            sideStep = FlxU.random(2, 20);

            int animSpeed = Convert.ToInt32(runSpeed / 10);

            addAnimation("runE", generateFrameNumbersBetween(0, 7), animSpeed, true);
            addAnimation("runN", generateFrameNumbersBetween(8, 15), animSpeed, true);
            addAnimation("runNE", generateFrameNumbersBetween(16, 23), animSpeed, true);
            addAnimation("runNW", generateFrameNumbersBetween(24, 31), animSpeed, true);
            addAnimation("runS", generateFrameNumbersBetween(32, 39), animSpeed, true);
            addAnimation("runSE", generateFrameNumbersBetween(40, 47), animSpeed, true);
            addAnimation("runSW", generateFrameNumbersBetween(48,55), animSpeed, true);
            addAnimation("runW", generateFrameNumbersBetween(56, 63), animSpeed, true);



            setDrags(1350, 1350);

            //angle = 90;

            isSelected = false;

            height = 32/2;
            width = 32/2;

            offset.X = 32/2;
            offset.Y = 48/2;

            //setOffset(9,9);


            boundingBoxOverride = true;

            mode = MODE_ATTACK;

        }

        override public void update()
        {
            if (isSelected)
            {
                //sidestep (aka juke)
                if (FlxG.keys.justPressed(Keys.M) || FlxG.gamepads.isNewButtonPress(Buttons.X))
                {
                    x += sideStep;
                }
                if (FlxG.keys.justPressed(Keys.N) || FlxG.gamepads.isNewButtonPress(Buttons.Y))
                {
                    x -= sideStep;
                }
            }
            if (velocity.X != 0)
            {
                if (velocity.X>0)
                    if (velocity.Y<0)
                        play("runNE");
                    else if (velocity.Y>0)  
                        play("runSE");
                    else
                        play("runE");
                else if (velocity.X < 0)
                    if (velocity.Y < 0)
                        play("runNW");
                    else if (velocity.Y > 0)
                        play("runSW");
                    else
                        play("runW");

            }
            else if (velocity.Y != 0)
            {
                if (velocity.Y > 0)
                    play("runS");
                else if (velocity.Y < 0)
                    play("runN");
            }
            else
            {
                play("idle");
            }


            if (isSelected)
            {

                pass();

                float adjustedRunSpeed = runSpeed;
                if (FlxG.keys.L)
                {
                    adjustedRunSpeed *= 2;
                }

                if ((FlxControl.LEFT && FlxControl.DOWN) || (FlxControl.LEFT && FlxControl.UP) ||
                    (FlxControl.RIGHT && FlxControl.DOWN) || (FlxControl.RIGHT && FlxControl.UP))
                {
                    adjustedRunSpeed *= 0.85f;
                }

                if (FlxControl.LEFT)
                {
                    this.velocity.X = adjustedRunSpeed * -1;
                }
                if (FlxControl.RIGHT)
                {
                    this.velocity.X = adjustedRunSpeed;
                }
                if (FlxControl.UP)
                {
                    this.velocity.Y = adjustedRunSpeed * -1;
                }
                if (FlxControl.DOWN)
                {
                    this.velocity.Y = adjustedRunSpeed;
                }
            }


            selectedPlayerIcon.at(this);
            selectedPlayerIcon.x -= 8;
            selectedPlayerIcon.y -= 8;
            selectedPlayerIcon.update();

            jerseyText.at(this); 
            jerseyText.x -=8;
            jerseyText.y += this.height;
            jerseyText.update();



            base.update();

            //hold onto ball if in possession.
            if (hasBall)
            {
                ball.isHeld = true;
                //ball.atCenter(this, 0, 10);
                ball.at(this);
            }

        }

        public void pass()
        {
            // Passsing
            if (hasBall)
            {
                if (FlxG.keys.justPressed(Keys.OemPeriod) || FlxG.gamepads.isNewButtonPress(Buttons.RightShoulder))
                {
                    Player p = team.getNextPlayerToLeft(false);

                    float newAngle = FlxU.getAngle(new Vector2(x + (width / 2), y + (height / 2)), new Vector2(p.x + (width / 2), p.y + (height / 2)));

                    double radians = Math.PI / 180 * (newAngle + 90);

                    double velocity_x = Math.Cos((float)radians);
                    double velocity_y = Math.Sin((float)radians);

                    Console.WriteLine("This player is at : {0} {1} and player to the right is {2} {3} And the angle is {4}", x + (width / 2), y + (height / 2), p.x + (width / 2), p.y + (height / 2), newAngle);

                    passBall(200 * (float)velocity_x * -1, 200 * (float)velocity_y * -1);
                }
                if (FlxG.keys.justPressed(Keys.OemComma) || FlxG.gamepads.isNewButtonPress(Buttons.LeftShoulder))
                {
                    Player p = team.getNextPlayerToRight(false);

                    float newAngle = FlxU.getAngle(new Vector2(x + (width / 2), y + (height / 2)), new Vector2(p.x + (width / 2), p.y + (height / 2)));

                    double radians = Math.PI / 180 * (newAngle + 90);

                    double velocity_x = Math.Cos((float)radians);
                    double velocity_y = Math.Sin((float)radians);

                    Console.WriteLine("This player is at : {0} {1} and player to the right is {2} {3} And the angle is {4}", x + (width / 2), y + (height / 2), p.x + (width / 2), p.y + (height / 2), newAngle);

                    passBall(200 * (float)velocity_x * -1, 200 * (float)velocity_y * -1);

                }
                if (FlxG.keys.justReleased(Keys.K) || FlxG.gamepads.isNewButtonRelease(Buttons.LeftTrigger))
                {
                    double radians = Math.PI / 180 * (selectedPlayerIcon.angle + 180);
                    double velocity_x = Math.Cos((float)radians);
                    double velocity_y = Math.Sin((float)radians);
                    passBall(200 * (float)velocity_x * -1, 200 * (float)velocity_y * -1);
                }
                else if (FlxG.keys.justReleased(Keys.L) || FlxG.gamepads.isNewButtonRelease(Buttons.RightTrigger))
                {
                    double radians = Math.PI / 180 * (selectedPlayerIcon.angle + 180);
                    double velocity_x = Math.Cos((float)radians);
                    double velocity_y = Math.Sin((float)radians);
                    passBall(200 * (float)velocity_x * -1, 200 * (float)velocity_y * -1);
                }
                if (FlxG.keys.K || FlxG.gamepads.isButtonDown(Buttons.LeftTrigger))
                {
                    selectedPlayerIcon.angle += 3;
                }
                else if (FlxG.keys.L || FlxG.gamepads.isButtonDown(Buttons.RightTrigger))
                {
                    selectedPlayerIcon.angle -= 3;
                }
                else
                {
                    selectedPlayerIcon.angle = 90;
                }
            }
        }


        public override void render(SpriteBatch spriteBatch)
        {

            if (isSelected)
            {
                selectedPlayerIcon.render(spriteBatch);
            }

            base.render(spriteBatch);
            jerseyText.render(spriteBatch);
        }


        public void passBall(float xVel, float yVel)
        {
            ball.x = x - 3;
            ball.y = y + height + 3;
            ball.timeSincePass = 0;
            ball.setVelocity(xVel, yVel);
            ball.isHeld = false;
            hasBall = false;

        }
        public void passBallUsingAngle(float Angle, float Thrust)
        {
            ball.x = x - 3;
            ball.y = y + height + 3;
            ball.timeSincePass = 0;

            ball.angle = Angle;
            ball.thrust = Thrust;

            ball.isHeld = false;
            hasBall = false;

        }


        public override void overlapped(FlxObject obj)
        {

            base.overlapped(obj);
        }

    }
}
