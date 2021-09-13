using System.Collections.Generic;
using System.Linq;

namespace Emby.Plugin.PlaylistConverter.Models
{
  /// <summary>
  ///   Represents a validation result
  /// </summary>
  public class ValidationResult
  {
    #region Properties

    /// <summary>
    ///   Gets or sets a value indicating whether the validation was a success.
    /// </summary>
    /// <value>
    ///   <c>true</c> if success; otherwise, <c>false</c>.
    /// </value>
    public bool Success { get; set; }

    /// <summary>
    ///   Gets or sets the validation messages.
    /// </summary>
    /// <value>
    ///   The validation messages.
    /// </value>
    public IList<ValidationResultItem> ValidationMessages { get; }

    #endregion

    #region Constructors

    /// <summary>
    ///   Initializes a new instance of the <see cref="ValidationResult" /> class.
    /// </summary>
    public ValidationResult()
    {
      ValidationMessages = new List<ValidationResultItem>();
    }

    #endregion

    #region Methods
    
    #region Overrides of Object

    /// <inheritdoc />
    public override string ToString()
    {
      return ToString(ValidationResultItem.Severity.Error);
    }

    /// <summary>
    /// Converts to string. The severity limits which messages are added to the result.
    /// Only messages with a severity that is higher or equal to the given severity will be added to the result
    /// </summary>
    /// <param name="severity">The severity.</param>
    /// <returns>
    /// A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public string ToString(ValidationResultItem.Severity severity)
    {
      var messages = string.Join("; ", ValidationMessages.Where(message => message.ValidationSeverity >= severity));
      return $"Validation {(Success ? "succeeded" : "failed")}. {(string.IsNullOrWhiteSpace(messages) ? "" : $"Details: {messages}")}";
    }

    #endregion

    #endregion
  }
}