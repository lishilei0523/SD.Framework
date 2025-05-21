namespace SD.Infrastructure.Avalonia.Enums
{
    /// <summary>
    /// Specifies the buttons that are displayed on a message box.
    /// Used as an argument of the Overload:System.Windows.MessageBox.Show method.
    /// </summary>
    public enum MessageBoxButton
    {
        /// <summary>
        /// The message box displays an OK button.
        /// </summary>
        OK = 0,

        /// <summary>
        /// The message box displays OK and Cancel buttons.
        /// </summary>
        OKCancel = 1,

        /// <summary>
        /// The message box displays Yes, No, and Cancel buttons.
        /// </summary>
        YesNoCancel = 3,

        /// <summary>
        /// The message box displays Yes and No buttons.
        /// </summary>
        YesNo = 4
    }
}
