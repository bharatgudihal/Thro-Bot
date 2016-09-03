using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thro_Bot
{
   public class HealthPowerUp : PowerUp
   {


        protected override float m_MinScale { get {return 0.6f;} }

        protected override float m_MaxScale { get { return 0.8f; } }

        protected override string m_TexturePath { get { return "Graphics/HealthPowerUp"; } }
        public override int m_PointValue { get { return 20; } }

        protected override int m_Lower { get { return 5; } }

        protected override int m_Upper { get { return 15; } }

        protected override int m_SpawnTimeRange{ get {return random.Next(m_Lower, m_Upper);} }

        public override void Initialize(Texture2D texture, Vector2 position)
        {
            base.Initialize(texture, position);
        }


        

    }
}
