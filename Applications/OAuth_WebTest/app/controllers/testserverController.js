'use strict';
app.controller('testserverController', ['$scope', '$http', 'constants', function ($scope, $http, constants) {

    $scope.message = "";

    $scope.pushme = function () {

        $http.get(constants.ResourceServiceBase + "api/now")
            .success(function (response) { $scope.message = response; });
        // example of an alternative call that gives you more control
        //$http({
        //    url:constants.ResourceServiceBase + "api/now",
        //    method: "GET",
        //    withCredentials:true,
        //    headers: { 'Content-Type': 'application/json; charset=utf-8'}
        //}).success(function (response) { $scope.message = response; });
    };

}]);