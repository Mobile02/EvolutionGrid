using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EvolutionGrid.Model
{
    public class FileOperation
    {
        private Constants constants = new Constants();

        public void SaveBrain(Square[][] worldMap)
        {
            for (int y = 1; y < constants.WorldSizeY - 1; y++)
            {
                for (int x = 1; x < constants.WorldSizeX - 1; x++)
                {
                    if (worldMap[y][x].TypeSquare == TypeSquare.BIO)
                    {
                        using (StreamWriter streamWriter = new StreamWriter("brain.txt", true))
                        {
                            string text = "";
                            for (int j = 0; j < worldMap[y][x].Brain.Length; j++)
                            {
                                text += worldMap[y][x].Brain[j].ToString() + " ";
                            }

                            streamWriter.WriteLine(text);
                        };
                    }
                }
            }
            using (StreamWriter streamWriter = new StreamWriter("brain.txt", true))
            {
                streamWriter.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            }
        }

        public void SaveTimeLife(int[] arrayTimeLife)
        {
            using (StreamWriter streamWriter = new StreamWriter("TimeLife.txt", false))
            {
                for (int i = 0; i < arrayTimeLife.Length; i++)
                {
                    if (arrayTimeLife[i] == 0)
                        break;
                    streamWriter.WriteLine(arrayTimeLife[i].ToString());
                }
            };
        }
    }
}
