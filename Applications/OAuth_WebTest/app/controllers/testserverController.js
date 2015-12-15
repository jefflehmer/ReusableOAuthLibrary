'use strict';
app.controller('testserverController', ['$scope', '$http', 'constants', function ($scope, $http, constants) {

    var serviceBase = constants.ResourceServiceBase;
    $scope.message = "";

    $scope.pushme = function () {

        $http.get(serviceBase + "api/now")
            .success(function (response) { $scope.message = response; });
        // example of an alternative call that gives you more control
        //$http({
        //    url:serviceBase + "api/now",
        //    method: "GET",
        //    withCredentials:true,
        //    headers: { 'Content-Type': 'application/json; charset=utf-8'}
        //}).success(function (response) { $scope.message = response; });
    };

}]);