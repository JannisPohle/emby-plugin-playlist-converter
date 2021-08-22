using System;
using Emby.Plugin.M3U.Playlists.Abstractions;
using Emby.Plugin.M3U.Playlists.Models;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Model.Services;

namespace Emby.Plugin.M3U.Playlists.Service.ParameterModels
{
  /// <summary>
  ///   Represents a request to export a playlist
  /// </summary>
  /// <seealso cref="byte" />
  [Route("/playlist/{Id}", "GET")]
  public class ExportPlaylist: IReturn<byte[]>, IValidatingModel
  {
    #region Properties

    /// <summary>
    ///   Gets or sets the playlist identifier that should be exported.
    /// </summary>
    /// <value>
    ///   The identifier.
    /// </value>
    [ApiMember(Name = nameof(Id), Description = "The id of the playlist that should be exported", IsRequired = true, DataType = "string",
               ParameterType = "path", Verb = "GET")]
    public Guid Id { get; set; }

    /// <summary>
    ///   Gets or sets the user identifier.
    /// </summary>
    /// <value>
    ///   The user identifier.
    /// </value>
    [ApiMember(Name = nameof(UserId), Description = "The user id", IsRequired = true, DataType = "string", ParameterType = "path", Verb = "GET")]
    public Guid UserId { get; set; }

    #endregion

    #region Methods

    #region Overrides of Object

    /// <inheritdoc />
    public override string ToString()
    {
      return $"{nameof(Id)}: {Id}; {nameof(UserId)}: {UserId}";
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