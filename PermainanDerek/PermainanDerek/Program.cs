using System;
using System.Numerics;
using System.Xml.Serialization;
using System.Threading;

class Program
{
    public static void Main(string[] args)
    {
        string input = " ";
        Console.WriteLine("\t\tSELAMAT DATANG DI GAME DEREK\n\n");
        Console.WriteLine("Ini merupakan game berbasis console command prompt yang");
        Console.WriteLine("dimainkan 1 vs 1. Game ini terinspirasi dari permainan ");
        Console.WriteLine("tradisional anak-anak di desa. Pemain diharuskan memindahkan ");
        Console.WriteLine("semua pion miliknya (disimbolkan dengan icon huruf) hingga ");
        Console.WriteLine("membentuk deret vertikal ataupun horizontal. Jika deret sudah ");
        Console.WriteLine("dibentuk maka pemain dinyatakan menang. Gunakan perintah");
        Console.WriteLine("posisi_awal to posisi_akhir untuk memindahkan pion\n\n");
        while (input != "1" || input != "2" )
        {
            Console.WriteLine("1. Mulai Permainan\n2. Lihat Koordinat Papan Derek\nPilihan [1/2]:");
            input = Console.ReadLine();
            if (input == "1")
            {
                startGame();
            }
            if (input == "2")
            {
                Console.WriteLine("\nBerikut adalah koordinat dari papan derek");
                Console.WriteLine($"+---+---+---+");
                Console.WriteLine($"| 1 | 2 | 3 |");
                Console.WriteLine($"+---+---+---+");
                Console.WriteLine($"| 4 | 5 | 6 |");
                Console.WriteLine($"+---+---+---+");
                Console.WriteLine($"| 7 | 8 | 9 |");
                Console.WriteLine($"+---+---+---+\n\n");
                Console.WriteLine("Mulai permainan [y/n]: ");
                string pilihan = Console.ReadLine();
                if (pilihan == "y")
                {
                    Console.WriteLine("\n ");
                    startGame();
                }
                else 
                {
                    Console.WriteLine("Hmmmmmm, kasian");
                }
            }
        }
    }

    public static void startGame()
    {
        Board board = new Board();
        Pawn pawn = new Pawn(board);

        //Inisiasi Player
        Player player = new Player(board);
        player.Pawn1 = 7;
        player.Pawn2 = 8;
        player.Pawn3 = 9;
        player.setPawn("O");

        //Inisiasi Bot
        Bot bot = new Bot(board);
        bot.Pawn1 = 1;
        bot.Pawn2 = 2;
        bot.Pawn3 = 3;
        bot.setPawn("X");

        board.makeBoard();
        Console.WriteLine($"icon player: {player.icon} icon bot: {bot.icon}");


        while (!pawn.isSequence(board, player, bot))
        {
            player.command(board, player, bot);
            board.makeBoard();

            // Pengecekan kemenangan setelah langkah player
            if (pawn.isSequence(board, player, bot))
            {
                Console.WriteLine("Anda menang!");
                break; // Keluar dari loop jika player menang
            }

            Thread.Sleep(1500);//delay selama 1.5 detik
            bot.movePawn(board, bot);
            board.makeBoard();

            // Pengecekan kemenangan setelah langkah bot
            if (pawn.isSequence(board, player, bot))
            {
                Console.WriteLine("Bot menang!");
                break; // Keluar dari loop jika bot menang
            }
        }
    }
}

class Board
{
    private string[] board;

    public Board()
    {
        board = new string[]
        {
            " ", " ", " ",
            " ", " ", " ",
            " ", " ", " "
        };
    }

    public void makeBoard()
    {
        Console.WriteLine($"+---+---+---+");
        Console.WriteLine($"| {board[0]} | {board[1]} | {board[2]} |");
        Console.WriteLine($"+---+---+---+");
        Console.WriteLine($"| {board[3]} | {board[4]} | {board[5]} |");
        Console.WriteLine($"+---+---+---+");
        Console.WriteLine($"| {board[6]} | {board[7]} | {board[8]} |");
        Console.WriteLine($"+---+---+---+");
    }

    public bool isSequence(string icon)
    {
        //cek mendatar
        for (int i = 0; i < 3; i++)
        {
            if (board[i * 3] == icon && board[i * 3 + 1] == icon && board[i * 3 + 2] == icon)
            {
                return true; // Ada deretan pion secara horizontal
            }
        }

        //cek menurun
        for (int i = 0; i < 3; i++)
        {
            if (board[i] == icon && board[i + 3] == icon && board[i + 6] == icon)
            {
                return true; // Ada deretan pion secara vertikal
            }
        }
        return false;
    }

