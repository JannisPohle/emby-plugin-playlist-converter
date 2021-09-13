namespace Emby.Plugin.PlaylistConverter.Abstractions
{
  /// <summary>
  /// Interface for the result of an operation
  /// </summary>
  public interface IOperationResult
  {
    /// <summary>
    ///   Gets or sets a value indicating whether the operation was a success.
    /// </summary>
    /// <value>
    ///   <c>true</c> if success; otherwise, <c>false</c>.
    /// </value>
    bool Success { get; set; }

    /// <summary>
    ///   Gets or sets a message, with additional information about the operation.
    /// </summary>
    /// <value>
    ///   The message.
    /// </value>
    string Message { get; set; }
  }
}