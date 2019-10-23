using System;
using System.Windows;

namespace PolyPaint.Attached_Properties
{
    public abstract class BaseAttachedProperty<TParent, TProperty>
        where TParent : new()
    {
        #region Public Events

        public event Action<DependencyObject, DependencyPropertyChangedEventArgs> ValueChanged = (sender, e) => { };

        public event Action<DependencyObject, object> ValueUpdated = (sender, value) => { };

        #endregion

        #region Public Properties

        public static TParent Instance { get; private set; } = new TParent();

        #endregion

        #region Attached Property Definitions

        public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached(
            "Value", 
            typeof(TProperty), 
            typeof(BaseAttachedProperty<TParent, TProperty>), 
            new UIPropertyMetadata(
                default(TProperty),
                new PropertyChangedCallback(OnValuePropertyChanged),
                new CoerceValueCallback(OnValuePropertyUpdated)
                ));

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Call the parent function
            (Instance as BaseAttachedProperty<TParent, TProperty>)?.OnValueChanged(d, e);

            // Call event listeners
            (Instance as BaseAttachedProperty<TParent, TProperty>)?.ValueChanged(d, e);
        }

        private static object OnValuePropertyUpdated(DependencyObject d, object value)
        {
            // Call the parent function
            (Instance as BaseAttachedProperty<TParent, TProperty>)?.OnValueUpdated(d, value);

            // Call event listeners
            (Instance as BaseAttachedProperty<TParent, TProperty>)?.ValueUpdated(d, value);

            // Return the value
            return value;
        }

        public static TProperty GetValue(DependencyObject d) => (TProperty)d.GetValue(ValueProperty);

        public static void SetValue(DependencyObject d, TProperty value) => d.SetValue(ValueProperty, value);

        #endregion

        #region Event Methods

        public virtual void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) { }

        public virtual void OnValueUpdated(DependencyObject sender, object value) { }

        #endregion
    }
}
