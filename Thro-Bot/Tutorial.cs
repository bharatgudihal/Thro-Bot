using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thro_Bot
{
    class Tutorial
    {

        //The states of the tutorial
        public enum Action
        {
            Hide,
            ReleaseProjectile,
            Recall,
            HitEnemy,
            SmashEnemy,
            Combo
        }

        public Action tutorialState = Action.Hide;

        //The time to display the tutorial message and freeze the game
        public TimeSpan m_currentTutorialMessageTime = TimeSpan.Zero;

        //The time to display the tutorial message and freeze the game
        public TimeSpan m_TutorialMessageTime = TimeSpan.FromSeconds(2);

        //Boolean to pause the game
        public bool m_bPauseGame = false;

        //The success counter
        public int m_successCounter = 0;

        //The state if the enemy spawned for the first hit sequence
        public bool m_bSpawnedSequence = false;

        //Check if the sequence was well executed
        public bool m_bSequenceSuccessful = false;


        public void PauseGame(GameTime gameTime)
        {
            m_currentTutorialMessageTime += gameTime.ElapsedGameTime;

            if (m_currentTutorialMessageTime > m_TutorialMessageTime)
            {
                m_bPauseGame = false;
                m_currentTutorialMessageTime = TimeSpan.Zero;
            }
        }


        public void Update(GameTime gameTime)
        {
            if (m_bPauseGame)
            {
                PauseGame(gameTime);
            }
        }



        //Draw all of the tutorial messages that are needed
        public void Draw(SpriteBatch spriteBatch,SpriteFont font)
        {
            if (tutorialState == Action.ReleaseProjectile)
            {

                //Draw the combo indicator
                spriteBatch.DrawString(font, "    Press 'SPACE' \n    to release \n   the projectile", new Vector2(0, 430), Color.White);
            }

            else if (tutorialState == Action.Recall)
            {

                //Draw the combo indicator
                spriteBatch.DrawString(font, "      Press 'R' \n     to recall \n   the projectile", new Vector2(0, 430), Color.White);
            }

            else if (tutorialState == Action.HitEnemy)
            {
                //Draw the combo indicator
                spriteBatch.DrawString(font, "  Release the projectile\n   on an enemy\n   to destroy it!", new Vector2(0, 430), Color.White);
            }

            else if (tutorialState == Action.SmashEnemy)
            {
                //Draw the combo indicator
                spriteBatch.DrawString(font, "   Press and hold 'SPACE'\n   to spin the projectile \n   and smash straight \n   through an enemy!", new Vector2(0, 430), Color.White);
            }

            else if (tutorialState == Action.Combo)
            {
                //Draw the combo indicator
                spriteBatch.DrawString(font, "   Hit 2 enemies\n   in 3 seconds \n   to Combo!", new Vector2(0, 430), Color.White);
            }


        }




    }
}
