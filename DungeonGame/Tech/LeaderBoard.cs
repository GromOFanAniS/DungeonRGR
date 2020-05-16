using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame
{
    [Serializable]
    class LeaderBoard
    {
        private static LeaderBoard instance;

        private List<KeyValuePair<string, int>> _board;
        [NonSerialized]
        private Label _boardLabel;
        public Label BoardLabel => _boardLabel;

        private LeaderBoard()
        {
            _board = new List<KeyValuePair<string, int>>();
            _boardLabel = new Label(20, 20, "");
        }

        public void AddToBoard(string name, int score)
        {
            _board.Add(new KeyValuePair<string, int>(name, score));
            _board.OrderByDescending(x => x.Value);
            refreshLabel();
        }

        public void SaveBoard()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(@"saves/scoreboard.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, this);
            }
        }

        public static LeaderBoard GetLeaderBoard()
        {
            if (instance == null)
            {
                instance = LoadBoard();
            }
            return instance;
        }

        private static LeaderBoard LoadBoard()
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream(@"saves/scoreboard.dat", FileMode.OpenOrCreate))
                {
                    LeaderBoard leaderBoard = (LeaderBoard)formatter.Deserialize(fs);
                    leaderBoard.refreshLabel();
                    return leaderBoard;
                }
            }
            catch (IOException)
            {
                return new LeaderBoard();
            }
            catch (SerializationException)
            {
                return new LeaderBoard();
            }
        }

        private void refreshLabel()
        {
            _boardLabel = new Label(20, 20, "");
            foreach (var entry in _board)
            {
                _boardLabel.Text += $"{entry.Key}, Счёт: {entry.Value}\n";
            }
        }

    }
}
