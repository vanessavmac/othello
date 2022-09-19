#nullable enable
using System;
using static System.Console;

namespace Bme121
{
	class Player
    {
		public string color, name;
		public int score;
		
        public Player(string color, string name) 
        {
			this.color = color;
			this.score = 2;
			
			if (name == "") this.name = color;
			else this.name = name;
		}
		
		public override string ToString() 
		{
			return name;
		}
    }
    
    static partial class Program
    {
		//black is player 1 and white is player 2.
		static int currentPlayer = 1;
		static bool endGame = false;
		
        static void Main( )
        {
            Welcome();
            
            Player[ ] players = NewPlayer();
            Player blackDisk = players[0];
            Player whiteDisk = players[1];

            int[] size = GetBoardSize();
            while (size[0] % 2 != 0 || size[1] % 2 != 0 || size[0] < 4 || size[1] < 4 || size[0] > 26 || size[1] > 26) size = GetBoardSize();
			
			string[ , ] game = NewBoard( rows: size[0], cols: size[1] );
            
            while (!endGame)
            {
				Welcome();
				DisplayBoard( game );
				UpdateScore(whiteDisk, blackDisk, game);
				DisplayScore(blackDisk, whiteDisk);
				
				if (currentPlayer == 1) DisplayTurn(blackDisk);
				else DisplayTurn(whiteDisk);
				
				Write("Enter Your Choice: ");
				string choice = ReadLine() !;
				WriteLine( );
				
				if (choice == "quit") 
				{
					endGame = true;
					UpdateScore(whiteDisk, blackDisk, game);
					
					Welcome();
					DisplayBoard( game );
					WriteLine( );
					WriteLine("Game Over");
					DisplayScore(blackDisk, whiteDisk);
					DisplayWinners(blackDisk, whiteDisk);
				}
				else if (choice == "skip") ChangeTurn();
				else
				{
					TryMove(choice, game);
					UpdateScore(whiteDisk, blackDisk, game);
				}
				
				if (GameOver(game, size, whiteDisk, blackDisk))
				{
					endGame = true;
					Welcome();
					DisplayBoard( game );
					WriteLine( );
					WriteLine("Game Over");
					DisplayScore(blackDisk, whiteDisk);
					DisplayWinners(blackDisk, whiteDisk);
				}
			} 
        }
        
        static void Welcome()
        {
			Console.Clear( );
			WriteLine( );
            WriteLine( "Welcome to Othello!" );
            WriteLine( );
		}
		
		static Player[ ] NewPlayer()
		{
			WriteLine("The black disc player will go first");
            Write("Type the black disc (X) player name [or <Enter> for 'Black']: ");
            string name = ReadLine() !;
            Player blackDisk = new Player("Black", name);
            
            Write("Type the black disc (O) player name [or <Enter> for 'White']: ");
            name = ReadLine() !;
            Player whiteDisk = new Player("White", name);
            
            Player[ ] players = {blackDisk, whiteDisk};
            
            return players;
		}
		
		static int[] GetBoardSize()
		{
			WriteLine();
			int[] size = new int[2];
			Write("Enter board rows (4 to 26, even) [or <Enter> for 8]: ");
            string numRows = ReadLine() !;
            Write("Enter board columns (4 to 26, even) [or <Enter> for 8]: ");
            string numCols = ReadLine() !;
			
            if (numRows == "") size[0] = 8;
            else size[0] = int.Parse(numRows);
            if (numCols == "") size[1] = 8;
            else size[1] = int.Parse(numCols);
                        
            if (size[0] % 2 != 0 || size[1] % 2 != 0 || size[0] < 4 || size[1] < 4 || size[0] > 26 || size[1] > 26) 
            {
				Write("The number of rows and columns should be an even number greater than 3 and less than 27. \nYour choice didn't work! Press <Enter> to try again!");
				ReadLine();
			}
			
            return size;
		}
		
		static void DisplayScore(Player blackDisk, Player whiteDisk)
		{
			WriteLine( );
			WriteLine($"Scores: {blackDisk.name} {blackDisk.score}, {whiteDisk.name} {whiteDisk.score}");
			WriteLine( );
		}
    
