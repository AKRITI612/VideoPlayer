using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace VideoPlayerApplication.ViewModel
{
    /// <summary>
    /// The common base class for setting up value of the property and handle INotify event..
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notify the view that the given property has changed.
        /// </summary>
        /// <param name="p_PropertyName">Name of the property that changed. If not specified, the caller's property name will be used.</param>
        protected void OnPropertyChanged([CallerMemberName] string p_PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p_PropertyName));
        }

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
