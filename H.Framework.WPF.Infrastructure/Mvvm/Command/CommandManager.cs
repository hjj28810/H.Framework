using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace H.Framework.WPF.Infrastructure.Mvvm.Command
{
    public class CommandManager
    {
        #region DependencyProperty declarations

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
                typeof(ICommand),
                typeof(CommandManager),
                new PropertyMetadata(new PropertyChangedCallback(OnCommandChanged)));

        public static readonly DependencyProperty CommandEventNameProperty =
            DependencyProperty.RegisterAttached("CommandEventName",
                typeof(String),
                typeof(CommandManager),
                new PropertyMetadata(new PropertyChangedCallback(OnCommandEventNameChanged)));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                typeof(object),
                typeof(CommandManager),
                new PropertyMetadata(new PropertyChangedCallback(OnCommandParameterChanged)));

        #endregion DependencyProperty declarations

        #region DependencyProperty Get and Set methods

        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, ICommand command)
        {
            obj.SetValue(CommandProperty, command);
        }

        public static string GetCommandEventName(DependencyObject obj)
        {
            return (string)obj.GetValue(CommandEventNameProperty);
        }

        public static void SetCommandEventName(DependencyObject obj, string commandEventName)
        {
            obj.SetValue(CommandEventNameProperty, commandEventName);
        }

        public static string GetCommandParameter(DependencyObject obj)
        {
            return (string)obj.GetValue(CommandParameterProperty);
        }

        public static void SetCommandParameter(DependencyObject obj, string commandParameter)
        {
            obj.SetValue(CommandParameterProperty, commandParameter);
        }

        #endregion DependencyProperty Get and Set methods

        static CommandManager()
        {
            _commandDeclarations = new Dictionary<DependencyObject, CommandDeclaration>();
        }

        #region DependencyProperty Changed Callbacks

        private static void OnCommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            GetCommandDeclaration(obj).Command = (ICommand)e.NewValue;
        }

        private static void OnCommandEventNameChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            GetCommandDeclaration(obj).EventName = (string)e.NewValue;
        }

        private static void OnCommandParameterChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            GetCommandDeclaration(obj).CommandParameter = e.NewValue;
        }

        #endregion DependencyProperty Changed Callbacks

        #region CommandDeclaration dictionary

        private static Dictionary<DependencyObject, CommandDeclaration> _commandDeclarations;

        private static CommandDeclaration GetCommandDeclaration(DependencyObject obj)
        {
            if (!_commandDeclarations.ContainsKey(obj))
            {
                CommandDeclaration decl = new CommandDeclaration(obj);
                _commandDeclarations.Add(obj, decl);
            }
            return _commandDeclarations[obj];
        }

        #endregion CommandDeclaration dictionary

        private class CommandDeclaration
        {
            private DependencyObject _object;
            private ICommand _cmd = null;
            private string _eventName = String.Empty;

            public CommandDeclaration(DependencyObject obj)
            {
                _object = obj;
            }

            internal void ConnectHandler()
            {
                if (Command != null && !String.IsNullOrEmpty(EventName))
                {
                    EventInfo ev = GetEventInfo(_eventName);
                    if (ev == null)
                        throw new Exception("Cannot find event: " + _eventName);

                    ev.AddEventHandler(_object, GetDelegate());
                }
            }

            internal void DisconnectHandler()
            {
                if (!String.IsNullOrEmpty(EventName))
                {
                    EventInfo ev = GetEventInfo(EventName);
                    if (EventName != null)
                    {
                        ev.RemoveEventHandler(_object, GetDelegate());
                    }
                }
            }

            [Obfuscation]
            public void Handler(object sender, EventArgs e)
            {
                if (Command != null && Command.CanExecute(CommandParameter))
                    Command.Execute(CommandParameter);
            }

            private EventInfo GetEventInfo(string eventName)
            {
                Type t = _object.GetType();
                EventInfo ev = t.GetEvent(eventName);
                return ev;
            }

            private Delegate GetDelegate()
            {
                EventInfo ev = GetEventInfo(EventName);
                return Delegate.CreateDelegate(ev.EventHandlerType, this, this.GetType().GetMethod("Handler"), true);
            }

            public DependencyObject Object
            {
                get
                {
                    return _object;
                }
            }

            public object CommandParameter
            {
                get;
                set;
            }

            public ICommand Command
            {
                get
                {
                    return _cmd;
                }
                set
                {
                    if (_cmd == value)
                        return;

                    if (_cmd != null)
                        DisconnectHandler();

                    _cmd = value;

                    if (_cmd != null)
                        ConnectHandler();
                }
            }

            public string EventName
            {
                get
                {
                    return _eventName;
                }
                set
                {
                    if (_eventName == value)
                        return;

                    if (!string.IsNullOrEmpty(_eventName))
                        DisconnectHandler();

                    _eventName = value;

                    if (string.IsNullOrEmpty(_eventName))
                        return;

                    if (!String.IsNullOrEmpty(_eventName))
                        ConnectHandler();
                }
            }
        }
    }
}