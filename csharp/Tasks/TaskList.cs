using System.Threading;

namespace Tasks
{
	public sealed class TaskList
	{
		private readonly Projects _projects = new ();
		private readonly IConsole _console;
		
		private long _lastIdentifier;

		public static void Main()
		{
			var cancellationToken = CancellationToken.None;
			new TaskList(new RealConsole()).Run(cancellationToken);
		}

		public TaskList(IConsole console)
		{
			this._console = console;
		}

		public void Run(CancellationToken token)
        {
            while (RunOnce())
            {
				token.ThrowIfCancellationRequested();
            }
        }

        private bool RunOnce()
        {
            _console.Write("> ");
            var command = _console.ReadLine();
            if (command == "quit") return false;

            Execute(command);

			return true;
        }

		private void Execute(string commandLine)
		{
			var commandRest = commandLine.Split(' ', 2);
			var command = commandRest[0];

			switch (command) {
			case "show":
				Show();
				return;
			case "add":
				Add(commandRest[1]);
				return;
			case "check":
				Check(commandRest[1], true);
				return;
			case "uncheck":
				Check(commandRest[1], false);
				return;
			case "help":
				Help();
				return;
			}

            Error(command);
        }

        private void Show() => _projects.PrintInto(_console);

		private void Add(string commandLine)
		{
			var subcommandRest = commandLine.Split(' ', 2);
			var subcommand = subcommandRest[0];

			if (subcommand == "project") {
				AddProject(subcommandRest[1]);
                return;
            }
            
            if (subcommand == "task") {
				var projectTask = subcommandRest[1].Split(' ', 2);
				AddTask(projectTask[0], projectTask[1]);
            }
		}

		private void AddProject(string name) => _projects.Add(name);

		private void AddTask(string project, string description)
        {
            _projects.AddTaskToProject(project,
                NextId(),
                description,
                false,
				_console
            );
        }

		private void Check(string idString, bool check)
		{
			SetDone(idString, check);
		}

		private void SetDone(string idString, bool done) => _projects.SetTaskDone(idString, done, _console);

		private void Help()
		{
			_console.WriteLine("Commands:");
			_console.WriteLine("  show");
			_console.WriteLine("  add project <project name>");
			_console.WriteLine("  add task <project name> <task description>");
			_console.WriteLine("  check <task ID>");
			_console.WriteLine("  uncheck <task ID>");
			_console.WriteLine();
		}

		private void Error(string command)
		{
			_console.WriteLine($"I don't know what the command \"{command}\" is.");
		}

		private long NextId()
		{
			return ++_lastIdentifier;
		}
	}
}
