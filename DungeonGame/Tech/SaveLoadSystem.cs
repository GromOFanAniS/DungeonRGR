using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame
{
    static class SaveLoadSystem
    {
        public static void SaveGame(Player player)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(@"saves/save.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, player);
            }
        }
        public static Player LoadGame()
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream(@"saves/save.dat", FileMode.OpenOrCreate))
                {
                    Player player = (Player)formatter.Deserialize(fs);
                    player.Initialize();
                    return player;
                }
            }
            catch (IOException /*e*/)
            {
                //Console.WriteLine(e.Message + "\n" + e.StackTrace + "\n" + e.Source + "\n" + e.InnerException);
                return new Player()
                {
                    Name = "Игрок"
                };
            }
        }
    }
}
