using System;
using Emby.Plugin.M3U.Playlists.Abstractions;
using Emby.Plugin.M3U.Playlists.Models;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Model.Services;

namespace Emby.Plugin.M3U.Playlists.Service.ParameterModels
{
  /// <summary>
  ///   Represents a request to import a playlist
  /// </summary>
  /// <seealso cref="bool" />
  [Route("/playlist", "POST")]
  public class ImportPlaylist: IReturn<bool>, IValidatingModel
  {
    #region Properties

    /// <summary>
    ///   Gets or sets the playlist data.
    /// </summary>
    /// <value>
    ///   The playlist data.
    /// </value>
    [ApiMember(Name = nameof(PlaylistData), Description = "The raw playlist data from an M3U file", IsRequired = true, DataType = "byte[]",
               ParameterType = "body", Verb = "POST")]
    public byte[] PlaylistData { get; set; }

    /// <summary>
    ///   Gets or sets the name of the playlist.
    /// </summary>
    /// <value>
    ///   The name of the playlist.
    /// </value>
    [ApiMember(Name = nameof(PlaylistName), Description = "The name of the playlist that should be imported", IsRequired = false, DataType = "string",
               ParameterType = "body", Verb = "POST")]
    public string PlaylistName { get; set; }

    /// <summary>
    ///   Gets or sets the user identifier.
    /// </summary>
    /// <value>
    ///   The user identifier.
    /// </value>
    [ApiMember(Name = nameof(UserId), Description = "The user id", IsRequired = true, DataType = "string", ParameterType = "path", Verb = "POST")]
    public Guid UserId { get; set; }

    /// <summary>
    ///   Gets or sets the type of the media.
    /// </summary>
    /// <value>
    ///   The type of the media.
    /// </value>
    [ApiMember(Name = nameof(MediaType), Description = "The type of media for which a playlist shall be imported", IsRequired = true,
               DataType = "string", ParameterType = "body", Verb = "POST")]
    public string MediaType { get; set; }

    /// <summary>
    ///   Gets or sets the playlist format.
    /// </summary>
    /// <value>
    ///   The playlist format.
    /// </value>
    [ApiMember(Name = nameof(PlaylistFormat), Description = "The format of the playlist that shall be imported", IsRequired = true,
               DataType = "string", ParameterType = "body", Verb = "POST")]
    public string PlaylistFormat { get; set; }

    #endregion

    #region Methods

    #region Overrides of Object

    /// <inheritdoc />
    public override string ToString()
    {
      return $"{nameof(PlaylistName)}: {PlaylistName}; {nameof(PlaylistFormat)}: {PlaylistFormat}; {nameof(MediaType)}: {MediaType}; {nameof(UserId)}: {UserId}";
    }

    #endregion

    #endregion

    #region Interfaces

    #region Implementation of IValidatingModel

    /// <inheritdoc />
    public ValidationResult Validate()
    {
      throw new NotImplementedException();
    }

    #endregion

    #endregion
  }
}