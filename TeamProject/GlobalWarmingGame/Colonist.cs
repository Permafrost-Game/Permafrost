﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using GlobalWarmingGame.Action;
using Engine;

namespace GlobalWarmingGame
{
    class Colonist : PathFindable
    {

        private Queue<Instruction> actions; 

        public float Health { get; private set; }
        public string Name { get; private set; }
         

        public Colonist(Vector2 position, Texture2D texture) : base
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            rotationOrigin: new Vector2(0,0),
            tag: "Colonist",
            depth: 1f,
            texture: texture,
            speed: 10f
        )
        {
            Health = 10f;
            actions = new Queue<Instruction>();
        }

        public void AddInstruction(Instruction instruction)
        {
            instruction.ActiveMember = this;
        }
    }
}
