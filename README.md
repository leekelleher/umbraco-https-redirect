# HTTPS Redirect

HTTPS Redirect provides a simple mechanism to switch a URL from HTTP to HTTPS (SSL) based on the document-type (alias), node id or template alias.

This package will add the following appSetting keys to your Web.config file: `HttpsRedirect:DocTypes`, `HttpsRedirect:PageIds`, `HttpsRedirect:Templates` `HttpsRedirect:StripPort`, `HttpsRedirect:UseTemporaryRedirects` and `HttpsRedirect:XForwardedProto`.

* DocTypes - a comma separated list of doc type aliases that should be served as HTTPS.
* PageIds - a comma separated list of page IDs that should be served as HTTPS.
* Templates - a comma separated list of template aliases that should be served as HTTPS.
* StripPort - used to strip out the port portion of the URL - use this when you use a non-standard port internally to your firewall (e.g. 81 or 444).
* UseTemporaryRedirects - boolean value indicating whether HTTPS redirects should be temporary (302).
* XForwardedProto - boolean value indicating whether to use a load balancer with SSL termination that adds the X-Forwarded-Proto header.
