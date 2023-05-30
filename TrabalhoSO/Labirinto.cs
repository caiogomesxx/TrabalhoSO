using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TrabalhoSO
{
    public class Labirinto
    {
        private int[,] matrizLabirinto;
        private int tamanho;
        private int numRatos;
        private int[] posicaoQueijo;

        public Labirinto(int tamanho, int numRatos)
        {
            this.tamanho = tamanho;
            this.numRatos = numRatos;
            this.matrizLabirinto = new int[tamanho, tamanho];
            this.posicaoQueijo = new int[] { 0, 0 }; // Posição inicial do queijo

            // Inicializar a matriz do labirinto com células vazias
            for (int i = 0; i < tamanho; i++)
            {
                for (int j = 0; j < tamanho; j++)
                {
                    matrizLabirinto[i, j] = 0;
                }
            }

            // Posicionar o queijo no labirinto
            matrizLabirinto[0, 0] = 1;

            // Posicionar os ratos aleatoriamente no labirinto
            Random random = new Random();
            for (int i = 0; i < numRatos; i++)
            {
                int x = random.Next(tamanho);
                int y = random.Next(tamanho);
                if (matrizLabirinto[x, y] == 0)
                {
                    matrizLabirinto[x, y] = 2;
                }
                else
                {
                    i--; // Tentar novamente posicionar o rato em uma célula vazia
                }
            }

            // Gerar as paredes do labirinto aleatoriamente
            for (int i = 0; i < tamanho; i++)
            {
                for (int j = 0; j < tamanho; j++)
                {
                    if (matrizLabirinto[i, j] == 0)
                    {
                        if (random.Next(100) < 20) // 20% de chance de uma célula se tornar uma parede
                        {
                            matrizLabirinto[i, j] = -1;
                        }
                    }
                }
            }
        }
        public void ImprimirLabirinto()
        {
            for (int i = 0; i < tamanho; i++)
            {
                for (int j = 0; j < tamanho; j++)
                {
                    Console.Write(matrizLabirinto[i, j].ToString().Replace("-1", "P").Replace("1", "Q").Replace("2", "R").Replace("0", "-") + "\t");
                }
                Console.WriteLine();
            }
        }

        public void Iniciar()
        {
            for (int i = 0; i < numRatos; i++)
            {
                Thread ratoThread = new Thread(new ParameterizedThreadStart(MoverRato));
                ratoThread.Start(i);
            }
        }
        private void MoverRato(object idRato)
        {
            int id = (int)idRato;
            Random random = new Random();

            while (true)
            {
                int x = -1, y = -1;
                bool encontrouQueijo = false;

                // Encontrar a posição atual do rato
                for (int i = 0; i < tamanho; i++)
                {
                    for (int j = 0; j < tamanho; j++)
                    {
                        if (matrizLabirinto[i, j] == 2 + id)
                        {
                            x = i;
                            y = j;
                            break;
                        }
                    }

                    if (x != -1)
                    {
                        break;
                    }
                }

                // Mover o rato
                while (!encontrouQueijo)
                {
                    // Definir os movimentos possíveis
                    List<int[]> movimentosPossiveis = new List<int[]>();
                    if (x - 1 >= 0 && matrizLabirinto[x - 1, y] != -1) // Movimento para cima
                    {
                        movimentosPossiveis.Add(new int[] { x - 1, y });
                    }
                    if (x + 1 < tamanho && (x + 1) != 0 && matrizLabirinto[x + 1, y] != -1) // Movimento para baixo
                    {
                        movimentosPossiveis.Add(new int[] { x + 1, y });
                    }
                    if (y - 1 >= 0 && matrizLabirinto[x, y - 1] != -1) // Movimento para a esquerda
                    {
                        movimentosPossiveis.Add(new int[] { x, y - 1 });
                    }
                    if (y + 1 < tamanho && (y + 1) != 0 && matrizLabirinto[x, y + 1] != -1) // Movimento para a direita
                    {
                        movimentosPossiveis.Add(new int[] { x, y + 1 });
                    }

                    if (movimentosPossiveis.Count > 0)
                    {
                        // Escolher um movimento aleatório
                        int[] movimento = movimentosPossiveis[random.Next(movimentosPossiveis.Count)];

                        // Atualizar a posição do rato
                        matrizLabirinto[x, y] = 0;
                        x = movimento[0];
                        y = movimento[1];
                        matrizLabirinto[x, y] = 2 + id;

                        // Verificar se o rato encontrou o queijo
                        if (x == posicaoQueijo[0] && y == posicaoQueijo[1])
                        {
                            encontrouQueijo = true;
                            Console.WriteLine("O rato " + id + " encontrou o queijo!");
                        }

                        // Imprimir o labirinto
                        ImprimirLabirinto();
                        Console.WriteLine("=================================");
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}
