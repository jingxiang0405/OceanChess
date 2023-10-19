namespace AppEnum
{
    public class CellState
    {
   
        public enum Ship
        {
            AIRCRAFT_CARRIER=0, BATTLESHIP, RECONSHIP, SUBMARINE, LIGHT_CRUISER
        }
        public enum Addition
        {
            BOMBER=5, AC_CORE
        }
        public enum Condition
        {
            RUIN = 7, FAIL, SINK, BLANK
        }
    };

    public enum SelectStage
    {
        SELECT_SHIP, SELECT_ADDITION, SELECT_INGAME
    };

    public enum Alignment
    {
        NA, VERTICAL, HORIZONTAL
    }

    public enum Direction
    {
        NA, TOP, DOWN, LEFT, RIGHT 
    }

    public enum Action
    {
        BASIC_ATTACK, BOMBER, BATTLESHIP_ATTACK, RECON
    }

}