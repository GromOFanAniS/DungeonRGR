using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DungeonGame.GameState;

namespace DungeonGame
{
    static class MusicPlayer
    {
        private static Song songToPlay;
        private static Song currentSong;
        private static ContentManager content;
        public static void Load(ContentManager Content)
        {
            content = new ContentManager(Content.ServiceProvider, "Content/Music/SoundTrack");
            MediaPlayer.Volume = 0.06f;
            songToPlay = content.Load<Song>("MainMenu/01 - For Varldarna Nio");
        }
        public static void Update()
        {
            if (currentSong == songToPlay)
                return;
            content.Unload();

            ChangeSong(Game1.gameState);

            //MediaPlayer.Play(songToPlay);
            currentSong = songToPlay;
        }
        private static void ChangeSong(GameState gameState)
        {
            switch(gameState)
            {
                case MenuScene:
                    songToPlay = content.Load<Song>("MainMenu/01 - For Varldarna Nio");
                    break;
            }
        }
    }
}