		static void DisplayTurn(Player player)
		{
			if (currentPlayer == 1) WriteLine($"Turn is black disk (X) player: {player.name}");
			else WriteLine($"Turn is white disk (O) player: {player.name}");
		}
		
		static void DisplayWinners(Player blackDisk, Player whiteDisk)
		{
			if (blackDisk.score > whiteDisk.score) 
			{
				WriteLine($"Winner: {blackDisk.name}");
				int scoreDiff = blackDisk.score - whiteDisk.score;
				if (scoreDiff >= 2 && scoreDiff <= 10) WriteLine($"Defeated white by {scoreDiff} in a close game.");
				else if (scoreDiff >= 12 && scoreDiff <= 24) WriteLine($"Defeated white by {scoreDiff} in a hot game.");
				else if (scoreDiff >= 26 && scoreDiff <= 38) WriteLine($"Defeated white by {scoreDiff} in a fight game.");
				else if (scoreDiff >= 40 && scoreDiff <= 52) WriteLine($"Defeated white by {scoreDiff} in a walkaway game.");
				else if (scoreDiff >= 54 && scoreDiff <= 64) WriteLine($"Defeated white by {scoreDiff} in a perfect game.");
			}
			else if (blackDisk.score < whiteDisk.score) 
			{
				WriteLine($"Winner: {whiteDisk.name}");
				int scoreDiff = whiteDisk.score - blackDisk.score;
				if (scoreDiff >= 2 && scoreDiff <= 10) WriteLine($"Defeated black by {scoreDiff} in a close game.");
				else if (scoreDiff >= 12 && scoreDiff <= 24) WriteLine($"Defeated black by {scoreDiff} in a hot game.");
				else if (scoreDiff >= 26 && scoreDiff <= 38) WriteLine($"Defeated black by {scoreDiff} in a fight game.");
				else if (scoreDiff >= 40 && scoreDiff <= 52) WriteLine($"Defeated black by {scoreDiff} in a walkaway game.");
				else if (scoreDiff >= 54 && scoreDiff <= 64) WriteLine($"Defeated black by {scoreDiff} in a perfect game.");
			}
			else WriteLine($"Winners: {blackDisk.name} and {whiteDisk.name}");
			
			WriteLine();
		}
		
		static void ChangeTurn()
		{
			if (currentPlayer == 1) currentPlayer = 2;
			else currentPlayer = 1;
		}
		
		static void MakeMove(string move, string[ , ] game)
		{
			if (currentPlayer == 1) game[IndexAtLetter(move.Substring(0,1)), IndexAtLetter(move.Substring(1))] = "X";
			else game[IndexAtLetter(move.Substring(0,1)), IndexAtLetter(move.Substring(1))] = "O";
		}
    
		static void TryMove(string move, string[ , ] game)
		{
			int x = -1;
			int y = -1;
			
			//Check that the move is written correctly
			if (move.Length != 2)
			{
				Write("The move should be 2 characters, one for the row and one for the column. \nYour choice didn't work! Press <Enter> to try again!");
				ReadLine();
			}
			else 
			{
				x = IndexAtLetter(move.Substring(0,1));
				y = IndexAtLetter(move.Substring(1));
			
				if (x > game.GetLength(0) - 1 || y > game.GetLength(1) - 1 || x < 0 || y < 0)
				{
					Write($"The board only has {game.GetLength(0)} rows and {game.GetLength(1)} columns. \nYour choice didn't work! Press <Enter> to try again!");
					ReadLine();
				}
				else {
				
					//Check that the box isn't occupied
					if (game[x,y] == "X" || game[x,y] == "O") 
					{
						Write("Please place a disk on an unoccupied square. \nYour choice didn't work! Press <Enter> to try again!");
						ReadLine();
					}
					else
					{
						MakeMove(move, game);
						bool flip = false;
						
						for (int i = 0; i < 8; i++) 
						{
							if (TryDirection(i, move, game)) flip = true;
						}
						
						if (flip) ChangeTurn();
						else
						{
							Write("Your move does not outflank your opponent. If you cannot outflank your opponent you must skip your turn. \nYour choice didn't work! Press <Enter> to try again!");
							UndoMove(move, game);
							ReadLine();
						}
					}
				}
			}
		}
    
