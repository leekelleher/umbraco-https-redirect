# HTTPS Redirect for Umbraco

HTTPS Redirect provides a simple mechanism to switch a URL from HTTP to HTTPS (SSL) based on the document-type (alias), node id or template alias.

This package will add the following appSetting keys to your Web.config file: `HttpsRedirect:DocTypes`, `HttpsRedirect:PageIds`, `HttpsRedirect:Templates` `HttpsRedirect:StripPort`, `HttpsRedirect:UseTemporaryRedirects` and `HttpsRedirect:XForwardedProto`.

* DocTypes - a comma separated list of doc type aliases that should be served as HTTPS.
* PageIds - a comma separated list of page IDs that should be served as HTTPS.
* Templates - a comma separated list of template aliases that should be served as HTTPS.
* StripPort - used to strip out the port portion of the URL - use this when you use a non-standard port internally to your firewall (e.g. 81 or 444).
* UseTemporaryRedirects - boolean value indicating whether HTTPS redirects should be temporary (302).
* XForwardedProto - boolean value indicating whether to use a load balancer with SSL termination that adds the X-Forwarded-Proto header.

---

## Getting Started

### Installation

> *Note:* HTTPS Redirect has been developed against **Umbraco v6.1.2** and will support that version and above.

HTTPS Redirect can be installed from either Our Umbraco or build manually from the source-code:

#### Our Umbraco package repository

To install from Our Umbraco, please download the package from:

> <https://our.umbraco.org/projects/website-utilities/https-redirect/>


#### Manual build

If you prefer, you can compile the project yourself, you'll need:

* Visual Studio 2012 (or above)

To clone it locally click the "Clone in Windows" button above or run the following git commands.

	git clone https://github.com/leekelleher/umbraco-https-redirect.git umbraco-https-redirect
	cd umbraco-https-redirect
	.\build.cmd

---

## Contributing to this project

Anyone and everyone is welcome to contribute. Please take a moment to review the [guidelines for contributing](CONTRIBUTING.md).

* [Bug reports](CONTRIBUTING.md#bugs)
* [Feature requests](CONTRIBUTING.md#features)
* [Pull requests](CONTRIBUTING.md#pull-requests)


## Contact

Have a question?

* [Raise an issue](https://github.com/leekelleher/umbraco-https-redirect/issues) on GitHub


## License

Copyright &copy; 2011 Lee Kelleher, Umbrella Inc, Our Umbraco

Licensed under the [MIT License](LICENSE.md)
