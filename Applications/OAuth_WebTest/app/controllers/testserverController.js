'use strict';
app.controller('testserverController', ['$scope', '$http', function ($scope, $http) {

    $scope.message = "";

    $scope.pushme = function () {

        $http.get("http://localhost:26583/api/now")
            .success(function (response) { $scope.message = response; });
        //$http({
        //    url:'http://localhost:24483/api/now',
        //    method: "GET",
        //    withCredentials:true,
        //    headers: { 'Content-Type': 'application/json; charset=utf-8'}
        //}).success(function (response) { $scope.message = response; });
    };

}]);