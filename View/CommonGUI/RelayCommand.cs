using System;
using System.Windows.Input;

namespace VideoPlayerApplication.View.CommonGUI
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _mExecuteMethod;
        private readonly Func<object, bool> _mCanExecuteMethod;

        /// <summary>
        /// Initializes a new instance of <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="pExecuteMethod">The execution logic.</param>
        /// <param name="pCanExecuteMethod">(Optional) The execution status logic. Always true if not provided.</param>
        public RelayCommand(Action<object> pExecuteMethod, Func<object, bool> pCanExecuteMethod = null)
        {
            _mExecuteMethod = pExecuteMethod ?? throw new ArgumentNullException(nameof(pExecuteMethod));
            _mCanExecuteMethod = pCanExecuteMethod;
        }

        /// <inheritdoc />
        /// <summary>
        /// Determines whether this <see cref="T:SaberApp.GUI.CommonGUI.RelayCommand" /> can execute in its current state.
        /// </summary>
        /// <param name="pParameter">
        /// (Optional) Data used by the command.
        /// </param>
        /// <returns>True if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object pParameter = null)
        {
            return _mCanExecuteMethod?.Invoke(pParameter) ?? true;
        }

        /// <inheritdoc />
        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <inheritdoc />
        /// <summary>
        /// Executes the <see cref="T:SaberApp.GUI.CommonGUI.RelayCommand" /> on the current command target.
        /// </summary>
        /// <param name="pParameter">
        /// (Optional) Data used by the command.
        /// </param>
        public void Execute(object pParameter = null)
        {
            _mExecuteMethod(pParameter);
        }

        /// <summary>
        /// Update the command when needed.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
