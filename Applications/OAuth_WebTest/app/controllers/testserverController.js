'use strict';
app.controller('testserverController', ['$scope', '$http', function ($scope, $http) {

    $scope.message = "";

    $scope.pushme = function () {

        $http.get("http://localhost:59918/api/now")
            .success(function (response) { $scope.message = response; });
        // example of an alternative call that gives you more control
        //$http({
        //    url:'http://localhost:59918/api/now',
        //    method: "GET",
        //    withCredentials:true,
        //    headers: { 'Content-Type': 'application/json; charset=utf-8'}
        //}).success(function (response) { $scope.message = response; });
    };

}]);