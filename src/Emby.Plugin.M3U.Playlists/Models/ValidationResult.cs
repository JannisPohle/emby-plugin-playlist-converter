namespace Emby.Plugin.M3U.Playlists.Models
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

    #endregion

    #region Methods

    #region Overrides of Object

    /// <inheritdoc />
    public override string ToString()
    {
      //TODO implement
      return base.ToString();
    }

    #endregion

    #endregion
  }
}