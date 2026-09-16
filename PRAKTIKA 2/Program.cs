using System;
using System.Collections.Generic;

// 1. Виды чая
enum TeaType { Black, Green, Herbal, Oolong }

// 2. Структура для объема со строковым представлением
struct Volume
{
    public double Value { get; set; }
    public Volume(double value) { Value = value; }
    public override string ToString() => $"{Value} мл";
}

// 3. Базовый класс Drink
class Drink
{
    private string type;
    public Volume Volume { get; set; }
    public string Type
    {
        get => type;
        set => type = value;
    }

    public const string CafeName = "Чайная";

    public Drink() : this(200, "Напиток") { }
    public Drink(double v, string t)
    {
        Volume = new Volume(v);
        type = t;
    }

    public void Deconstruct(out Volume v, out string t)
    {
        v = Volume;
        t = type;
    }

    public override string ToString() => $"{type}, {Volume}";
}

// 4. Класс Tea
class Tea : Drink
{
    public int Quantity { get; set; }
    public TeaType TeaKind { get; set; }

    public Tea() : base(200, "Чай")
    {
        Quantity = 1;
        TeaKind = TeaType.Black;
    }
    public Tea(double v, TeaType kind, int qty) : base(v, "Чай")
    {
        TeaKind = kind;
        Quantity = qty;
    }

    // Метод 1 — заварить
    public void Brew()
    {
        if (Quantity > 0)
            Console.WriteLine($"Чай {TeaKind} заварен.");
        else
            Console.WriteLine("Нет пакетиков, заваривать нечего!");
    }

    // Метод 2 — выпить (уменьшает объём и пакетики)
    public void DrinkUp()
    {
        if (Quantity <= 0 && Volume.Value <= 0)
        {
            Console.WriteLine("Чай уже закончился!");
            return;
        }

        if (Quantity > 0) Quantity--;

        double newVol = Volume.Value - 50;
        if (newVol < 0) newVol = 0;
        Volume = new Volume(newVol);

        Console.WriteLine($"Чай {TeaKind} выпит. Осталось пакетиков: {Quantity}, объём: {Volume}");
    }

    // Метод 3 — описание
    public string Describe() => $"Чай {TeaKind}, объем {Volume}, пакетиков: {Quantity}";

    public void AddBag() => Quantity++;

    public void RemoveBag()
    {
        if (Quantity > 0) Quantity--;
        else Console.WriteLine("Пакетиков больше нет!");
    }

    // Перегрузка арифметической операции
    public static Tea operator +(Tea a, Tea b)
        => new Tea(a.Volume.Value + b.Volume.Value, a.TeaKind, a.Quantity + b.Quantity);

    // Перегрузка операции присваивания
    public void Assign(Tea other)
    {
        Volume = other.Volume;
        TeaKind = other.TeaKind;
        Quantity = other.Quantity;
    }

    public override string ToString() => Describe();
}

// 6. Класс Coffee
class Coffee : Drink
{
    public Coffee() : base(150, "Кофе") { }
    public Coffee(double v) : base(v, "Кофе") { }

    public void Brew() => Console.WriteLine("Кофе сварен.");
    public override string ToString() => $"Кофе, {Volume}";
}

// 7. Интерфейс ICafe
interface ICafe
{
    void ShowDrinks();
}

// 8. Класс Cafe
class Cafe : ICafe
{
    public List<Drink> Drinks { get; set; } = new List<Drink>();

    public void ShowDrinks()
    {
        Console.WriteLine($"=== {Drink.CafeName} ===");
        if (Drinks.Count == 0)
        {
            Console.WriteLine("(список пуст)");
            return;
        }
        foreach (var d in Drinks) Console.WriteLine(d);
    }
}

class Program
{
    static void Main()
    {
        Cafe cafe = new Cafe();
        Tea tea = new Tea();

        while (true)
        {
            Console.WriteLine("\n1. Задать параметры чая");
            Console.WriteLine("2. Показать свойства");
            Console.WriteLine("3. Заварить");
            Console.WriteLine("4. Выпить");
            Console.WriteLine("5. +1 пакетик");
            Console.WriteLine("6. -1 пакетик");
            Console.WriteLine("7. Добавить чай и кофе в кафе и показать");
            Console.WriteLine("8. Очистить кафе");
            Console.WriteLine("0. Выход");
            Console.Write("Выбор: ");

            int c;
            if (!int.TryParse(Console.ReadLine(), out c)) continue;

            switch (c)
            {
                case 1:
                    Console.Write("Объем (мл): ");
                    double v = double.Parse(Console.ReadLine());
                    Console.Write("Тип (0-Black, 1-Green, 2-Herbal, 3-Oolong): ");
                    TeaType t = (TeaType)int.Parse(Console.ReadLine());
                    Console.Write("Пакетиков: ");
                    int q = int.Parse(Console.ReadLine());
                    tea = new Tea(v, t, q);
                    break;

                case 2:
                    Console.WriteLine(tea.ToString());
                    break;

                case 3:
                    tea.Brew();
                    break;

                case 4:
                    tea.DrinkUp();
                    break;

                case 5:
                    tea.AddBag();
                    Console.WriteLine($"Пакетиков теперь: {tea.Quantity}");
                    break;

                case 6:
                    tea.RemoveBag();
                    Console.WriteLine($"Пакетиков теперь: {tea.Quantity}");
                    break;

                case 7:
                    cafe.Drinks.Clear();                 
                    cafe.Drinks.Add(tea);
                    cafe.Drinks.Add(new Coffee(180));
                    cafe.ShowDrinks();
                    break;

                case 8:
                    cafe.Drinks.Clear();
                    Console.WriteLine("Список кафе очищен.");
                    break;

                case 0:
                    return;

                default:
                    Console.WriteLine("Нет такого пункта.");
                    break;
            }
        }
    }
}