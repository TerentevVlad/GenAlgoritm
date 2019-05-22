using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgoritm2
{   
    class Program
    {
        static int[] Randomaizer(int[] numbers, int Count_numbers, ref Random rnd, int min_random, int max_random)
        {
            for (int i = 0; i < Count_numbers; i++)
            {
                numbers[i] = rnd.Next(min_random, max_random);
                Console.Write(numbers[i] + " ");
            }
            Console.Write("\n");
            return numbers;
        }
        static Cell Mutation(Cell cell, ref Random rnd, int percent_mutation)
        {
            int chance = percent_mutation;
            //Если выполнить мутацию
            if (0 == rnd.Next(0, chance))
            {
                //Выбираем индекс гена для мутации
                int index_mutation = rnd.Next(0, cell.Size);
                bool flag = true;
                while (flag)
                {
                    //Выбираем на что менять
                    int var_mutation = rnd.Next(0, cell.Count_var_gen);
                    //Console.WriteLine("index_mutation: " + index_mutation + "на что поменять: "+ var_mutation);
                    if (var_mutation != cell.gens[index_mutation])
                    {
                        cell.gens[index_mutation] = var_mutation;
                        flag = false;
                    }
                }
            }
            return cell;

        }
        static Cell Randomaizer(int Count_numbers, ref Random rnd, int min_random, int max_random)
        {
            Cell cell = new Cell(Count_numbers, 2);
            for (int i = 0; i < Count_numbers; i++)
            {
                cell.Set_gen_index(rnd.Next(0, max_random), i);
            }
            return cell;
        }

        static List<Cell> Breeding(List<Cell> Pull_Cells, ref Random rnd)
        {
            int n = Pull_Cells.Count;
            List<Cell> Male_Cells = new List<Cell>();
            List<Cell> Female_Cells = new List<Cell>();
            List<Cell> Child_Cells = new List<Cell>();


            //Разделяем на женские и мужкие клетки по четным и нечетным
            for (int i = 0; i < n; i++)
            {
                if (i % 2 == 0)
                {
                    Male_Cells.Add(Pull_Cells[i]);
                }
                else
                {
                    Female_Cells.Add(Pull_Cells[i]);
                }
            }
            //Начинаем скрещивать
            for (int i = 0; i < Male_Cells.Count; i++)
            {

                Cell male_cell = Male_Cells[i];
                Cell female_cell = Female_Cells[i];
                Cell child = male_cell;

                for (int j = 0; j < male_cell.Size; j++)
                {
                    //Console.Write("Index: " + j + " ");
                    //Выбираем будем засовывать ген мужской или женский
                    int male_or_female = rnd.Next(0, 2);
                    //Console.Write("Male: " + male_or_female + "\n");
                    //Если засовываем в ребенка мужской ген
                    if (male_or_female == 0)
                        child.gens[j] = male_cell.gens[j];
                    //Если засовываем в ребенка женский ген
                    else
                        child.gens[j] = female_cell.gens[j];

                }
                //Сохраняем ребенка
                Child_Cells.Add(child);
            }
            return Child_Cells;
        }
        static void Main(string[] args)
        {
            const bool log = false;                 //Лог

            Random rnd = new Random();

            const int Count_numbers = 100;           //Колчиество чисел

            const int Count_Cells = 24;                 //Количество клеток Кратное 2

            int[] numbers = new int[Count_numbers];     //Данные числа
          //int[] numbers = { 100, 15, 5, 3, 2, 15, 4, 4, 1, 1};

            int MinBalance = 100000;

            Cell minCell = new Cell(Count_numbers, 2);

            numbers = Randomaizer(numbers, Count_numbers, ref rnd, 0, 1000);

            int Sum = 0;
            for (int i = 0; i < Count_numbers; i++)
            {
                Sum += numbers[i];
            }
            Console.WriteLine("Sum: " + Sum);

            Cell exemplar = new Cell(Count_numbers, 2);

            //Пул клеток
            List<Cell> Pull_Cells = new List<Cell>();

            

            //Первоначальная Инициализация Пула разными клетками
            if (log)
            {
                Console.WriteLine("Инициализация");
            }
            for (int i = 0; i < Count_Cells; i++)
            {
                Pull_Cells.Add(Randomaizer(Count_numbers, ref rnd, 0, 2));
                if (log)
                    Pull_Cells[i].Print();
            }
            if (log)
                Console.WriteLine("============================");
            int K = 0;
            while (K < 1000)
            {
                if (log)
                    Console.WriteLine("Мутация");
                for (int i = 0; i < Count_Cells; i++)
                {
                    Pull_Cells[i] = Mutation(Pull_Cells[i], ref rnd, 2);
                    if (log)
                        Pull_Cells[i].Print();
                }
                if (log)
                    Console.WriteLine("============================");


                Pull_Cells = Breeding(Pull_Cells, ref rnd);

                if (log)
                {
                    Console.WriteLine("Скрещивание");
                    for (int i = 0; i < Pull_Cells.Count; i++)
                    {

                        Pull_Cells[i].Print();
                    }
                }
                if (log)
                    Console.WriteLine("============================");
                List<int> mass_cells_A = new List<int>();
                List<int> mass_cells_B = new List<int>();
                List<int> balance = new List<int>();
                List<int> index = new List<int>();

                //Находим кучи А и Б
                for (int i = 0; i < Pull_Cells.Count; i++)
                {
                    mass_cells_A.Add(0);
                    mass_cells_B.Add(0);
                    for (int j = 0; j < Pull_Cells[i].gens.Length; j++)
                    {
                        if (Pull_Cells[i].gens[j] == 0)
                            mass_cells_A[i] += numbers[j];
                        else
                            mass_cells_B[i] += numbers[j];
                    }
                }

                if (log)
                {
                    Console.WriteLine("Кучи А и Б");
                    for (int i = 0; i < Pull_Cells.Count; i++)
                    {
                        Console.Write(mass_cells_A[i] + " ");
                        Console.WriteLine(mass_cells_B[i]);
                    }
                }
                //находим балансы
                if (log)
                    Console.WriteLine("Балансы: ");

                for (int i = 0; i < Pull_Cells.Count; i++)
                {
                    balance.Add(Math.Abs(mass_cells_A[i] - mass_cells_B[i]));

                    if (log)
                        Console.Write(balance[i] + "\t");
                    index.Add(i);
                }

                if (log)
                    Console.WriteLine("\n==============================");
                //Сортировка
                for (int i = 0; i < Pull_Cells.Count; i++)
                {
                    for (int j = 0; j < Pull_Cells.Count - 1; j++)
                    {
                        if (balance[j] > balance[j + 1])
                        {
                            int b = index[j];
                            index[j] = index[j + 1];
                            index[j + 1] = b;
                            b = balance[j];
                            balance[j] = balance[j + 1];
                            balance[j + 1] = b;
                        }
                    }
                }
                if (log)
                {
                    for (int i = 0; i < Pull_Cells.Count; i++)
                    {
                        Console.Write(balance[i] + " ");
                    }
                    Console.Write("\n");

                    for (int i = 0; i < Pull_Cells.Count; i++)
                    {

                        Console.Write(index[i] + " ");
                    }
                    Console.Write("\n");
                }
                for (int i = 0; i < Pull_Cells.Count; i++)
                {
                    if (MinBalance > balance[i])
                    {
                        MinBalance = balance[i];
                        minCell = Pull_Cells[index[i]];
                    }
                }
                //Отсеивание половины клеток
                List<Cell> temp_Pull_Cells = new List<Cell>();
                for (int i = 0; i < Pull_Cells.Count / 2 + Pull_Cells.Count / 4; i++)
                {
                    temp_Pull_Cells.Add(Pull_Cells[index[i]]);
                }


                Pull_Cells = temp_Pull_Cells;
                if (log)
                {
                    Console.WriteLine("Оставшиеся клетки");
                    for (int i = 0; i < Pull_Cells.Count; i++)
                    {
                        Pull_Cells[i].Print();
                    }
                    Console.WriteLine("Выданные цифры");
                    for (int i = 0; i < Pull_Cells.Count; i++)
                    {
                        Console.Write(numbers[index[i]] + " ");
                    }
                }

                Console.WriteLine("\t\t\t\t\tМинимальный баланс: " + MinBalance);
                if (log)
                {
                    Console.Write("\t\t\t\t\tНаилучший вариант : ");
                    minCell.Print();
                }
                Console.Write("\t\t\t\t\t=========================\n");
                for (int i = Pull_Cells.Count; i < Count_Cells; i++)
                {
                    Pull_Cells.Add(Randomaizer(Count_numbers, ref rnd, 0, 2));
                }
            }















            Console.ReadKey();

        }
    }
}
