Shortcodes for Umbraco provides a filter that will parse your content pages for shortcodes, replacing them with the appropriate value.

This initial release uses shortcodes that support Macro parameter syntax and enabled RestExtension methods.

Example shortcodes:

* Place the following shortcode anywhere in your content (RTE, Razor, XSLT or template), and [#pageName] will return the name of the content page.

* To use a RestExtension, try the following syntax: [{alias}:{method}({parameters})] ... e.g. [Shortcodes:Today(yyyy-MM-dd)]