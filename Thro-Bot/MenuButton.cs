using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thro_Bot
{
    class MenuButton
    {

        public enum ButtonState
        {
            Unpressed,
            Hover,
            Clicked

        }


        public ButtonState currentButtonState = ButtonState.Unpressed;

        //Texture of the button
        Texture2D m_TextureButton;

        //The postion of the play button
        Vector2 m_ButtonPosition;

        //The origin of the button
        Vector2 m_ButtonOrigin;

        //The rectangle to draw the button
        public Rectangle m_ButtonRec;

        //The rectangle to detect collision the button
        public Rectangle m_ButtonRecCol;

        //The color of the button
        public Color m_ButtonColor = Color.White;


        //Initialize the button
        public void InitializeButton(Texture2D buttonTexture, Vector2 position)
        {

            //Set the texture of the button
            m_TextureButton = buttonTexture;

            //Set the position of the button
            m_ButtonPosition = position;

            //Set the origin of the button
            m_ButtonOrigin = new Vector2(m_TextureButton.Width/2, m_TextureButton.Height/2);

            //Set the rect of the button
            m_ButtonRec = new Rectangle(0,0, m_TextureButton.Width, m_TextureButton.Height);

            //Set the collision rectangle
            m_ButtonRecCol = new Rectangle((int)m_ButtonPosition.X - m_TextureButton.Width/2, (int)m_ButtonPosition.Y - m_TextureButton.Height / 2, m_TextureButton.Width, m_TextureButton.Height);

        }

        public void Update()
        {
            //Check the current state of the button
            if (currentButtonState.Equals(ButtonState.Unpressed))
            {
                m_ButtonColor = Color.White;

            }

            else if (currentButtonState.Equals(ButtonState.Hover))
            {
                m_ButtonColor = Color.CornflowerBlue;

            }

            else if (currentButtonState.Equals(ButtonState.Clicked))
            {
                m_ButtonColor = Color.Red;

            }

        }



        public void Draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Draw(m_TextureButton, m_ButtonPosition, m_ButtonRec, m_ButtonColor, 0f, m_ButtonOrigin, 1f, SpriteEffects.None, 0f);

        }

   }
}
