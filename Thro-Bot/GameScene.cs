using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Thro_Bot
{
    class GameScene
    {

        public enum GameState
        {
            MainMenu,
            Credits,
            Playing,
            Quit
        }

        public GameState CurrentGameState = GameState.MainMenu;

        //Main Menu
        //Texture of the Main Menu Background
        Texture2D m_MainMenuBackground;

        //The position of the main Menu background
        Vector2 m_MainMenuBackPosition;

        //The origin of the background of the main menu
        Vector2 m_MainMenuBackOrigin;

        //The source rectangle of the background of the main menu
        Rectangle m_MainMenuBackRec;

       
        //Texture of the credits button
        Texture2D m_CeditsButton;

        //Texture of the quit button
        Texture2D m_QuitButton;



    }
}
