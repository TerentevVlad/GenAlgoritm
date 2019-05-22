using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgoritm2
{
    class Cell
    {
        public int[] gens;
        public int Size;
        //Количество вариантов гена
        public int Count_var_gen { get; }
        public Cell(int count_gen, int Count_var_gen)
        {
            this.Count_var_gen = Count_var_gen;
            Size = count_gen;
            gens = new int[count_gen];
        }
        public void Set_gen_index(int value, int index)
        {
            gens[index] = value;
        }
        public void Print()
        {
            Console.Write("  ");
            for (int i = 0; i < Size; i++)
            {
                Console.Write(gens[i] + " ");
            }
            Console.Write("\n");
        }
    }
}
