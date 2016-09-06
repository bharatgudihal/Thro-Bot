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
            Pressed,
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
        Rectangle m_ButtonRec;



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

        }




    }
}
