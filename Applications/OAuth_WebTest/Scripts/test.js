(function () {

    var app = angular.module("OAuthTestApp");

    app.filter('webformatname', function () {
        return function (rawhtml) {
            if (rawhtml) {
                var result = rawhtml.replace('<sup>&reg;</sup>', '®');
                result = result.replace('<sup>TM</sup>', '®');
                return result;
            }
            return rawhtml;
        }
    });
}());
