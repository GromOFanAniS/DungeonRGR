using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame
{
    enum MusicState
    {
        Battle,
        Peaceful,
        GameOver,
        Doom,
        DoomMenu
    }
    static class MusicPlayer
    {
        public static bool DoomMode = false;

        private static List<Song> MenuMusic;
        private static List<Song> BattleMusic;
        private static List<Song> DoomMusic;

        private static Song doomMenuSong;
        private static Song gameOverSong;
        private static Song songToPlay;
        private static Song currentSong;
        private static ContentManager content;
        public static void Load(ContentManager Content)
        {
            content = new ContentManager(Content.ServiceProvider, "Content/Music/SoundTrack");
            MediaPlayer.Volume = 0.03f;

            MenuMusic = new List<Song>()
            {
                content.Load<Song>("MainMenu/01 - For Varldarna Nio"),
                content.Load<Song>("MainMenu/01 - Nio Natters Led"),
                content.Load<Song>("MainMenu/04 - Flykt"),
                content.Load<Song>("MainMenu/04 - Nar Gudarna Kalla"),
                content.Load<Song>("MainMenu/06 - Jag Vet Ett Tempel Sta")
            };
            BattleMusic = new List<Song>()
            {
                content.Load<Song>("BattleMusic/03 82nd All the Way"),
                content.Load<Song>("BattleMusic/04 The Attack of the Dead Men"),
                content.Load<Song>("BattleMusic/06 The Red Baron (Soundtrack Version)"),
                content.Load<Song>("BattleMusic/07 Great War (Soundtrack Version)"),
                content.Load<Song>("BattleMusic/09 Fields of Verdun")
            };
            DoomMusic = new List<Song>()
            {
                content.Load<Song>("Doom/02. Rip & Tear"),
                content.Load<Song>("Doom/Flesh & Metal"),
                content.Load<Song>("Doom/Bfg Division"),
                content.Load<Song>("Doom/22. Damnation"),
                content.Load<Song>("Doom/26. SkullHacker"),
            };

            doomMenuSong = content.Load<Song>("Doom/MenuTheme_mixdown");
            gameOverSong = content.Load<Song>("BattleMusic/11 In Flanders Fields (Soundtrack Version)");

            ChangeSong(MusicState.Peaceful);
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
        }

        private static void MediaPlayer_MediaStateChanged(object sender, EventArgs e)
        {
            if (DoomMode)
            {
                songToPlay = DoomMusic[Game1.random.Next(DoomMusic.Count)];
            }
        }

        public static void Update()
        {
            
            if (currentSong == songToPlay)
                return;

            if (MediaPlayer.Volume > 0)
            {
                MediaPlayer.Volume -= 0.001f;
            }
            else
            {
                
                MediaPlayer.Play(songToPlay);
                currentSong = songToPlay;
                MediaPlayer.Volume = 0.03f;
            }
        }

        public static void ChangeSong(MusicState state)
        {
            if(DoomMode && (state != MusicState.Doom))
                state = MusicState.Doom;
            switch(state)
            {
                case MusicState.Battle:
                    songToPlay = BattleMusic[Game1.random.Next(BattleMusic.Count)];
                    break;
                case MusicState.GameOver:
                    songToPlay = gameOverSong;
                    break;
                case MusicState.Peaceful:
                    songToPlay = MenuMusic[Game1.random.Next(MenuMusic.Count)];
                    break;
                case MusicState.DoomMenu:
                    songToPlay = doomMenuSong;
                    break;
                case MusicState.Doom:
                    songToPlay = DoomMusic[Game1.random.Next(DoomMusic.Count)];
                    break;
            }
        }
    }
}
