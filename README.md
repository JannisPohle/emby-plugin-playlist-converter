# emby-plugin-playlist-converter
A plugin for [emby](https://github.com/MediaBrowser/Emby) to import/export playlists.

Supported playlist formats:
* M3U

***Currently only importing a playlist is supported.***

---

The plugin adds a menu entry in the server management to import a playlist, where a playlist file can be uploaded.
The file is then parsed in the service and the plugin tries to find matching media items in emby for the entries of the playlist file.

**Note:**
For the import to work well, it is important that the playlist file contains information about the artist and the track title.
e.g. for the M3U format it is important that extended information is provided:
```
#EXTINF:451,Artist - Track Title
path/to/file/01 Title.flac
```

---
See also [How to build a server plugin](https://github.com/MediaBrowser/Emby/wiki/How-to-build-a-Server-Plugin)

The plugin is automatically copied to the plugin folder of an emby installation on a successful build, if the environment variable `EMBY_PLUGIN_PATH` is set to the full path of that plugin folder