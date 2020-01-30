﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;

namespace BackgroundMusic.ViewModel
{
    /// <summary>
    /// A simple relay Command for easy use of the Command pattern.
    /// </summary>
    /// <remarks></remarks>
    [Serializable]
    public class RelayCommand : ICommand
    {
        #region Fields

        /// <summary>
        /// The action to execute.
        /// </summary>
        private readonly Action<object> _execute;

        /// <summary>
        /// The Predicate to indicate if this command can execute or not.
        /// </summary>
        private readonly Predicate<object> _canExecute;

        #endregion 

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        /// <remarks></remarks>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            this._execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this._canExecute = canExecute;
        }
        #endregion 

        #region ICommand Members

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        /// <remarks></remarks>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <remarks></remarks>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        #endregion 
    }
}
