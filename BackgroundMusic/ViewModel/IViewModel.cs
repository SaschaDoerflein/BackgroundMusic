﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BackgroundMusic.ViewModel
{
    /// <summary>
    /// Interface for ViewModel.
    /// </summary>
    public interface IViewModel : INotifyPropertyChanged
    {

    }

    /// <summary>
    /// Interface for ViewModel.
    /// </summary>
    /// <typeparam name="TModel">The type of the Model.</typeparam>
    public interface IViewModel<TModel> : IViewModel
    {
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        [Browsable(false)]
        [Bindable(false)]
        TModel Model { get; set; }
    }
}
