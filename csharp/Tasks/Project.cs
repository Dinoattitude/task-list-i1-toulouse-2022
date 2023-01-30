using System.Collections.Generic;
using System.Dynamic;

namespace Tasks
{
    internal class Project
    {
        private Dictionary<long, dynamic> _tasks = new Dictionary<long, dynamic>();

        public void Add(long identifier, string description, bool done)
        {
            dynamic task = new ExpandoObject();
            task.description = description;
            task.done = done;
            
            _tasks.Add(identifier, task);
        }

        public void PrintInto(IConsole console)
        {
            foreach (var task in _tasks)
            {
                var value = task.Value;
                var identifier = task.Key;
                var description = value.description;
                var done = value.done;
                console.WriteLine($"    [{(done ? 'x' : ' ')}] {identifier}: {description}");
            }
        }

        public void SetDoneIfExists(string identifier, bool done, IConsole console)
        {
            var longIdentifier = long.Parse(identifier);

            if (_tasks.ContainsKey(longIdentifier))
            {
                _tasks[longIdentifier].done = done;
            }
        }
    }
}
