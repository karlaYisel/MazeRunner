using System.Globalization;

using MazeRunner.Core.InteractiveObjects;

namespace MazeRunner.Core.GameSystem
{
    public class Player
    {
        public string Name { get; private set; }
        public List<Character> Tokens { get; private set; }

        public Player (string name)
        {
            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            this.Name = ti.ToTitleCase(name);
            this.Tokens = new List<Character>();
        }

        public Player (string name, List<Character> tokens)
        {
            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            this.Name = ti.ToTitleCase(name);
            this.Tokens = tokens;
        }

        public void ChangeTokens (List<Character> tokens)
        {
            this.Tokens = tokens;
        }
    }
}