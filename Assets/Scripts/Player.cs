namespace Assets.Scripts
{
    public class Player
    {
        public string Name { get; set; }

        public int Score { get; set; }

        public Seed Type { get; set; }

        public Player(string name, int score, Seed type)
        {
            Name = name;
            Score = score;
            Type = type;
        }

        public override string ToString()
        {
            return Name + ": " + Score;
        }
    }
}