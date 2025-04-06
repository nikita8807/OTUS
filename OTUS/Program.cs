namespace OTUS
{
    internal class Program
    {

        private static List<string> tasks = new List<string>(); 
        public static int taskCountLimit = 0;
        public static int taskLengthLimit = 0;
        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать в программу!");
            while (taskCountLimit < 1 || taskCountLimit > 100) 
            {
                try
                {
                    Console.WriteLine("Введите максимально допустимое количество задач:");
                    string? maxCountInput = Console.ReadLine();
                    taskCountLimit = ParseAndValidateInt(maxCountInput, 1, 100);
                    Console.WriteLine($"Максимальное количество задач установлено: {taskCountLimit}");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            while (taskLengthLimit < 1 || taskLengthLimit > 100) 
            {
                try
                {
                    Console.WriteLine("Введите максимально допустимую длину задачи:");
                    string? maxLengthLimitInput = Console.ReadLine();
                    taskLengthLimit = ParseAndValidateInt(maxLengthLimitInput, 1, 100); 
                    Console.WriteLine($"Максимально допустимая длина задачи установлена: {taskLengthLimit}");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            string name = string.Empty;    
            while (true)
            {
                try
                {
                    Console.WriteLine(!string.IsNullOrWhiteSpace(name)  
                        ? $"\n{name}, список доступных для Вас команд: \n/start \n/addtask \n/showtasks \n/removetask \n/help \n/info \n/exit" 
                        : "\nСписок доступных команд: \n/start \n/addtask \n/showtasks \n/removetask \n/help \n/info \n/exit"); 
                    if (!string.IsNullOrWhiteSpace(name))    
                        Console.WriteLine("/echo");

                    string? input = Console.ReadLine();
                   
                    string[] parts = input.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
                    string command = parts[0].ToLower(); 
                    string? argument = parts.Length > 1     
                        ? parts[1]  
                        : null; 

                    if (command == "/start")
                        CommandStart(ref name);

                    else if (command == "/addtask")
                        CommandAddTask(name, taskCountLimit, taskLengthLimit);

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

                    else if (command == "/echo" && !string.IsNullOrWhiteSpace(name)) 
                        CommandEcho(argument, name);

                   
                    else
                    {
                        Console.Write($"{name}! ");
                        Console.WriteLine("Неизвестная команда. Попробуйте снова.");
                    }
                }

                catch (TaskCountLimitException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (TaskLengthLimitException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (DuplicateTaskException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Произошла непредвиденная ошибка:");
                    Console.WriteLine($"Type: {ex.GetType()}");
                    Console.WriteLine($"Message: {ex.Message}");
                    Console.WriteLine($"StackTrace: {ex.StackTrace}");
                    Console.WriteLine($"StackTrace: {ex.InnerException}");
                }
            }
            Console.ReadLine();
        }

        static void CommandStart(ref string name)  
        {
            Console.WriteLine("Введите Ваше имя");
            name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
                Console.WriteLine($"Приятно познакомиться, {name}!");
            Console.WriteLine();
        }

        static void CommandAddTask(string name, int taskCountLimit, int taskLengthLimit)
        {
            if (tasks.Count >= taskCountLimit) 
            {
                throw new TaskCountLimitException(taskCountLimit); 
            }

            Console.WriteLine(!string.IsNullOrWhiteSpace(name)
                ? $"{name}, введите описание задачи:"
                : "Введите описание задачи:");
            string? taskDescription = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(taskDescription)) 
            {
                if (tasks.Contains(taskDescription)) 
                {
                    throw new DuplicateTaskException(taskDescription);
                }

                int taskLength = taskDescription.Length; 

                if (taskLength > taskLengthLimit) 
                {
                    throw new TaskLengthLimitException(taskLength, taskLengthLimit); 
                }
                else
                {
                    tasks.Add(taskDescription);
                    Console.WriteLine($"Задача добавлена: \"{taskDescription}\"");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: описание задачи не может быть пустым");
            }
            CommandShowTasks(name);  
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

            CommandShowTasks(name); 

            Console.WriteLine("\nВведите номер задачи для удаления:");
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count) 
                                                                                                        
            {
                string removedTask = tasks[taskNumber - 1];
                tasks.RemoveAt(taskNumber - 1); 
                Console.WriteLine($"Задача \"{removedTask}\" успешно удалена.");
            }
            else
            {
                Console.WriteLine("Ошибка: введен некорректный номер задачи.");
            }

            CommandShowTasks(name);  
        }

        static void CommandHelp(string name)
        {
            Console.WriteLine(!string.IsNullOrWhiteSpace(name)
                ? $"{name}, для Вас cправка по программе:" 
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
                ? $"{name}, для Вас информация о программе:" 
                : "Информация о программе:");
            Console.WriteLine("Версия: 1.1");
            Console.WriteLine("Дата создания: 05.04.2025");
            Console.WriteLine("Разработчик: Лукин Н.С.");
            Console.WriteLine();
        }

        static void CommandEcho(string argument, string name)
        {
            if (argument != null)   
            {
                Console.WriteLine(argument);    
                Console.WriteLine();
            }
            else
                Console.WriteLine($"Ошибка: {name}, Вы не указали текст для команды /echo");
        }

        static void CommandExit(string name)
        {
            Console.WriteLine(!string.IsNullOrWhiteSpace(name)
                ? $"До свидания, {name}!"   
                : "До свидания!");
        }

        static int ParseAndValidateInt(string? str, int min, int max)
        {
            if (!int.TryParse(str, out int result)) 
            {
                throw new ArgumentException($"Некорректное значение. Введите целое число.");
            }

            if (result < min || result > max)
            {
                throw new ArgumentException($"Число должно быть в диапазоне от {min} до {max}.");
            }

            return result;
        }

    }

    public class TaskCountLimitException : Exception
    {
        public int TaskCountLimit { get; } 

        public TaskCountLimitException(int taskCountLimit) 
            : base($"Превышено максимальное количество задач равное {taskCountLimit}")
        {
            TaskCountLimit = taskCountLimit;
        }
    }
    public class TaskLengthLimitException : Exception
    {
        public int TaskLength { get; }
        public int TaskLengthLimit { get; }

        public TaskLengthLimitException(int taskLength, int taskLengthLimit) 
            : base($"Длина задачи ‘{taskLength}’ превышает максимально допустимое значение {taskLengthLimit}")
        {
            TaskLength = taskLength;
            TaskLengthLimit = taskLengthLimit;
        }
    }
    public class DuplicateTaskException : Exception
    {
        public string Task { get; }

        public DuplicateTaskException(string task) 
            : base($"Задача ‘{task}’ уже существует")
        {
            Task = task;
        }
    }
}
