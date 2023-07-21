namespace CrossyRoad.Score
{
    public struct Score
    {
        public readonly int bestScore;

        public readonly int score;

        public Score(int score, int bestScore)
        {
            this.score = score;
            this.bestScore = bestScore;
        }
    }
}