using System;

namespace DNTBreadCrumb.Core;

/// <summary>
///     Represents the current BreadCrumb
/// </summary>
public class BreadCrumb
{
    private string? _url;

    /// <summary>
    ///     A constant URL of the current item
    /// </summary>
    public string? Url
    {
        set => _url = value;
        get => _url ?? UrlFactory?.Invoke();
    }

    /// <summary>
    ///     Title of the current item
    /// </summary>
    public string? Title { set; get; }

    /// <summary>
    ///     An optional glyph icon of the current item
    /// </summary>
    public string? GlyphIcon { set; get; }

    /// <summary>
    ///     Oder of the current item in the final list
    /// </summary>
    public int Order { set; get; }

    /// <summary>
    ///     Forces the generated bread crumbs url to be rendered as an achor tag
    /// </summary>
    public bool ForceUrl { set; get; }

    /// <summary>
    ///     Deferred URL generation, if URL is also set, URL has a higher priority
    /// </summary>
    public Func<string>? UrlFactory { set; get; }
}