    public string[] arrayBoard
    {
        get { return board; }
        set { board = value; }
    }
}

class Pawn
{
    protected string icon;
    protected int pawn1;
    protected int pawn2;
    protected int pawn3;
    private Board board;
    public Pawn(Board board)
    {
        this.board = board; 
    }
    protected Board BoardInstance
    {
        get { return board; }
        set { board = value; }
    }

    protected void setPawn(string icon)
    {
        board.arrayBoard[pawn1 - 1] = icon;
        board.arrayBoard[pawn2 - 1] = icon;
        board.arrayBoard[pawn3 - 1] = icon;
    }

    public bool isSequence(Board board, Player player, Bot bot)
    {
        if (player.isAllMoved())
        {
            for (int i = 0; i < 3; i++)
            {
                //cek pion player secara menurun
                if (board.arrayBoard[i] == player.icon && board.arrayBoard[i + 3] == player.icon && board.arrayBoard[i + 6] == player.icon)
                {
                    return true;
                }
                //cek pion player secara mendatar
                else if (board.arrayBoard[(i * 3)] == player.icon && board.arrayBoard[(i * 3) + 1] == player.icon && board.arrayBoard[(i * 3) + 2] == player.icon)
                {
                    return true;
                }
            }
        }
        else if (bot.isAllMoved())
        {
            for (int i = 0;i < 3; i++)
            {
                //cek pion bot secara menurun
                if (board.arrayBoard[i] == bot.icon && board.arrayBoard[i + 3] == bot.icon && board.arrayBoard[i + 6] == bot.icon)
                {
                    return true;
                }
                //cek pion bot secara mendatar
                else if (board.arrayBoard[(i * 3)] == bot.icon && board.arrayBoard[(i * 3) + 1] == bot.icon && board.arrayBoard[(i * 3) + 2] == bot.icon)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public string Icon
    {
        get { return icon; }
        set { icon = value; }
    }
    public int Pawn1
    {
        get { return pawn1; }
        set { pawn1 = value; }
    }

    public int Pawn2
    {
        get { return pawn2; }
        set { pawn2 = value; }
    }

    public int Pawn3
    {
        get { return pawn3; }
        set { pawn3 = value; }
    }
}

class Player : Pawn
{
    private Board BoardInstance;
    public Player(Board boardInstance) : base(boardInstance)
    {
       this.BoardInstance = boardInstance;
    }

    public string icon;
    public void setPawn(string icon)
    {
        this.icon = icon; // Mengatur nilai properti icon
        base.setPawn(icon); // Memanggil setPawn dari kelas Pawn
    }

    public void command(Board board, Player player, Bot bot)
    {
        bool inputValid = false;

        while (!inputValid)
        {
            // Meminta user memasukkan perintah
            Console.WriteLine("Masukkan Perintah: ");
            string input = Console.ReadLine();

            // Memisahkan input menjadi 2 nilai
            string[] values = input.Split("to", StringSplitOptions.RemoveEmptyEntries);

            // Memeriksa apakah memuat kata "to" dan memiliki 2 input
            if (values.Length == 2 && input.Contains("to"))
            {
                string src = values[0];
                string dst = values[1];

                // Memeriksa apakah src dan dst dapat diubah menjadi integer
                if (int.TryParse(src, out int srcInt) && int.TryParse(dst, out int dstInt))
                {
                    // Memeriksa apakah posisi awal dan tujuan sama
                    if (srcInt != dstInt)
                    {
                        // Memeriksa apakah posisi awal dan posisi akhir berada di rentang 1 - 9
                        if (srcInt <= 9 && dstInt <= 9)
                        {
                            //deklarasikan icon
                            string icon = player.icon;

                            // Memeriksa apakah posisi awal adalah pion player dan tujuannya kosong
                            if (board.arrayBoard[srcInt - 1] == icon  && board.arrayBoard[dstInt - 1] == " ")
                            {
                                inputValid = true;
                                movePawn(srcInt, dstInt, board, icon);
                            }
                            // Memeriksa apakah posisi awal adalah pion player dan tujuannya adalah posisi player juga
                            else if (board.arrayBoard[srcInt - 1] == icon && board.arrayBoard[dstInt - 1] == icon)
                            {
                                Console.WriteLine($"Posisi tujuan adalah posisi pion anda, gunakan posisi tujuan yang kosong");
                            }
                            // Memeriksa apakah posisi awal adalah pion player dan tujuannya adalah posisi pion bot
                            else if (board.arrayBoard[srcInt - 1] == icon && board.arrayBoard[dstInt - 1] == bot.icon)
                            {
                                Console.WriteLine($"Posisi tujuan adalah posisi pion Bot, gunakan posisi tujuan yang kosong");
                            }
                            // Memeriksa apakah posisi awal adalah posisi pion Bot
                            else if (board.arrayBoard[srcInt - 1] == "X")
                            {
                                Console.WriteLine($"Posisi awal adalah posisi pion Bot, gunakan posisi pion anda ({icon})");
                            }
                            // Memeriksa apakah posisi awal adalah posisi kosong
                            else
                            {
                                Console.WriteLine($"Posisi awal kosong, gunakan posisi pion anda ({icon})");
                            }
                        }
                        else if (srcInt > 9 && dstInt <= 9)
                        {
                            Console.WriteLine($"Posisi awal salah, tidak ada posisi {srcInt}");
                        }
                        else if (srcInt <= 9 && dstInt > 9)
                        {
                            Console.WriteLine($"Posisi akhir salah, tidak ada posisi {dstInt}");
                        }
                        else
                        {
                            Console.WriteLine($"Posisi awal dan posisi akhir salah");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Posisi awal dan posisi akhir harus berbeda");
                    }
                }
                else
                {
                    Console.WriteLine("Maaf, posisi harus berupa angka.");
                }
            }
            else
            {
                Console.WriteLine("Maaf harusnya kamu mengirimkan format : posisi_awal to posisi_akhir");
            }
        }
    }

    //Deklarasi array untuk mengecek apakah pion sudah bergerak
    private bool[] playerMoved = new bool[3];

    public void movePawn(int src, int dst, Board board, string icon)
    {
        Console.WriteLine($"Player menggerakkan pion dari posisi {src} ke posisi {dst}");
        board.arrayBoard[dst - 1] = icon;
        board.arrayBoard[src - 1] = " ";

        if (src == 7)
        {
            playerMoved[0] = true;
        }
        else if (src == 8)
        {
            playerMoved[1] = true;
        }
        else if (src == 9)
        {
            playerMoved[2] = true;
        }
    }

    //cek apakah semua pawn player sudah bergerak
    public bool isAllMoved()
    {
        return playerMoved.All(b  => b);
    }
}

class Bot : Pawn
{
    public string icon;
    int[] pawnPos = new int[3];
    int[] emptySpaces = new int[3];
    private bool[] botMoved = new bool[3];


    public Bot(Board boardInstance) : base(boardInstance)
    {
        //Konstruktor
    }

    public void setPawn(string icon)
    {
        this.icon = icon; // Mengatur nilai properti icon
        base.setPawn(icon); // Memanggil setPawn dari kelas Pawn
    }

    public void movePawn(Board board, Bot bot)
    {
        int a = 0, b = 0;//indikator posisi 
        //cari posisi pion Bot
        for (int i = 0; i < 9; i++)
        {
            if (board.arrayBoard[i] == bot.icon)
            {
                pawnPos[a] = i;
                a++;
            }
        }

        //cari posisi kotak kosong
        for (int i = 0; i < 9; i++)
        {
            if (board.arrayBoard[i] == " ")
            {
                emptySpaces[b] = i;
                b++;
            }
        }

        //Acak posisi pion yang akan dipindahkan
        Random random1 = new Random();
        int rand1 = random1.Next(3);
        int src = pawnPos[rand1]; 

        //Acak posisi kotak kosong untuk mendapatkan nilai tujuan
        Random random2 = new Random();
        int rand2 = random2.Next(3);
        int dst = emptySpaces[rand2];

        //Pindahkan posisi awal ke posisi tujuan
        board.arrayBoard[dst] = bot.icon;
        board.arrayBoard[src] = " ";
        Console.WriteLine($"Bot memindahkan pion dari {src + 1} ke {dst + 1}");

        if (src == bot.Pawn1)
        {
            botMoved[0] = true;
        }
        else if (src == bot.Pawn2)
        {
            botMoved[1] = true;
        }
        else if (src == bot.pawn3)
        {
            botMoved[2] = true;
        }
    }

    public bool isAllMoved()
    {
        return botMoved.All(b  => b);
    }
}