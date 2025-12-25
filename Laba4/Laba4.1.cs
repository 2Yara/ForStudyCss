using System;

public class Date {
    private int _year;
    private int _month;
    private int _day;

    private const int MinYear = 1;
    private const int MaxYear = 9999;
    private readonly int[] _daysInMonth = { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

    // Конструктор без параметров (устанавливает текущую дату)
    public Date() {
        DateTime today = DateTime.Today;
        _year = today.Year;
        _month = today.Month;
        _day = today.Day;
    }

    // Конструктор с вводом параметров вручную
    public Date(int year, int month, int day) {
        SetDate(year, month, day);
    }

    // Проверка года с константами
    public int Year {
        get { return _year; }
        set {
            if (value < MinYear || value > MaxYear)
                throw new ArgumentOutOfRangeException(nameof(value), 
                    $"Год должен быть в диапазоне от {MinYear} до {MaxYear}");
            
            _year = value;
            AdjustDayForNewYearMonth(); // Корректируем день при изменении года
        }
    }

    // Свойство для месяца с проверкой
    public int Month {
        get { return _month; }
        set {
            if (value < 1 || value > 12)
                throw new ArgumentOutOfRangeException(nameof(value), 
                    "Месяц должен быть в диапазоне от 1 до 12");
            
            _month = value;
            AdjustDayForNewYearMonth(); // Корректируем день при изменении месяца
        }
    }

    // Свойство для дня с проверкой
    public int Day {
        get { return _day; }
        set {
            if (value < 1 || value > GetDaysInMonth(_year, _month))
                throw new ArgumentOutOfRangeException(nameof(value), 
                    "День должен быть в допустимом диапазоне для данного месяца и года");
            
            _day = value;
        }
    }

    // Метод для проверки високосного года
    private bool IsLeapYear(int year) {
        return (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
    }

    // Метод для изменения количества дней в феврале при условии високосного года
    private int GetDaysInMonth(int year, int month) {
        if (month == 2 && IsLeapYear(year))
            return 29;
        
        return _daysInMonth[month];
    }

    // Метод для корректировки дня при изменении года/месяца
    private void AdjustDayForNewYearMonth() {
        int maxDays = GetDaysInMonth(_year, _month);
        if (_day > maxDays)
            _day = maxDays;
    }

    // Метод для установки полной даты
    public void SetDate(int year, int month, int day) {
        if (year < MinYear || year > MaxYear)
            throw new ArgumentOutOfRangeException(nameof(year), 
                $"Год должен быть в диапазоне от {MinYear} до {MaxYear}");
        
        if (month < 1 || month > 12)
            throw new ArgumentOutOfRangeException(nameof(month), 
                "Месяц должен быть в диапазоне от 1 до 12");
        
        int maxDays = GetDaysInMonth(year, month);
        if (day < 1 || day > maxDays)
            throw new ArgumentOutOfRangeException(nameof(day), 
                $"День должен быть в диапазоне от 1 до {maxDays} для {month}/{year}");
        
        _year = year;
        _month = month;
        _day = day;
    }

    // Метод для изменения даты на другой день
    public void AddDays(int days) {
        if (days == 0) return;
        DateTime currentDate = new DateTime(_year, _month, _day);
        DateTime newDate = currentDate.AddDays(days);
        
        SetDate(newDate.Year, newDate.Month, newDate.Day);
    }

    // Метод для изменения даты на другой месяц
    public void AddMonths(int months) {
        if (months == 0) return;

        DateTime currentDate = new DateTime(_year, _month, _day);
        DateTime newDate = currentDate.AddMonths(months);
        
        SetDate(newDate.Year, newDate.Month, newDate.Day);
    }

    // Метод для изменения даты на другой год
    public void AddYears(int years) {
        if (years == 0) return;

        DateTime currentDate = new DateTime(_year, _month, _day);
        DateTime newDate = currentDate.AddYears(years);
        
        SetDate(newDate.Year, newDate.Month, newDate.Day);
    }
    public override string ToString() {
        return $"{_day:00}.{_month:00}.{_year:0000}";
    }

    // Статический метод для проверки корректности даты
    public static bool IsValidDate(int year, int month, int day) {
        try {
            new Date(year, month, day);
            return true;
        }
        catch {
            return false;
        }
    }
}

// Демонстрационная программа
class Program {
    static void Main() {
        Console.WriteLine("Работа класса даты\n");

        try {
            // Тесты конструкторов
            Date date1 = new Date(); 
            Console.WriteLine($"Текущая дата: {date1}");
            Date date2 = new Date(2024, 3, 15); 
            Console.WriteLine($"Заданная дата: {date2}");
            //Date date3 = new Date(2024);
            

            // Тесты свойств
            Date testDate = new Date(2024, 2, 28);
            Console.WriteLine($"Заданная дата: {testDate}");
            testDate.Year = 2025;
            Console.WriteLine($"Изменили год на 2025: {testDate}");
            testDate.Month = 4;
            Console.WriteLine($"Изменили месяц на апрель: {testDate}");
            testDate.Day = 15;
            Console.WriteLine($"Изменили день на 15: {testDate}");
            
            
            // Тесты методов изменения даты, по факту +сколько-то дней/месяцев/лет
            Date changingDate = new Date(2024, 1, 31);
            Console.WriteLine($"Заданная дата: {changingDate}");
            changingDate.AddDays(10);
            Console.WriteLine($"Изменение дней: {changingDate}");
            changingDate.AddMonths(2);
            Console.WriteLine($"Изменение месяца: {changingDate}");
            changingDate.AddYears(1);
            Console.WriteLine($"Изменение года: {changingDate}");
            
            
            // Тесты на високосный год
            Date leapDate = new Date(2024, 2, 28);
            Console.WriteLine($"Исходная дата (високосный год): {leapDate}");
            leapDate.AddDays(1);
            Console.WriteLine($"Следующая дата: {leapDate}");
            Date nonLeapDate = new Date(2023, 2, 28);
            Console.WriteLine($"Исходная дата (не високосный год): {nonLeapDate}");
            nonLeapDate.AddDays(1);
            Console.WriteLine($"Следующая дата: {nonLeapDate}");
            

            // Тесты исключений
            TestException("Создание даты с неверным годом (0)", () =>  {
                var d = new Date(0, 1, 1);
            });
            TestException("Создание даты с неверным месяцем (13)", () =>  {
                var d = new Date(2024, 13, 1);
            });
            TestException("Создание даты с неверным днем (32)", () =>  {
                var d = new Date(2024, 1, 32);
            });
            TestException("Установка неверного дня (29 февраля в невисокосный год)", () =>  {
                var d = new Date(2023, 2, 28);
                d.Day = 29;
            });
            

            // Тесты статического метода
            Console.WriteLine($"Дата 2024-02-29 корректна: {Date.IsValidDate(2024, 2, 29)}");
            Console.WriteLine($"Дата 2023-02-29 корректна: {Date.IsValidDate(2023, 2, 29)}");
            Console.WriteLine($"Дата 2024-13-01 корректна: {Date.IsValidDate(2024, 13, 1)}");
            
            
            // Дополнительные тесты
            Date complexDate = new Date(2024, 12, 31);
            Console.WriteLine($"Исходная дата: {complexDate}");
            complexDate.AddDays(1);
            Console.WriteLine($"После добавления 1 дня: {complexDate}");
            complexDate.AddMonths(1);
            Console.WriteLine($"После добавления 1 месяца: {complexDate}");
            complexDate.AddYears(1);
            Console.WriteLine($"После добавления 1 года: {complexDate}");
            
        }
        catch (Exception ex) {
            Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
        }
        
    }
    
    // Метод для тестирования исключений
    static void TestException(string testName, Action testAction) {
        try {
            testAction();
            Console.WriteLine($"{testName}: ОШИБКА - исключение не было выброшено!");
        }
        catch (Exception ex) {
            Console.WriteLine($"{testName}: Выброшено исключение - {ex.GetType().Name}: {ex.Message}");
        }
    }
}
