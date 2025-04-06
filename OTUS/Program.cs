namespace OTUS
{
    internal class Program
    {

        private static List<string> tasks = new List<string>(); // Список для хранения задач

        static void Main(string[] args)
        {

            string name = string.Empty;     // ""
            Console.WriteLine("Добро пожаловать в программу!");

            while (true)
            {
                Console.WriteLine(!string.IsNullOrWhiteSpace(name)   // IsNullOrWhiteSpace - проверка на null и на пустое значение, ! - инверсия 
                    ? $"\n{name}, список доступных для Вас команд: \n/start \n/addtask \n/showtasks \n/removetask \n/help \n/info \n/exit"  // имя введено
                    : "\nСписок доступных команд: \n/start \n/addtask \n/showtasks \n/removetask \n/help \n/info \n/exit"); // имя не введено
                if (!string.IsNullOrWhiteSpace(name))    // имя введено (добавление команды /echo)
                    Console.WriteLine("/echo");

                string? input = Console.ReadLine();
                // Split - разбивает строку на части,
                // new[] { ' ' } - новый массив, разделитель пробел,
                // 2 — максимальное количество элементов в результирующем массиве
                // StringSplitOptions.RemoveEmptyEntries - удаление лишних пробелов
                string[] parts = input.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
                string command = parts[0].ToLower(); // первая часть массива присваивается в поле команда, ToLower - приводит команду к нижнему регистру
                string? argument = parts.Length > 1     // проверка есть ли вторая часть 
                    ? parts[1]  // если есть то присваивается в поле аргумент
                    : null; // если второй части нет, то аргумент null

                if (command == "/start")
                    CommandStart(ref name);

                else if (command == "/addtask")
                    CommandAddTask(name);

                else if (command == "/showtasks")
                    CommandShowTasks(name);

                else if (command == "/removetask")
                    CommandRemoveTask(name);

                else if (command == "/help")
                    CommandHelp(name);

                else if (command == "/info")
                    CommandInfo(name);

                else if (command == "/exit")
                {
                    CommandExit(name);
                    break;
                }

                else if (command == "/echo" && !string.IsNullOrWhiteSpace(name)) // введена команда echo и задано имя
                    CommandEcho(argument, name);


                // если команда не введена
                else
                {
                    Console.Write($"{name}! ");
                    Console.WriteLine("Неизвестная команда. Попробуйте снова.");
                }
            }

            Console.ReadLine();
        }

        static void CommandStart(ref string name)   //параметр передаётся по ссылке, что позволяет изменять исходную переменную.
        {
            Console.WriteLine("Введите Ваше имя");
            name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
                Console.WriteLine($"Приятно познакомиться, {name}!");
            Console.WriteLine();
        }

        static void CommandAddTask(string name)
        {
            Console.WriteLine(!string.IsNullOrWhiteSpace(name)
                ? $"{name}, введите описание задачи:"
                : "Введите описание задачи:");
            string? taskDescription = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(taskDescription))
            {
                tasks.Add(taskDescription);
                Console.WriteLine($"Задача добавлена: \"{taskDescription}\"");
            }
            else
            {
                Console.WriteLine("Ошибка: описание задачи не может быть пустым");
            }
            CommandShowTasks(name);  // Показываем список задач с номерами
        }

        static void CommandShowTasks(string name)
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine(!string.IsNullOrWhiteSpace(name)
                    ? $"{name}, список задач пуст."
                    : "Список задач пуст.");
                return;
            }

            Console.WriteLine("\nСписок задач:");
            for (int i = 0; i < tasks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {tasks[i]}");
            }
        }

        static void CommandRemoveTask(string name)
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine(!string.IsNullOrWhiteSpace(name)
                    ? $"{name}, список задач пуст. Нечего удалять."
                    : "Список задач пуст. Нечего удалять.");
                return;
            }

            CommandShowTasks(name);  // Показываем список задач с номерами

            Console.WriteLine("\nВведите номер задачи для удаления:");
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count) // Пытается преобразовать строку input в число (int),
                                                                                                        // Если успешно — сохраняет результат в переменную taskNumber и возвращает true
            {
                string removedTask = tasks[taskNumber - 1];
                tasks.RemoveAt(taskNumber - 1); // удаляет задачу
                Console.WriteLine($"Задача \"{removedTask}\" успешно удалена.");
            }
            else
            {
                Console.WriteLine("Ошибка: введен некорректный номер задачи.");
            }

            CommandShowTasks(name);  // Показываем список задач с номерами
        }

        static void CommandHelp(string name)
        {
            Console.WriteLine(!string.IsNullOrWhiteSpace(name)
                ? $"{name}, для Вас cправка по программе:" // Если name введена
                : "Справка по программе:");
            Console.WriteLine("/start - Ввести имя");
            Console.WriteLine("/addtask - Добавить задачу в список");
            Console.WriteLine("/showtasks - Показать список задач");
            Console.WriteLine("/removetask - Удалить задачу из списка");
            Console.WriteLine("/help - Получить справку");
            Console.WriteLine("/info - Получить информацию о программе");
            Console.WriteLine("/echo <текст> - Вывести текст (доступно после ввода имени)");
            Console.WriteLine("/exit - Выйти из программы");
            Console.WriteLine();
        }

        static void CommandInfo(string name)
        {
            Console.WriteLine(!string.IsNullOrWhiteSpace(name)
                ? $"{name}, для Вас информация о программе:" // Если name введена
                : "Информация о программе:");
            Console.WriteLine("Версия: 1.1");
            Console.WriteLine("Дата создания: 05.04.2025");
            Console.WriteLine("Разработчик: Лукин Н.С.");
            Console.WriteLine();
        }

        static void CommandEcho(string argument, string name)
        {
            if (argument != null)   // если аргумент не null
            {
                Console.WriteLine(argument);    //повторяет аргумент на консоль
                Console.WriteLine();
            }
            else
                Console.WriteLine($"Ошибка: {name}, Вы не указали текст для команды /echo");
        }

        static void CommandExit(string name)
        {
            Console.WriteLine(!string.IsNullOrWhiteSpace(name)
                ? $"До свидания, {name}!"   // Если name введена
                : "До свидания!");
        }
    }
}
