﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.flixel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace RugbyLeague
{
    class SelectedPlayerIcon : FlxSprite
    {

        public SelectedPlayerIcon(int xPos, int yPos)
            : base(xPos, yPos)
        {
            loadGraphic(FlxG.Content.Load<Texture2D>("selectedPlayerIcon"), true, false, 48, 48);
            //addAnimation("selected", new int[] { 0, 1 }, 12, true);
            //play("selected");
            alpha = 0.25f;
            setOffset(9, 9);
            angle = 90;

            boundingBoxOverride = false;

        }

        override public void update()
        {
            angle += 2;

            base.update();

        }


    }
}
