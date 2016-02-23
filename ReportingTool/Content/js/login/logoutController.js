'use strict'

loginModule.controller("logoutController", ['$scope', '$state', '$http', '$stateParams', 'LoginService', 
    function ($scope, $state, $http, $stateParams, LoginService) {

    $scope.showLogout = LoginService.GetShowLogoutStatus();

    //angular.element(document).ready(function () {
    //    $scope.showLogout.show = LoginService.GetShowLogoutStatus();
    //});


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
                    $scope.showLogout= LoginService.GetShowLogoutStatus();
                    $state.transitionTo('loginView');
                }
            },
            function (response) {
                alert("Server error");
            }
         );
    }
}]);