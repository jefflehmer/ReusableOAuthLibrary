var app = angular.module('OAuthTestApp', ['ngRoute', 'LocalStorageModule', 'angular-loading-bar']);

app.config(function ($routeProvider) {

    $routeProvider.when("/login", {
        controller: "loginController",
        templateUrl: "/app/views/login.html"
    });

    $routeProvider.when("/signup", {
        controller: "signupController",
        templateUrl: "/app/views/signup.html"
    });

    $routeProvider.when("/testserver", {
        controller: "testserverController",
        templateUrl: "/app/views/testserver.html"
    });

    $routeProvider.when("/confirm", {
        controller: "signupController",
        templateUrl: "/app/views/confirm.html"
    });

    $routeProvider.otherwise({ redirectTo: "/testserver" });
});

app.config(['$httpProvider', function ($httpProvider) {

    $httpProvider.defaults.useXDomain = true;
    $httpProvider.interceptors.push('authInterceptorService');
}]);

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);