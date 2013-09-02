# HTTPS Redirect

HTTPS Redirect provides a simple mechanism to switch a URL from HTTP to HTTPS (SSL) based on the document-type (alias), node id or template alias.

This package will add the following appSetting keys to your Web.config file: `HttpsRedirect:DocTypes`, `HttpsRedirect:PageIds`, `HttpsRedirect:Templates`, `HttpsRedirect:Properties` and `HttpsRedirect:StripPort`.

* DocTypes - a comma separated list of doc type aliases that should be served as HTTPS.
* PageIds - a comma separated list of page IDs that should be served as HTTPS.
* Templates - a comma separated list of template aliases that should be served as HTTPS.
* Properties - a comma separated list of property aliases and values, in the foramt property:value, that when matched, should cause the page to be served as HTTPS.
* StripPort - used to strip out the port portion of the URL - use this when you use a non-stanrard port internally to your firewall (eg 81 or 444).
* UsePermanentRedirects - boolean value indicating whether HTTPS redirects should be permanent (301).