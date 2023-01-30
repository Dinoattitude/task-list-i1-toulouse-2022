using System.Collections.Generic;

namespace Tasks
{
    internal class Projects
    {
        private readonly IDictionary<string, Project> _projects = new Dictionary<string, Project>();

        public void Add(string name)
        {
            _projects.Add(name, new Project());
        }

        public void PrintInto(IConsole console)
        {
            foreach (var project in _projects)
            {
                var key = project.Key;
                var value = project.Value;
                console.WriteLine(key);
                value.PrintInto(console);
                console.WriteLine();
            }
        }

        public void AddTaskToProject(string projectName, long identifier, string description, bool done, IConsole console)
        {
            if (!_projects.TryGetValue(projectName, out Project project))
            {
                console.WriteLine($"Could not find a project with the name \"{projectName}\".");
                return;
            }

            project.Add(identifier, description, done);
        }

        public void SetTaskDone(string taskIdentifier, bool done, IConsole console)
        {
            foreach (var project in _projects.Values)
                project.SetDoneIfExists(taskIdentifier, done, console);
        }
    }
}
