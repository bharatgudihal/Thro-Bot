using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Thro_Bot
{
    class GameScene
    {

        public enum GameState
        {
            MainMenu,
            Credits,
            Playing,
            Tutorial,
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


        //Title
        Texture2D m_GameTitle;

        //Title position
        Vector2 m_GameTitlePosition;

        //The origin of the Game Title
        Vector2 m_GameTitleOrigin;

        //The source rectangle of the game title
        Rectangle m_GameTitleRec;

        //The play Button
        public MenuButton m_PlayButton;

        //The credits button
        public MenuButton m_CreditsButton;

        //The sprite font for the credits
        public SpriteFont m_CreditsFont;

        //The tutorial button
        public MenuButton m_TutorialButton;

        //The quit button
        public MenuButton m_QuitButton;

        //The back button
        public MenuButton m_BackButton;

        //The state of the mouse
        MouseState mouseState;
        //The previous state of the mouse
        MouseState previousMouseState;
        
            //The position of the mouse
        Point mousePosition;

        //The position of the mouse in Vector2
        Vector2 m_MousePositionVect;

        //The mouse Texture
        public Texture2D m_MouseTexture;


        public void InitializeBackground(Texture2D texture, Vector2 position) {

            //Set up the main menu Texture
            m_MainMenuBackground = texture;

            //Set up the position
            m_MainMenuBackPosition = position;

            //Set up the origin
            m_MainMenuBackOrigin = new Vector2(m_MainMenuBackground.Width/2, m_MainMenuBackground.Height / 2);

            //Set the rectangle of the background
            m_MainMenuBackRec = new Rectangle(0, 0, m_MainMenuBackground.Width, m_MainMenuBackground.Height);
        }

        public void InitializeGameTitle(Texture2D texture, Vector2 position)
        {

            //Set up the game title Texture
            m_GameTitle = texture;

            //Set up the position
            m_GameTitlePosition = position;

            //Set up the origin
            m_GameTitleOrigin = new Vector2(m_GameTitle.Width / 2, m_GameTitle.Height / 2);

            //Set the rectangle of the background
            m_GameTitleRec = new Rectangle(0, 0, m_GameTitle.Width, m_GameTitle.Height);
        }


        public void InitializeButtons() {
            m_PlayButton = new MenuButton();
            m_CreditsButton = new MenuButton();
            m_TutorialButton = new MenuButton();
            m_QuitButton = new MenuButton();
            m_BackButton = new MenuButton();

        }



        public void Update()
        {
            

            //Store as the previous
            previousMouseState = mouseState;

            //Get the mouse state
            mouseState = Mouse.GetState();

            //Check the mouse position
            mousePosition = new Point(mouseState.Position.X, mouseState.Position.Y);

            //Check the position of the mouse

            m_MousePositionVect = new Vector2(mouseState.Position.X, mouseState.Position.Y);

            if (CurrentGameState.Equals(GameState.MainMenu))
            {

                //Update the buttons
                m_PlayButton.Update();
                m_CreditsButton.Update();
                m_TutorialButton.Update();
                m_QuitButton.Update();
                

                //Check the state of the buttons
                CheckButtonState(m_PlayButton);
                CheckButtonState(m_CreditsButton);
                CheckButtonState(m_TutorialButton);
                CheckButtonState(m_QuitButton);

                //Check if the play button has been pressed
                if (m_PlayButton.currentButtonState == MenuButton.ButtonState.Clicked)
                {
                    CurrentGameState = GameState.Playing;
                }

                //Check if the credits button has been pressed
                if (m_CreditsButton.currentButtonState == MenuButton.ButtonState.Clicked)
                {
                    CurrentGameState = GameState.Credits;
                }

                //Check if the tutorial button is pressed
                if (m_TutorialButton.currentButtonState == MenuButton.ButtonState.Clicked)
                {
                    CurrentGameState = GameState.Tutorial;
                }

                //Check if the quit button has been pressed
                if (m_QuitButton.currentButtonState == MenuButton.ButtonState.Clicked)
                {
                    CurrentGameState = GameState.Quit;
                }


            }
            else if (CurrentGameState.Equals(GameState.Credits))
            {
                //Update the buttons
                m_BackButton.Update();
                //Check the state of the buttons
                CheckButtonState(m_BackButton);


                //Check if the back button has been pressed
                if (m_BackButton.currentButtonState == MenuButton.ButtonState.Clicked)
                {
                    CurrentGameState = GameState.MainMenu;
                }

            }


        }

        private void CheckButtonState(MenuButton button) {

            //Check if the credits button has been hovered over
            if (button.m_ButtonRecCol.Contains(mousePosition))
            {

                button.currentButtonState = MenuButton.ButtonState.Hover;
                //Check if the  credits button has been pressed
                if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                {
                    button.currentButtonState = MenuButton.ButtonState.Clicked;
                }

            }
            //Check if the  credits button is not currently hovered over
            else
            {
                button.currentButtonState = MenuButton.ButtonState.Unpressed;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            //Draw the Main Menu
            if (CurrentGameState == GameState.MainMenu)
            {
                //Draw the background
                spriteBatch.Draw(m_MainMenuBackground, m_MainMenuBackPosition, m_MainMenuBackRec, Color.White, 0f, m_MainMenuBackOrigin, 0.85f, SpriteEffects.None, 0f);

                //Draw the title
                spriteBatch.Draw(m_GameTitle, m_GameTitlePosition, m_GameTitleRec, Color.White, 0f, m_GameTitleOrigin, 1f, SpriteEffects.None, 0f);

                //Draw the buttons
                m_PlayButton.Draw(spriteBatch);
                m_CreditsButton.Draw(spriteBatch);
                m_TutorialButton.Draw(spriteBatch);
                m_QuitButton.Draw(spriteBatch);

                //Draw the mouse
                spriteBatch.Draw(m_MouseTexture, m_MousePositionVect, new Rectangle (0,0, m_MouseTexture.Width, m_MouseTexture.Height), Color.White, 0f, new Vector2(m_MouseTexture.Width/2, m_MouseTexture.Height / 2), 0.1f, SpriteEffects.None, 0f);
            }

            //Draw the Credits Menu
            if (CurrentGameState == GameState.Credits)
            {

                //Draw the background
                spriteBatch.Draw(m_MainMenuBackground, m_MainMenuBackPosition, m_MainMenuBackRec, Color.White, 0f, m_MainMenuBackOrigin, 0.85f, SpriteEffects.None, 0f);

                //Draw the title
                spriteBatch.Draw(m_GameTitle, m_GameTitlePosition, m_GameTitleRec, Color.White, 0f, m_GameTitleOrigin, 1f, SpriteEffects.None, 0f);

                //Draw the back button
                m_BackButton.Draw(spriteBatch);

                //Draw the mouse
                spriteBatch.Draw(m_MouseTexture, m_MousePositionVect, new Rectangle(0, 0, m_MouseTexture.Width, m_MouseTexture.Height), Color.White, 0f, new Vector2(m_MouseTexture.Width / 2, m_MouseTexture.Height / 2), 0.1f, SpriteEffects.None, 0f);

                spriteBatch.DrawString(m_CreditsFont, "     Producer\n Jaxon Whittaker\n\n     Artist\n  Lalitha Gunda\n\n     Artist\n  Ruohan Tang\n\n Technical Artist\n   Aaron Desin\n\n     Engineer\n  Bharat Gudihal\n\n     Engineer\nJean-Paul Peschard\n\n", new Vector2(140,200), Color.White);

            }


        }







    }
}
