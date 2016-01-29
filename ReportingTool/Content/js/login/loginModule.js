'use strict'

var loginModule = angular.module("loginModule", ['ngMaterial', 'ngMessages']);

loginModule.config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
    var loginView = {
        name: 'loginView',
        url: 'Login',
        templateUrl: 'Content/templates/loginView.html',
        controller: 'loginController'
    }

    $stateProvider.state(loginView);
}]);