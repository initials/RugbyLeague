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


            loadGraphic(FlxG.Content.Load<Texture2D>("examples/running"), true, false, 32, 32);

            addAnimation("idle", new int[] { 0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52 }, (int)FlxU.random(12,24), true);
            addAnimation("run", new int[] { 53, 54, 55, 56, 57, 58, 59, 60,61,62,63,64,65,66,67,68,69,70,71 }, 24, true);
            addAnimation("tackled", new int[] { 53, 63 }, 24, true);
            play("idle");

            setDrags(1350, 1350);

            angle = 90;

            isSelected = false;

            height = 16;
            width = 16;
            //offset.X = 12;
            //offset.Y = 12;

            setOffset(9,9);

            runSpeed = FlxU.random(150, 200);
            sideStep = FlxU.random(2, 20);


            mode = MODE_ATTACK;

        }

        override public void update()
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

                if (FlxG.keys.justReleased(Keys.K) || FlxG.gamepads.isNewButtonRelease(Buttons.LeftTrigger))
                {
                    double radians = Math.PI / 180 * (selectedPlayerIcon.angle + 90);
                    double velocity_x = Math.Cos((float)radians);
                    double velocity_y = Math.Sin((float)radians);
                    passBall(200 * (float)velocity_x * -1, 200 * (float)velocity_y * -1);
                }
                else if (FlxG.keys.justReleased(Keys.L) || FlxG.gamepads.isNewButtonRelease(Buttons.RightTrigger))
                {
                    double radians = Math.PI / 180 * (selectedPlayerIcon.angle + 90);
                    double velocity_x = Math.Cos((float)radians);
                    double velocity_y = Math.Sin((float)radians);
                    passBall(200 * (float)velocity_x * -1, 200 * (float)velocity_y * -1);
                }
            }



            if (isSelected)
            {
                //sidestep (aka juke)
                if (FlxG.keys.justPressed(Keys.M) || FlxG.gamepads.isNewButtonPress(Buttons.X))
                {
                    x += sideStep;
                }
            }
            if (velocity.X != 0)
            {
                play("run");
                setAngleBasedOnVelocity();
                //flicker(555);
            }
            else if (velocity.Y != 0)
            {
                play("run");
                setAngleBasedOnVelocity();
            }
            else
            {
                play("idle");
                //flicker(0.001f);
            }

            if (isSelected && this.mode != MODE_PLAYTHEBALL)
            {
                if (FlxControl.LEFT)
                {
                    this.velocity.X = runSpeed * -1;
                }
                if (FlxControl.RIGHT)
                {
                    this.velocity.X = runSpeed;
                }
                if (FlxControl.UP)
                {
                    this.velocity.Y = runSpeed * -1;
                }
                if (FlxControl.DOWN)
                {
                    this.velocity.Y = runSpeed;
                }
            }
            else
            {
                if (this.mode == MODE_ATTACK)
                {
                    if (y < ball.y-64)
                    {
                        velocity.Y = runSpeed;

                    }
                }
                if (this.mode == MODE_DEFENSE)
                {

                    if (FlxU.getDistance(new Vector2(x, y), new Vector2(ball.x, ball.y)) < 80)
                    {
                        float newAngle = FlxU.getAngle(new Vector2(x, y), new Vector2(ball.x, ball.y));

                        double radians = Math.PI / 180 * (newAngle + 90);

                        double velocity_x = Math.Cos((float)radians);
                        double velocity_y = Math.Sin((float)radians);
                        this.velocity.X = runSpeed * (float)velocity_x * -1;
                        this.velocity.Y = runSpeed * (float)velocity_y * -1;
                    }
                    else
                    {
                        float newAngle = FlxU.getAngle(new Vector2(x, y), new Vector2(ball.x, ball.y));

                        double radians = Math.PI / 180 * (newAngle + 90);

                        double velocity_x = Math.Cos((float)radians);
                        double velocity_y = Math.Sin((float)radians);
                        this.velocity.X = (runSpeed / 4) * (float)velocity_x * -1;
                        this.velocity.Y = (runSpeed / 4) * (float)velocity_y * -1;

                    }


                }
                if (this.mode == MODE_TACKLED)
                {
                    float tenMeters = ball.y + 160;

                    if (y < tenMeters)
                    {
                        velocity.Y = runSpeed ;

                    }
                    else
                    {
                        this.velocity.Y = 0;
                        this.mode = MODE_WAIT;

                    }
                }
                else if (this.mode == MODE_PLAYTHEBALL)
                {
                    if (jerseyNumber == 9)
                    {
                        float newAngle = FlxU.getAngle(new Vector2(x, y), new Vector2(ball.x, ball.y-20));

                        double radians = Math.PI / 180 * (newAngle + 90);

                        double velocity_x = Math.Cos((float)radians);
                        double velocity_y = Math.Sin((float)radians);
                        this.velocity.X = runSpeed * (float)velocity_x * -1;
                        this.velocity.Y = runSpeed * (float)velocity_y * -1;

                        if (x == ball.x && y==ball.y)
                        {
                            this.mode = MODE_ATTACK;
                        }

                    }
                }
            }



            selectedPlayerIcon.at(this);
            selectedPlayerIcon.x -= 8;
            selectedPlayerIcon.y -= 8;
            selectedPlayerIcon.update();

            jerseyText.at(this); 
            jerseyText.x -=8;
            jerseyText.y +=24;
            jerseyText.update();



            base.update();

            //hold onto ball if in possession.
            if (hasBall)
            {
                ball.isHeld = true;
                ball.atCenter(this, 0, 10);
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
