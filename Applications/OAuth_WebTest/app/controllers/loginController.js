'use strict';
app.controller('loginController', ['$scope', '$location', 'authService', function ($scope, $location, authService) {

    $scope.loginData = {
        userName: "",
        password: ""
    };

    $scope.message = "";

    $scope.login = function () {

        authService.login($scope.loginData).then(function (response) {

            //window.location.replace('/home/access');
            $location.path('#/home');
        },
         function (err) {
             $scope.message = err.error_description;
         });
    };

}]);