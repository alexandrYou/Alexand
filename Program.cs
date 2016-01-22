using Shop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Slastenin
{
    public static class Program
    {
        static void Main(string[] args)
        {

            if (File.Exists("base.txt"))
            {
                try
                {
                    StreamReader stream = new StreamReader("base.txt");
                    List<IItem> catalog = new List<IItem>();
                    while (!stream.EndOfStream)
                    {
                        string[] bd = stream.ReadLine().Split('|');
                        catalog.Add(new Shop1.GuitarItem(bd[0], bd[1], Convert.ToInt32(bd[2]), (Shop1.getMemType)(Convert.ToInt32(bd[3])), bd[4], bd[5]));
                    }

                    catalog.Add(new Shop.Shop1.Keyboard("Defender", "Color", 2000, 106, 15, "Selicon"));
                    Shop1 shop = new Shop1(catalog);
                    Console.WriteLine("Список товаров:\n");
                    Console.WriteLine("\n\n");
                    Shop1.Warehouse wareHouse = new Shop1.Warehouse(catalog);
                    wareHouse.showCatalog();

                    Console.WriteLine("\nИспользование IEnumerable<IItem>\n");
                    foreach (IItem item in shop)
                    {
                        Console.WriteLine(item.ToString());

                    }

                    Console.WriteLine("\nИспользование IList<IItem>\n");
                    for (int i = 0; i < shop.Count; i++)
                    {
                        Console.WriteLine(shop[i].ToString());
                    }
                    Console.Write("\n\nМЕНЮ сортировщика (отсортировать по полю):\n1. Фирма \n2. Модель \n3. Цена \n4. Материал \n\nВведите (через пробел) поля для сортировки:");
                    string fix = string.Empty;
                    while (true)
                    {
                        fix = Console.ReadLine();
                        List<int> column = new List<int>();
                        foreach (char ch in fix)
                        {
                            try
                            {
                                string numbStr = ch.ToString();
                                int numb = Convert.ToInt32(numbStr);
                                if (numb < 7)
                                    column.Add(numb);
                            }
                            catch (Exception)
                            { }
                        }
                        foreach (int n in column)
                        {
                            switch (n)
                            {

                                case 1:
                                    {
                                        shop.Sort(shop.comparemanufacturer, false);


                                        break;
                                    }
                                case 2:
                                    {
                                        shop.Sort(shop.compareModel, false);
                                        break;
                                    }

                                case 3:
                                    {
                                        shop.Sort(shop.comparePrice, false);


                                        break;
                                    }
                                case 4:
                                    {
                                        shop.Sort(shop.compareMaterial, false);
                                        break;
                                    }

                                default: { Console.WriteLine("Неверный номер команды!\n\n"); break; }
                            }
                        }
                        Console.WriteLine("\n\nОтсортированные товары:\n");
                        shop.ShowItems();

                        Console.ReadKey();

                    }

                }
                catch (Exception) { Console.ReadKey(); }

            }

            else
            {
                Console.WriteLine("Файл базы не создан.");
                Console.ReadKey();
            }



        }


    }
}
