namespace Emby.Plugin.PlaylistConverter.Models
{
  /// <summary>
  ///   Represents the validation result for a single property
  /// </summary>
  public class ValidationResultItem
  {
    #region Enums

    /// <summary>
    ///   The severity of the validation message
    /// </summary>
    public enum Severity
    {
      /// <summary>
      ///   An informational validation message
      /// </summary>
      Info = 1,

      /// <summary>
      ///   A validation message containing a warning
      /// </summary>
      Warning = 2,

      /// <summary>
      ///   A validation message containing an error
      /// </summary>
      Error = 3
    }

    #endregion

    #region Properties

    /// <summary>
    ///   Gets or sets the validation severity.
    /// </summary>
    /// <value>
    ///   The validation severity.
    /// </value>
    public Severity ValidationSeverity { get; }

    /// <summary>
    ///   Gets or sets the message.
    /// </summary>
    /// <value>
    ///   The message.
    /// </value>
    public string Message { get; }

    /// <summary>
    /// Gets or sets the name of the property for which this validation message was created.
    /// </summary>
    /// <value>
    /// The name of the property.
    /// </value>
    public string PropertyName { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationResultItem"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="propertyName">The name of the property for which this validation message is created</param>
    /// <param name="validationSeverity">The validation severity.</param>
    public ValidationResultItem(string message, string propertyName, Severity validationSeverity = Severity.Error)
    {
      ValidationSeverity = validationSeverity;
      Message = message;
      PropertyName = propertyName;
    }

    #endregion

    #region Overrides of Object

    /// <inheritdoc />
    public override string ToString()
    {
      return $"{{ {PropertyName} ({ValidationSeverity}): {Message} }}";
    }

    #endregion
  }
}