		static bool TryDirection(int direction, string move, string[ , ] game)
		{
			string currentPlayerChar;
			if (currentPlayer == 1) currentPlayerChar = "X";
			else currentPlayerChar = "O";
			int positionX = IndexAtLetter(move.Substring(0,1));
			int positionY = IndexAtLetter(move.Substring(1));
			int stepsTaken = 0;
			
			//Vertically Up
			if (direction == 0)
			{
				//Check if the first step is possible
				if (positionX-1 < 0) return false;
				else
				{
					//Check if the first cell hit is not the same colour as the player making the move
					if (game[positionX-1, positionY] == currentPlayerChar || game[positionX-1, positionY] == " ") return false;
					else
					{
						positionX--;
						stepsTaken++;
					}
				}
				
				//Return true if a flip is possible and change the board accordingly
				while (positionX < game.GetLength(0) && positionX >= 0)
				{
					if (game[positionX, positionY] == currentPlayerChar) 
					{
						for (int j = stepsTaken; j > 0; j--)
						{
							game[positionX, positionY] = currentPlayerChar;
							positionX++;
						}
						return true;
					}
					else if (game[positionX, positionY] == " ") return false;
					else
					{
						positionX--;
						stepsTaken++;
					}
				}
				return false;	
			}
			//Vertically Down
			else if (direction == 1)
			{
				if (positionX+1 > game.GetLength(0) - 1) return false;
				else
				{
					if (game[positionX+1, positionY] == currentPlayerChar || game[positionX+1, positionY] == " ") return false;
					else
					{
						positionX++;
						stepsTaken++;
					}
				}
				
				while (positionX < game.GetLength(0) && positionX >= 0)
				{
					if (game[positionX, positionY] == currentPlayerChar) 
					{
						for (int j = stepsTaken; j > 0; j--)
						{
							game[positionX, positionY] = currentPlayerChar;
							positionX--;
						}
						return true;
					}
					else if (game[positionX, positionY] == " ") return false;
					else
					{
						positionX++;
						stepsTaken++;
					}
				}
				return false;	
			}
			//Horizontally Left
			else if (direction == 2)
			{
				if (positionY-1 < 0) return false;
				else
				{
					if (game[positionX, positionY-1] == currentPlayerChar || game[positionX, positionY-1] == " ") return false;
					else
					{
						positionY--;
						stepsTaken++;
					}
				}
				
				while (positionY < game.GetLength(1) && positionY >= 0)
				{
					if (game[positionX, positionY] == currentPlayerChar) 
					{
						for (int j = stepsTaken; j > 0; j--)
						{
							game[positionX, positionY] = currentPlayerChar;
							positionY++;
						}
						return true;
					}
					else if (game[positionX, positionY] == " ") return false;
					else
					{
						positionY--;
						stepsTaken++;
					}
				}
				return false;	
			}
			//Horizontally Right
			else if (direction == 3)
			{
				if (positionY+1 > game.GetLength(1) - 1) return false;
				else
				{
					if (game[positionX, positionY+1] == currentPlayerChar || game[positionX, positionY+1] == " ") return false;
					else
					{
						positionY++;
						stepsTaken++;
					}
				}
				
				while (positionY < game.GetLength(1) && positionY >= 0)
				{
					if (game[positionX, positionY] == currentPlayerChar) 
					{
						for (int j = stepsTaken; j > 0; j--)
						{
							game[positionX, positionY] = currentPlayerChar;
							positionY--;
						}
						return true;
					}
					else if (game[positionX, positionY] == " ") return false;
					else
					{
						positionY++;
						stepsTaken++;
					}
				}
				return false;	
			}
			//Diagonally Upper Left
			else if (direction == 4)
			{
				if (positionY-1 < 0 || positionX-1 < 0) return false;
				else
				{
					if (game[positionX-1, positionY-1] == currentPlayerChar || game[positionX-1, positionY-1] == " ") return false;
					else
					{
						positionY--;
						positionX--;
						stepsTaken++;
					}
				}
				
				while (positionY < game.GetLength(1) && positionY >= 0 && positionX < game.GetLength(0) && positionX >= 0)
				{
					if (game[positionX, positionY] == currentPlayerChar) 
					{
						for (int j = stepsTaken; j > 0; j--)
						{
							game[positionX, positionY] = currentPlayerChar;
							positionY++;
							positionX++;
						}
						return true;
					}
					else if (game[positionX, positionY] == " ") return false;
					else
					{
						positionY--;
						positionX--;
						stepsTaken++;
					}
				}
				return false;	
			}
			//Diagonally Lower Left
			else if (direction == 5)
			{
				if (positionY-1 < 0 || positionX+1 > game.GetLength(0) - 1) return false;
				else
				{
					if (game[positionX+1, positionY-1] == currentPlayerChar || game[positionX+1, positionY-1] == " ") return false;
					else
					{
						positionY--;
						positionX++;
						stepsTaken++;
					}
				}
				
				while (positionY < game.GetLength(1) && positionY >= 0 && positionX < game.GetLength(0) && positionX >= 0)
				{
					if (game[positionX, positionY] == currentPlayerChar) 
					{
						for (int j = stepsTaken; j > 0; j--)
						{
							game[positionX, positionY] = currentPlayerChar;
							positionY++;
							positionX--;
						}
						return true;
					}
					else if (game[positionX, positionY] == " ") return false;
					else
					{
						positionY--;
						positionX++;
						stepsTaken++;
					}
				}
				return false;	
			}
			//Diagonally Upper Right
			else if (direction == 6)
			{
				if (positionY+1 > game.GetLength(1) - 1 || positionX-1 < 0) return false;
				else
				{
					if (game[positionX-1, positionY+1] == currentPlayerChar || game[positionX-1, positionY+1] == " ") return false;
					else
					{
						positionY++;
						positionX--;
						stepsTaken++;
					}
				}
				
				while (positionY < game.GetLength(1) && positionY >= 0 && positionX < game.GetLength(0) && positionX >= 0)
				{
					if (game[positionX, positionY] == currentPlayerChar) 
					{
						for (int j = stepsTaken; j > 0; j--)
						{
							game[positionX, positionY] = currentPlayerChar;
							positionY--;
							positionX++;
						}
						return true;
					}
					else if (game[positionX, positionY] == " ") return false;
					else
					{
						positionY++;
						positionX--;
						stepsTaken++;
					}
				}
				return false;	
			}
			//Diagonally Lower Right
			else if (direction == 7)
			{
				if (positionY+1 > game.GetLength(1) - 1 || positionX+1 > game.GetLength(0) - 1) return false;
				else
				{
					if (game[positionX+1, positionY+1] == currentPlayerChar || game[positionX+1, positionY+1] == " ") return false;
					else
					{
						positionY++;
						positionX++;
						stepsTaken++;
					}
				}
				
				while (positionY < game.GetLength(1) && positionY >= 0 && positionX < game.GetLength(0) && positionX >= 0)
				{
					if (game[positionX, positionY] == currentPlayerChar) 
					{
						for (int j = stepsTaken; j > 0; j--)
						{
							game[positionX, positionY] = currentPlayerChar;
							positionY--;
							positionX--;
						}
						return true;
					}
					else if (game[positionX, positionY] == " ") return false;
					else
					{
						positionY++;
						positionX++;
						stepsTaken++;
					}
				}
				return false;	
			} else return false; //This will never be reached since for loop goes up till direction = 7
			
		}
		
		static void UndoMove(string move, string[ , ] game)
		{
			game[IndexAtLetter(move.Substring(0,1)), IndexAtLetter(move.Substring(1))] = " ";
		}
		
		static void UpdateScore(Player whiteDisk, Player blackDisk, string[ , ] game)
		{
			int numWhiteDisks = 0;
			int numBlackDisks = 0;
			
			foreach (string str in game)
			{
				if (str == "X") numBlackDisks++;
				else if (str == "O") numWhiteDisks++;
				else { };
			}
			
			whiteDisk.score = numWhiteDisks;
			blackDisk.score = numBlackDisks;
		}
		
		static bool GameOver(string[ , ] game, int[ ] boardSize, Player whiteDisk, Player blackDisk)
		{
			if (whiteDisk.score + blackDisk.score > boardSize[0] * boardSize[1] || whiteDisk.score == 0 || blackDisk.score == 0) return true;
			
			foreach (string str in game)
			{
				if (str == " ") return false;
			}

			return true;
		}
    }
}
