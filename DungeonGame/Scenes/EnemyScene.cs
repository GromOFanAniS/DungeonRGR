using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonGame
{
    enum FightingState
    {
        None,
        Player,
        Enemy
    }
    class EnemyScene : Scene
    {
        private Enemy enemy;
        private Door door;
        public FightingState fightingState;
        public EnemyScene()
        {
            DoNewGenerate = false;
            enemy = Enemy.Generate();
        }

        public override void Draw(SpriteBatch s)
        {
            enemy.Draw(s);
        }

        public override void Update(GameTime gameTime)
        {
            enemy.Update(gameTime);
        }
    }
}
