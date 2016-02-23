'use strict'

loginModule.controller("logoutController", ['$scope', '$state', '$http', '$stateParams', 'LoginService', '$rootScope', function ($scope, $state, $http, $stateParams, LoginService, $rootScope) {

    angular.element(document).ready(function () {
        $scope.showLogout = LoginService.GetShowLogoutStatus();
    });

    $rootScope.showLogout = false;

    $scope.Logout = function () {

        var req = {
            url: 'Login/Logout',
            method: 'GET',
            headers: { 'content-type': 'application/json' }
        };

        $http(req).then(
            function (r) {
                if (r.data.Status == "loggedOut") {
                    LoginService.SetShowLogoutStatus(false);
                    $scope.showLogout = LoginService.GetShowLogoutStatus();
                    //alert($scope.showLogout);
                    $state.transitionTo('loginView');
                }
            },
            function (response) {
                alert("Server error");
            }
         );
    }
}]);