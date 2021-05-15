using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace connect4
{
    class Program
    {
        static void Main(string[] args)
        {
            var gameover = false;
            Board board = new Board();
            board.print();
            var color = "Red";
            while (!gameover)
            {
                try
                {
                    Console.WriteLine("{0}'s turn. Which column?", color);
                    var input = Convert.ToInt16(Console.ReadLine());
                    if(input<0|input>6){throw new Exception("out of bounds");}
                    board.place(input, color);
                    board.print();
                    if (board.checkwins())
                    {
                        Console.WriteLine("Game over!");
                        gameover = true;
                    }
                    if (color == "Red") { color = "Blue"; } else { color = "Red"; }
                }catch{

                }
            }
        }
    }
    class Board
    {
        //board is 6 tall, 7 wide, 42 total
        string content;
        byte[] reds = new byte[6];
        byte[] blues = new byte[6];
        //0x000000000 all the way down
        public Board()
        {


        }
        public void print()
        {
            Console.Clear();
            for (int row = 0; row < 6; row++)
            {
                string buffer = "";
                for (int col = 0; col < 7; col++)
                {
                    char n = "-"[0];
                    if ((reds[row] & (0x1 << col)) != 0x0)
                    {
                        n = "R"[0];
                    }
                    if ((blues[row] & (0x1 << col)) != 0x0)
                    {
                        n = "B"[0];
                    }
                    buffer += n;
                }
                Console.WriteLine(buffer);
            }
        }
        public void place(Int32 col, string color)
        {
            int row = 5; //row number, starting from the bottom
            var placed = false;
            while (row > -1) //formerly >-1
            {
                if ((reds[row] & (0x1 << col)) == 0x0
                & (blues[row] & (0x1 << col)) == 0x0)
                {
                    if (color == "Red")
                    {
                        reds[row] |= Convert.ToByte(0x1 << col);
                    }
                    if (color == "Blue")
                    {
                        blues[row] |= Convert.ToByte(0x1 << col);
                    }
                    placed = true;
                    row = 0; //exit loop
                }
                row--;
            }
            if (placed == false)
            {
                throw new Exception("Column full!");
            }
        }
        public Boolean checkwins()
        {
            //TO-DO: change to IF statemtents within loops, eliminate vars

            Int32 checkrow = 0;
            for (int row = 0; row < 6; row++)
            {
                checkrow = reds[row] & (reds[row] << 1) & (reds[row] << 2) & (reds[row] << 3);
                checkrow |= blues[row] & (blues[row] << 1) & (blues[row] << 2) & (blues[row] << 3);
            }
            if (checkrow != 0) { return true; }
            Int32 redcheckcol = 0;
            Int32 bluecheckcol = 0;
            for (int row = 0; row <= 2; row++)
            {
                redcheckcol |= reds[row] & reds[row + 1] & reds[row + 2] & reds[row + 3];
                bluecheckcol |= blues[row] & blues[row + 1] & blues[row + 2] & blues[row + 3];
            }
            if (redcheckcol != 0 | bluecheckcol != 0) { return true; }
            Int32 redacross = 0;
            Int32 blueacross = 0;
            for (int row = 0; row <= 2; row++)
            {
                redacross |= reds[row] & (reds[row + 1] << 1) & (reds[row + 2] << 2) & (reds[row + 3] << 3);
                blueacross |= blues[row] & (blues[row + 1] << 1) & (blues[row + 2] << 2) & (blues[row + 3] << 3);
            }
            if (redacross != 0 | blueacross != 0) { return true; }


            return false;
        }

    }
}
