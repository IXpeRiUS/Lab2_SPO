namespace Lab2
{
    public class Token
    {
        private int _startingPosition;
        private int _endingPosition;

        private int _state;
       

        public int StartingPosition { get => _startingPosition; set => _startingPosition = value; }
        public int EndingPosition { get => _endingPosition; set => _endingPosition = value; }
        public int State { get => _state; set => _state = value; }

        public Token(int startingPisition, int endingPosition, int state)
        {
            _startingPosition = startingPisition;
            _endingPosition = endingPosition;
            
            _state = state;
        }


    }
}
