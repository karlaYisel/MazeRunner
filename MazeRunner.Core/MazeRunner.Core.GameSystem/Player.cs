using System.Globalization;

using MazeRunner.Core.InteractiveObjects;

namespace MazeRunner.Core.GameSystem
{
    public enum OptionsByPlayer
    {
        SelectToken,
        SkipTurn,
    }

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

        public void ChangeName (string? name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
                this.Name = ti.ToTitleCase(name);
            }
            else
            {
                this.Name = "UNKNOWN";
            }
        }

        public void ChangeTokens (List<Character> tokens)
        {
            this.Tokens = tokens;
        }

        public void ClearTokens ()
        {
            this.Tokens.Clear();
        }
    }
}