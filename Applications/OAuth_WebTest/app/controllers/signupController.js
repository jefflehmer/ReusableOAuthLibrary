'use strict';
app.controller('signupController', ['$scope', '$location', '$timeout', 'authService', function ($scope, $location, $timeout, authService) {

    $scope.savedSuccessfully = false;
    $scope.message = "";

    $scope.registration = {
        userName: "",
        email: "",
        password: "",
        confirmPassword: ""
    };

    var startTimer = function () {
        var timer = $timeout(function () {
            $timeout.cancel(timer);
            $location.path('/login');
        }, 2000);
    }

    $scope.signUp = function () {

        authService.saveRegistration($scope.registration).then(function (response) {

            $scope.savedSuccessfully = true;
            $scope.message = "User has been registered successfully, you will be redirected to login page in 2 seconds.";
            startTimer();

        },
         function (response) {
             var errors = [];
             for (var key in response.data.modelState) {
                 for (var i = 0; i < response.data.modelState[key].length; i++) {
                     errors.push(response.data.modelState[key][i]);
                 }
             }

             $scope.message = "Failed to register user due to:" + errors.join(' ');
         });
    };

    $scope.confirmData = {
        password: "",
        userId: "",
        code: ""
    };

    $scope.confirm = function () {

        $scope.confirmData.userId = $location.search()['userId'];
        $scope.confirmData.code = $location.search()['code'];

        authService.confirm($scope.confirmData).then(function (response) {

            $scope.ilmessaggioro = "Email Confirmed! Redirecting to login in two seconds!";
            startTimer();
        },
         function (err) {

             $scope.ilmessaggioro = err.error_description;
         });
    };

}]);