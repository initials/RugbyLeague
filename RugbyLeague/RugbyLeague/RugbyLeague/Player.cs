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


            loadGraphic(FlxG.Content.Load<Texture2D>("player"), true, false, 32, 32);

            //addAnimation("idle", new int[] { 0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52 }, (int)FlxU.random(12,24), true);
            //addAnimation("run", new int[] { 53, 54, 55, 56, 57, 58, 59, 60,61,62,63,64,65,66,67,68,69,70,71 }, 36, true);
            //addAnimation("tackled", new int[] { 53, 63 }, 24, true);
            //play("idle");

            addAnimation("idle", new int[] { 0 }, (int)FlxU.random(12,24), true);
            addAnimation("run", new int[] { 1,2,3 }, 12, true);

            setDrags(1350, 1350);

            //angle = 90;

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
                play("run");
            }
            else if (velocity.Y != 0)
            {
                play("run");
            }
            else
            {
                play("idle");
            }
            if (isSelected)
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
                //ball.atCenter(this, 0, 10);
                ball.at(this);
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
