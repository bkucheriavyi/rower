using System;
using System.Collections.Generic;
using System.IO;
using M.A.R.S.Core.Application.Interfaces;

namespace M.A.R.S.Core.Application
{
    public class ConsoleApplication<T> where T : IActor
    {
        public const string EXIT_CONST = "exit";

        private readonly IDictionary<int, (string, IAction<T>)> _actions = new SortedDictionary<int, (string, IAction<T>)>();

        public void Register(int key, string name, IAction<T> action)
        {
            if (_actions.ContainsKey(key))
            {
                throw new InvalidOperationException($"Sorry ,the item with key {key} was already registered.");
            }

            _actions.Add(key, (name, action));
        }

        public int Run(T actor, TextReader input, TextWriter output)
        {
            output.WriteLine($"Hello {actor.Name}, choose action and press enter when ready:");
            PrintInfo(output);

            string value;
            while (!(value = input.ReadLine()).Equals(EXIT_CONST, StringComparison.OrdinalIgnoreCase))
            {
                var (actionId, error) = this.ParseActionId(value, actor);

                if (error != null)
                {
                    output.WriteLine(error);
                    PrintInfo(output);
                    continue;
                }

                Execute(actionId, actor, input, output);
                PrintInfo(output);
            }

            return 0;
        }

        private IEnumerable<string> GetRegisteredActionsInfo()
        {
            foreach (var action in _actions)
            {
                var (name, _) = action.Value;
                yield return $"{action.Key}. {name}";
            }
        }

        private void PrintInfo(TextWriter output) 
        {
            output.WriteLine(string.Join(Environment.NewLine, this.GetRegisteredActionsInfo()));
        }

        private (int, string) ParseActionId(string value,IActor actor)
        {
            if (!int.TryParse(value, out int actionId))
            {
                return (-1, "Wrong input, expected integer ID");
            }

            if (!_actions.ContainsKey(actionId))
            {
                return (-1, $"Sorry {actor.Name}, there is no action registered with #{actionId}");
            }

            return (actionId, null);
        }

        private void Execute(int actionId, T actor, TextReader input, TextWriter output)
        {
            var (name, action) = _actions[actionId];
            try
            {
                action.Execute(new ActorContext<T>(actor, input, output));
            }
            catch (Exception ex)
            {
                output.WriteLine($"Unhandled exception occured while action execution.\n#{actionId} {name}", ex);
            }
        }
    }
}