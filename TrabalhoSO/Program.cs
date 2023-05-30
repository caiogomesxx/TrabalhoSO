using TrabalhoSO;

int tamanhoLabirinto = 5;
int numRatos = 5;

Labirinto labirinto = new Labirinto(tamanhoLabirinto, numRatos);
labirinto.ImprimirLabirinto();
Console.WriteLine("=================================");

labirinto.Iniciar();

Console.ReadKey();
